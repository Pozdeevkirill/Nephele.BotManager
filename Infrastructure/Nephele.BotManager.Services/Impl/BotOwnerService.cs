using Nephele.BotManager.Models.DomainModels;
using Nephele.BotManager.Models.HttpModels.Requests;
using Nephele.BotManager.Repository;
using Nephele.BotManager.Services.Interfaces;

namespace Nephele.BotManager.Services.Impl;

public class BotOwnerService : IBotOwnerService
{
   private readonly IBotManagerDbContext _context;
   
   public BotOwnerService(
      IBotManagerDbContext context)
   {
      _context = context;
   }
   
   public Guid CreateService(CreateBotOwnerRequest request)
   {
      if(_context.BotOwner.Any(x => x.Name == request.Name))
         throw new Exception("Service with name " + request.Name + " already exists");
      
      var botOwner = new BotOwner(request.Name, request.Endpoint);
      Guid resultId = Guid.Empty;
      _context.Execute(x =>
      {
         var entity = x.Add(botOwner);
         resultId = entity.Entity.Id;
      });
      return resultId;
   }
}