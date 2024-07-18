using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Shared.DB.Classes.User;

// Add profile data for application users by adding properties to the User class
public sealed class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [MaxLength(100)] public string UserName { get; set; }
    [MaxLength(100)] public string? NormalizedUserName { get; set; }
    [JsonIgnore, MaxLength(100)] public string? PasswordHash { get; set; }

    /// <summary>
    /// DO NOT TOUCH. FOR YOUR HANDS EXISTS: Access
    /// </summary>
    public uint AccessFlags { get; set; } // Хранить флаги доступа в виде числа

    /// <summary>
    /// Use '|' to add permssions like:
    /// user.Access |= AccessLevel.Teacher;
    /// To delete perm use:
    /// user.Access &= ~AccessLevel.Teacher;
    /// </summary>
    [NotMapped, JsonIgnore]
    public AccessLevel Access
    {
        get => (AccessLevel)AccessFlags;
        set => AccessFlags = (uint)value;
    }

    [JsonIgnore] public List<UserGroup>? UserGroups { get; set; }

    public User()
    {
    }

    public User(string userName) : this()
    {
        UserName = userName;
    }


    public override string ToString()
        => UserName ?? string.Empty;
}