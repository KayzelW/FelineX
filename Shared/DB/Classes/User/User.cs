using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Shared.DB.Classes.User;

// Add profile data for application users by adding properties to the User class
public sealed class User
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), MaxLength(30)]
    public string Id { get; set; }

    public string? UserName { get; set; }
    public string? NormalizedUserName { get; set; }
    public string? PasswordHash { get; set; }
    public List<AccessLevel> Access { get; set; } = new();
    public List<Test>? CreatedTests { get; set; }

    public User()
    {
        Id = Guid.NewGuid().ToString();
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