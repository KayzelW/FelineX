using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.DB.Test.Task;
using Shared.DB.User;
using Shared.DB.Test.Task;

namespace Shared.DB.Test;

public class TestSettings
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public List<UserGroup> TestGroups { get; set; }
    public List<ThemeTask> TasksThemes { get; set; }
}