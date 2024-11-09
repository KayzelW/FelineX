using System.Text.Json.Serialization;
using Shared.Types;

namespace Shared.Models;

public class UserDto
{
    public Guid Id { get; set; }
    public Guid? UserName { get; set; }
    public uint AccessFlags { get; set; } = new();
    [JsonIgnore] public AccessLevel Access
    {
        get => (AccessLevel)AccessFlags;
        set => AccessFlags = (uint)value;
    }
}