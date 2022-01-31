using System.Net;
using Infrastructure;
using Infrastructure.Identity;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Basket.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowClientPolicy)]
[Scope("basket.basketitem")]
[Route(ComponentDefaults.DefaultRoute)]
public class BasketItemController : ControllerBase
{
    private readonly ILogger<BasketBffController> _logger;
    private readonly IInternalHttpClientService _internalHttpClientService;
    private readonly IOptions<AppSettings> _settings;

    public BasketItemController(
        ILogger<BasketBffController> logger,
        IInternalHttpClientService internalHttpClientService,
        IOptions<AppSettings> settings)
    {
        _logger = logger;
        _internalHttpClientService = internalHttpClientService;
        _settings = settings;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public Task<IActionResult> TestInternal()
    {
        _logger.LogInformation($"Sent request to {_settings.Value.CatalogUrl}/testInternal");
        _internalHttpClientService.SendAsync<object, object>(
            $"{_settings.Value.CatalogUrl}/testInternal",
            HttpMethod.Get,
            null);

        return Task.FromResult<IActionResult>(Ok());
    }
}