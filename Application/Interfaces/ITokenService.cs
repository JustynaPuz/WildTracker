using WildTracker.Domain.Entities;

namespace WildTracker.Application.Interfaces;

public interface ITokenService
{
    (string Token, DateTime ExpiresAt) GenerateToken(AppUser user);
}
