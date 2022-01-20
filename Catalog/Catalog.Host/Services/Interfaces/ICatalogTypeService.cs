namespace Catalog.Host.Services.Interfaces;

public interface ICatalogTypeService
{
    Task<int?> CreateAsync(string title);
    Task<bool> UpdateAsync(int id, string title);
    Task<bool> DeleteAsync(int id);
}