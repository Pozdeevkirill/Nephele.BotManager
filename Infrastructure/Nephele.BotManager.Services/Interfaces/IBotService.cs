using Nephele.BotManager.Models.ApplicationObjects;

namespace Nephele.BotManager.Services.Interfaces;

/// <summary>
/// Сервис ботов
/// </summary>
public interface IBotService
{
    /// <summary>
    /// Запускает бота
    /// </summary>
    /// <param name="token"></param>
    /// <param name="customBotId"></param>
    Task<BotInstance> StartBotAsync(string token, string? customBotId = null);
    
    /// <summary>
    /// Останавливает бота
    /// </summary>
    /// <param name="botId"></param>
    /// <returns></returns>
    Task<bool> StopBotAsync(string botId);
    
    /// <summary>
    /// Останавливает всех ботов
    /// </summary>
    /// <returns></returns>
    Task<bool> StopAllBotsAsync();
    
    /// <summary>
    /// Получает информациб о боте
    /// </summary>
    /// <param name="botId"></param>
    BotInstance? GetBot(string botId);
    
    /// <summary>
    /// Получает информацию о всех запущенных ботах
    /// </summary>
    /// <returns></returns>
    IEnumerable<BotInstance> GetAllBots();
    
    /// <summary>
    /// Получает статусы ботов
    /// </summary>
    /// <returns></returns>
    Dictionary<string, string> GetBotStatuses();
}