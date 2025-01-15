using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Shared.Interfaces;

namespace Shared.Data.Test.Task;

public class TaskSettings : IInnerIdentity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();
    public UniqueTask AssignedTask { get; set; }

    public string? SqlQueryInstall { get; set; } = "";
    public string? SqlQueryCheck { get; set; } = "";

    public TaskSettings()
    {
    }
}