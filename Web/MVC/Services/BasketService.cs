using MVC.Services.Interfaces;
using MVC.ViewModels;

namespace MVC.Services;

public class BasketService : IBasketService
{
    private readonly IOptions<AppSettings> _settings;
    private readonly IHttpClientService _httpClient;
    private readonly ILogger<CatalogService> _logger;

    public BasketService(IHttpClientService httpClient, ILogger<CatalogService> logger, IOptions<AppSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings;
        _logger = logger;
    }

    public Task LogUserAsync()
    {
        return _httpClient.SendAsync<object, string>(
            $"{_settings.Value.BasketUrl}/logUser",
            HttpMethod.Get,
            null);
    }

    public Task LogAnonymousAsync()
    {
        return _httpClient.SendAsync<object, string>(
            $"{_settings.Value.BasketUrl}/logAnonymous",
            HttpMethod.Get,
            null);
    }
}