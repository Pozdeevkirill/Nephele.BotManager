using System.ComponentModel.DataAnnotations;

namespace Nephele.BotManager.Models.HttpModels.Requests;

public class StartBotRequest
{
    [Required]
    public string Token { get; set; } = string.Empty;
    
    public string? BotId { get; set; }
}