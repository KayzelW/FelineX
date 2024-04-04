using Shared.DB.Classes.User;
using Shared.Models;

namespace Shared.Extensions;

public static partial class UserExtensions
{
    /// <summary>
    /// Возвращает UserDTO из User
    /// </summary>
    /// <param name="user"></param>
    /// <returns>UserDTO</returns>
    public static UserDto ToDTO(this User user)
    {
        return new UserDto()
        {
            Id = user.Id,
            UserName = user.UserName,
            AccessFlags = user.AccessFlags,
        };
    }

    /// <summary>
    /// Возвращает User из UserDTO
    /// </summary>
    /// <param name="dto"></param>
    /// <returns>User</returns>
    public static User FromDto(this UserDto dto)
    {
        return new User()
        {
            Id = dto.Id,
            UserName = dto.UserName,
            AccessFlags = dto.AccessFlags
        };
    }
}