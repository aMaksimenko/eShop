using Catalog.Host.Data;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Enums;
using Catalog.Host.Models.Response;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class CatalogService : BaseDataService<ApplicationDbContext>, ICatalogService
{
    private readonly ICatalogItemRepository _catalogItemRepository;
    private readonly IMapper _mapper;

    public CatalogService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogItemRepository catalogItemRepository,
        IMapper mapper)
        : base(dbContextWrapper, logger)
    {
        _catalogItemRepository = catalogItemRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedItemsResponse<CatalogItemDto>?> GetCatalogItemsAsync(
        int pageSize,
        int pageIndex,
        Dictionary<CatalogTypeFilter, int>? filters)
    {
        return await ExecuteSafeAsync(
            async () =>
            {
                int? brandFilter = null;
                int? typeFilter = null;

                if (filters != null)
                {
                    if (filters.TryGetValue(CatalogTypeFilter.Brand, out var brand))
                    {
                        brandFilter = brand;
                    }

                    if (filters.TryGetValue(CatalogTypeFilter.Type, out var type))
                    {
                        typeFilter = type;
                    }
                }

                var result = await _catalogItemRepository.GetByPageAsync(pageIndex, pageSize, brandFilter, typeFilter);

                if (result == null)
                {
                    return null;
                }

                return new PaginatedItemsResponse<CatalogItemDto>()
                {
                    Count = result.TotalCount,
                    Data = result.Data.Select(s => _mapper.Map<CatalogItemDto>(s)).ToList(),
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
            });
    }

    public async Task<CatalogItemDto> GetByIdAsync(int id)
    {
        return await ExecuteSafeAsync(
            async () =>
            {
                var result = await _catalogItemRepository.GetByIdAsync(id);

                return _mapper.Map<CatalogItemDto>(result);
            });
    }

    public async Task<IEnumerable<CatalogItemDto>> GetByBrandAsync(string brandTitle)
    {
        return await ExecuteSafeAsync(
            async () =>
            {
                var result = await _catalogItemRepository.GetByBrandAsync(brandTitle);

                return result.Select(s => _mapper.Map<CatalogItemDto>(s)).ToList();
            });
    }

    public async Task<IEnumerable<CatalogItemDto>> GetByTypeAsync(string typeTitle)
    {
        return await ExecuteSafeAsync(
            async () =>
            {
                var result = await _catalogItemRepository.GetByTypeAsync(typeTitle);

                return result.Select(s => _mapper.Map<CatalogItemDto>(s)).ToList();
            });
    }

    public async Task<IEnumerable<CatalogBrandDto>> GetBrandsAsync()
    {
        return await ExecuteSafeAsync(
            async () =>
            {
                var result = await _catalogItemRepository.GetBrandsAsync();

                return result.Select(s => _mapper.Map<CatalogBrandDto>(s)).ToList();
            });
    }

    public async Task<IEnumerable<CatalogTypeDto>> GetTypesAsync()
    {
        return await ExecuteSafeAsync(
            async () =>
            {
                var result = await _catalogItemRepository.GetTypesAsync();

                return result.Select(s => _mapper.Map<CatalogTypeDto>(s)).ToList();
            });
    }
}