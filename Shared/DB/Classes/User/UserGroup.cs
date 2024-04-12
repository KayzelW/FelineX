using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DB.Classes.User;

public class UserGroup
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public User? GroupCreator { get; set; }
    public List<User>? Students { get; set; }

    public UserGroup()
    {
        
    }
}