using Shared.Types;

namespace Shared.Extensions;

public static class TaskExtentions
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
    
    public static string DBMSTypeToString(this DBMS dbType)
    {
        return dbType switch
        {
            DBMS.None => "Неизвестен",
            DBMS.SqLite => "SQLite",
            DBMS.MySQL => "MySQL",
            DBMS.PostgreSQL => "PostgreSQL",
            _ => "Unknown"
        };
    }
}