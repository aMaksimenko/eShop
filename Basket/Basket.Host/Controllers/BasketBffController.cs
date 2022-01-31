using System.Net;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
[Route(ComponentDefaults.DefaultRoute)]
public class BasketBffController : ControllerBase
{
    private readonly ILogger<BasketBffController> _logger;

    public BasketBffController(ILogger<BasketBffController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public Task<IActionResult> LogUser()
    {
        _logger.LogInformation($"User Id {User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value}");

        return Task.FromResult<IActionResult>(Ok());
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public Task<IActionResult> LogAnonymous()
    {
        _logger.LogInformation("Any log information");

        return Task.FromResult<IActionResult>(Ok());
    }
}