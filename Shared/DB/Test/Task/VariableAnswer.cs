using System.ComponentModel.DataAnnotations.Schema;
using Shared.Interfaces;

namespace Shared.DB.Test.Task;

public class VariableAnswer : IInnerIdentity<VariableAnswer>
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string? StringAnswer { get; set; } = "";
    public bool? Truthful { get; set; } = false;

    public VariableAnswer()
    {
    }

    public VariableAnswer(string varAnswer) : this()
    {
        this.StringAnswer = varAnswer;
    }
}