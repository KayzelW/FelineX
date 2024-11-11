using System.Data;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging.Abstractions;
using Npgsql;
using Shared.Data.Test.Task;
using Shared.Interfaces;
using Shared.Types;
using Web.Services;

namespace Web.Extensions;

public static class TaskExtension
{
    private static ILogger<TestWarrior> _logger = new Logger<TestWarrior>(new NullLoggerFactory());

    public static async Task<DbConnection?> SetupConnection(UniqueTask task)
    {
        try
        {
            if (task.DatabaseType == null) return null;
            if (!TestWarrior.AvailableDBMS.TryGetValue(task.DatabaseType.Value, out var connString)) return null;

            DbConnection? connection = task.DatabaseType switch
            {
                DBMS.SqLite => new SqliteConnection(connString),
                // DBMS.MySQL => new MySqlConnection(connString),
                DBMS.PostgreSQL => new NpgsqlConnection(connString),
                _ => null
            };

            if (connection == null) return null;

            await connection.OpenAsync();
            var cmd = connection.CreateCommand();
            cmd.CommandText = task.Settings.SqlQueryInstall;
            await cmd.ExecuteNonQueryAsync();

            return connection;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Something were wrong while setup connection");
            return null;
        }
    }

    public static async Task<List<string>?> FetchQuery(UniqueTask task, DbConnection connection)
    {
        try
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = task.Settings.SqlQueryCheck;
            var reader = await cmd.ExecuteReaderAsync();
            var itemRows = ExtractItemRows(reader);

            await connection.CloseAsync();

            return itemRows;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Something were wrong while executing queries");
            return null;
        }
    }

    /// <summary>
    /// Extract item from rows as <see cref="string"/>!
    /// </summary>
    /// <param name="reader">List of <see cref="string"/></param>
    /// <returns></returns>
    public static List<string> ExtractItemRows(DbDataReader reader)
    {
        var dataTable = new DataTable();
        dataTable.Load(reader);

        var itemRows = new List<string>();
        foreach (DataRow row in dataTable.Rows)
        {
            if (row.ItemArray.All(x => x != null))
            {
                // Преобразуем все элементы в строки
                var stringValues = row.ItemArray.Select(x => x.ToString()).ToList();
                itemRows.AddRange(stringValues);
            }
        }

        return itemRows;
    }

    /// <summary>
    /// Inner exec of <see cref="SetupConnection"/> and <see cref="FetchQuery"/>
    /// </summary>
    /// <param name="task">Task from DB configured as SQLTask</param>
    /// <returns>List of <see cref="string"/></returns>
    public static async Task<List<string>?> SetupAndFetch(UniqueTask task)
    {
        try
        {
            var conn = await SetupConnection(task);
            if (conn != null)
            {
                return await FetchQuery(task, conn);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Something were wrong while setup/fetch task connection");
            return null;
        }

        return null;
    }

    public static void SyncList<T>(this ICollection<T>? existing, ICollection<T>? incoming)
        where T : IInnerIdentity //update inner fields Interface
    {
        if (existing == null)
        {
            return;
        }
    
        if (incoming == null)
        {
            existing.Clear();
        }
    
        foreach (var cur in existing.ToList())
        {
            if (!incoming.Any(x => x.Id == cur.Id))
            {
                existing.Remove(cur);
            }
        }
    
        (existing as List<T>)?.AddRange(incoming.Except(existing));
    }
}