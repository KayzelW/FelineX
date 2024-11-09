using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using Shared.Models;
using Shared.Types;

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

    public static bool HasAccess(AccessLevel required, AccessLevel userAccess)
    {
        if (userAccess == null)
            return false;

        return required == (uint)AccessLevel.Exists
               || (userAccess & required) == required;
    }

    public static bool HasAccess(uint required, AccessLevel userAccess)
    {
        if (userAccess == null)
            return false;

        return required == (uint)AccessLevel.Exists
               || ((uint)userAccess & required) == required;
        
    }
}