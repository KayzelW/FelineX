using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Shared.DB.Test;

public class Test
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public TestSettings Settings { get; set; }
    public Guid SettingsId { get; set; }

    [StringLength(100)] public string? TestName { get; set; }
    [JsonIgnore]
    public User.User? Creator { get; set; }
    public Guid? CreatorId { get; set; }
    [Column(TypeName = "timestamp(6)")]
    public DateTime? CreationTime { get; set; } = DateTime.Now;
    public List<DB.Test.Task.Task>? Tasks { get; set; } = [];
    
    public Test()
    {
    }
}