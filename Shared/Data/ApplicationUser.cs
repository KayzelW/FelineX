using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Shared.Interfaces;
using Shared.Types;

namespace Web.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public sealed class ApplicationUser : IdentityUser
{
    [JsonIgnore] public List<UserGroup>? UserGroups { get; set; } = [];

    public override string ToString() => NormalizedUserName ?? UserName ?? Email ?? Id.ToString();
}

public sealed class ApplicationRole : IdentityRole
{
    
}