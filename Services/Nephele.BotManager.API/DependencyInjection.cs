using Nephele.BotManager.Repository;

namespace Nephele.BotManager.API;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        #region Common
        services.AddScoped<IBotManagerDbContext>(provider => provider.GetRequiredService<BotManagerDbContext>());
        #endregion
        
        return services;
    }
}