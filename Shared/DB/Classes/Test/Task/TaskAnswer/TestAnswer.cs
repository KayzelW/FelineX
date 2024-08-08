using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Shared.DB.Classes.Test.Task.TaskAnswer;

public class TestAnswer
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public User.User? Student { get; set; }
    public Guid? StudentId { get; set; }

    public Test? AnsweredTest { get; set; }
    public Guid? AnsweredTestId { get; set; }
    public List<TaskAnswer>? TaskAnswers { get; set; } = new List<TaskAnswer>();
    
    [Column(TypeName = "timestamp(6)")]
    public DateTime PassingDate { get; set; }
    
    public double Score { get; set; }
    
    public string FantomName { get; set; } = "";

    public TestAnswer()
    {
    }
}