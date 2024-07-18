using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Shared.DB.Interfaces;

namespace Shared.DB.Classes.Test.Task;

public sealed class Task : ITask
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = new Guid();

    [MaxLength(1000)] public string? Question { get; set; } = "";
    [ForeignKey(nameof(ThemeTask))] public List<ThemeTask>? Thematics { get; set; } = new List<ThemeTask>();
    public InteractionType InteractionType { get; set; } = InteractionType.LongStringTask;
    public List<VariableAnswer>? VariableAnswers { get; set; } = new List<VariableAnswer>();
    [JsonIgnore] public User.User? Creator { get; set; }
    public Guid? CreatorId { get; set; }
    [JsonIgnore] public List<Test>? Tests { get; set; } = new List<Test>();


    [NotMapped, JsonIgnore]
    public int CountVariables
    {
        get => VariableAnswers!.Count;
        set => FixCountVariables(value);
    }

    public bool IsStringTask() => this.InteractionType is
        InteractionType.LongStringTask or
        InteractionType.ShortStringTask or
        InteractionType.SqlQueryTask;

    #region Constructors

    public Task()
    {
    }

    public Task(string? question, InteractionType interactionType) :
        this()
    {
        Question = question ?? "";
        InteractionType = interactionType;
    }

    public Task(string? question, InteractionType interactionType, params string[] answers) :
        this(question, interactionType)
    {
        foreach (var answer in answers)
        {
            VariableAnswers.Add(new VariableAnswer(answer));
        }
    }

    #endregion

    private void FixCountVariables(int targetCount)
    {
        int countInTask = VariableAnswers!.Count;

        if (targetCount <= 0)
        {
            VariableAnswers!.Clear();
            return;
        }

        if (countInTask > targetCount)
        {
            var tempArray = VariableAnswers!.ToArray();
            VariableAnswers.Clear();
            for (var i = 0; i < targetCount; i++)
            {
                VariableAnswers!.Add(tempArray[i]);
            }

            return;
        }

        if (countInTask < targetCount)
        {
            for (int i = countInTask; i < targetCount; i++)
            {
                VariableAnswers!.Add(new VariableAnswer());
            }

            return;
        }
    }
}