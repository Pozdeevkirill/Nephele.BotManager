using Serilog.Core;
using Serilog.Events;

namespace Nephele.BotManager.Logger;

public class ReleaseLevelEnricher : ILogEventEnricher
{
    private readonly LogEventLevel _level;
    private readonly string _levelDisplayName;

    public ReleaseLevelEnricher(LogEventLevel level, string levelDisplayName)
    {
        _level = level;
        _levelDisplayName = levelDisplayName;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.AddOrUpdateProperty(new LogEventProperty("CustomLevel_u3", new ScalarValue(
            logEvent.Level == _level
                ? FormatLevelName(_levelDisplayName)
                : FormatLevelName(logEvent.Level.ToString())
        )));

        logEvent.AddOrUpdateProperty(new LogEventProperty("CustomLevel", new ScalarValue(
            logEvent.Level == _level
                ? _levelDisplayName.ToUpperInvariant()
                : logEvent.Level.ToString().ToUpperInvariant()
        )));
    }

    private string FormatLevelName(string levelName)
    {
        if (levelName.Length <= 3) return levelName.ToUpperInvariant();

        return levelName.ToUpperInvariant().Substring(0, 3);
    }
}
