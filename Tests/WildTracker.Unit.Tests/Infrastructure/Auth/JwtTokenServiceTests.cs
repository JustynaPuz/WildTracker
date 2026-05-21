using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using WildTracker.Infrastructure.Auth;
using WildTracker.Unit.Tests.Builders;

namespace WildTracker.Unit.Tests.Infrastructure.Auth;

[TestFixture]
public class JwtTokenServiceTests
{
    private JwtTokenService _service = null!;
    private JwtSettings _settings = null!;

    [SetUp]
    public void SetUp()
    {
        _settings = new JwtSettings
        {
            Secret        = "wildtracker-test-secret-key-32chars!!",
            Issuer        = "wildtracker-test",
            Audience      = "wildtracker-test",
            ExpiryMinutes = 60
        };
        _service = new JwtTokenService(Options.Create(_settings));
    }

    [Test]
    public void GenerateToken_ReturnsNonEmptyToken()
    {
        var user         = new AppUserBuilder().Build();
        var (token, _)   = _service.GenerateToken(user);

        Assert.That(token, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public void GenerateToken_ExpiresAtIsInFuture()
    {
        var user           = new AppUserBuilder().Build();
        var (_, expiresAt) = _service.GenerateToken(user);

        Assert.That(expiresAt, Is.GreaterThan(DateTime.UtcNow));
    }

    [Test]
    public void GenerateToken_ExpiresAtMatchesConfiguredExpiryMinutes()
    {
        var user           = new AppUserBuilder().Build();
        var before         = DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes - 1);
        var after          = DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes + 1);
        var (_, expiresAt) = _service.GenerateToken(user);

        Assert.That(expiresAt, Is.InRange(before, after));
    }

    [Test]
    public void GenerateToken_TokenContainsExpectedClaims()
    {
        var user         = new AppUserBuilder().Build();
        var (token, _)   = _service.GenerateToken(user);
        var jwt          = new JwtSecurityTokenHandler().ReadJwtToken(token);

        Assert.Multiple(() =>
        {
            Assert.That(jwt.Subject, Is.EqualTo(user.Id.ToString()));
            Assert.That(jwt.Claims.First(c => c.Type == JwtRegisteredClaimNames.Email).Value,
                Is.EqualTo(user.Email));
            Assert.That(jwt.Issuer,   Is.EqualTo(_settings.Issuer));
        });
    }
}
