namespace WildTracker.Contracts.DTOs;

public record AuthResponseDto
{
    public string Token { get; init; } = default!;
    public DateTime ExpiresAt { get; init; }
    public AppUserDto User { get; init; } = default!;
}
