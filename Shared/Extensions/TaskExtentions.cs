

using Shared.DB.Classes.Test.Task;

namespace Shared.Extensions;

public static partial class TaskExtentions
{
    public static string TaskTypeToString(this InteractionType taskType)
    {
        return taskType switch
        {
            InteractionType.None => "Неизвестен",
            InteractionType.ShortStringTask => "Короткий текст",
            InteractionType.LongStringTask => "Длинный текст",
            InteractionType.OneVariantTask => "Один вариант ответа",
            InteractionType.ManyVariantsTask => "Несколько вариантов ответа",
            InteractionType.SqlQueryTask => "SQL запрос",
            _ => "Unknown"
        };
    }
}