namespace Infrastucture.Configuration;

public class JwtSettings
{
    public TimeSpan Expires { get; set; }
    public string SecretKey { get; set; } = null!;
}
