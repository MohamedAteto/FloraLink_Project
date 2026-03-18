using FloraLink.Domain.Entities;

namespace FloraLink.Application.Interfaces;

public interface IPlantTypeService
{
    Task<IEnumerable<PlantType>> GetAllAsync();
}
