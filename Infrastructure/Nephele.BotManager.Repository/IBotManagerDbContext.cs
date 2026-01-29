using Microsoft.EntityFrameworkCore;
using Nephele.BotManager.Models.DomainModels;

namespace Nephele.BotManager.Repository;

public interface IBotManagerDbContext : IUnitOfWork<BotManagerDbContext>
{
    DbSet<BotInfo> BotInfo { get; }
}