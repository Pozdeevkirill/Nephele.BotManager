using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Nephele.BotManager.Repository;

public interface IUnitOfWork<out T> 
    where T : DbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    bool HasActiveTransaction { get; }
    IDbContextTransaction GetCurrentTransaction();
    IDbContextTransaction BeginTransaction();
    void Commit(IDbContextTransaction transaction);
    void Execute(Action<T> action);
}