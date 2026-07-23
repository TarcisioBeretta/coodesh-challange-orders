using Microsoft.EntityFrameworkCore;
using OrderAccumulator.Infrastructure.Data.Entities;
using System.Reflection;

namespace OrderAccumulator.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<ExposureEntity> Exposures { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExposureEntity>().HasKey(e => e.Symbol);
        modelBuilder.Entity<OrderEntity>().HasKey(e => e.Id);
        
        base.OnModelCreating(modelBuilder);
    }
}
