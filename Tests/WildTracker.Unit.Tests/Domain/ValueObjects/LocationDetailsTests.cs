using NUnit.Framework;
using WildTracker.Domain.ValueObjects;

namespace WildTracker.Unit.Tests.Domain.ValueObjects;

[TestFixture]
public class LocationDetailsTests
{
    private const int MaxRegionLength        = 100;
    private const int MaxForestDistrictLength = 100;
    private const int MaxDescriptionLength   = 300;

    private static readonly Coordinates ValidCoordinates = new(52.0, 19.0);

    [Test]
    public void Constructor_WithValidData_SetsProperties()
    {
        var location = new LocationDetails(ValidCoordinates, "Mazury", "Forest District A", "desc");

        Assert.Multiple(() =>
        {
            Assert.That(location.Coordinates,    Is.EqualTo(ValidCoordinates));
            Assert.That(location.Region,          Is.EqualTo("Mazury"));
            Assert.That(location.ForestDistrict,  Is.EqualTo("Forest District A"));
            Assert.That(location.Description,     Is.EqualTo("desc"));
        });
    }

    [Test]
    public void Constructor_WithOnlyCoordinates_SetsOptionalFieldsToNull()
    {
        var location = new LocationDetails(ValidCoordinates);

        Assert.Multiple(() =>
        {
            Assert.That(location.Region,         Is.Null);
            Assert.That(location.ForestDistrict, Is.Null);
            Assert.That(location.Description,    Is.Null);
        });
    }

    [Test]
    public void Constructor_WithNullCoordinates_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _ = new LocationDetails(null!));
    }

    [Test]
    public void Constructor_WithRegionExceedingMaxLength_ThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            _ = new LocationDetails(ValidCoordinates, region: new string('a', MaxRegionLength + 1)));

        Assert.That(ex!.Message, Does.Contain($"Value cannot exceed {MaxRegionLength} characters."));
    }

    [Test]
    public void Constructor_WithForestDistrictExceedingMaxLength_ThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            _ = new LocationDetails(ValidCoordinates, forestDistrict: new string('a', MaxForestDistrictLength + 1)));

        Assert.That(ex!.Message, Does.Contain($"Value cannot exceed {MaxForestDistrictLength} characters."));
    }

    [Test]
    public void Constructor_WithDescriptionExceedingMaxLength_ThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            _ = new LocationDetails(ValidCoordinates, description: new string('a', MaxDescriptionLength + 1)));

        Assert.That(ex!.Message, Does.Contain($"Value cannot exceed {MaxDescriptionLength} characters."));
    }

    [Test]
    public void Equals_WithSameValues_ReturnsTrue()
    {
        var a = new LocationDetails(new Coordinates(52.0, 19.0), "Mazury");
        var b = new LocationDetails(new Coordinates(52.0, 19.0), "Mazury");

        Assert.That(a, Is.EqualTo(b));
    }

    [Test]
    public void Equals_WithDifferentCoordinates_ReturnsFalse()
    {
        var a = new LocationDetails(new Coordinates(52.0, 19.0));
        var b = new LocationDetails(new Coordinates(48.0, 21.0));

        Assert.That(a, Is.Not.EqualTo(b));
    }

    [Test]
    public void Equals_WithDifferentRegion_ReturnsFalse()
    {
        var a = new LocationDetails(new Coordinates(52.0, 19.0), "Mazury");
        var b = new LocationDetails(new Coordinates(52.0, 19.0), "Tatry");

        Assert.That(a, Is.Not.EqualTo(b));
    }
}
