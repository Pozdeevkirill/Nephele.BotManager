using Nephele.BotManager.Models.HttpModels.Requests;

namespace Nephele.BotManager.Services.Interfaces;

public interface IBotOwnerService
{
    Guid CreateService(CreateBotOwnerRequest request);
}