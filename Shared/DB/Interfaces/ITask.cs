using Shared.DB.Classes.Test.Task;

namespace Shared.DB.Interfaces;

public interface ITask
{
    Guid Id { get; }
    string Question { get; set; }
    public List<VariableAnswer> VariableAnswers { get; set; }
    public List<ThemeTask>? Thematics { get; set; }
}