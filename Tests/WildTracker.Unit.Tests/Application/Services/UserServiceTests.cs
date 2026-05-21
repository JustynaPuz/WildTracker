using NUnit.Framework;
using NSubstitute;
using WildTracker.Application.Exceptions;
using WildTracker.Application.Services;
using WildTracker.Domain.Entities;
using WildTracker.Domain.Enums;
using WildTracker.Domain.Repositories;
using WildTracker.Unit.Tests.Builders;

namespace WildTracker.Unit.Tests.Application.Services;

[TestFixture]
public class UserServiceTests
{
    private IAppUserRepository _repo = null!;
    private UserService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _repo    = Substitute.For<IAppUserRepository>();
        _service = new UserService(_repo);
    }

    [Test]
    public async Task GetAllAsync_ReturnsMappedUsers()
    {
        var userFirst  = new AppUserBuilder().WithFirstName("Jane").WithEmail("jane@example.com").Build();
        var userSecond = new AppUserBuilder().WithFirstName("John").WithEmail("john@example.com").Build();
        _repo.GetAllAsync().Returns(new List<AppUser> { userFirst, userSecond });

        var result = await _service.GetAllAsync();

        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.Select(r => r.Email), Does.Contain("jane@example.com").And.Contain("john@example.com"));
    }

    [Test]
    public async Task GetAllAsync_WhenNoUsersExist_ReturnsEmptyList()
    {
        _repo.GetAllAsync().Returns(new List<AppUser>());

        var result = await _service.GetAllAsync();

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetByIdAsync_WhenUserNotFound_ThrowsNotFoundException()
    {
        _repo.GetByIdAsync(Arg.Any<Guid>()).Returns((AppUser?)null);

        Assert.That(
            async () => await _service.GetByIdAsync(Guid.NewGuid()),
            Throws.TypeOf<NotFoundException>());
    }

    [Test]
    public async Task GetByIdAsync_WhenUserFound_ReturnsMappedDto()
    {
        var user = new AppUserBuilder().Build();
        _repo.GetByIdAsync(Arg.Any<Guid>()).Returns(user);

        var result = await _service.GetByIdAsync(user.Id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(user.Id));
    }

    [Test]
    public void ChangeRoleAsync_WhenUserNotFound_ThrowsNotFoundException()
    {
        _repo.GetByIdAsync(Arg.Any<Guid>()).Returns((AppUser?)null);

        Assert.That(
            async () => await _service.ChangeRoleAsync(Guid.NewGuid(), UserRole.Admin),
            Throws.TypeOf<NotFoundException>());
    }

    [Test]
    public async Task ChangeRoleAsync_WhenUserFound_ChangesRoleAndReturnsDto()
    {
        var user = new AppUserBuilder().WithRole(UserRole.Viewer).Build();
        _repo.GetByIdAsync(Arg.Any<Guid>()).Returns(user);

        var result = await _service.ChangeRoleAsync(user.Id, UserRole.Admin);

        await _repo.Received(1).UpdateAsync(user);
        Assert.That(result.Role, Is.EqualTo(UserRole.Admin));
    }

    [Test]
    public void ActivateAsync_WhenUserNotFound_ThrowsNotFoundException()
    {
        _repo.GetByIdAsync(Arg.Any<Guid>()).Returns((AppUser?)null);

        Assert.That(
            async () => await _service.ActivateAsync(Guid.NewGuid()),
            Throws.TypeOf<NotFoundException>());
    }

    [Test]
    public async Task ActivateAsync_WhenUserFound_ActivatesAndReturnsDto()
    {
        var user = new AppUserBuilder().Build();
        user.Deactivate();
        _repo.GetByIdAsync(Arg.Any<Guid>()).Returns(user);

        var result = await _service.ActivateAsync(user.Id);

        await _repo.Received(1).UpdateAsync(user);
        Assert.That(result.IsActive, Is.True);
    }

    [Test]
    public void DeactivateAsync_WhenUserNotFound_ThrowsNotFoundException()
    {
        _repo.GetByIdAsync(Arg.Any<Guid>()).Returns((AppUser?)null);

        Assert.That(
            async () => await _service.DeactivateAsync(Guid.NewGuid()),
            Throws.TypeOf<NotFoundException>());
    }

    [Test]
    public async Task DeactivateAsync_WhenUserFound_DeactivatesAndReturnsDto()
    {
        var user = new AppUserBuilder().Build();
        _repo.GetByIdAsync(Arg.Any<Guid>()).Returns(user);

        var result = await _service.DeactivateAsync(user.Id);

        await _repo.Received(1).UpdateAsync(user);
        Assert.That(result.IsActive, Is.False);
    }
}
