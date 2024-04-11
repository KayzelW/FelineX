using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DB.Classes.Test;

public class Test
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; protected set; } = new Guid();

    [MaxLength(100)] public string? TestName { get; set; }
    public User.User Creator { get; set; }
    public DateTime CreationTime { get; set; } = DateTime.Now;
    public List<Task.Task>? Tasks { get; protected set; }
}