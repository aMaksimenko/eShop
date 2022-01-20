using Catalog.Host.Data;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class CatalogTypeService : BaseDataService<ApplicationDbContext>, ICatalogTypeService
{
    private readonly ICatalogTypeRepository _catalogTypeRepository;

    public CatalogTypeService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogTypeRepository catalogTypeRepository)
        : base(dbContextWrapper, logger)
    {
        _catalogTypeRepository = catalogTypeRepository;
    }

    public Task<int?> CreateAsync(string title)
    {
        return ExecuteSafe(() => _catalogTypeRepository.CreateAsync(title));
    }

    public Task<bool> UpdateAsync(int id, string title)
    {
        return ExecuteSafe(() => _catalogTypeRepository.UpdateAsync(id, title));
    }

    public Task<bool> DeleteAsync(int id)
    {
        return ExecuteSafe(() => _catalogTypeRepository.DeleteAsync(id));
    }
}