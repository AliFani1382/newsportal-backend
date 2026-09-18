public sealed class AuthResponseDto
{
    public int UserId { get; set; }
    public required string Username { get; set; }
    public required string Token { get; set; }
    public required string RefreshToken { get; set; }
    public bool IsEmailVerified { get; set; }
}