using NUnit.Framework;
using WildTracker.Application.Validators;
using WildTracker.Contracts.Requests;
using WildTracker.Domain.Enums;

namespace WildTracker.Unit.Tests.Application.Validators;

[TestFixture]
public class AnimalValidatorTests
{
    private CreateAnimalValidator _createValidator = null!;
    private UpdateAnimalValidator _updateValidator = null!;

    [SetUp]
    public void SetUp()
    {
        _createValidator = new CreateAnimalValidator();
        _updateValidator = new UpdateAnimalValidator();
    }

    [Test]
    public void CreateAnimal_WithValidRequest_PassesValidation()
    {
        var request = new CreateAnimalRequest
        {
            Identifier   = "WOLF-001",
            Name         = "Grey Wolf",
            Species      = Species.Wolf,
            HealthStatus = AnimalHealthStatus.Healthy
        };

        Assert.That(_createValidator.Validate(request).IsValid, Is.True);
    }

    [TestCase("")]
    [TestCase("   ")]
    public void CreateAnimal_WithEmptyIdentifier_FailsValidation(string identifier)
    {
        var request = new CreateAnimalRequest
        {
            Identifier   = identifier,
            Name         = "Grey Wolf",
            Species      = Species.Wolf,
            HealthStatus = AnimalHealthStatus.Healthy
        };
        var result = _createValidator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Select(e => e.PropertyName), Does.Contain(nameof(CreateAnimalRequest.Identifier)));
    }

    [Test]
    public void CreateAnimal_WithIdentifierExceedingMaxLength_FailsValidation()
    {
        var request = new CreateAnimalRequest
        {
            Identifier   = new string('a', 51),
            Name         = "Grey Wolf",
            Species      = Species.Wolf,
            HealthStatus = AnimalHealthStatus.Healthy
        };
        var result = _createValidator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Select(e => e.PropertyName), Does.Contain(nameof(CreateAnimalRequest.Identifier)));
    }

    [TestCase("")]
    [TestCase("   ")]
    public void CreateAnimal_WithEmptyName_FailsValidation(string name)
    {
        var request = new CreateAnimalRequest
        {
            Identifier   = "WOLF-001",
            Name         = name,
            Species      = Species.Wolf,
            HealthStatus = AnimalHealthStatus.Healthy
        };
        var result = _createValidator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Select(e => e.PropertyName), Does.Contain(nameof(CreateAnimalRequest.Name)));
    }

    [Test]
    public void CreateAnimal_WithNameExceedingMaxLength_FailsValidation()
    {
        var request = new CreateAnimalRequest
        {
            Identifier   = "WOLF-001",
            Name         = new string('a', 101),
            Species      = Species.Wolf,
            HealthStatus = AnimalHealthStatus.Healthy
        };
        var result = _createValidator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Select(e => e.PropertyName), Does.Contain(nameof(CreateAnimalRequest.Name)));
    }

    [Test]
    public void CreateAnimal_WithNullDescription_PassesValidation()
    {
        var request = new CreateAnimalRequest
        {
            Identifier   = "WOLF-001",
            Name         = "Grey Wolf",
            Species      = Species.Wolf,
            HealthStatus = AnimalHealthStatus.Healthy,
            Description  = null
        };

        Assert.That(_createValidator.Validate(request).IsValid, Is.True);
    }

    [Test]
    public void CreateAnimal_WithDescriptionExceedingMaxLength_FailsValidation()
    {
        var request = new CreateAnimalRequest
        {
            Identifier   = "WOLF-001",
            Name         = "Grey Wolf",
            Species      = Species.Wolf,
            HealthStatus = AnimalHealthStatus.Healthy,
            Description  = new string('a', 1001)
        };
        var result = _createValidator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Select(e => e.PropertyName), Does.Contain(nameof(CreateAnimalRequest.Description)));
    }

    [Test]
    public void UpdateAnimal_WithValidRequest_PassesValidation()
    {
        var request = new UpdateAnimalRequest
        {
            Name         = "Grey Wolf",
            Species      = Species.Wolf,
            HealthStatus = AnimalHealthStatus.Healthy
        };

        Assert.That(_updateValidator.Validate(request).IsValid, Is.True);
    }

    [Test]
    public void UpdateAnimal_WithEmptyName_FailsValidation()
    {
        var request = new UpdateAnimalRequest
        {
            Name         = "",
            Species      = Species.Wolf,
            HealthStatus = AnimalHealthStatus.Healthy
        };
        var result = _updateValidator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Select(e => e.PropertyName), Does.Contain(nameof(UpdateAnimalRequest.Name)));
    }

    [Test]
    public void UpdateAnimal_WithDescriptionExceedingMaxLength_FailsValidation()
    {
        var request = new UpdateAnimalRequest
        {
            Name         = "Grey Wolf",
            Species      = Species.Wolf,
            HealthStatus = AnimalHealthStatus.Healthy,
            Description  = new string('a', 1001)
        };
        var result = _updateValidator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Select(e => e.PropertyName), Does.Contain(nameof(UpdateAnimalRequest.Description)));
    }
}
