using FloraLink.Application.Interfaces;
using FloraLink.Domain.Entities;
using FloraLink.Domain.Interfaces;

namespace FloraLink.Application.Services;

public class PlantTypeService : IPlantTypeService
{
    private readonly IPlantTypeRepository _repo;

    public PlantTypeService(IPlantTypeRepository repo)
    {
        _repo = repo;
    }

    public Task<IEnumerable<PlantType>> GetAllAsync() => _repo.GetAllAsync();
}
