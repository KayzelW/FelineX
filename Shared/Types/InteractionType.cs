﻿namespace Shared.Types;

public enum InteractionType
{
    None = 0,
    ShortStringTask = 1,
    LongStringTask = 2,
    OneVariantTask = 3,
    ManyVariantsTask = 4,
    SqlQueryTask = 5
}