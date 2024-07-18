using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using Shared.DB.Classes.User;
using Shared.Models;

namespace Shared.Extensions;

public static partial class UserExtensions
{
    public static async Task<string> HashPasswordAsync(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        var builder = new StringBuilder();

        foreach (var t in bytes)
        {
            builder.Append(t.ToString("x2"));
        }

        return builder.ToString();
    }

    public static bool HasAccess(this uint access, AccessLevel level) => (access & (uint)level) != 0;
}