using System.Data.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nephele.BotManager.Logger;
using Nephele.BotManager.Models.DomainModels;
using Nephele.BotManager.Settings;

namespace Nephele.BotManager.Repository;

public static class DbExtension
{
    public static IHostApplicationBuilder AddDatabase(this IHostApplicationBuilder builder)
    {
        DbTypes.Init();
        builder.Services.AddDbContext<BotManagerDbContext>(x =>
        {
            x.EnableSensitiveDataLogging();
            if (Global.Database.DBType == "sqlserver")
            {
                x.UseSqlServer(Global.Database.ConnectionString,
                    y => y.MigrationsAssembly("Nephele.BotManager.Migrations.SqlServer")
                );
            }
            else
            {
                x.UseNpgsql(Global.Database.ConnectionString,
                    y => y.MigrationsAssembly("Nephele.BotManager.Migrations.PostgreSql")
                );
            }
            x.UseModel(BotManagerCompiledModel.Instance);
        });
        
        
        return builder;
    }
    
    public static void InitDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BotManagerDbContext>();
        try
        {
            GlobalLogger.LogRelease("Initializing database..");
            dbContext.Initialize();
        }
        catch(Exception ex)
        {
            throw new Exception("Error initializing database.", ex);
        }
    }
    
    public static bool Exist(this DatabaseFacade database)
    {
        GlobalLogger.LogRelease("Checking for existing database..");
        var result = database.CanConnect();
        GlobalLogger.LogRelease(result ? "Database existed!" : "Database not existed.");
        return result;
    }
    
    public static void CreateDatabase(this DatabaseFacade database)
    {
        GlobalLogger.LogRelease("Creating database..");
        string dbName = database.GetDatabaseName();
        if(string.IsNullOrWhiteSpace(dbName))
            throw new ConnectionAbortedException("Can't create a database because the database name is empty.");
        database.ExecuteSqlRawAsync($"CREATE DATABASE {dbName}");
        GlobalLogger.LogRelease("Database will be created!");
    }
    
    public static void MigrateDatabase(this DatabaseFacade database)
    {
        if (database.GetPendingMigrations().Any())
        {
            GlobalLogger.LogRelease("Applying pending migrations..");
            database.Migrate();
            GlobalLogger.LogRelease("Migrations applied successfully!");
        }
        else 
            GlobalLogger.LogRelease("No pending migrations found.");
    } 
    
    public static void AddEntityFieldMap<TEntity>(this EntityTypeBuilder<TEntity> entity)
        where TEntity : Entity
    {
        entity.HasKey(e => e.Id);

        entity.Property(x => x.Id)
            .HasColumnType(DbTypes.Guid)
            .ValueGeneratedOnAdd();
        
        entity.Property(x => x.Version)
            .IsRequired()
            .HasColumnType(DbTypes.DateTime)
            .IsConcurrencyToken()
            .HasDefaultValueSql(DbTypes.GetDate)
            .ValueGeneratedOnAddOrUpdate();
    }
    
    private static string GetDatabaseName(this DatabaseFacade database)
    {
        string connectionString = database.GetConnectionString();
        if (string.IsNullOrEmpty(connectionString))
            return null;

        var builder = new DbConnectionStringBuilder { ConnectionString = connectionString };

        // Check common keys for database name
        if (builder.TryGetValue("Database", out var dbName) && dbName != null)
            return dbName.ToString();
        if (builder.TryGetValue("Initial Catalog", out dbName) && dbName != null)
            return dbName.ToString();
        if (builder.TryGetValue("AttachDBFilename", out dbName) && dbName != null)
            return System.IO.Path.GetFileNameWithoutExtension(dbName.ToString());
        if (builder.TryGetValue("Data Source", out dbName) && dbName != null)
            return System.IO.Path.GetFileNameWithoutExtension(dbName.ToString());

        return null; // Database name not found
    }
}