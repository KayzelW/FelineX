using Shared.DB.User;

namespace Shared.Attributes;

[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
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

