using System.Runtime.Serialization;

namespace Shared.Models;

public class AuthAnswer
{
    public string? UserToken { get; set; }
    public string? UserName { get; set; }
    public uint? Access { get; set; }
}