using Nephele.BotManager.Models.HttpModels.Requests;

namespace Nephele.BotManager.Services.Interfaces;

public interface IBotInfoService
{
    Guid CreateBotInfo(CreateBotRequest request);
}