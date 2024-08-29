using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.DB.Test.Task;
using Shared.DB.User;
using Shared.DB.Test.Task;
using Shared.Interfaces;

namespace Shared.DB.Test;

public class TestSettings : IInnerIdentity<TestSettings>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public List<UserGroup>? TestGroups { get; set; } = [];
    public List<ThemeTask>? TasksThemes { get; set; } = [];
    public List<User.User>? TestUsers { get; set; } = [];

}