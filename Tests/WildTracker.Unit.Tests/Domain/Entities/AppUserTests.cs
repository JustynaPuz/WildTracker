using NUnit.Framework;
using WildTracker.Domain.Entities;
using WildTracker.Domain.Enums;
using WildTracker.Unit.Tests.Builders;

namespace WildTracker.Unit.Tests.Domain.Entities;

[TestFixture]
public class AppUserTests
{
    private const int MaxNameLength = 100;
    private const int MaxEmailLength = 200;

    private static IEnumerable<TestCaseData> InvalidNames =>
    [
        new TestCaseData("", "Value cannot be empty."),
        new TestCaseData(new string('a', MaxNameLength + 1), $"Value cannot exceed {MaxNameLength} characters."),
    ];

    private static IEnumerable<TestCaseData> InvalidEmails =>
    [
        new TestCaseData("", "Email cannot be empty."),
        new TestCaseData("invalidemail.com", "Email format is invalid."),
        new TestCaseData(new string('j', MaxEmailLength + 1), "Email cannot exceed 200 characters."),
    ];

    [Test]
    public void Constructor_WithValidData_SetsDefaults()
    {
        var user = new AppUser("Jane", "Smith", "jan@example.com", "hash");

        Assert.Multiple(() =>
        {
            Assert.That(user.IsActive, Is.True);
            Assert.That(user.Role, Is.EqualTo(UserRole.Viewer));
        });
    }

    [TestCaseSource(nameof(InvalidNames))]
    public void Constructor_WithInvalidFirstName_ThrowsArgumentException(string firstName, string expectedMessage)
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            _ = new AppUser(firstName, "Doe", "jane@example.com", "hash");
        });

        Assert.That(ex!.Message, Does.Contain(expectedMessage));
    }

    [TestCaseSource(nameof(InvalidNames))]
    public void Constructor_WithInvalidLastName_ThrowsArgumentException(string lastName, string expectedMessage)
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            _ = new AppUser("Jane", lastName, "jane@example.com", "hash");
        });

        Assert.That(ex!.Message, Does.Contain(expectedMessage));
    }

    [TestCaseSource(nameof(InvalidEmails))]
    public void Constructor_WithInvalidEmail_ThrowsArgumentException(string email, string expectedMessage)
    {
        var ex = Assert.Throws<ArgumentException>(() => { _ = new AppUser("Jane", "Smith", email, "hash"); });

        Assert.That(ex!.Message, Does.Contain(expectedMessage));
    }

    [Test]
    public void Constructor_NormalizesEmailToLowercase()
    {
        var user = new AppUser("Jane", "Smith", "JAN@EXAMPLE.COM", "hash");

        Assert.That(user.Email, Is.EqualTo("jan@example.com"));
    }

    [Test]
    public void Constructor_TrimsWhitespaceFromEmail()
    {
        var user = new AppUser("Jane", "Smith", "  jan@example.com  ", "hash");

        Assert.That(user.Email, Is.EqualTo("jan@example.com"));
    }

    [Test]
    public void UpdateProfile_WithValidData_UpdatesProperties()
    {
        var user = new AppUserBuilder().Build();

        user.UpdateProfile("NewFirst", "NewLast", "new@example.com");

        Assert.Multiple(() =>
        {
            Assert.That(user.FirstName, Is.EqualTo("NewFirst"));
            Assert.That(user.LastName, Is.EqualTo("NewLast"));
            Assert.That(user.Email, Is.EqualTo("new@example.com"));
        });
    }

    [TestCaseSource(nameof(InvalidNames))]
    public void UpdateProfile_WithInvalidFirstName_ThrowsArgumentException(string firstName, string expectedMessage)
    {
        var user = new AppUserBuilder().Build();

        var ex = Assert.Throws<ArgumentException>(() =>
            user.UpdateProfile(firstName, "Doe", "jane@example.com"));

        Assert.That(ex!.Message, Does.Contain(expectedMessage));
    }

    [TestCaseSource(nameof(InvalidNames))]
    public void UpdateProfile_WithInvalidLastName_ThrowsArgumentException(string lastName, string expectedMessage)
    {
        var user = new AppUserBuilder().Build();

        var ex = Assert.Throws<ArgumentException>(() =>
            user.UpdateProfile("Jane", lastName, "jane@example.com"));

        Assert.That(ex!.Message, Does.Contain(expectedMessage));
    }

    [Test]
    public void UpdateProfile_WithEmptyEmail_ThrowsArgumentException()
    {
        var user = new AppUserBuilder().Build();

        var ex = Assert.Throws<ArgumentException>(() =>
            user.UpdateProfile("Jane", "Smith", ""));

        Assert.That(ex!.Message, Does.Contain("Email cannot be empty."));
    }

    [Test]
    public void ChangeRole_SetsNewRole()
    {
        var user = new AppUserBuilder().Build();

        user.ChangeRole(UserRole.Admin);

        Assert.That(user.Role, Is.EqualTo(UserRole.Admin));
    }

    [Test]
    public void Deactivate_SetsIsActiveToFalse()
    {
        var user = new AppUserBuilder().Build();

        user.Deactivate();

        Assert.That(user.IsActive, Is.False);
    }

    [Test]
    public void Activate_SetsIsActiveToTrue()
    {
        var user = new AppUserBuilder().Build();
        user.Deactivate();

        user.Activate();

        Assert.That(user.IsActive, Is.True);
    }
}
