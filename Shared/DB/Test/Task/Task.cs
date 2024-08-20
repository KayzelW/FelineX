using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Shared.DB.Test.Task;
using Shared.DB.Interfaces;
using Shared.Types;

namespace Shared.DB.Test.Task;

public sealed partial class Task : ITask
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = new Guid();

    [StringLength(1000)] public string? Question { get; set; } = "";
    public TaskSettings Settings { get; set; }
    [ForeignKey(nameof(ThemeTask))] public List<ThemeTask>? Thematics { get; set; } = [];
    public InteractionType InteractionType { get; set; } = InteractionType.LongStringTask;
    public List<VariableAnswer>? VariableAnswers { get; set; } = [];
    [JsonIgnore] public User.User? Creator { get; set; }
    public Guid? CreatorId { get; set; }
    [JsonIgnore] public List<Test>? Tests { get; set; } = [];
    public DBMS? DatabaseType { get; set; }
    public List<string>? DataRows { get; set; }
    


    [NotMapped, JsonIgnore]
    public int CountVariables
    {
        get => VariableAnswers!.Count;
        set => FixCountVariables(value);
    }

    public bool IsShortStringTask() => this.InteractionType is
        InteractionType.ShortStringTask;

    public bool IsLongStringTask() => this.InteractionType is
        InteractionType.LongStringTask;

    public bool IsSqlTask() => this.InteractionType is
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
        Settings = new TaskSettings
        {
            SqlQueryInstall = "",
            SqlQueryCheck = ""
        };
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
        var countInTask = VariableAnswers!.Count;

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
            for (var i = countInTask; i < targetCount; i++)
            {
                VariableAnswers!.Add(new VariableAnswer());
            }

            return;
        }
    }
}