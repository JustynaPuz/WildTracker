using NUnit.Framework;
using WildTracker.Domain.Entities;
using WildTracker.Domain.Enums;
using WildTracker.Unit.Tests.Builders;

namespace WildTracker.Unit.Tests.Domain.Entities;

[TestFixture]
public class AnimalTests
{
    private const int MaxIdentifierLength = 50;
    private const int MaxNameLength = 100;
    private const int MaxDescriptionLength = 1000;

    private static IEnumerable<TestCaseData> InvalidIdentifiers =>
    [
        new TestCaseData("", "Value cannot be empty."),
        new TestCaseData(new string('a', MaxIdentifierLength + 1),
            $"Value cannot exceed {MaxIdentifierLength} characters."),
    ];

    private static IEnumerable<TestCaseData> InvalidNames =>
    [
        new TestCaseData("", "Value cannot be empty."),
        new TestCaseData(new string('a', MaxNameLength + 1), $"Value cannot exceed {MaxNameLength} characters."),
    ];

    [Test]
    public void Constructor_WithValidData_SetsProperties()
    {
        var animal = new Animal("WOLF-001", "Grey Wolf", Species.Wolf, AnimalHealthStatus.Healthy, "desc");

        Assert.Multiple(() =>
        {
            Assert.That(animal.Identifier, Is.EqualTo("WOLF-001"));
            Assert.That(animal.Name, Is.EqualTo("Grey Wolf"));
            Assert.That(animal.Species, Is.EqualTo(Species.Wolf));
            Assert.That(animal.HealthStatus, Is.EqualTo(AnimalHealthStatus.Healthy));
            Assert.That(animal.Description, Is.EqualTo("desc"));
        });
    }

    [TestCaseSource(nameof(InvalidIdentifiers))]
    public void Constructor_WithInvalidIdentifier_ThrowsArgumentException(string identifier, string expectedMessage)
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            _ = new Animal(identifier, "name", Species.Bear, AnimalHealthStatus.Healthy, "description");
        });

        Assert.That(ex!.Message, Does.Contain(expectedMessage));
    }

    [TestCaseSource(nameof(InvalidNames))]
    public void Constructor_WithInvalidName_ThrowsArgumentException(string name, string expectedMessage)
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            _ = new Animal("id", name, Species.Bear, AnimalHealthStatus.Healthy, "description");
        });

        Assert.That(ex!.Message, Does.Contain(expectedMessage));
    }

    [Test]
    public void Constructor_WithDescriptionExceeding1000Chars_ThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            _ = new Animal("id", "name", Species.Bear, AnimalHealthStatus.Healthy,
                new string('a', MaxDescriptionLength + 1));
        });

        Assert.That(ex!.Message, Does.Contain($"Value cannot exceed {MaxDescriptionLength} characters."));
    }

    [Test]
    public void UpdateDetails_WithValidData_UpdatesProperties()
    {
        var animal = new AnimalBuilder().Build();

        animal.UpdateDetails("newName", Species.Bear, AnimalHealthStatus.Dead, "description");

        Assert.Multiple(() =>
        {
            Assert.That(animal.Name, Is.EqualTo("newName"));
            Assert.That(animal.Species, Is.EqualTo(Species.Bear));
            Assert.That(animal.HealthStatus, Is.EqualTo(AnimalHealthStatus.Dead));
            Assert.That(animal.Description, Is.EqualTo("description"));
        });
    }

    [TestCaseSource(nameof(InvalidNames))]
    public void UpdateDetails_WithInvalidName_ThrowsArgumentException(string name, string expectedMessage)
    {
        var animal = new AnimalBuilder().Build();

        var ex = Assert.Throws<ArgumentException>(() =>
            animal.UpdateDetails(name, Species.Bear, AnimalHealthStatus.Healthy, "description"));

        Assert.That(ex!.Message, Does.Contain(expectedMessage));
    }

    [Test]
    public void UpdateDetails_WithDescriptionExceeding1000Chars_ThrowsArgumentException()
    {
        var animal = new AnimalBuilder().Build();

        var ex = Assert.Throws<ArgumentException>(() =>
            animal.UpdateDetails("name", Species.Bear, AnimalHealthStatus.Healthy,
                new string('a', MaxDescriptionLength + 1)));

        Assert.That(ex!.Message, Does.Contain($"Value cannot exceed {MaxDescriptionLength} characters."));
    }

    [Test]
    public void MarkSeen_SetsLastSeenAtUtc()
    {
        var animal = new AnimalBuilder().Build();
        var seenAt = new DateTime(2000, 1, 1);

        animal.MarkSeen(seenAt);

        Assert.That(animal.LastSeenAtUtc, Is.EqualTo(seenAt));
    }
}
