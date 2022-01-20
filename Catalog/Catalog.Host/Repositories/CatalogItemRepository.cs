using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;

namespace Catalog.Host.Repositories;

public class CatalogItemRepository : ICatalogItemRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CatalogItemRepository> _logger;

    public CatalogItemRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogItemRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<PaginatedItems<CatalogItem>> GetByPageAsync(
        int pageIndex,
        int pageSize,
        int? brandFilter,
        int? typeFilter)
    {
        IQueryable<CatalogItem> query = _dbContext.CatalogItems;

        if (brandFilter.HasValue)
        {
            query = query.Where(w => w.CatalogBrandId == brandFilter.Value);
        }

        if (typeFilter.HasValue)
        {
            query = query.Where(w => w.CatalogTypeId == typeFilter.Value);
        }

        var totalItems = await query.LongCountAsync();

        var itemsOnPage = await query.OrderBy(c => c.Name)
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedItems<CatalogItem>() { TotalCount = totalItems, Data = itemsOnPage };
    }

    public async Task<CatalogItem?> GetByIdAsync(int id)
    {
        var res = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .FirstOrDefaultAsync(ci => ci.Id == id);

        return res;
    }

    public async Task<IEnumerable<CatalogItem>> GetByBrandAsync(string brandTitle)
    {
        var res = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .Where(ci => ci.CatalogBrand.Brand == brandTitle)
            .ToListAsync();

        return res;
    }

    public async Task<IEnumerable<CatalogItem>> GetByTypeAsync(string typeTitle)
    {
        var res = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .Where(ci => ci.CatalogType.Type == typeTitle)
            .ToListAsync();

        return res;
    }

    public async Task<IEnumerable<CatalogBrand>> GetBrandsAsync()
    {
        var res = await _dbContext.CatalogBrands
            .ToListAsync();

        return res;
    }

    public async Task<IEnumerable<CatalogType>> GetTypesAsync()
    {
        var res = await _dbContext.CatalogTypes
            .ToListAsync();

        return res;
    }

    public async Task<int?> CreateAsync(
        string name,
        string description,
        decimal price,
        int availableStock,
        int catalogBrandId,
        int catalogTypeId,
        string pictureFileName)
    {
        var item = await _dbContext.CatalogItems.AddAsync(
            new CatalogItem
            {
                CatalogBrandId = catalogBrandId,
                CatalogTypeId = catalogTypeId,
                Description = description,
                Name = name,
                PictureFileName = pictureFileName,
                Price = price
            });

        await _dbContext.SaveChangesAsync();

        return item.Entity.Id;
    }

    public async Task<bool> UpdateAsync(
        int id,
        string name,
        string description,
        decimal price,
        int availableStock,
        int catalogBrandId,
        int catalogTypeId,
        string pictureFileName)
    {
        var item = await _dbContext.CatalogItems.FirstOrDefaultAsync(ci => ci.Id == id);

        if (item != null)
        {
            item.Name = name;
            item.Description = description;
            item.Price = price;
            item.AvailableStock = availableStock;
            item.CatalogBrandId = catalogBrandId;
            item.CatalogTypeId = catalogTypeId;
            item.PictureFileName = pictureFileName;

            _dbContext.CatalogItems.Update(item);
            await _dbContext.SaveChangesAsync();
        }

        return item != null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var item = await _dbContext.CatalogItems.FirstOrDefaultAsync(ci => ci.Id == id);

        if (item != null)
        {
            _dbContext.CatalogItems.Remove(item);
            await _dbContext.SaveChangesAsync();
        }

        return item != null;
    }
}