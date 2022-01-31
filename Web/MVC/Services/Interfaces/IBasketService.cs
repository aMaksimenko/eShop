namespace MVC.Services.Interfaces;

public interface IBasketService
{
    Task LogUserAsync();
    Task LogAnonymousAsync();
}