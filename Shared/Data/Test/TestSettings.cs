using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Interfaces;
using Shared.Data.Test.Task;

namespace Shared.Data.Test;

public class TestSettings : IInnerIdentity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    public List<UserGroup>? TestGroups { get; set; } = [];
    public List<ThemeTask>? TasksThemes { get; set; } = [];
    public List<ApplicationUser>? TestUsers { get; set; } = [];
}