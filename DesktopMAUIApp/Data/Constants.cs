namespace DesktopMAUIApp.Data;

public static class Constants
{
    public const string DatabaseFilename = "AppSQLite.db3";

    public static string DatabasePath =>
        $"Data Source={Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename)}";

    public static string API_URL = "https://192.168.0.10:8081";
}