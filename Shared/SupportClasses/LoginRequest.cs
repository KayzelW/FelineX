namespace Shared.SupportClasses;

public sealed class LoginRequest
{
    public required string Login { get; init; }
    public required string Password { get; init; }
}