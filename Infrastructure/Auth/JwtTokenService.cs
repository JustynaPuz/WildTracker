using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WildTracker.Application.Interfaces;
using WildTracker.Domain.Entities;

namespace WildTracker.Infrastructure.Auth;

public class JwtTokenService : ITokenService
{
    private readonly JwtSettings _settings;

    public JwtTokenService(IOptions<JwtSettings> settings)
    {
        _settings = settings.Value;
    }

    public (string Token, DateTime ExpiresAt) GenerateToken(AppUser user)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes);
        var key       = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
        var creds     = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub,             user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email,           user.Email),
            new Claim(JwtRegisteredClaimNames.GivenName,       user.FirstName),
            new Claim(JwtRegisteredClaimNames.FamilyName,      user.LastName),
            new Claim(ClaimTypes.Role,                         user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti,             Guid.NewGuid().ToString()),
        };

        var token = new JwtSecurityToken(
            issuer:             _settings.Issuer,
            audience:           _settings.Audience,
            claims:             claims,
            expires:            expiresAt,
            signingCredentials: creds
        );

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }
}
