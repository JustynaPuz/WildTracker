using NUnit.Framework;
using NSubstitute;
using WildTracker.Application.Exceptions;
using WildTracker.Application.Interfaces;
using WildTracker.Application.Services;
using WildTracker.Domain.Repositories;
using WildTracker.Unit.Tests.Builders;

namespace WildTracker.Unit.Tests.Application.Services;

// How to approach these tests:
// 1. Create mocks: Substitute.For<IAppUserRepository>(), IPasswordHasher, ITokenService.
// 2. Build a new AuthService(_userRepo, _passwordHasher, _tokenService).
// 3. For login tests — configure _userRepo.GetByEmailAsync(...).Returns(user or null).
// 4. For password tests — configure _passwordHasher.Verify(...).Returns(true/false).
// 5. For token tests — configure _tokenService.GenerateToken(...).Returns(("token", expiry)).

[TestFixture]
public class AuthServiceTests
{
    // --- LoginAsync ---

    [Test, Ignore("TODO")]
    public async Task LoginAsync_WhenUserNotFound_ThrowsUnauthorizedException() { }

    [Test, Ignore("TODO")]
    public async Task LoginAsync_WhenUserIsDeactivated_ThrowsUnauthorizedException() { }

    [Test, Ignore("TODO")]
    public async Task LoginAsync_WhenPasswordIsInvalid_ThrowsUnauthorizedException() { }

    [Test, Ignore("TODO")]
    public async Task LoginAsync_WhenCredentialsAreValid_ReturnsAuthResponseWithToken() { }

    // --- RegisterAsync ---

    [Test, Ignore("TODO")]
    public async Task RegisterAsync_WhenEmailAlreadyExists_ThrowsConflictException() { }

    [Test, Ignore("TODO")]
    public async Task RegisterAsync_WithValidData_CreatesUserAndReturnsAuthResponse() { }
}
