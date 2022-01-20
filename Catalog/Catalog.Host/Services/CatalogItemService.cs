using Catalog.Host.Data;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class CatalogItemService : BaseDataService<ApplicationDbContext>, ICatalogItemService
{
    private readonly ICatalogItemRepository _catalogItemRepository;

    public CatalogItemService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogItemRepository catalogItemRepository)
        : base(dbContextWrapper, logger)
    {
        _catalogItemRepository = catalogItemRepository;
    }

    public Task<int?> CreateAsync(
        string name,
        string description,
        decimal price,
        int availableStock,
        int catalogBrandId,
        int catalogTypeId,
        string pictureFileName)
    {
        return ExecuteSafe(
            () => _catalogItemRepository.CreateAsync(
                name,
                description,
                price,
                availableStock,
                catalogBrandId,
                catalogTypeId,
                pictureFileName));
    }

    public Task<bool> UpdateAsync(
        int id,
        string name,
        string description,
        decimal price,
        int availableStock,
        int catalogBrandId,
        int catalogTypeId,
        string pictureFileName)
    {
        return ExecuteSafe(
            () => _catalogItemRepository.UpdateAsync(
                id,
                name,
                description,
                price,
                availableStock,
                catalogBrandId,
                catalogTypeId,
                pictureFileName));
    }

    public Task<bool> DeleteAsync(int id)
    {
        return ExecuteSafe(() => _catalogItemRepository.DeleteAsync(id));
    }
}