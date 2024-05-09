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
    public List<VariableAnswer>? GotVariables { get; set; } = new List<VariableAnswer>(); // TODO: move to guid
    public List<VariableAnswer>? MarkedVariables { get; set; } = new List<VariableAnswer>();// TODO: move to guid
    [MaxLength(1000)] public string? StringAnswer { get; set; } = "";

    public TaskAnswer()
    {
    }
    
}