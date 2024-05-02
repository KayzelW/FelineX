using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Shared.DB.Classes.Test.Task.TaskAnswer;

public class TaskAnswer
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [JsonIgnore] public User.User? Student { get; set; }
    public Guid? StudentId { get; set; }
    [JsonIgnore] public Task? AnsweredTask { get; set; }
    public Guid? AnsweredTaskId { get; set; }
    public List<VariableAnswer>? GotVariables { get; set; } = new List<VariableAnswer>();
    public List<VariableAnswer>? MarkedVariables { get; set; } = new List<VariableAnswer>();
    [MaxLength(1000)] public string? StringAnswer = "";

    public TaskAnswer()
    {
    }
}