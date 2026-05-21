using NUnit.Framework;
using WildTracker.Infrastructure.Auth;

namespace WildTracker.Unit.Tests.Infrastructure.Auth;

[TestFixture]
public class BcryptPasswordHasherTests
{
    private BcryptPasswordHasher _hasher = null!;

    [SetUp]
    public void SetUp()
    {
        _hasher = new BcryptPasswordHasher();
    }

    [Test]
    public void Hash_ReturnsNonEmptyHash()
    {
        var hash = _hasher.Hash("password123");

        Assert.That(hash, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public void Hash_TwoCallsWithSamePassword_ReturnDifferentHashes()
    {

        var hash1 = _hasher.Hash("password123");
        var hash2 = _hasher.Hash("password123");

        Assert.That(hash1, Is.Not.EqualTo(hash2));
    }

    [Test]
    public void Verify_WithCorrectPassword_ReturnsTrue()
    {
        var hash   = _hasher.Hash("password123");
        var result = _hasher.Verify("password123", hash);

        Assert.That(result, Is.True);
    }

    [Test]
    public void Verify_WithWrongPassword_ReturnsFalse()
    {
        var hash   = _hasher.Hash("password123");
        var result = _hasher.Verify("wrongpassword", hash);

        Assert.That(result, Is.False);
    }
}
