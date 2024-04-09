using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DB.Classes.Test.Task;

public class VariableAnswer
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = new Guid();

    public string StringAnswer { get; set; } = "someVarAnswer";
    public bool Truthful = false;
    public Task? Task { get; set; }

    public VariableAnswer()
    {
    }

    public VariableAnswer(string varAnswer)
    {
        this.StringAnswer = varAnswer;
    }
}