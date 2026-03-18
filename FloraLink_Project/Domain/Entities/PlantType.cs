namespace FloraLink.Domain.Entities;

public class PlantType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Ideal ranges
    public double MinMoisture { get; set; }
    public double MaxMoisture { get; set; }
    public double MinTemperature { get; set; }
    public double MaxTemperature { get; set; }

    // Critical threshold — triggers auto-watering
    public double CriticalMoistureThreshold { get; set; }

    public ICollection<Plant> Plants { get; set; } = new List<Plant>();
}
