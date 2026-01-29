using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nephele.BotManager.Models.DomainModels;

namespace Nephele.BotManager.Repository;

public class BotManagerCompiledModel
{
    public static IModel Instance = BuildModel();

    private static IModel BuildModel()
    {
        var builder = new ModelBuilder();

        builder.Entity<BotInfo>(entity =>
        {
            entity.ToTable("BotInfo");
            
            entity.AddEntityFieldMap();
            
            entity.Property(x => x.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasColumnType(DbTypes.StringLength(300));

            entity.Property(x => x.Token)
                .IsRequired()
                .HasColumnName("Token")
                .HasColumnType(DbTypes.StringLength(50));
            
            entity.Property(x => x.IsActive)
                .IsRequired()
                .HasColumnName("IsActive")
                .HasColumnType(DbTypes.Boolean);
            
            entity.Property(x => x.CreateDate)
                .HasColumnName("CreateDate")
                .HasColumnType(DbTypes.DateTime);

            entity.Property(x => x.DateStart)
                .HasColumnName("DateStart")
                .HasColumnType(DbTypes.DateTime);
        });
        
        return builder.FinalizeModel();
    }
}