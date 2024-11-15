namespace DesktopMAUIApp.Data;

public static class Constants
{
    public const string DatabaseFilename = "AppSQLite.db3";

    public static string DatabasePath =>
        $"Data Source={Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename)}";

    public static string API_URL = "https://95.84.209.46:8081";
}