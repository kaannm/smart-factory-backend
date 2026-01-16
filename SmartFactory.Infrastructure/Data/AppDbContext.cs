using Microsoft.EntityFrameworkCore;
using SmartFactory.Domain.Entities;

namespace SmartFactory.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<ProductionLine> ProductionLines { get; set; }
    public DbSet<ProductionRecord> ProductionRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<ProductionLine>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<ProductionRecord>().HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<ProductionLine>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<ProductionRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.ProductionLine)
                  .WithMany()
                  .HasForeignKey(e => e.ProductionLineId);
        });
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedDate = DateTime.UtcNow;
                    break;
            }
        }
    }
}

