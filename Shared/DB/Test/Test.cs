using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Shared.Interfaces;

namespace Shared.DB.Test;

public class Test : IInnerIdentity<Test>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public TestSettings Settings { get; set; } = new TestSettings();
    public Guid SettingsId { get; set; } 

    [StringLength(100)] public string? TestName { get; set; }
    [JsonIgnore]
    public User.User? Creator { get; set; }
    public Guid? CreatorId { get; set; }
    [Column(TypeName = "timestamp(6)")]
    public DateTime? CreationTime { get; set; } = DateTime.Now;
    public List<Task.Task>? Tasks { get; set; } = [];
    
    public Test()
    {
    }
}