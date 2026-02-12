namespace Nephele.BotManager.Models.HttpModels.Requests;

public class CreateBotRequest
{
    public Guid BotOwnerId { get; set; }
    public string Name { get; set; }
    public string Token { get; set; }
}