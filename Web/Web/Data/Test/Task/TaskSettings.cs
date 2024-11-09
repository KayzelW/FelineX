using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Interfaces;

namespace Web.Data.Test.Task;

public class TaskSettings : IInnerIdentity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; } = Guid.NewGuid();

    public UniqueTask AssignedTask { get; set; }

    public string? SqlQueryInstall { get; set; } = "";
    public string? SqlQueryCheck { get; set; } = "";

    public TaskSettings()
    {
    }
}