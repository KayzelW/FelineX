using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Interfaces;

namespace Web.Data.Test.Task;

public class ThemeTask : IInnerIdentity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; } = Guid.NewGuid();

    [StringLength(100)] public string? Theme { get; set; } = "";
    public List<UniqueTask>? Tasks { get; set; } = [];

    public ThemeTask()
    {
    }
}