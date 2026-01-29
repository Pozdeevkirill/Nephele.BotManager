using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Nephele.BotManager.Models.DomainModels;

namespace Nephele.BotManager.Repository;

public class BotManagerDbContext : DbContext, IBotManagerDbContext
{
    private static readonly bool _useCompiledModel = 
        !RuntimeFeature.IsDynamicCodeSupported; // Use compiled model in AOT
    
    public BotManagerDbContext(DbContextOptions<BotManagerDbContext> options) 
        : base(options) { }
    
    public DbSet<BotInfo> BotInfo => Set<BotInfo>();
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (_useCompiledModel)
        {
            var model = BotManagerCompiledModel.Instance;
            modelBuilder.Model.SetProductVersion(model.GetProductVersion());
            modelBuilder.Model.AddAnnotations(model.GetAnnotations());
        }
        else
        {
            base.OnModelCreating(modelBuilder);
        }
    }
    
    public void Initialize()
    {
        if(!Database.Exist())
            Database.CreateDatabase();
        
        Database.MigrateDatabase();
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
    
    private IDbContextTransaction _currentTransaction;
    public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;
    public bool HasActiveTransaction => _currentTransaction != null;
    
    public IDbContextTransaction BeginTransaction()
    {
        if (_currentTransaction != null)
        {
            return null;
        }

        _currentTransaction = Database.BeginTransaction();

        return _currentTransaction;
    }

    public void Commit(IDbContextTransaction transaction)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            SaveChanges();
            transaction.Commit();
        }
        catch(Exception ex)
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null!;
            }
        }
    }

    private void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null!;
            }
        }
    }

    public void Execute(Action<BotManagerDbContext> action)
    {
        var transaction = BeginTransaction();
        
        action(this);

        Commit(transaction);
    }

}