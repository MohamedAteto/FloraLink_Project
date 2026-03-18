using FloraLink.Application.Services;
using FloraLink.Domain.Entities;

namespace FloraLink.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static void Seed(FloraLinkDbContext db)
    {
        if (db.Users.Any()) return;

        var user = new User
        {
            Username = "demo",
            Email = "demo@floralink.io",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("demo1234"),
            CreatedAt = DateTime.UtcNow.AddDays(-60)
        };
        db.Users.Add(user);
        db.SaveChanges();

        var plantTypes = db.PlantTypes.ToList();
        PlantType TypeById(int id) => plantTypes.First(t => t.Id == id);

        var plants = new[]
        {
            new Plant { Name = "Sandy",    SensorId = "ESP32-001", PlantTypeId = 1, UserId = user.Id, CreatedAt = DateTime.UtcNow.AddDays(-30) },
            new Plant { Name = "Tropico",  SensorId = "ESP32-002", PlantTypeId = 2, UserId = user.Id, CreatedAt = DateTime.UtcNow.AddDays(-25) },
            new Plant { Name = "Basilio",  SensorId = "ESP32-003", PlantTypeId = 3, UserId = user.Id, CreatedAt = DateTime.UtcNow.AddDays(-20) },
            new Plant { Name = "Fernanda", SensorId = "ESP32-004", PlantTypeId = 4, UserId = user.Id, CreatedAt = DateTime.UtcNow.AddDays(-15) },
        };
        db.Plants.AddRange(plants);
        db.SaveChanges();

        var rng = new Random(42);
        foreach (var plant in plants)
        {
            SeedReadings(db, plant, TypeById(plant.PlantTypeId), rng);
            SeedWateringEvents(db, plant, rng);
            SeedDiaryEntries(db, plant, rng);
        }
        SeedAlerts(db, plants);
        db.SaveChanges();
    }

    private static void SeedReadings(FloraLinkDbContext db, Plant plant, PlantType pt, Random rng)
    {
        double moisture = rng.NextDouble() * (pt.MaxMoisture - pt.MinMoisture) + pt.MinMoisture;
        var readings = new List<SensorReading>();
        for (int i = 30 * 6; i >= 0; i--)
        {
            moisture -= rng.NextDouble() * 1.5;
            if (moisture < pt.CriticalMoistureThreshold + 5)
                moisture = pt.MinMoisture + rng.NextDouble() * 15;
            moisture = Math.Clamp(moisture, pt.CriticalMoistureThreshold - 2, pt.MaxMoisture + 5);
            double midTemp = (pt.MinTemperature + pt.MaxTemperature) / 2.0;
            double temp = midTemp + (rng.NextDouble() * 6 - 3);
            double health = HealthCalculator.Calculate(moisture, temp, pt);
            readings.Add(new SensorReading
            {
                PlantId      = plant.Id,
                SoilMoisture = Math.Round(moisture, 1),
                Temperature  = Math.Round(temp, 1),
                HealthScore  = Math.Round(health, 1),
                PlantStatus  = HealthCalculator.GetStatus(health),
                RecordedAt   = DateTime.UtcNow.AddHours(-i * 4)
            });
        }
        db.SensorReadings.AddRange(readings);
    }

    private static void SeedWateringEvents(FloraLinkDbContext db, Plant plant, Random rng)
    {
        var events = new List<WateringEvent>();
        for (int day = 28; day >= 0; day -= rng.Next(4, 7))
        {
            events.Add(new WateringEvent
            {
                PlantId       = plant.Id,
                WateredAt     = DateTime.UtcNow.AddDays(-day).AddHours(rng.Next(8, 20)),
                WaterAmountMl = 150 + rng.Next(0, 100),
                IsAutomatic   = rng.Next(2) == 0,
                Notes         = day % 2 == 0 ? "Routine watering" : null
            });
        }
        db.WateringEvents.AddRange(events);
    }

    private static readonly string[] DiaryNotes =
    {
        "Looking healthy today, new leaves sprouting!",
        "Noticed some yellowing on lower leaves.",
        "Repotted into a larger container.",
        "Added fertilizer, hope it helps.",
        "Growth is really picking up this week.",
        "Moved closer to the window for more light.",
        "Trimmed a few dead leaves.",
        "Plant looks very happy after watering!"
    };

    private static void SeedDiaryEntries(FloraLinkDbContext db, Plant plant, Random rng)
    {
        var entries = new List<PlantDiaryEntry>();
        for (int i = 0; i < 4; i++)
        {
            entries.Add(new PlantDiaryEntry
            {
                PlantId   = plant.Id,
                Notes     = DiaryNotes[rng.Next(DiaryNotes.Length)],
                PhotoUrl  = null,
                EntryDate = DateTime.UtcNow.AddDays(-rng.Next(1, 28))
            });
        }
        db.PlantDiaryEntries.AddRange(entries);
    }

    private static void SeedAlerts(FloraLinkDbContext db, Plant[] plants)
    {
        db.Alerts.AddRange(
            new Alert { PlantId = plants[1].Id, Message = $"{plants[1].Name} moisture below ideal range. Consider watering soon.", Severity = "Warning",  IsRead = false, CreatedAt = DateTime.UtcNow.AddHours(-2) },
            new Alert { PlantId = plants[2].Id, Message = $"{plants[2].Name} health score dropped to 55. Check soil moisture.",    Severity = "Warning",  IsRead = false, CreatedAt = DateTime.UtcNow.AddHours(-5) },
            new Alert { PlantId = plants[3].Id, Message = $"{plants[3].Name} soil moisture critically low! Water immediately.",    Severity = "Critical", IsRead = false, CreatedAt = DateTime.UtcNow.AddMinutes(-30) }
        );
    }
}
