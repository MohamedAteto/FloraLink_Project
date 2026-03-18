using FloraLink.Domain.Entities;
using FloraLink.Domain.Interfaces;
using FloraLink.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FloraLink.Infrastructure.Repositories;

public class PlantTypeRepository : IPlantTypeRepository
{
    private readonly FloraLinkDbContext _db;

    public PlantTypeRepository(FloraLinkDbContext db) => _db = db;

    public async Task<IEnumerable<PlantType>> GetAllAsync() =>
        await _db.PlantTypes.ToListAsync();

    public Task<PlantType?> GetByIdAsync(int id) =>
        _db.PlantTypes.FindAsync(id).AsTask();
}
