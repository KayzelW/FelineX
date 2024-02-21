using System.ComponentModel.DataAnnotations;
using Shared.DBClasses;
using Swashbuckle.AspNetCore.Annotations;

namespace Shared.Models;

public class UserDTO
{
    [SwaggerSchema("Id пользователя")]
    public string? Id { get; set; }
    [Required ,SwaggerSchema("Login пользователя")]
    public string? UserName { get; set; }
    public List<AccessLevel> Access { get; set; } = new();
}