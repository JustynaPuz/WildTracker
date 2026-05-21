using NUnit.Framework;
using WildTracker.Application.Mappers;
using WildTracker.Contracts.Requests;
using WildTracker.Domain.Enums;
using WildTracker.Unit.Tests.Builders;

namespace WildTracker.Unit.Tests.Application.Mappers;

[TestFixture]
public class AnimalMapperTests
{

    [Test]
    public void ToDto_MapsAllProperties()
    {
        var animal = new AnimalBuilder()
            .WithIdentifier("WOLF-001")
            .WithName("Grey Wolf")
            .WithSpecies(Species.Wolf)
            .WithHealthStatus(AnimalHealthStatus.Healthy)
            .WithDescription("A grey wolf")
            .Build();

        var result = AnimalMapper.ToDto(animal);

        Assert.Multiple(() =>
        {
            Assert.That(result.Id,           Is.EqualTo(animal.Id));
            Assert.That(result.Identifier,   Is.EqualTo("WOLF-001"));
            Assert.That(result.Name,         Is.EqualTo("Grey Wolf"));
            Assert.That(result.Species,      Is.EqualTo(Species.Wolf));
            Assert.That(result.HealthStatus, Is.EqualTo(AnimalHealthStatus.Healthy));
            Assert.That(result.Description,  Is.EqualTo("A grey wolf"));
            Assert.That(result.CreatedAtUtc, Is.EqualTo(animal.CreatedAtUtc));
        });
    }

    [Test]
    public void ToEntity_MapsAllProperties()
    {
        var request = new CreateAnimalRequest
        {
            Identifier   = "BEAR-001",
            Name         = "Brown Bear",
            Species      = Species.Bear,
            HealthStatus = AnimalHealthStatus.Injured,
            Description  = "A brown bear"
        };

        var result = AnimalMapper.ToEntity(request);

        Assert.Multiple(() =>
        {
            Assert.That(result.Identifier,   Is.EqualTo("BEAR-001"));
            Assert.That(result.Name,         Is.EqualTo("Brown Bear"));
            Assert.That(result.Species,      Is.EqualTo(Species.Bear));
            Assert.That(result.HealthStatus, Is.EqualTo(AnimalHealthStatus.Injured));
            Assert.That(result.Description,  Is.EqualTo("A brown bear"));
        });
    }

    [Test]
    public void ToEntity_WithNullDescription_SetsDescriptionToNull()
    {
        var request = new CreateAnimalRequest
        {
            Identifier   = "WOLF-001",
            Name         = "Grey Wolf",
            Species      = Species.Wolf,
            HealthStatus = AnimalHealthStatus.Healthy,
            Description  = null
        };

        var result = AnimalMapper.ToEntity(request);

        Assert.That(result.Description, Is.Null);
    }
}
