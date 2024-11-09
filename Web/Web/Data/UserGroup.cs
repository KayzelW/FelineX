using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Shared.Interfaces;

namespace Web.Data;

public class UserGroup : IInnerIdentity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; } = Guid.NewGuid();

    [JsonIgnore] public ApplicationUser? GroupCreator { get; set; }
    public Guid? GroupCreatorId { get; set; }

    public List<ApplicationUser>? Students { get; set; }

    [StringLength(100)] public string GroupName { get; set; }

    public UserGroup()
    {
    }
}