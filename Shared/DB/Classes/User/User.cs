using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Shared.DB.Classes.User;

// Add profile data for application users by adding properties to the User class
public sealed class User
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(100)] public string? UserName { get; set; }
    [MaxLength(100)] public string? NormalizedUserName { get; set; }
    [MaxLength(100)] public string? PasswordHash { get; set; }
    public List<AccessLevel> Access { get; set; } = new();
    public List<Test>? CreatedTests { get; set; }

    public User()
    {
        if (Access.Count == 0)
            Access.Add(AccessLevel.Student);
    }

    public User(string userName) : this()
    {
        UserName = userName;
    }


    public override string ToString()
        => UserName ?? string.Empty;
}