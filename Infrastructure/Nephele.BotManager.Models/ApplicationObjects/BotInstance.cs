using Telegram.Bot;

namespace Nephele.BotManager.Models.ApplicationObjects;

public class BotInstance
{
    public string BotId { get; set; } = Guid.NewGuid().ToString();
    public string Token { get; set; } = string.Empty;
    public string? Username { get; set; }
    public string Status { get; set; } = "Stopped";
    public DateTime? StartedAt { get; set; }
    public CancellationTokenSource? CancellationTokenSource { get; set; }
    public TelegramBotClient? BotClient { get; set; }
}