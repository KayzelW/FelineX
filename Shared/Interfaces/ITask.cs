using Shared.DB.Test.Task;
using Shared.DB.Test.Task;

namespace Shared.DB.Interfaces;

public interface ITask
{
    Guid Id { get; }
    string Question { get; set; }
    public List<VariableAnswer> VariableAnswers { get; set; }
    public List<ThemeTask>? Thematics { get; set; }
}