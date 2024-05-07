using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using Shared.DB.Classes.User;
using Shared.Models;

namespace Shared.Extensions;

public static partial class UserExtensions
{
    public static string HashPassword(string password)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }

    public static bool HasAccess(this uint access, AccessLevel level) => (access & (uint)level) != 0;
}