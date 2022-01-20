using Catalog.Host.Models.Requests.Brand;
using Catalog.Host.Models.Response;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Controllers;

[ApiController]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogBrandController : ControllerBase
{
    private readonly ILogger<CatalogBrandController> _logger;
    private readonly ICatalogBrandService _catalogBrandService;

    public CatalogBrandController(
        ILogger<CatalogBrandController> logger,
        ICatalogBrandService catalogItemService)
    {
        _logger = logger;
        _catalogBrandService = catalogItemService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddItemResponse<int>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Create(CreateBrandRequest request)
    {
        var result = await _catalogBrandService.CreateAsync(request.Brand);

        return Ok(new AddItemResponse<int?>() { Id = result });
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update(UpdateBrandRequest request)
    {
        await _catalogBrandService.UpdateAsync(request.Id, request.Brand);

        return Ok();
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> Delete(DeleteBrandRequest request)
    {
        await _catalogBrandService.DeleteAsync(request.Id);

        return Ok();
    }
}