using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DB.Classes.Test.Task;

public class VariableAnswer
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string StringAnswer { get; set; } = "";
    public bool Truthful { get; set; }

    public VariableAnswer()
    {
    }

    public VariableAnswer(string varAnswer) : this()
    {
        this.StringAnswer = varAnswer;
    }
}