using Nephele.BotManager.Extensions;
using Nephele.BotManager.Repository;

namespace Nephele.BotManager.API;

public class Startup
{
    private const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

    public Startup()
    {
    }

    public void ConfigureServices(WebApplicationBuilder builder)
    {
        builder
            .AddCustomConfiguration()
            .AddLoggerConfiguration()
            .AddSwaggerConfiguration("Nephele.BotManager.API")
            .AddCorsConfiguration(MyAllowSpecificOrigins)
            ;

        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddDependencyInjection(); // Регистрация зависимостей
        
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
    }
    
    public void Configure(WebApplication app)
    {
        app.UseCors(MyAllowSpecificOrigins);
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseStaticFiles();
        //app.UseMiddleware<ExceptionHandlingMiddleware>();
        //app.InitDb(); //Инициализация бд гос ключа

        // Конфигурация Swagger
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}