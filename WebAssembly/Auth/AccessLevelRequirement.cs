using Microsoft.AspNetCore.Authorization;
using Shared.Types;

namespace WebAssembly.Auth;

public sealed class AccessLevelRequirement : IAuthorizationRequirement
{
    public uint RequiredLevel { get; }

    public AccessLevelRequirement(uint requiredLevel)
    {
        RequiredLevel = requiredLevel;
    }

    public AccessLevelRequirement(AccessLevel requiredLevel)
    {
        RequiredLevel = (uint)requiredLevel;
    }
}