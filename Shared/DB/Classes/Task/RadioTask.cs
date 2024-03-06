namespace Shared.DB.Classes.Task;

public class RadioTask : Task
{
    public override List<(string, bool)> VariableAnswers
    {
        get => _variableAnswers;
        set
        {
            if (value.Select(x => x.Item2 == true).Count() > 1)
            {
                return;
            }

            _variableAnswers = value;
        }
    }


    #region Constructors

    public RadioTask(string question) : base(question)
    {
    }

    public RadioTask(string question, params string[] answers) : base(question, answers)
    {
    }

    #endregion
}