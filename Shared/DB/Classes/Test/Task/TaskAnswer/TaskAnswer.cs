using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DB.Classes.Test.Task.TaskAnswer;

public class TaskAnswer
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public User.User? Student { get; set; }
    public Task? AnsweredTask { get; set; }
    public List<VariableAnswer>? GotVariables { get; set; }
    public List<VariableAnswer>? MarkedVariables { get; set; }

    public TaskAnswer()
    {
        
    }
}