using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Shared.Interfaces;
using Shared.Data.Test.Task;

namespace Shared.Data.Test;

public class UniqueTest : IInnerIdentity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [JsonIgnore] public ApplicationUser? Creator { get; set; }
    public string CreatorId { get; set; }
    public List<TestSettings> Settings { get; set; }
    [StringLength(100)] public string? TestName { get; set; }
    [Column(TypeName = "timestamp(6)")] public DateTime? CreationTime { get; set; } = DateTime.Now;
    public List<UniqueTask>? Tasks { get; set; } = [];

}