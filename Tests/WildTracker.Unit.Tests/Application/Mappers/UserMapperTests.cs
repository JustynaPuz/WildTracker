using NUnit.Framework;
using WildTracker.Application.Mappers;
using WildTracker.Domain.Enums;
using WildTracker.Unit.Tests.Builders;

namespace WildTracker.Unit.Tests.Application.Mappers;

[TestFixture]
public class UserMapperTests
{

    [Test]
    public void ToDto_MapsAllProperties()
    {
        var user = new AppUserBuilder()
            .WithFirstName("Jane")
            .WithLastName("Doe")
            .WithEmail("jane@example.com")
            .WithRole(UserRole.Ranger)
            .Build();

        var result = UserMapper.ToDto(user);

        Assert.Multiple(() =>
        {
            Assert.That(result.Id,           Is.EqualTo(user.Id));
            Assert.That(result.FirstName,    Is.EqualTo("Jane"));
            Assert.That(result.LastName,     Is.EqualTo("Doe"));
            Assert.That(result.Email,        Is.EqualTo("jane@example.com"));
            Assert.That(result.Role,         Is.EqualTo(UserRole.Ranger));
            Assert.That(result.IsActive,     Is.True);
            Assert.That(result.CreatedAtUtc, Is.EqualTo(user.CreatedAtUtc));
        });
    }

    [Test]
    public void ToDto_WhenUserIsDeactivated_MapsIsActiveAsFalse()
    {
        var user = new AppUserBuilder().Build();
        user.Deactivate();

        var result = UserMapper.ToDto(user);

        Assert.That(result.IsActive, Is.False);
    }
}
