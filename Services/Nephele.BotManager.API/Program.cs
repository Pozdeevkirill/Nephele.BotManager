using Nephele.BotManager.API;
using Nephele.BotManager.Logger;

var builder = WebApplication.CreateBuilder(args);

GlobalLogger.LogRelease("Application starting...");

// Создаем экземпляр Startup
var startup = new Startup();

// Передаем builder для конфигурации
startup.ConfigureServices(builder);

var app = builder.Build();

// Конфигурируем пайплайн
startup.Configure(app);

// Логирование при запуске
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
lifetime.ApplicationStarted.Register(() =>
{
    GlobalLogger.LogRelease("Application started");
});

app.Run();