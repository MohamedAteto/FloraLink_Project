using FloraLink.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FloraLink.Infrastructure.Data;

public class FloraLinkDbContext : DbContext
{
    public FloraLinkDbContext(DbContextOptions<FloraLinkDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Plant> Plants => Set<Plant>();
    public DbSet<PlantType> PlantTypes => Set<PlantType>();
    public DbSet<SensorReading> SensorReadings => Set<SensorReading>();
    public DbSet<WateringEvent> WateringEvents => Set<WateringEvent>();
    public DbSet<PlantDiaryEntry> PlantDiaryEntries => Set<PlantDiaryEntry>();
    public DbSet<Alert> Alerts => Set<Alert>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email).IsUnique();

        // Plant → User
        modelBuilder.Entity<Plant>()
            .HasOne(p => p.User)
            .WithMany(u => u.Plants)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Plant → PlantType
        modelBuilder.Entity<Plant>()
            .HasOne(p => p.PlantType)
            .WithMany(pt => pt.Plants)
            .HasForeignKey(p => p.PlantTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // SensorReading → Plant
        modelBuilder.Entity<SensorReading>()
            .HasOne(r => r.Plant)
            .WithMany(p => p.SensorReadings)
            .HasForeignKey(r => r.PlantId)
            .OnDelete(DeleteBehavior.Cascade);

        // WateringEvent → Plant
        modelBuilder.Entity<WateringEvent>()
            .HasOne(w => w.Plant)
            .WithMany(p => p.WateringEvents)
            .HasForeignKey(w => w.PlantId)
            .OnDelete(DeleteBehavior.Cascade);

        // PlantDiaryEntry → Plant
        modelBuilder.Entity<PlantDiaryEntry>()
            .HasOne(d => d.Plant)
            .WithMany(p => p.DiaryEntries)
            .HasForeignKey(d => d.PlantId)
            .OnDelete(DeleteBehavior.Cascade);

        // Alert → Plant
        modelBuilder.Entity<Alert>()
            .HasOne(a => a.Plant)
            .WithMany(p => p.Alerts)
            .HasForeignKey(a => a.PlantId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed plant types
        modelBuilder.Entity<PlantType>().HasData(
            new PlantType { Id = 1, Name = "Cactus", Description = "Desert succulent, very drought tolerant", MinMoisture = 20, MaxMoisture = 40, MinTemperature = 20, MaxTemperature = 35, CriticalMoistureThreshold = 10 },
            new PlantType { Id = 2, Name = "Tropical", Description = "Tropical plants needing high humidity", MinMoisture = 60, MaxMoisture = 80, MinTemperature = 18, MaxTemperature = 30, CriticalMoistureThreshold = 40 },
            new PlantType { Id = 3, Name = "Herb", Description = "Kitchen herbs like basil and mint", MinMoisture = 50, MaxMoisture = 70, MinTemperature = 15, MaxTemperature = 28, CriticalMoistureThreshold = 30 },
            new PlantType { Id = 4, Name = "Fern", Description = "Shade-loving moisture-hungry ferns", MinMoisture = 70, MaxMoisture = 90, MinTemperature = 15, MaxTemperature = 25, CriticalMoistureThreshold = 50 },
            new PlantType { Id = 5, Name = "Succulent", Description = "Drought-tolerant succulent plants", MinMoisture = 25, MaxMoisture = 50, MinTemperature = 15, MaxTemperature = 32, CriticalMoistureThreshold = 15 }
        );
    }
}
