using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.DB.Interfaces;

namespace Shared.DB.Classes;

public class Test
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; protected set; }

    public User.User Creator { get; set; }
    public List<Task.Task> Tasks { get; protected set; }
}