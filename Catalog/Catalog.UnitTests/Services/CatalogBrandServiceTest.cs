using System.Threading;
using Catalog.Host.Data.Entities;

namespace Catalog.UnitTests.Services;

public class CatalogBrandServiceTest
{
    private readonly ICatalogBrandService _catalogBrandService;
    private readonly Mock<ICatalogBrandRepository> _catalogBrandRepository;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ILogger<CatalogBrandService>> _logger;

    private readonly CatalogBrand _testItem = new CatalogBrand()
    {
        Id = 1,
        Brand = "Brand"
    };

    public CatalogBrandServiceTest()
    {
        _catalogBrandRepository = new Mock<ICatalogBrandRepository>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogBrandService>>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(dbContextTransaction.Object);

        _catalogBrandService = new CatalogBrandService(
            _dbContextWrapper.Object,
            _logger.Object,
            _catalogBrandRepository.Object);
    }

    [Fact]
    public async Task CreateAsync_Success()
    {
        // arrange
        var testResult = 1;

        _catalogBrandRepository.Setup(s => s.CreateAsync(It.IsAny<string>()))
            .ReturnsAsync(testResult);

        // act
        var result = await _catalogBrandService.CreateAsync(_testItem.Brand);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task CreateAsync_Failed()
    {
        // arrange
        int? testResult = null;

        _catalogBrandRepository.Setup(s => s.CreateAsync(It.IsAny<string>()))
            .ReturnsAsync(testResult);

        // act
        var result = await _catalogBrandService.CreateAsync(_testItem.Brand);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateAsync_Success()
    {
        // arrange
        var testResult = true;

        _catalogBrandRepository.Setup(
            s => s.UpdateAsync(
                It.IsAny<int>(),
                It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogBrandService.UpdateAsync(_testItem.Id, _testItem.Brand);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateAsync_Failed()
    {
        // arrange
        var testResult = false;

        _catalogBrandRepository.Setup(
            s => s.UpdateAsync(
                It.IsAny<int>(),
                It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogBrandService.UpdateAsync(_testItem.Id, _testItem.Brand);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task DeleteAsync_Success()
    {
        // arrange
        var testResult = true;

        _catalogBrandRepository.Setup(s => s.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync(testResult);

        // act
        var result = await _catalogBrandService.DeleteAsync(_testItem.Id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task DeleteAsync_Failed()
    {
        // arrange
        var testResult = false;

        _catalogBrandRepository.Setup(s => s.DeleteAsync(It.IsAny<int>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogBrandService.DeleteAsync(_testItem.Id);

        // assert
        result.Should().Be(testResult);
    }
}