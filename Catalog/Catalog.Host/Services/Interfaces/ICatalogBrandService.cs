namespace Catalog.Host.Services.Interfaces;

public interface ICatalogBrandService
{
    Task<int?> CreateAsync(string title);
    Task<bool> UpdateAsync(int id, string title);
    Task<bool> DeleteAsync(int id);
}