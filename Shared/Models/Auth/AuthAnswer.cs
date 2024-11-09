namespace Shared.Models.Auth;

/// <summary>
/// DTO, contains the UserToken, UserName and Access(<see cref="uint"/>)
/// </summary>
public class AuthAnswer
{
    public string? UserToken { get; set; }
    public string? UserName { get; set; }
    public uint? Access { get; set; }
}