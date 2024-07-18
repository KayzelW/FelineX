using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DB.Classes.Test.Task;

public class ThemeTask
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = new Guid();

    [MaxLength(100)] public string? Theme { get; set; } = "";
    public List<Task>? Tasks { get; set; } = [];

    public ThemeTask()
    {
        
    }
}