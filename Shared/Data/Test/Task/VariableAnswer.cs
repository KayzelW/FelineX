using System.ComponentModel.DataAnnotations.Schema;
using Shared.Interfaces;

namespace Web.Data.Test.Task;

public class VariableAnswer : IInnerIdentity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

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