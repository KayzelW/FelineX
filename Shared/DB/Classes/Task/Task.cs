using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.DB.Interfaces;

namespace Shared.DB.Classes.Task;

public sealed class Task : ITask
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; private set; }

    [MaxLength(1000)] public string Question { get; set; }
    [ForeignKey(nameof(ThemeTask))] public ThemeTask Thematic { get; set; }
    public InteractionType InteractionType { get; set; }
    public List<VariableAnswer> VariableAnswers { get; set; }
    [ForeignKey(nameof(User.User))] public User.User Creator { get; set; }

    #region Constructors

    private Task()
    {
        Id = new Guid();
    }

    public Task(string? question, InteractionType interactionType) : this()
    {
        Question = question ?? "Вопрос задания";
        InteractionType = interactionType;
    }

    public Task(string? question, InteractionType interactionType, params string[] answers) : this(question,
        interactionType)
    {
        foreach (var answer in answers)
        {
            VariableAnswers.Add(new VariableAnswer(answer));
        }
    }

    #endregion
}