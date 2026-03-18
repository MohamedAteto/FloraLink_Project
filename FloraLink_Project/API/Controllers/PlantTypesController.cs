using FloraLink.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FloraLink.API.Controllers;

[ApiController]
[Route("api/plant-types")]
public class PlantTypesController : ControllerBase
{
    private readonly IPlantTypeService _service;

    public PlantTypesController(IPlantTypeService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _service.GetAllAsync());
}
