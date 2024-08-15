using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Shared.DB.Test.Task;
using OriginalTask = Shared.DB.Test.Task.Task;

namespace Shared.DB.Test.Answers;

public class TaskAnswer
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [JsonIgnore] public User.User? Student { get; set; }
    public Guid? StudentId { get; set; }
    [NotMapped] public OriginalTask? AnsweredTask { get; set; }
    public Guid? AnsweredTaskId { get; set; }
    public List<VariableAnswer>? MarkedVariables { get; set; } = [];
    // public List<Guid> MarkedVariableIds { get; set; } = new List<Guid>();
    [NotMapped, JsonIgnore] public TestAnswer? TestAnswer { get; set; }
    
    [StringLength(1000)] public string? StringAnswer { get; set; }
    [StringLength(1000)] public string? Result { get; set; } // For SQL

    public Guid TestAnswerId { get; set; }

    public bool IsFailedCheck { get; set; } // Logic was failed
    public bool IsSuccess { get; set; } // Check success and logic correctly end work
    [NotMapped] public bool IsCheckEnded { get; set; } = false; //Check operations ended


    public TaskAnswer()
    {
    }

    public TaskAnswer(Guid? userId, OriginalTask task) : this()
    {
        StudentId = userId;
        AnsweredTaskId = task.Id;

        if (task.IsLongStringTask() || task.IsShortStringTask() || task.IsSqlTask())
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
                    
                    MarkedVariables!.Add(varAns);
                }
            }
        }
    }

    public TaskAnswer(User.User? user, OriginalTask task) : this(user?.Id, task)
    {
    }
}