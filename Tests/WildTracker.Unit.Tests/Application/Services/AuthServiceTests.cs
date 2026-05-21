using NUnit.Framework;
using NSubstitute;
using WildTracker.Application.Exceptions;
using WildTracker.Application.Interfaces;
using WildTracker.Application.Services;
using WildTracker.Contracts.Requests;
using WildTracker.Domain.Entities;
using WildTracker.Domain.Repositories;
using WildTracker.Unit.Tests.Builders;

namespace WildTracker.Unit.Tests.Application.Services;

[TestFixture]
public class AuthServiceTests
{
    private IAppUserRepository _userRepo = null!;
    private IPasswordHasher _passwordHasher = null!;
    private ITokenService _tokenService = null!;
    private AuthService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _userRepo       = Substitute.For<IAppUserRepository>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _tokenService   = Substitute.For<ITokenService>();
        _service        = new AuthService(_userRepo, _passwordHasher, _tokenService);
    }

    [Test]
    public async Task LoginAsync_WhenUserNotFound_ThrowsUnauthorizedException()
    {
        _userRepo.GetByEmailAsync(Arg.Any<string>()).Returns((AppUser?)null);

        Assert.That(
            async () => await _service.LoginAsync(new LoginRequest { Email = "a@b.com", Password = "pass" }),
            Throws.TypeOf<UnauthorizedException>());
    }

    [Test]
    public async Task LoginAsync_WhenUserIsDeactivated_ThrowsUnauthorizedException()
    {
        var user = new AppUserBuilder().Build();
        user.Deactivate();
        _userRepo.GetByEmailAsync(Arg.Any<string>()).Returns(user);

        Assert.That(
            async () => await _service.LoginAsync(new LoginRequest { Email = user.Email, Password = "pass" }),
            Throws.TypeOf<UnauthorizedException>());
    }

    [Test]
    public async Task LoginAsync_WhenPasswordIsInvalid_ThrowsUnauthorizedException()
    {
        var user = new AppUserBuilder().Build();
        _userRepo.GetByEmailAsync(Arg.Any<string>()).Returns(user);
        _passwordHasher.Verify(Arg.Any<string>(), Arg.Any<string>()).Returns(false);

        Assert.That(
            async () => await _service.LoginAsync(new LoginRequest { Email = user.Email, Password = "wrong" }),
            Throws.TypeOf<UnauthorizedException>());
    }

    [Test]
    public async Task LoginAsync_WhenCredentialsAreValid_ReturnsAuthResponseWithToken()
    {
        var user      = new AppUserBuilder().Build();
        var expiresAt = DateTime.UtcNow.AddHours(1);
        _userRepo.GetByEmailAsync(Arg.Any<string>()).Returns(user);
        _passwordHasher.Verify(Arg.Any<string>(), Arg.Any<string>()).Returns(true);
        _tokenService.GenerateToken(user).Returns(("test-token", expiresAt));

        var result = await _service.LoginAsync(new LoginRequest { Email = user.Email, Password = "correct" });

        Assert.Multiple(() =>
        {
            Assert.That(result.Token,    Is.EqualTo("test-token"));
            Assert.That(result.ExpiresAt, Is.EqualTo(expiresAt));
            Assert.That(result.User.Id,  Is.EqualTo(user.Id));
        });
    }

    [Test]
    public async Task RegisterAsync_WhenEmailAlreadyExists_ThrowsConflictException()
    {
        _userRepo.GetByEmailAsync(Arg.Any<string>()).Returns(new AppUserBuilder().Build());

        Assert.That(
            async () => await _service.RegisterAsync(new RegisterRequest
            {
                FirstName = "Jane",
                LastName  = "Doe",
                Email     = "jane@example.com",
                Password  = "password123"
            }),
            Throws.TypeOf<ConflictException>());
    }

    [Test]
    public async Task RegisterAsync_WithValidData_CreatesUserAndReturnsAuthResponse()
    {
        var expiresAt = DateTime.UtcNow.AddHours(1);
        _userRepo.GetByEmailAsync(Arg.Any<string>()).Returns((AppUser?)null);
        _passwordHasher.Hash(Arg.Any<string>()).Returns("hashed-password");
        _tokenService.GenerateToken(Arg.Any<AppUser>()).Returns(("test-token", expiresAt));

        var result = await _service.RegisterAsync(new RegisterRequest
        {
            FirstName = "Jane",
            LastName  = "Doe",
            Email     = "jane@example.com",
            Password  = "password123"
        });

        await _userRepo.Received(1).AddAsync(Arg.Any<AppUser>());
        Assert.Multiple(() =>
        {
            Assert.That(result.Token,          Is.EqualTo("test-token"));
            Assert.That(result.User.Email,     Is.EqualTo("jane@example.com"));
            Assert.That(result.User.FirstName, Is.EqualTo("Jane"));
        });
    }
}
