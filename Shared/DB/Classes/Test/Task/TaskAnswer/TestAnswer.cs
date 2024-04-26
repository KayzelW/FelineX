using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Shared.DB.Classes.Test.Task.TaskAnswer;

public class TestAnswer
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [JsonIgnore] public User.User? Student { get; set; }
    public Guid? StudentId { get; set; }

    [JsonIgnore] public Test? AnsweredTest { get; set; }
    public Guid? AnsweredTestId { get; set; }
    public List<TaskAnswer>? TaskAnswers { get; set; } = new List<TaskAnswer>();

    public TestAnswer()
    {
    }
}