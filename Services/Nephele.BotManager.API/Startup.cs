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
        
        
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors("SSE");
        app.UseAuthentication();
        app.UseAuthorization();
        //app.UseMiddleware<ExceptionHandlingMiddleware>();
        //app.InitDb(); //Инициализация бд гос ключа

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        // Конфигурация Swagger
        app.UseSwagger();
        app.UseSwaggerUI();

    }
}