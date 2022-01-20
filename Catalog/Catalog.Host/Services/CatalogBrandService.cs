using Catalog.Host.Data;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class CatalogBrandService : BaseDataService<ApplicationDbContext>, ICatalogBrandService
{
    private readonly ICatalogBrandRepository _catalogBrandRepository;

    public CatalogBrandService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogBrandRepository catalogBrandRepository)
        : base(dbContextWrapper, logger)
    {
        _catalogBrandRepository = catalogBrandRepository;
    }

    public Task<int?> CreateAsync(string title)
    {
        return ExecuteSafe(() => _catalogBrandRepository.CreateAsync(title));
    }

    public Task<bool> UpdateAsync(int id, string title)
    {
        return ExecuteSafe(() => _catalogBrandRepository.UpdateAsync(id, title));
    }

    public Task<bool> DeleteAsync(int id)
    {
        return ExecuteSafe(() => _catalogBrandRepository.DeleteAsync(id));
    }
}