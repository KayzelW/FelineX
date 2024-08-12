using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Shared.DB.Test.Answers;

public class TestAnswer
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public User.User? Student { get; set; }
    public Guid? StudentId { get; set; }

    public Test? AnsweredTest { get; set; }
    public Guid? AnsweredTestId { get; set; }
    public List<TaskAnswer>? TaskAnswers { get; set; } = [];
    
    [Column(TypeName = "timestamp(6)")]
    public DateTime PassingDate { get; set; }
    
    public double Score { get; set; }
    [JsonIgnore, NotMapped] public int TaskWeight { get; set; } 
    [StringLength(100)] public string? FantomName { get; set; }
    public string ClientConnectionLog { get; set; }
}