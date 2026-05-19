using NUnit.Framework;
using NSubstitute;
using WildTracker.Application.Exceptions;
using WildTracker.Application.Services;
using WildTracker.Domain.Entities;
using WildTracker.Domain.Queries;
using WildTracker.Domain.Repositories;
using WildTracker.Domain.ValueObjects;
using WildTracker.Unit.Tests.Builders;

namespace WildTracker.Unit.Tests.Application.Services;

[TestFixture]
public class AnimalServiceTests
{
    // --- GetByIdAsync ---

    [Test]
    public async Task GetByIdAsync_WhenAnimalNotFound_ThrowsNotFoundException()
    {
        var animalRepo = Substitute.For<IAnimalRepository>();
        var reportRepo = Substitute.For<ISightingReportRepository>();
        var service = new AnimalService(animalRepo, reportRepo);

        animalRepo.GetByIdAsync(Arg.Any<Guid>()).Returns((Animal?)null);

        Assert.ThrowsAsync<NotFoundException>(() => service.GetByIdAsync(Guid.NewGuid()));
        
    }

    [Test]
    public async Task GetByIdAsync_WhenAnimalFound_ReturnsMappedDto()
    {
        var animalRepo = Substitute.For<IAnimalRepository>();
        var reportRepo = Substitute.For<ISightingReportRepository>();
        var service = new AnimalService(animalRepo, reportRepo);
        
        var animal = new AnimalBuilder().Build();

        animalRepo.GetByIdAsync(Arg.Any<Guid>()).Returns(animal); 
        
        var result = await service.GetByIdAsync(animal.Id);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(animal.Id));

    }

    // --- GetAllAsync ---

    [Test]
    public async Task GetAllAsync_ReturnsMappedDtos()
    {
        var animalRepo = Substitute.For<IAnimalRepository>();
        var reportRepo = Substitute.For<ISightingReportRepository>();
        var service = new AnimalService(animalRepo, reportRepo);
        
        var animalFirst  = new AnimalBuilder().WithIdentifier("WOLF-001").WithName("Grey Wolf").Build();
        var animalSecond = new AnimalBuilder().WithIdentifier("BEAR-001").WithName("Brown Bear").Build();
        List<Animal> animals = [animalFirst, animalSecond];
        
        animalRepo.GetAllAsync().Returns(animals); 
        
        var result = await service.GetAllAsync();
        
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.Select(r => r.Name), Does.Contain("Grey Wolf").And.Contain("Brown Bear"));
    }

    // --- GetMovementAsync ---

    [Test]
    public async Task GetMovementAsync_WhenAnimalNotFound_ThrowsNotFoundException()
    {
        var animalRepo = Substitute.For<IAnimalRepository>();
        var reportRepo = Substitute.For<ISightingReportRepository>();
        var service = new AnimalService(animalRepo, reportRepo);

        animalRepo.GetByIdAsync(Arg.Any<Guid>()).Returns((Animal?)null);

        Assert.ThrowsAsync<NotFoundException>(() => service.GetMovementAsync(Guid.NewGuid(), 10));
    }

    [Test]
    public async Task GetMovementAsync_WhenAnimalFound_ReturnsMovementPoints()
    {
        var animalRepo = Substitute.For<IAnimalRepository>();
        var reportRepo = Substitute.For<ISightingReportRepository>();
        var service = new AnimalService(animalRepo, reportRepo);

        var observedAt = new DateTime(2025, 6, 1, 12, 0, 0, DateTimeKind.Utc);
        var location   = new LocationDetails(new Coordinates(52.0, 19.0), region: "Mazury");

        var animal = new AnimalBuilder().Build();
        var report = new SightingReportBuilder()
            .WithObservedAt(observedAt)
            .WithLocation(location)
            .Build();

        IReadOnlyList<SightingReport> sightingReports = [report];

        animalRepo.GetByIdAsync(Arg.Any<Guid>()).Returns(animal);
        reportRepo.SearchAsync(Arg.Any<SightingReportFilter>()).Returns((sightingReports, 1));

        var result = await service.GetMovementAsync(animal.Id, 10);

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(result[0].Latitude,      Is.EqualTo(52.0));
            Assert.That(result[0].Longitude,     Is.EqualTo(19.0));
            Assert.That(result[0].Region,        Is.EqualTo("Mazury"));
            Assert.That(result[0].ObservedAtUtc, Is.EqualTo(observedAt));
        });
    }

    // --- CreateAsync ---

    [Test, Ignore("TODO")]
    public async Task CreateAsync_CreatesAnimalAndReturnsDto() { }

    // --- UpdateAsync ---

    [Test, Ignore("TODO")]
    public async Task UpdateAsync_WhenAnimalNotFound_ThrowsNotFoundException() { }

    [Test, Ignore("TODO")]
    public async Task UpdateAsync_WhenAnimalFound_UpdatesAndReturnsDto() { }

    // --- DeleteAsync ---

    [Test, Ignore("TODO")]
    public async Task DeleteAsync_WhenAnimalNotFound_ThrowsNotFoundException() { }

    [Test, Ignore("TODO")]
    public async Task DeleteAsync_WhenAnimalFound_DeletesEntity() { }
}
