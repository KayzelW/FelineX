using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Auth;

public record AuthData
{
    [Required, DataType(DataType.Text)] public string UserName { get; init; }

    [Required, DataType(DataType.Password)]
    public string Password { get; init; }

    [Display(Name = "Remember me?")] public bool RememberMe { get; init; } = false;
}