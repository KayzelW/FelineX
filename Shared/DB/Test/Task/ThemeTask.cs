using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Interfaces;

namespace Shared.DB.Test.Task;

public class ThemeTask : IInnerIdentity<ThemeTask>
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = new Guid();

    [StringLength(100)] public string? Theme { get; set; } = "";
    public List<Task>? Tasks { get; set; } = [];

    public ThemeTask()
    {
        
    }
}