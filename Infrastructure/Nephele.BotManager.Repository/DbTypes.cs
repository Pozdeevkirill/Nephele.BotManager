using Nephele.BotManager.Settings;

namespace Nephele.BotManager.Repository;

public static class DbTypes
{
    private static bool isSqlServer = true;

    public static void Init()
    {
        isSqlServer = Global.Database.DBType == "sqlserver";
    }

    public static string DateTime
    {
        get
        {
            var value = isSqlServer
                ? "datetime2"
                : "timestamp without time zone";
            return value;
        }
    }

    public static string String
    {
        get
        {
            var value = isSqlServer
                ? "nvarchar(max)"
                : "character varying";
            return value;
        }
    }
        
    
    public static string StringLength(int length) => 
        isSqlServer
            ? $"nvarchar({length})"
            : $"character varying({length})";

    public static string Guid
    {
        get
        {
            var value = isSqlServer
                ? "uniqueidentifier"
                : "uuid";
            return value;
        }
    }
        

    public static string ByteArray
    {
        get
        {
            var value = isSqlServer
                ? "varbinary(max)"
                : "bytea";
            return value;
        }
    }
        
    
    public static string ByteArrayLength(int length) =>
        isSqlServer
            ? $"varbinary({length})"
            : "bytea";

    public static string Int64 = "bigint";

    public static string Int32 = "integer";

    public static string Int16 
    {
        get
        {
            var value =  isSqlServer
                ? "tinyint"
                : "smallint";
            return value;
        }
    }

    public static string GetDate 
    {
        get
        {
            var value = isSqlServer
                ? "getdate()"
                : "now()";
            return value;
        }
    }

    public static string TimeStamp
    {
        get
        {
            var value = isSqlServer
                ? "datetime"
                : "timestamp without time zone";
            
            return value;
        }
    }

    public static string Boolean
    {
        get
        {
            var value = isSqlServer
                ? "bit"
                : "boolean";
            return value;
        }
    }
}