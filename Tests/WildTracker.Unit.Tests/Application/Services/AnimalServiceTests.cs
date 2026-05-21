using NUnit.Framework;
using NSubstitute;
using WildTracker.Application.Exceptions;
using WildTracker.Application.Services;
using WildTracker.Contracts.Requests;
using WildTracker.Domain.Entities;
using WildTracker.Domain.Enums;
using WildTracker.Domain.Queries;
using WildTracker.Domain.Repositories;
using WildTracker.Domain.ValueObjects;
using WildTracker.Unit.Tests.Builders;

namespace WildTracker.Unit.Tests.Application.Services;

[TestFixture]
public class AnimalServiceTests
{
    private IAnimalRepository _animalRepo = null!;
    private ISightingReportRepository _reportRepo = null!;
    private AnimalService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _animalRepo = Substitute.For<IAnimalRepository>();
        _reportRepo = Substitute.For<ISightingReportRepository>();
        _service    = new AnimalService(_animalRepo, _reportRepo);
    }

    [Test]
    public async Task GetByIdAsync_WhenAnimalNotFound_ThrowsNotFoundException()
    {
        _animalRepo.GetByIdAsync(Arg.Any<Guid>()).Returns((Animal?)null);

        Assert.That(
            async () => await _service.GetByIdAsync(Guid.NewGuid()),
            Throws.TypeOf<NotFoundException>());
    }

    [Test]
    public async Task GetByIdAsync_WhenAnimalFound_ReturnsMappedDto()
    {
        var animal = new AnimalBuilder().Build();
        _animalRepo.GetByIdAsync(Arg.Any<Guid>()).Returns(animal);

        var result = await _service.GetByIdAsync(animal.Id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(animal.Id));
    }

    [Test]
    public async Task GetAllAsync_ReturnsMappedDtos()
    {
        var animalFirst  = new AnimalBuilder().WithIdentifier("WOLF-001").WithName("Grey Wolf").Build();
        var animalSecond = new AnimalBuilder().WithIdentifier("BEAR-001").WithName("Brown Bear").Build();
        _animalRepo.GetAllAsync().Returns(new List<Animal> { animalFirst, animalSecond });

        var result = await _service.GetAllAsync();

        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.Select(r => r.Name), Does.Contain("Grey Wolf").And.Contain("Brown Bear"));
    }

    [Test]
    public async Task GetMovementAsync_WhenAnimalNotFound_ThrowsNotFoundException()
    {
        _animalRepo.GetByIdAsync(Arg.Any<Guid>()).Returns((Animal?)null);

        Assert.That(
            async () => await _service.GetMovementAsync(Guid.NewGuid(), 10),
            Throws.TypeOf<NotFoundException>());
    }

    [Test]
    public async Task GetMovementAsync_WhenAnimalFound_ReturnsMovementPoints()
    {
        var observedAt = new DateTime(2025, 6, 1, 12, 0, 0, DateTimeKind.Utc);
        var location   = new LocationDetails(new Coordinates(52.0, 19.0), region: "Mazury");

        var animal = new AnimalBuilder().Build();
        var report = new SightingReportBuilder()
            .WithObservedAt(observedAt)
            .WithLocation(location)
            .Build();

        _animalRepo.GetByIdAsync(Arg.Any<Guid>()).Returns(animal);
        _reportRepo.SearchAsync(Arg.Any<SightingReportFilter>())
                   .Returns((new List<SightingReport> { report }, 1));

        var result = await _service.GetMovementAsync(animal.Id, 10);

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(result[0].Latitude,      Is.EqualTo(52.0));
            Assert.That(result[0].Longitude,     Is.EqualTo(19.0));
            Assert.That(result[0].Region,        Is.EqualTo("Mazury"));
            Assert.That(result[0].ObservedAtUtc, Is.EqualTo(observedAt));
        });
    }

    [Test]
    public async Task CreateAsync_CreatesAnimalAndReturnsDto()
    {
        var request = new CreateAnimalRequest
        {
            Identifier   = "WOLF-001",
            Name         = "Grey Wolf",
            Species      = Species.Wolf,
            HealthStatus = AnimalHealthStatus.Healthy
        };

        var result = await _service.CreateAsync(request);

        await _animalRepo.Received(1).AddAsync(Arg.Any<Animal>());
        Assert.Multiple(() =>
        {
            Assert.That(result.Identifier, Is.EqualTo("WOLF-001"));
            Assert.That(result.Name,       Is.EqualTo("Grey Wolf"));
        });
    }

    [Test]
    public async Task UpdateAsync_WhenAnimalNotFound_ThrowsNotFoundException()
    {
        _animalRepo.GetByIdAsync(Arg.Any<Guid>()).Returns((Animal?)null);

        Assert.That(
            async () => await _service.UpdateAsync(Guid.NewGuid(), new UpdateAnimalRequest()),
            Throws.TypeOf<NotFoundException>());
    }

    [Test]
    public async Task UpdateAsync_WhenAnimalFound_UpdatesAndReturnsDto()
    {
        var animal  = new AnimalBuilder().WithIdentifier("WOLF-001").Build();
        var request = new UpdateAnimalRequest
        {
            Name         = "Grey Wolf",
            Species      = Species.Wolf,
            HealthStatus = AnimalHealthStatus.Healthy
        };
        _animalRepo.GetByIdAsync(Arg.Any<Guid>()).Returns(animal);

        var result = await _service.UpdateAsync(animal.Id, request);

        await _animalRepo.Received(1).UpdateAsync(animal);
        Assert.Multiple(() =>
        {
            Assert.That(result.Name,         Is.EqualTo("Grey Wolf"));
            Assert.That(result.Species,      Is.EqualTo(Species.Wolf));
            Assert.That(result.HealthStatus, Is.EqualTo(AnimalHealthStatus.Healthy));
        });
    }

    [Test]
    public async Task DeleteAsync_WhenAnimalNotFound_ThrowsNotFoundException()
    {
        _animalRepo.GetByIdAsync(Arg.Any<Guid>()).Returns((Animal?)null);

        Assert.That(
            async () => await _service.DeleteAsync(Guid.NewGuid()),
            Throws.TypeOf<NotFoundException>());
    }

    [Test]
    public async Task DeleteAsync_WhenAnimalFound_DeletesEntity()
    {
        var animal = new AnimalBuilder().Build();
        _animalRepo.GetByIdAsync(Arg.Any<Guid>()).Returns(animal);

        await _service.DeleteAsync(animal.Id);

        await _animalRepo.Received(1).DeleteAsync(animal);
    }
}
