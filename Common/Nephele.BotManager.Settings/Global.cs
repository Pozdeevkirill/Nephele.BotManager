namespace Nephele.BotManager.Settings;

public static class Global
{
    public static DBSettings Database { get; set; } = new();
    public static CommonSettings Common { get; set; } = new();
    
    public static void SetDatabase(string database)
    {
        Database.ConnectionString = database;
        Database.DBType = database.Contains("Port=") ? "pgsql" : "sqlserver";
    } 
    
}

public class DBSettings
{
    public string DBType { get; set; }

    public string ConnectionString { get; set; }
}

public class CommonSettings
{
    public CommonSettings()
    {
        DateStart = DateTime.Now;
    }

    /// <summary>
    /// Дата запуска сервиса
    /// </summary>
    public DateTime DateStart { get; set; }
}