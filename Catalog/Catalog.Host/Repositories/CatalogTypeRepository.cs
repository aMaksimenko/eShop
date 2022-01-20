using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Repositories;

public class CatalogTypeRepository : ICatalogTypeRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CatalogTypeRepository> _logger;

    public CatalogTypeRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogTypeRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<int?> CreateAsync(string title)
    {
        var item = await _dbContext.CatalogTypes.AddAsync(
            new CatalogType()
            {
                Type = title
            });

        await _dbContext.SaveChangesAsync();

        return item.Entity.Id;
    }

    public async Task<bool> UpdateAsync(int id, string title)
    {
        var item = await _dbContext.CatalogTypes.FirstOrDefaultAsync(ci => ci.Id == id);

        if (item != null)
        {
            item.Type = title;
            _dbContext.CatalogTypes.Update(item);
            await _dbContext.SaveChangesAsync();
        }

        return item != null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var item = await _dbContext.CatalogTypes.FirstOrDefaultAsync(ci => ci.Id == id);

        if (item != null)
        {
            _dbContext.CatalogTypes.Remove(item);
            await _dbContext.SaveChangesAsync();
        }

        return item != null;
    }
}