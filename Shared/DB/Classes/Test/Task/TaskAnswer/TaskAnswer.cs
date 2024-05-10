using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MyTask = Shared.DB.Classes.Test.Task.Task;

namespace Shared.DB.Classes.Test.Task.TaskAnswer;

public class TaskAnswer
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [JsonIgnore] public User.User? Student { get; set; }
    public Guid? StudentId { get; set; }
    [JsonIgnore] public MyTask? AnsweredTask { get; set; }
    public Guid? AnsweredTaskId { get; set; }

    public List<VariableAnswer>? GotVariables { get; set; } = new List<VariableAnswer>();
    // public List<Guid>? GotVariableIds { get; set; } = new List<Guid>();

    public List<VariableAnswer>? MarkedVariables { get; set; } = new List<VariableAnswer>();
    // public List<Guid>? MarkedVariableIds { get; set; } = new List<Guid>();

    [MaxLength(1000)] public string? StringAnswer { get; set; } = "";

    public TaskAnswer()
    {
    }

    public TaskAnswer(Guid? userId, Task task)
    {
        StudentId = userId;
        AnsweredTaskId = task.Id;
        GotVariables = task.VariableAnswers;

        if (task.IsStringTask())
        {
            var varAns = task.VariableAnswers!.FirstOrDefault();
            if (varAns is not null)
            {
                this.StringAnswer = varAns.StringAnswer;
            }
        }
        else
        {
            foreach (var varAns in task.VariableAnswers!)
            {
                if (varAns.Truthful is true)
                {
                    this.MarkedVariables.Add(varAns);
                }
            }
        }
    }
}