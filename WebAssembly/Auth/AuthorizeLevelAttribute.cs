using Shared.DB.Classes.User;

namespace WebAssembly.Auth;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class AuthorizeLevelAttribute : Attribute
{
    public uint RequiredLevel { get; }

    public AuthorizeLevelAttribute(uint requiredLevel)
    {
        RequiredLevel = requiredLevel;
    }

    public AuthorizeLevelAttribute(AccessLevel requiredLevel)
    {
        RequiredLevel = (uint)requiredLevel;
    }
}