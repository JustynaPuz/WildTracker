using NUnit.Framework;
using WildTracker.Application.Validators;
using WildTracker.Contracts.Requests;

namespace WildTracker.Unit.Tests.Application.Validators;

[TestFixture]
public class ObservationNoteValidatorTests
{
    private CreateObservationNoteValidator _createValidator = null!;
    private UpdateObservationNoteValidator _updateValidator = null!;

    [SetUp]
    public void SetUp()
    {
        _createValidator = new CreateObservationNoteValidator();
        _updateValidator = new UpdateObservationNoteValidator();
    }

    [Test]
    public void CreateNote_WithValidRequest_PassesValidation()
    {
        var request = new CreateObservationNoteRequest { Content = "Observed near the river." };

        Assert.That(_createValidator.Validate(request).IsValid, Is.True);
    }

    [Test]
    public void CreateNote_WithEmptyContent_FailsValidation()
    {
        var request = new CreateObservationNoteRequest { Content = "" };
        var result  = _createValidator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Select(e => e.PropertyName), Does.Contain(nameof(CreateObservationNoteRequest.Content)));
    }

    [Test]
    public void CreateNote_WithContentExceedingMaxLength_FailsValidation()
    {
        var request = new CreateObservationNoteRequest { Content = new string('a', 1001) };
        var result  = _createValidator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Select(e => e.PropertyName), Does.Contain(nameof(CreateObservationNoteRequest.Content)));
    }

    [Test]
    public void UpdateNote_WithValidRequest_PassesValidation()
    {
        var request = new UpdateObservationNoteRequest { Content = "Updated observation." };

        Assert.That(_updateValidator.Validate(request).IsValid, Is.True);
    }

    [Test]
    public void UpdateNote_WithEmptyContent_FailsValidation()
    {
        var request = new UpdateObservationNoteRequest { Content = "" };
        var result  = _updateValidator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Select(e => e.PropertyName), Does.Contain(nameof(UpdateObservationNoteRequest.Content)));
    }

    [Test]
    public void UpdateNote_WithContentExceedingMaxLength_FailsValidation()
    {
        var request = new UpdateObservationNoteRequest { Content = new string('a', 1001) };
        var result  = _updateValidator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Select(e => e.PropertyName), Does.Contain(nameof(UpdateObservationNoteRequest.Content)));
    }
}
