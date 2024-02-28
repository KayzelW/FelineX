using Shared.DB.Classes;
using Shared.Models;

namespace Shared.Extensions;

public static partial class UserExtensions
{
    /// <summary>
    /// Возвращает UserDTO из User
    /// </summary>
    /// <param name="user"></param>
    /// <returns>UserDTO</returns>
    public static UserDTO ToDTO(this User user)
    {
        return new UserDTO()
        {
            Id = user.Id,
            UserName = user.UserName,
            Access = user.Access,
        };
    }

    /// <summary>
    /// Возвращает User из UserDTO
    /// </summary>
    /// <param name="dto"></param>
    /// <returns>User</returns>
    public static User FromDto(this UserDTO dto)
    {
        return new User()
        {
            Id = dto.Id,
            UserName = dto.UserName,
            Access = dto.Access
        };
    }
    
}