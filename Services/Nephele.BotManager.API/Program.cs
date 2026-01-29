using Nephele.BotManager.API;
using Nephele.BotManager.Logger;

var builder = WebApplication.CreateBuilder(args);

GlobalLogger.LogRelease("Application starting...");

// Создаем экземпляр Startup
var startup = new Startup();

// Передаем builder для конфигурации
startup.ConfigureServices(builder);