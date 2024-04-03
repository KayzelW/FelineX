using Shared.DB.Classes.Task;

namespace Shared.DB.Interfaces;

public interface ITask
{
    int Id { get; }
    string Question { get; set; }
    public List<VariableAnswer> VariableAnswers { get; set; }
    public ThemeTask Thematic { get; set; }
}