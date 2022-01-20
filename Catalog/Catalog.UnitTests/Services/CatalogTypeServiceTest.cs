using Catalog.Host.Data.Entities;

namespace Catalog.UnitTests.Services;

public class CatalogTypeServiceTest
{
    private readonly ICatalogTypeService _catalogTypeService;
    private readonly Mock<ICatalogTypeRepository> _catalogTypeRepository;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ILogger<CatalogTypeService>> _logger;

    private readonly CatalogType _testItem = new CatalogType()
    {
        Id = 1,
        Type = "Type"
    };

    public CatalogTypeServiceTest()
    {
        _catalogTypeRepository = new Mock<ICatalogTypeRepository>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogTypeService>>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransaction()).Returns(dbContextTransaction.Object);

        _catalogTypeService = new CatalogTypeService(
            _dbContextWrapper.Object,
            _logger.Object,
            _catalogTypeRepository.Object);
    }

    [Fact]
    public async Task CreateAsync_Success()
    {
        // arrange
        var testResult = 1;

        _catalogTypeRepository.Setup(s => s.CreateAsync(It.IsAny<string>()))
            .ReturnsAsync(testResult);

        // act
        var result = await _catalogTypeService.CreateAsync(_testItem.Type);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task CreateAsync_Failed()
    {
        // arrange
        int? testResult = null;

        _catalogTypeRepository.Setup(s => s.CreateAsync(It.IsAny<string>()))
            .ReturnsAsync(testResult);

        // act
        var result = await _catalogTypeService.CreateAsync(_testItem.Type);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateAsync_Success()
    {
        // arrange
        var testResult = true;

        _catalogTypeRepository.Setup(
            s => s.UpdateAsync(
                It.IsAny<int>(),
                It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogTypeService.UpdateAsync(_testItem.Id, _testItem.Type);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateAsync_Failed()
    {
        // arrange
        var testResult = false;

        _catalogTypeRepository.Setup(
            s => s.UpdateAsync(
                It.IsAny<int>(),
                It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogTypeService.UpdateAsync(_testItem.Id, _testItem.Type);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task DeleteAsync_Success()
    {
        // arrange
        var testResult = true;

        _catalogTypeRepository.Setup(s => s.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync(testResult);

        // act
        var result = await _catalogTypeService.DeleteAsync(_testItem.Id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task DeleteAsync_Failed()
    {
        // arrange
        var testResult = false;

        _catalogTypeRepository.Setup(s => s.DeleteAsync(It.IsAny<int>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogTypeService.DeleteAsync(_testItem.Id);

        // assert
        result.Should().Be(testResult);
    }
}