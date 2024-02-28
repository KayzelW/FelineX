using System.ComponentModel.DataAnnotations.Schema;
using Shared.DB.Interfaces;

namespace Shared.DB.Classes.Task;

public class Task : ITask
{
    public Guid Id { get; protected set; }
    public string Question { get; set; }
    protected virtual List<(string, bool)> _variableAnswers { get; set; } = new();
    [ForeignKey(nameof(ThemeTask))] public ThemeTask Thematic { get; set; }

    public virtual List<(string, bool)> VariableAnswers
    {
        get => _variableAnswers;
        set => _variableAnswers = value;
    }

    #region Constructors

    protected Task()
    {
        Id = new Guid();
    }

    public Task(string? question) : this()
    {
        Question = question ?? "Вопрос задания";
    }

    public Task(string? question, params string[] answers) : this(question)
    {
        foreach (var answer in answers)
        {
            _variableAnswers.Add((answer, false));
        }
    }

    #endregion
}