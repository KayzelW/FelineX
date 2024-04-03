using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DB.Classes.Task;

public class VariableAnswer
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string StringAnswer { get; set; } = "someVarAnswer";

    public bool Selected = false;

    public VariableAnswer()
    {
    }

    public VariableAnswer(string varAnswer)
    {
        this.StringAnswer = varAnswer;
    }
}