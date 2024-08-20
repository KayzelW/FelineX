using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.DB.Test.Task;

public class TaskSettings
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid TaskId { get; set; }
    public string? SqlQueryInstall { get; set; } = "";
    public string? SqlQueryCheck { get; set; } = "";

    public TaskSettings()
    {

    }
}