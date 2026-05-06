using WildTracker.Contracts.DTOs;
using WildTracker.Contracts.Requests;

namespace WildTracker.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginRequest request);
    Task<AuthResponseDto> RegisterAsync(RegisterRequest request);
}
