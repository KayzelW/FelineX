using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Shared.Data.Test.Task;
using Shared.Interfaces;
using Shared.Data.Test.Task;

namespace Shared.Data.Test.Answers;

public class TaskAnswer : IInnerIdentity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [JsonIgnore] public ApplicationUser? Student { get; set; }
    public UniqueTask? AnsweredTask { get; set; }
    [JsonIgnore] public TestAnswer? TestAnswer { get; set; }
    public string StudentId { get; set; }
    public Guid? AnsweredTaskId { get; set; }
    public Guid TestAnswerId { get; set; }

    public List<VariableAnswer>? MarkedVariables { get; set; } = [];

    // public List<Guid> MarkedVariableIds { get; set; } = new List<Guid>();

    [StringLength(1000)] public string? StringAnswer { get; set; }
    [StringLength(1000)] public string? Result { get; set; } // For SQL


    public bool IsFailedCheck { get; set; } // Logic was failed
    public bool IsSuccess { get; set; } // Check success and logic correctly end work
    public bool IsCheckEnded { get; set; } = false;


    public TaskAnswer()
    {
    }

    public TaskAnswer(string userId, UniqueTask uniqueTask) : this()
    {
        StudentId = userId;
        AnsweredTaskId = uniqueTask.Id;

        if (uniqueTask.IsLongStringTask() || uniqueTask.IsShortStringTask() || uniqueTask.IsSqlTask())
        {
            var varAns = uniqueTask.VariableAnswers!.FirstOrDefault();
            if (varAns is not null)
            {
                StringAnswer = varAns.StringAnswer;
            }
        }
        else
        {
            foreach (var varAns in uniqueTask.VariableAnswers!)
            {
                if (varAns.Truthful is true)
                {
                    MarkedVariables!.Add(varAns);
                }
            }
        }
    }

    public TaskAnswer(ApplicationUser user, UniqueTask uniqueTask) : this(user.Id, uniqueTask)
    {
    }
}