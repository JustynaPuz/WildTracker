using NUnit.Framework;
using WildTracker.Application.Validators;
using WildTracker.Contracts.Requests;

namespace WildTracker.Unit.Tests.Application.Validators;

[TestFixture]
public class AuthValidatorTests
{
    private LoginValidator _loginValidator = null!;
    private RegisterValidator _registerValidator = null!;

    [SetUp]
    public void SetUp()
    {
        _loginValidator    = new LoginValidator();
        _registerValidator = new RegisterValidator();
    }

    [Test]
    public void Login_WithValidRequest_PassesValidation()
    {
        var request = new LoginRequest { Email = "jane@example.com", Password = "password" };

        Assert.That(_loginValidator.Validate(request).IsValid, Is.True);
    }

    [TestCase("")]
    [TestCase("not-an-email")]
    public void Login_WithInvalidEmail_FailsValidation(string email)
    {
        var request = new LoginRequest { Email = email, Password = "password" };
        var result  = _loginValidator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Select(e => e.PropertyName), Does.Contain(nameof(LoginRequest.Email)));
    }

    [Test]
    public void Login_WithEmptyPassword_FailsValidation()
    {
        var request = new LoginRequest { Email = "jane@example.com", Password = "" };
        var result  = _loginValidator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Select(e => e.PropertyName), Does.Contain(nameof(LoginRequest.Password)));
    }

    [Test]
    public void Register_WithValidRequest_PassesValidation()
    {
        var request = new RegisterRequest
        {
            FirstName = "Jane",
            LastName  = "Doe",
            Email     = "jane@example.com",
            Password  = "password123"
        };

        Assert.That(_registerValidator.Validate(request).IsValid, Is.True);
    }

    [TestCase("")]
    [TestCase("   ")]
    public void Register_WithEmptyFirstName_FailsValidation(string firstName)
    {
        var request = new RegisterRequest { FirstName = firstName, LastName = "Doe", Email = "j@e.com", Password = "password123" };
        var result  = _registerValidator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Select(e => e.PropertyName), Does.Contain(nameof(RegisterRequest.FirstName)));
    }

    [Test]
    public void Register_WithFirstNameExceedingMaxLength_FailsValidation()
    {
        var request = new RegisterRequest { FirstName = new string('a', 101), LastName = "Doe", Email = "j@e.com", Password = "password123" };
        var result  = _registerValidator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Select(e => e.PropertyName), Does.Contain(nameof(RegisterRequest.FirstName)));
    }

    [Test]
    public void Register_WithEmptyLastName_FailsValidation()
    {
        var request = new RegisterRequest { FirstName = "Jane", LastName = "", Email = "j@e.com", Password = "password123" };
        var result  = _registerValidator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Select(e => e.PropertyName), Does.Contain(nameof(RegisterRequest.LastName)));
    }

    [TestCase("")]
    [TestCase("not-an-email")]
    public void Register_WithInvalidEmail_FailsValidation(string email)
    {
        var request = new RegisterRequest { FirstName = "Jane", LastName = "Doe", Email = email, Password = "password123" };
        var result  = _registerValidator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Select(e => e.PropertyName), Does.Contain(nameof(RegisterRequest.Email)));
    }

    [Test]
    public void Register_WithPasswordTooShort_FailsValidation()
    {
        var request = new RegisterRequest { FirstName = "Jane", LastName = "Doe", Email = "j@e.com", Password = "short" };
        var result  = _registerValidator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Select(e => e.PropertyName), Does.Contain(nameof(RegisterRequest.Password)));
    }

    [Test]
    public void Register_WithEmptyPassword_FailsValidation()
    {
        var request = new RegisterRequest { FirstName = "Jane", LastName = "Doe", Email = "j@e.com", Password = "" };
        var result  = _registerValidator.Validate(request);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Select(e => e.PropertyName), Does.Contain(nameof(RegisterRequest.Password)));
    }
}
