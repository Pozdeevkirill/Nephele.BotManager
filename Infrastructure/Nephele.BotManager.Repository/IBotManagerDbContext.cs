using Microsoft.EntityFrameworkCore;
using Nephele.BotManager.Models.DomainModels;

namespace Nephele.BotManager.Repository;

public interface IBotManagerDbContext : IUnitOfWork<BotManagerDbContext>
{
    DbSet<BotOwner> BotOwner { get; }
    DbSet<BotInfo> BotInfo { get; }
}