using System.ComponentModel.DataAnnotations;
using Shared.DB.Classes;
using Shared.DB.Classes.User;
using Swashbuckle.AspNetCore.Annotations;

namespace Shared.Models;

public class UserDto
{
    [SwaggerSchema("Id пользователя")]
    public int Id { get; set; }
    [Required ,SwaggerSchema("Login пользователя")]
    public string? UserName { get; set; }
    public List<AccessLevel> Access { get; set; } = new();
}