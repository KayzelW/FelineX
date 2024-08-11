using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Shared.DB.User;

public class UserGroup
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [JsonIgnore]
    public User? GroupCreator { get; set; }
    public Guid? GroupCreatorId { get; set; }
    public List<User>? Students { get; set; }

    public UserGroup()
    {
        
    }
}