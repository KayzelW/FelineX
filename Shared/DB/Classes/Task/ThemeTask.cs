using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DB.Classes.Task;

public class ThemeTask
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string? Theme { get; set; }
    public List<Task> Tasks { get; set; }
}