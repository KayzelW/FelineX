﻿namespace Shared.DB.Classes.User;

[Flags]
public enum AccessLevel : uint
{
    Exists = 0,
    Student = 1 << 0,
    Teacher = 1 << 1,
}