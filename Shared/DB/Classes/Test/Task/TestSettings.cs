using Shared.DB.Classes.User;

namespace Shared.DB.Classes.Test.Task;

public class TestSettings
{
    public Guid TestId { get; set; }
    public Test Test { get; set; }
    public List<UserGroup> TestGroups { get; set; }
    public List<ThemeTask> TasksThemes { get; set; }
}