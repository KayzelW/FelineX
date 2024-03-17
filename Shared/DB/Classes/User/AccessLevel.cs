namespace Shared.DB.Classes.User;

[Flags]
public enum AccessLevel: uint
{
    Student = 0,

    Teacher = 1 << 0,
        
    AllAccess = 1u << 31,
}