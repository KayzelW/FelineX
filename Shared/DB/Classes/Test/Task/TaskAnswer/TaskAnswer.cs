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
    public MyTask? AnsweredTask { get; set; }
    public Guid? AnsweredTaskId { get; set; }
    
    public List<VariableAnswer>? MarkedVariables { get; set; } = new List<VariableAnswer>();
    // public List<Guid>? MarkedVariableIds { get; set; } = new List<Guid>();

    [MaxLength(1000)] public string? StringAnswer { get; set; } = "";

    public TaskAnswer()
    {
    }

    public TaskAnswer(Guid? userId, MyTask task)
    {
        StudentId = userId;
        AnsweredTaskId = task.Id;
        
        if (task.IsStringTask() || task.IsShortStringTask())
        {
            var varAns = task.VariableAnswers!.FirstOrDefault();
            if (varAns is not null)
            {
                StringAnswer = varAns.StringAnswer;
            }
        }
        else
        {
            foreach (var varAns in task.VariableAnswers!)
            {
                if (varAns.Truthful is true)
                {
                    MarkedVariables.Add(varAns);
                }
            }
        }
    }

    public TaskAnswer(User.User? user, MyTask task):this(user?.Id, task){}
}