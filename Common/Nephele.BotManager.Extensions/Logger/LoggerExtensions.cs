using Serilog;
using Serilog.Events;

namespace Nephele.BotManager.Logger;

public static class LoggerExtensions
{
    public static readonly LogEventLevel ReleaseLevel = (LogEventLevel)11;
	
    public static void LogRelease(this ILogger logger, string messageTemplate, params object[] args)
    {
        string context = logger.GetType().GenericTypeArguments.Length > 0 ? logger.GetType().GenericTypeArguments[0].Name : "Global";
        // Use Serilog's Log.Logger to write with the custom level
        Log.Logger
            .ForContext("SourceContext", context)
            .Write(ReleaseLevel, messageTemplate, args);
    }

    public static void LogRelease(this ILogger logger, Exception exception, string messageTemplate, params object[] args)
    {
        string context = logger.GetType().GenericTypeArguments.Length > 0 ? logger.GetType().GenericTypeArguments[0].Name : "Global";
        Log.Logger
            .ForContext("SourceContext", context)
            .Write(ReleaseLevel, exception, messageTemplate, args);
    }
}