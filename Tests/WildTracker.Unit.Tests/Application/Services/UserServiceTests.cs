using NUnit.Framework;
using NSubstitute;
using WildTracker.Application.Exceptions;
using WildTracker.Application.Services;
using WildTracker.Domain.Enums;
using WildTracker.Domain.Repositories;
using WildTracker.Unit.Tests.Builders;

namespace WildTracker.Unit.Tests.Application.Services;

// How to approach these tests:
// 1. Create a mock: Substitute.For<IAppUserRepository>().
// 2. Build a new UserService(_repo).
// 3. For "not found" tests — configure _repo.GetByIdAsync(...).ReturnsNull().
// 4. For "found" tests — return a user from AppUserBuilder and assert the returned Dto
//    reflects the expected state change (role changed, active/inactive).

[TestFixture]
public class UserServiceTests
{
    // --- GetAllAsync ---

    [Test, Ignore("TODO")]
    public async Task GetAllAsync_ReturnsMappedUsers() { }

    // --- GetByIdAsync ---

    [Test, Ignore("TODO")]
    public async Task GetByIdAsync_WhenUserNotFound_ThrowsNotFoundException() { }

    [Test, Ignore("TODO")]
    public async Task GetByIdAsync_WhenUserFound_ReturnsMappedDto() { }

    // --- ChangeRoleAsync ---

    [Test, Ignore("TODO")]
    public async Task ChangeRoleAsync_WhenUserNotFound_ThrowsNotFoundException() { }

    [Test, Ignore("TODO")]
    public async Task ChangeRoleAsync_WhenUserFound_ChangesRoleAndReturnsDto() { }

    // --- ActivateAsync ---

    [Test, Ignore("TODO")]
    public async Task ActivateAsync_WhenUserNotFound_ThrowsNotFoundException() { }

    [Test, Ignore("TODO")]
    public async Task ActivateAsync_WhenUserFound_ActivatesAndReturnsDto() { }

    // --- DeactivateAsync ---

    [Test, Ignore("TODO")]
    public async Task DeactivateAsync_WhenUserNotFound_ThrowsNotFoundException() { }

    [Test, Ignore("TODO")]
    public async Task DeactivateAsync_WhenUserFound_DeactivatesAndReturnsDto() { }
}
