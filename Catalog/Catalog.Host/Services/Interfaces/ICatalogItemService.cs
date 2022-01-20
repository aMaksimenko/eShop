namespace Catalog.Host.Services.Interfaces;

public interface ICatalogItemService
{
    Task<int?> CreateAsync(
        string name,
        string description,
        decimal price,
        int availableStock,
        int catalogBrandId,
        int catalogTypeId,
        string pictureFileName);

    Task<bool> UpdateAsync(
        int id,
        string name,
        string description,
        decimal price,
        int availableStock,
        int catalogBrandId,
        int catalogTypeId,
        string pictureFileName);

    Task<bool> DeleteAsync(int id);
}