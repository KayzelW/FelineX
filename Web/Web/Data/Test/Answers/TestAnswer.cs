using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Shared.Interfaces;

namespace Web.Data.Test.Answers;

public class TestAnswer : IInnerIdentity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; } = Guid.NewGuid();

    public ApplicationUser? Student { get; set; }
    public Guid? StudentId { get; set; }
    public UniqueTest? AnsweredTest { get; set; }
    public Guid? AnsweredTestId { get; set; }
    
    public List<TaskAnswer>? TaskAnswers { get; set; } = [];
    
    [Column(TypeName = "timestamp(6)")]
    public DateTime PassingDate { get; set; }
    
    public double Score { get; set; }
    [JsonIgnore, NotMapped] public double TaskWeight { get; set; } 
    [StringLength(100)] public string? FantomName { get; set; }
    public string ClientConnectionLog { get; set; }
}