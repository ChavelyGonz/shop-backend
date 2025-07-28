
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Domain.Models;
using Domain.Repositories;
using Application.Features.Storage.Queries;
using Application.Features.GeneralPropose.DTOs;

public class ExistStoresIdQueryHandlerTests
{
    private readonly Mock<IEntityReadRepository<Store>> _storeRepoMock;
    private readonly ExistStoresIdQueryHandler _handler;

    public ExistStoresIdQueryHandlerTests()
    {
        _storeRepoMock = new Mock<IEntityReadRepository<Store>>();
        _handler = new ExistStoresIdQueryHandler(_storeRepoMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_AllExist_True_When_All_Ids_Exist()
    {
        // Arrange
        var stores = new List<int> { 1, 2, 3 };

        // Simulate all IDs exist
        _storeRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync((int id, CancellationToken ct) => new Store { Id = id });

        var query = new ExistStoresIdQuery { Stores = stores };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.AllExist);
        Assert.Null(result.FirstMissedId);
        Assert.Equal(stores, result.Ids);
    }

    [Fact]
    public async Task Handle_Should_Return_AllExist_False_When_An_Id_Is_Missing()
    {
        // Arrange
        var stores = new List<int> { 1, 2, 3 };

        // Simulate store with Id=2 does not exist
        _storeRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Store { Id = 1 });
        _storeRepoMock.Setup(r => r.GetByIdAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync((Store)null);
        _storeRepoMock.Setup(r => r.GetByIdAsync(3, It.IsAny<CancellationToken>())).ReturnsAsync(new Store { Id = 3 });

        var query = new ExistStoresIdQuery { Stores = stores };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.AllExist);
        Assert.Equal(2, result.FirstMissedId);
        Assert.Contains(1, result.Ids);
        Assert.Contains(3, result.Ids);
        Assert.DoesNotContain(2, result.Ids);
    }
}
