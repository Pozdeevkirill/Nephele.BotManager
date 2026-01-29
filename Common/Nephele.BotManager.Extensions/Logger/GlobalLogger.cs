using Microsoft.Extensions.Configuration;
using Nephele.BotManager.Extensions;
using Serilog;

namespace Nephele.BotManager.Logger;

public static class GlobalLogger
{
    static GlobalLogger()
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        var configurationRoot = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile(Path.Combine(ConfigurationBuilderExtension.GetSolutionItemDirectory(), "serilogConfig.json"), optional: false, reloadOnChange: true)
            .AddJsonFile(Path.Combine(ConfigurationBuilderExtension.GetSolutionItemDirectory(), $"serilogConfig.{env}.json"), optional: true)
            .AddEnvironmentVariables()
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configurationRoot)
            .Enrich.WithProperty("SourceContext", "Global")
            .Enrich.With(new ReleaseLevelEnricher(LoggerExtensions.ReleaseLevel, "Release"))
            .CreateLogger();
    }

    /// <summary>
    /// [REL]
    /// </summary>
    public static void LogRelease(string message) => Log.Write(LoggerExtensions.ReleaseLevel, message);
    
    /// <summary>
    /// [VRB] Аналог метода LogTrace из Microsoft.Extensions.Logging.
    /// </summary>
    public static void LogTrace(string message) => Log.Verbose(message);

    /// <summary>
    /// [DBG] Аналог метода LogDebug из Microsoft.Extensions.Logging.
    /// </summary>
    public static void LogDebug(string message) => Log.Debug(message);

    /// <summary>
    /// [INF] Аналог метода LogInformation из Microsoft.Extensions.Logging.
    /// </summary>
    public static void LogInformation(string message) => Log.Information(message);

    /// <summary>
    /// [WRN] Аналог метода LogWarning из Microsoft.Extensions.Logging.
    /// </summary>
    public static void LogWarning(string message) => Log.Warning(message);

    /// <summary>
    /// [ERR] Аналог метода LogError из Microsoft.Extensions.Logging.
    /// </summary>
    public static void LogError(string message) => Log.Error(message);

    /// <summary>
    /// [FTL] Аналог метода LogCritical из Microsoft.Extensions.Logging.
    /// </summary>
    public static void LogCritical(string message) => Log.Fatal(message);
}