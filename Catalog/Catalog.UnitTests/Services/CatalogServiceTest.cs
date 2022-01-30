using System.Threading;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;

namespace Catalog.UnitTests.Services;

public class CatalogServiceTest
{
    private readonly ICatalogService _catalogService;
    private readonly Mock<ICatalogItemRepository> _catalogItemRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ILogger<CatalogService>> _logger;

    public CatalogServiceTest()
    {
        _catalogItemRepository = new Mock<ICatalogItemRepository>();
        _mapper = new Mock<IMapper>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogService>>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(dbContextTransaction.Object);

        _catalogService = new CatalogService(
            _dbContextWrapper.Object,
            _logger.Object,
            _catalogItemRepository.Object,
            _mapper.Object);
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Success()
    {
        // arrange
        var testPageIndex = 0;
        var testPageSize = 4;
        var testTotalCount = 12;

        var pagingPaginatedItemsSuccess = new PaginatedItems<CatalogItem>()
        {
            Data = new List<CatalogItem>()
            {
                new CatalogItem()
                {
                    Name = "TestName",
                },
            },
            TotalCount = testTotalCount,
        };

        var catalogItemSuccess = new CatalogItem()
        {
            Name = "TestName"
        };

        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Name = "TestName"
        };

        _catalogItemRepository.Setup(
            s => s.GetByPageAsync(
                It.Is<int>(i => i == testPageIndex),
                It.Is<int>(i => i == testPageSize),
                It.IsAny<int?>(),
                It.IsAny<int?>())).ReturnsAsync(pagingPaginatedItemsSuccess);

        _mapper.Setup(s => s.Map<CatalogItemDto>(It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess))))
            .Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogItemsAsync(testPageSize, testPageIndex, null);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Failed()
    {
        // arrange
        var testPageIndex = 1000;
        var testPageSize = 10000;

        _catalogItemRepository.Setup(
            s => s.GetByPageAsync(
                It.Is<int>(i => i == testPageIndex),
                It.Is<int>(i => i == testPageSize),
                It.IsAny<int?>(),
                It.IsAny<int?>())).Returns((Func<PaginatedItemsResponse<CatalogItemDto>>)null!);

        // act
        var result = await _catalogService.GetCatalogItemsAsync(testPageSize, testPageIndex, null);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_Success()
    {
        // arrange
        var testId = 1;
        var catalogItem = new CatalogItem()
        {
            Name = "TestName"
        };

        var catalogItemDto = new CatalogItemDto()
        {
            Name = "TestName"
        };

        _catalogItemRepository.Setup(s => s.GetByIdAsync(testId))
            .ReturnsAsync(catalogItem);
        _mapper.Setup(s => s.Map<CatalogItemDto>(It.Is<CatalogItem>(i => i.Equals(catalogItem))))
            .Returns(catalogItemDto);

        // act
        var result = await _catalogService.GetByIdAsync(testId);

        // assert
        result.Should().NotBeNull();
        result.Should().Be(catalogItemDto);
    }

    [Fact]
    public async Task GetByIdAsync_Failed()
    {
        // arrange
        var testId = 1;
        CatalogItem? catalogItem = null;

        _catalogItemRepository.Setup(s => s.GetByIdAsync(testId))
            .ReturnsAsync(catalogItem);

        // act
        var result = await _catalogService.GetByIdAsync(testId).ConfigureAwait(false);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByBrandAsync_Success()
    {
        // arrange
        var brandTitle = "brandTitle";
        var catalogItems = new List<CatalogItem>()
        {
            new CatalogItem()
            {
                Name = "TestName",
            },
        };
        var catalogItem = new CatalogItem()
        {
            Name = "TestName",
        };
        var catalogItemDto = new CatalogItemDto()
        {
            Name = "TestName",
        };

        _catalogItemRepository.Setup(s => s.GetByBrandAsync(brandTitle))
            .ReturnsAsync(catalogItems);

        _mapper.Setup(s => s.Map<CatalogItemDto>(It.Is<CatalogItem>(i => i.Equals(catalogItem))))
            .Returns(catalogItemDto);

        // act
        var result = await _catalogService.GetByBrandAsync(brandTitle);

        // assert
        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetByBrandAsync_Failed()
    {
        // arrange
        var brandTitle = "brandTitle";
        var catalogItems = new List<CatalogItem>();
        var catalogItem = new CatalogItem()
        {
            Name = "TestName",
        };
        var catalogItemDto = new CatalogItemDto()
        {
            Name = "TestName",
        };

        _catalogItemRepository.Setup(s => s.GetByBrandAsync(brandTitle))
            .ReturnsAsync(catalogItems);
        _mapper.Setup(s => s.Map<CatalogItemDto>(It.Is<CatalogItem>(i => i.Equals(catalogItem))))
            .Returns(catalogItemDto);

        // act
        var result = await _catalogService.GetByBrandAsync(brandTitle);

        // assert
        result.Should().HaveCount(0);
    }

    [Fact]
    public async Task GetByTypeAsync_Success()
    {
        // arrange
        var typeTitle = "typeTitle";
        var catalogItems = new List<CatalogItem>()
        {
            new CatalogItem()
            {
                Name = "TestName",
            },
        };
        var catalogItem = new CatalogItem()
        {
            Name = "TestName",
        };
        var catalogItemDto = new CatalogItemDto()
        {
            Name = "TestName",
        };

        _catalogItemRepository.Setup(s => s.GetByTypeAsync(typeTitle))
            .ReturnsAsync(catalogItems);

        _mapper.Setup(s => s.Map<CatalogItemDto>(It.Is<CatalogItem>(i => i.Equals(catalogItem))))
            .Returns(catalogItemDto);

        // act
        var result = await _catalogService.GetByTypeAsync(typeTitle);

        // assert
        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetByTypeAsync_Failed()
    {
        // arrange
        var typeTitle = "typeTitle";
        var catalogItems = new List<CatalogItem>();

        _catalogItemRepository.Setup(s => s.GetByTypeAsync(typeTitle))
            .ReturnsAsync(catalogItems);

        // act
        var result = await _catalogService.GetByTypeAsync(typeTitle);

        // assert
        result.Should().HaveCount(0);
    }

    [Fact]
    public async Task GetBrandsAsync_Success()
    {
        // arrange
        var items = new List<CatalogBrand>()
        {
            new CatalogBrand()
            {
                Brand = "TestName",
            },
        };
        var item = new CatalogBrand()
        {
            Brand = "TestName",
        };
        var itemDto = new CatalogBrandDto()
        {
            Brand = "TestName",
        };

        _catalogItemRepository.Setup(s => s.GetBrandsAsync())
            .ReturnsAsync(items);

        _mapper.Setup(s => s.Map<CatalogBrandDto>(It.Is<CatalogBrand>(i => i.Equals(item))))
            .Returns(itemDto);

        // act
        var result = await _catalogService.GetBrandsAsync();

        // assert
        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetBrandsAsync_Failed()
    {
        // arrange
        var items = new List<CatalogBrand>();

        _catalogItemRepository.Setup(s => s.GetBrandsAsync())
            .ReturnsAsync(items);

        // act
        var result = await _catalogService.GetBrandsAsync();

        // assert
        result.Should().HaveCount(0);
    }

    [Fact]
    public async Task GetTypesAsync_Success()
    {
        // arrange
        var items = new List<CatalogType>()
        {
            new CatalogType()
            {
                Type = "TestName",
            },
        };
        var item = new CatalogType()
        {
            Type = "TestName",
        };
        var itemDto = new CatalogTypeDto()
        {
            Type = "TestName",
        };

        _catalogItemRepository.Setup(s => s.GetTypesAsync())
            .ReturnsAsync(items);

        _mapper.Setup(s => s.Map<CatalogTypeDto>(It.Is<CatalogBrand>(i => i.Equals(item))))
            .Returns(itemDto);

        // act
        var result = await _catalogService.GetTypesAsync();

        // assert
        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetTypesAsync_Failed()
    {
        // arrange
        var items = new List<CatalogType>();

        _catalogItemRepository.Setup(s => s.GetTypesAsync())
            .ReturnsAsync(items);

        // act
        var result = await _catalogService.GetTypesAsync();

        // assert
        result.Should().HaveCount(0);
    }
}