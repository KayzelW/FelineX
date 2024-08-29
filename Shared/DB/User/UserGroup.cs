using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.AccessControl;
using System.Text.Json.Serialization;
using Shared.Interfaces;

namespace Shared.DB.User;

public class UserGroup : IInnerIdentity<UserGroup>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [JsonIgnore]
    public User? GroupCreator { get; set; }
    public Guid? GroupCreatorId { get; set; }
    public List<User>? Students { get; set; }

    [StringLength(100)]public string GroupName { get; set; }

    public UserGroup()
    {
        
    }
}