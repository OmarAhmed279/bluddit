namespace Bluddit.Api.Configurations;

public record JwtOptions
{
    public string Secret { get; set; } = String.Empty;
    public int ExpiresInHours { get; set; } = 1;
}