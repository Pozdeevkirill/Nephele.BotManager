using System.Collections.Concurrent;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nephele.BotManager.Models.ApplicationObjects;
using Nephele.BotManager.Services.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Nephele.BotManager.Services.Impl;

public class BotService(
    ILogger<BotService> logger,
    IServiceProvider serviceProvider)
    : IHostedService, IBotService
{
    private readonly ConcurrentDictionary<string, BotInstance> _activeBots = new();
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<BotInstance> StartBotAsync(string token, string? customBotId = null)
    {
        var botId = customBotId ?? Guid.NewGuid().ToString();
        
        if (_activeBots.TryGetValue(botId, out var existingBot))
        {
            logger.LogWarning("Бот с ID {BotId} уже запущен", botId);
            existingBot.Status = "Already running";
            return existingBot;
        }

        var botInstance = new BotInstance
        {
            BotId = botId,
            Token = token,
            Status = "Starting",
            StartedAt = DateTime.UtcNow
        };

        try
        {
            var cts = new CancellationTokenSource();
            var bot = new TelegramBotClient(token, cancellationToken: cts.Token);
            
            // Получаем информацию о боте
            var me = await bot.GetMe(cts.Token);
            botInstance.Username = me.Username;
            botInstance.BotClient = bot;
            botInstance.CancellationTokenSource = cts;

            // Подписываемся на события
            bot.OnMessage += (msg, type) => OnMessageHandler(botId, msg, type);
            bot.OnUpdate += (update) => OnUpdateHandler(botId, update);
            bot.OnError += (ex, type) => OnErrorHandler(botId, ex, type);

            botInstance.Status = "Running";
            _activeBots[botId] = botInstance;

            logger.LogInformation("Бот {@BotName} запущен с ID {BotId}", me.Username, botId);
            
            return botInstance;
        }
        catch (Exception ex)
        {
            botInstance.Status = $"Error: {ex.Message}";
            logger.LogError(ex, "Ошибка при запуске бота с ID {BotId}", botId);
            throw new ApplicationException($"Не удалось запустить бота: {ex.Message}", ex);
        }
    }
    
    public async Task<bool> StopBotAsync(string botId)
    {
        if (!_activeBots.TryRemove(botId, out var botInstance))
        {
            logger.LogWarning("Бот с ID {BotId} не найден", botId);
            return false;
        }

        try
        {
            botInstance.CancellationTokenSource?.Cancel();
            botInstance.CancellationTokenSource?.Dispose();
            
            botInstance.Status = "Stopped";
            logger.LogInformation("Бот {BotId} остановлен", botId);
            
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при остановке бота {BotId}", botId);
            return false;
        }
    }

    public async Task<bool> StopAllBotsAsync()
    {
        var botIds = _activeBots.Keys.ToList();
        var results = new List<bool>();

        foreach (var botId in botIds)
        {
            results.Add(await StopBotAsync(botId));
        }

        return results.All(r => r);
    }

    public BotInstance? GetBot(string botId)
    {
        _activeBots.TryGetValue(botId, out var botInstance);
        return botInstance;
    }

    public IEnumerable<BotInstance> GetAllBots()
    {
        return _activeBots.Values;
    }

    public Dictionary<string, string> GetBotStatuses()
    {
        return _activeBots.ToDictionary(
            kvp => kvp.Key,
            kvp => $"{kvp.Value.Username} - {kvp.Value.Status}"
        );
    }

    // Обработчики событий для каждого бота
    private async Task OnMessageHandler(string botId, Message msg, UpdateType type)
    {
        if (msg.Text is null) return;
        
        logger.LogInformation("Бот {BotId}: Получено сообщение '{Text}' от {From}",
            botId, msg.Text, msg.From?.Username ?? "unknown");
        
        if (_activeBots.TryGetValue(botId, out var botInstance) && botInstance.BotClient != null)
        {
            try
            {
                // Пример обработки сообщения
                await botInstance.BotClient.SendMessage(
                    chatId: msg.Chat.Id,
                    text: $"Эхо: {msg.Text}",
                    cancellationToken: botInstance.CancellationTokenSource?.Token ?? default);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при отправке ответа ботом {BotId}", botId);
            }
        }
    }

    private Task OnUpdateHandler(string botId, Update update)
    {
        logger.LogDebug("Бот {BotId}: Обновление типа {Type}", botId, update.Type);
        return Task.CompletedTask;
    }

    private Task OnErrorHandler(string botId, Exception exception, HandleErrorSource? updateType)
    {
        logger.LogError(exception, "Бот {BotId}: Ошибка при обработке обновления {Type}", 
            botId, updateType?.ToString() ?? "unknown");
        return Task.CompletedTask;
    }

    // Методы жизненного цикла
    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Сервис управления ботами запущен");
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Останавливаем всех ботов...");
        await StopAllBotsAsync();
        logger.LogInformation("Сервис управления ботами остановлен");
    }
}