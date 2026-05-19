using NUnit.Framework;
using WildTracker.Domain.ValueObjects;

namespace WildTracker.Unit.Tests.Domain.ValueObjects;

[TestFixture]
public class CoordinatesTests
{
    private static IEnumerable<TestCaseData> InvalidLatitudes =>
    [
        new TestCaseData(-90.1, "Latitude must be between -90 and 90."),
        new TestCaseData(90.1,  "Latitude must be between -90 and 90."),
    ];

    private static IEnumerable<TestCaseData> InvalidLongitudes =>
    [
        new TestCaseData(-180.1, "Longitude must be between -180 and 180."),
        new TestCaseData(180.1,  "Longitude must be between -180 and 180."),
    ];

    // --- Constructor ---

    [Test]
    public void Constructor_WithValidData_SetsProperties()
    {
        var coords = new Coordinates(52.0, 19.0);

        Assert.Multiple(() =>
        {
            Assert.That(coords.Latitude,  Is.EqualTo(52.0));
            Assert.That(coords.Longitude, Is.EqualTo(19.0));
        });
    }

    [TestCase(-90.0,   0.0)]
    [TestCase(90.0,    0.0)]
    [TestCase(0.0,  -180.0)]
    [TestCase(0.0,   180.0)]
    public void Constructor_WithBoundaryValues_DoesNotThrow(double latitude, double longitude)
    {
        Assert.DoesNotThrow(() => _ = new Coordinates(latitude, longitude));
    }

    [TestCaseSource(nameof(InvalidLatitudes))]
    public void Constructor_WithInvalidLatitude_ThrowsArgumentOutOfRangeException(double latitude, string expectedMessage)
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => _ = new Coordinates(latitude, 0.0));

        Assert.That(ex!.Message, Does.Contain(expectedMessage));
    }

    [TestCaseSource(nameof(InvalidLongitudes))]
    public void Constructor_WithInvalidLongitude_ThrowsArgumentOutOfRangeException(double longitude, string expectedMessage)
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => _ = new Coordinates(0.0, longitude));

        Assert.That(ex!.Message, Does.Contain(expectedMessage));
    }

    // --- Equals ---

    [Test]
    public void Equals_WithSameValues_ReturnsTrue()
    {
        var a = new Coordinates(52.0, 19.0);
        var b = new Coordinates(52.0, 19.0);

        Assert.That(a, Is.EqualTo(b));
    }

    [Test]
    public void Equals_WithDifferentValues_ReturnsFalse()
    {
        var a = new Coordinates(52.0, 19.0);
        var b = new Coordinates(48.0, 21.0);

        Assert.That(a, Is.Not.EqualTo(b));
    }
}
