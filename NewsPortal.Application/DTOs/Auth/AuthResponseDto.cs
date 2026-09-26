public sealed record AuthResponseDto
{
    public int UserId { get; init; }
    public required string Username { get; init; }
    public required string Token { get; init; }
    public required string RefreshToken { get; init; }
    public bool IsEmailVerified { get; init; }
}