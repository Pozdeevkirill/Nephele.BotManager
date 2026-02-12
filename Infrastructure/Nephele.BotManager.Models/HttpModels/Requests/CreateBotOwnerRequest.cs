namespace Nephele.BotManager.Models.HttpModels.Requests;

public class CreateBotOwnerRequest
{
    public string Name { get; set; }
    
    public string Endpoint { get; set; }
}