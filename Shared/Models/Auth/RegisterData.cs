using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Auth;

public record RegisterData
{
    [Required, DataType(DataType.Text)] 
    public string UserName { get; init; }
    [DataType(DataType.Text)] 
    public string? NormalizedUserName { get; init; }

    [Required, DataType(DataType.Password)]
    public string Password { get; init; }
    
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "Пароль и подтверждение пароля не совпадают.")]
    public string ConfirmPassword { get; set; } = "";
}