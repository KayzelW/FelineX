using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Shared.Interfaces;
using Shared.Types;

namespace Web.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public sealed class ApplicationUser : IdentityUser, IInnerIdentity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public new Guid Id { get; } = Guid.NewGuid();

    public uint AccessFlags { get; set; }

    [NotMapped, JsonIgnore]
    public AccessLevel Access
    {
        get => (AccessLevel)AccessFlags;
        set => AccessFlags = (uint)value;
    }

    [JsonIgnore] public List<UserGroup>? UserGroups { get; set; } = [];

    public override string ToString() => NormalizedUserName ?? UserName ?? Email ?? Id.ToString();
}