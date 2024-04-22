using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.DB.Interfaces;

namespace Shared.DB.Classes.Test.Task;

public sealed partial class Task : ITask
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [MaxLength(1000)] public string? Question { get; set; } = "";
    [ForeignKey(nameof(ThemeTask))] public List<ThemeTask>? Thematics { get; set; } = new List<ThemeTask>();
    public InteractionType InteractionType { get; set; } = InteractionType.LongStringTask;
    public List<VariableAnswer>? VariableAnswers { get; set; } = new List<VariableAnswer>();
    [ForeignKey(nameof(User.User))] public User.User? Creator { get; set; }

    [NotMapped]
    public int CountVariables
    {
        get => VariableAnswers!.Count;
        set => FixCountVariables(value);
    }

    #region Constructors

    public Task()
    {
    }

    public Task(string? question, InteractionType interactionType) :
        this()
    {
        Question = question ?? "Вопрос задания";
        InteractionType = interactionType;
    }

    public Task(string? question, InteractionType interactionType, params string[] answers) :
        this(question,
            interactionType)
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

        if (targetCount == 0)
        {
            VariableAnswers!.Clear();
            return;
        }

        if (countInTask > targetCount)
        {
            for (int i = countInTask; i > targetCount; i--)
            {
                VariableAnswers!.RemoveAt(i);
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