namespace Catalog.Host.Repositories.Interfaces;

public interface ICatalogTypeRepository
{
    Task<int?> CreateAsync(string title);
    Task<bool> UpdateAsync(int id, string title);
    Task<bool> DeleteAsync(int id);
}