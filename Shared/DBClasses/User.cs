using Microsoft.AspNetCore.Identity;

namespace Shared.DBClasses;

// Add profile data for application users by adding properties to the User class
public class User : IdentityUser
{
    public User() : base()
    {
        if (Access.Count == 0)
            Access.Add(AccessLevel.Student);
    }
    public List<AccessLevel> Access { get; set; } = new();
}