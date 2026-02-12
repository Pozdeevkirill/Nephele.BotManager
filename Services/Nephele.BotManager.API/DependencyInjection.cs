using Nephele.BotManager.Repository;
using Nephele.BotManager.Services.Impl;
using Nephele.BotManager.Services.Interfaces;

namespace Nephele.BotManager.API;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        #region Common
        services.AddScoped<IBotManagerDbContext>(provider => provider.GetRequiredService<BotManagerDbContext>());
        #endregion
        
        services.AddSingleton<IBotService, BotService>();
        services.AddScoped<IBotInfoService, BotInfoService>();
        services.AddScoped<IBotOwnerService, BotOwnerService>();
        services.AddHostedService(provider => 
            (BotService)provider.GetRequiredService<IBotService>());
        
        return services;
    }
}