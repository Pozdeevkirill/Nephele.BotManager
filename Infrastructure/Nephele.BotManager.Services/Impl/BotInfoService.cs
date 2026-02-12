using Nephele.BotManager.Models.DomainModels;
using Nephele.BotManager.Models.HttpModels.Requests;
using Nephele.BotManager.Repository;
using Nephele.BotManager.Services.Interfaces;

namespace Nephele.BotManager.Services.Impl;

public class BotInfoService : IBotInfoService
{
    private readonly IBotManagerDbContext  _context;

    public BotInfoService(
        IBotManagerDbContext context)
    {
        _context = context;
    }
    
    public Guid CreateBotInfo(CreateBotRequest request)
    {
        //1.1. Проверить на существование 
        if (_context.BotInfo
            .Any(x => x.BotOwnerId == request.BotOwnerId && x.Name == request.Name))
            throw new Exception("Bot with name " + request.Name + " already exists");
        
        //1.2. Проверить наличие сервиса
        if (!_context.BotOwner.Any(x => x.Id == request.BotOwnerId))
            throw new Exception("Service with id = " + request.BotOwnerId + "not registered");

        var botInfo = new BotInfo(request.BotOwnerId, request.Name, request.Token);
        Guid resultId = Guid.Empty;
        _context.Execute(x =>
        {
            
            var botInto = x.BotInfo.Add(botInfo);
            resultId = botInto.Entity.Id;
        });
        
        return resultId;
    }
}