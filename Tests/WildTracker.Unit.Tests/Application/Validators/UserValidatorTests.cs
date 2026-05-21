using NUnit.Framework;
using WildTracker.Application.Validators;
using WildTracker.Contracts.Requests;
using WildTracker.Domain.Enums;

namespace WildTracker.Unit.Tests.Application.Validators;

[TestFixture]
public class UserValidatorTests
{
    private UpdateUserRoleValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new UpdateUserRoleValidator();
    }

    [TestCase(UserRole.Viewer)]
    [TestCase(UserRole.Ranger)]
    [TestCase(UserRole.Admin)]
    public void UpdateUserRole_WithValidRole_PassesValidation(UserRole role)
    {
        var request = new UpdateUserRoleRequest { Role = role };

        Assert.That(_validator.Validate(request).IsValid, Is.True);
    }

    [Test]
    public void UpdateUserRole_WithInvalidEnumValue_FailsValidation()
    {
        var request = new UpdateUserRoleRequest { Role = (UserRole)999 };
        var result  = _validator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Select(e => e.PropertyName), Does.Contain(nameof(UpdateUserRoleRequest.Role)));
    }
}
