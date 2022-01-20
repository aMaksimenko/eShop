using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories;

public class CatalogBrandRepository : ICatalogBrandRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CatalogBrandRepository> _logger;

    public CatalogBrandRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogBrandRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<int?> CreateAsync(string title)
    {
        var item = await _dbContext.CatalogBrands.AddAsync(
            new CatalogBrand()
            {
                Brand = title
            });

        await _dbContext.SaveChangesAsync();

        return item.Entity.Id;
    }

    public async Task<bool> UpdateAsync(int id, string title)
    {
        var item = await _dbContext.CatalogBrands.FirstOrDefaultAsync(ci => ci.Id == id);

        if (item != null)
        {
            item.Brand = title;
            _dbContext.CatalogBrands.Update(item);
            await _dbContext.SaveChangesAsync();
        }

        return item != null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var item = await _dbContext.CatalogBrands.FirstOrDefaultAsync(ci => ci.Id == id);

        if (item != null)
        {
            _dbContext.CatalogBrands.Remove(item);
            await _dbContext.SaveChangesAsync();
        }

        return item != null;
    }
}