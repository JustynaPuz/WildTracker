using WildTracker.Domain.Entities;
using WildTracker.Domain.Enums;

namespace WildTracker.Unit.Tests.Builders;

internal sealed class AppUserBuilder
{
    private string _firstName    = "Jane";
    private string _lastName     = "Doe";
    private string _email        = "jane.doe@example.com";
    private string _passwordHash = "hashed_password";
    private UserRole _role       = UserRole.Viewer;

    internal AppUserBuilder WithFirstName(string v)    { _firstName = v;     return this; }
    internal AppUserBuilder WithLastName(string v)     { _lastName = v;      return this; }
    internal AppUserBuilder WithEmail(string v)        { _email = v;         return this; }
    internal AppUserBuilder WithPasswordHash(string v) { _passwordHash = v;  return this; }
    internal AppUserBuilder WithRole(UserRole r)       { _role = r;          return this; }

    internal AppUser Build() =>
        new(_firstName, _lastName, _email, _passwordHash, _role);
}
