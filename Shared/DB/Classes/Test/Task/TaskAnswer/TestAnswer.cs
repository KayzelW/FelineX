using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DB.Classes.Test.Task.TaskAnswer;

public class TestAnswer
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public User.User? Student { get; set; }
    public Test? AnsweredTest { get; set; }
    public List<TaskAnswer>? TaskAnswers { get; set; }
}