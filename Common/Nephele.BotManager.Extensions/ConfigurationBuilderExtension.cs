using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nephele.BotManager.Logger;
using Nephele.BotManager.Settings;
using Serilog;
using LoggerExtensions = Nephele.BotManager.Logger.LoggerExtensions;

namespace Nephele.BotManager.Extensions;

public static class ConfigurationBuilderExtension
{
    #region consts
    private const string ERROR_EMPTY_PATH_MESSAGE =
        "Ошибка конфигурации приложения! Не удалось найти файл кофигурации.";

    private const string SOLUTION_EXTENSION = "*.sln";
    private const string DEFAULT_VERSION = "1.0.0";
    private const string SOLUTION_ITEMS_FOLDER = "Solution Items";

    private static bool _isTests = false;
    #endregion
    
    /// <summary>
    /// Общая конфигурация проекта<br/>
    /// !!! Всегда регистрируется первым !!!
    /// </summary>
    public static IHostApplicationBuilder AddCustomConfiguration(this IHostApplicationBuilder builder)
    {
        builder.Configuration.AddFileConfiguration();
        builder.Configuration.ConfigureSettingsClass();

        return builder;
    }
    
    /// <summary>
    /// Конфигурация логгера
    /// </summary>
    public static IHostApplicationBuilder AddLoggerConfiguration(this IHostApplicationBuilder builder)
    {
        try
        {
            var solutionDirectory = GetSolutionDirectory();
            builder.Configuration.AddJsonFile(
                Path.Combine(solutionDirectory.FullName, SOLUTION_ITEMS_FOLDER, "serilogConfig.json"),
                optional: false, reloadOnChange: true);
        }
        catch (Exception ex)
        {
            throw new Exception(ERROR_EMPTY_PATH_MESSAGE);
        }

        builder.Services.AddSerilog((services, lc) => lc
            .ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .Enrich.With(new ReleaseLevelEnricher(LoggerExtensions.ReleaseLevel, "Release"))
        );
        return builder;
    }
    
    /// <summary>
    /// Конфигурация сваггера
    /// </summary>
    /// <param name="title">Заголовок сваггера</param>
    public static IHostApplicationBuilder AddSwaggerConfiguration(this IHostApplicationBuilder builder, string title)
    {
        builder.Services.AddSwaggerGen(c =>
            {
                string appVersion = AssemblyVersion.FullVersion;

                c.SwaggerDoc("v1", new()
                {
                    Title = title,
                    Version = appVersion
                });
            }
        );
        return builder;
    }
    
    /// <summary>
    /// Добавить настройки конфигурации Cors
    /// </summary>
    public static IHostApplicationBuilder AddCorsConfiguration(this IHostApplicationBuilder builder, string policyName)
    {
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                policy =>
                {
                    policy
                        .WithOrigins("*")
                        .SetIsOriginAllowed((host) => true)
                        .WithMethods("*")
                        .WithHeaders("*");
                });
        });
        return builder;
    }
    
    #region Solution Directory
    public static string GetSolutionItemDirectory()
    {
        try
        {
            return Path.Combine(GetSolutionDirectory().FullName, SOLUTION_ITEMS_FOLDER);
        }
        catch (Exception ex)
        {
            throw new Exception(ERROR_EMPTY_PATH_MESSAGE);
        }

    }
    
    public static DirectoryInfo? GetSolutionDirectory()
    {
        var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        var solutionDirectory = isWindows
            ? TryGetSolutionDirectory()
            : TryGetSolutionDirectoryDocker(); 
        return solutionDirectory;
    }
    
    private static DirectoryInfo? TryGetSolutionDirectory()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (directory != null && !directory.GetFiles(SOLUTION_EXTENSION).Any())
        {
            directory = directory.Parent;
        }
        return directory;
    }
    
    private static DirectoryInfo? TryGetSolutionDirectoryDocker()
        => new DirectoryInfo(Directory.GetCurrentDirectory());
    
    #endregion
    
    #region private 
    private static void AddFileConfiguration(this IConfigurationBuilder builder)
    {
        var solutionDirectory = GetSolutionDirectory();

        if (solutionDirectory is null || string.IsNullOrEmpty(solutionDirectory.FullName))
        {
            throw new FileNotFoundException(ERROR_EMPTY_PATH_MESSAGE);
        }

        try
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower() ?? string.Empty;
    
            builder
                .AddYamlFile(Path.Combine(solutionDirectory.FullName, SOLUTION_ITEMS_FOLDER, "globalsettings.yml"), 
                    optional: false, reloadOnChange: true);

            if (!string.IsNullOrEmpty(environment))
            {
                builder.AddYamlFile(
                    Path.Combine(solutionDirectory.FullName, SOLUTION_ITEMS_FOLDER,
                        $"globalsettings.{environment}.yaml"),
                    optional: true, reloadOnChange: true);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ERROR_EMPTY_PATH_MESSAGE + " Exception: " + ex.Message);
        }
    }
    
    private static IConfigurationManager ConfigureSettingsClass(this IConfigurationManager configuration)
    {
        configuration.GetSection("common").Bind(Global.Common);
        Global.SetDatabase(configuration.GetSection("Database").GetValue<string>("Database"));
        return configuration;
    }
    #endregion
}