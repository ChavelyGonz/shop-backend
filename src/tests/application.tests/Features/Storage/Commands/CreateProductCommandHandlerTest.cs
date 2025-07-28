

using System.Threading;
using System.Threading.Tasks;
using Application.Features.Storage.Commands;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories;
using Application.Features.Storage.DTOs;
using Application.Features.Storage.Queries;
using Application.Features.GeneralPropose.DTOs;
using Domain.Exceptions;
using MediatR;
using System.Collections.Generic;

public class CreateProductCommandHandlerTests
{
    private readonly Mock<IEntityRepository<Product>> _productRepoMock = new();
    private readonly Mock<IEntityRepository<ProductStore>> _productStoreRepoMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ISender> _senderMock = new();

    private readonly CreateProductCommandHandler _handler;

    public CreateProductCommandHandlerTests()
    {
        _handler = new CreateProductCommandHandler(
            _productRepoMock.Object,
            _productStoreRepoMock.Object,
            _mapperMock.Object,
            _senderMock.Object
        );
    }

    [Fact]
    public async Task Handle_Should_Create_Product_And_ProductStores_When_Ids_Exist()
    {
        // Arrange
        var dto = new ProductDto 
        { 
            Name = "Test Product",
            Price = 10.99f,
            Unit = UnitOfMeasurement.kg
        };
        var command = new CreateProductCommand
        {
            DTO = dto,
            Stores = new List<int> { 1, 2 }
        };

        var cancellationToken = CancellationToken.None;

        var newProduct = new Product 
        {
            Name = "Test Product",
            Price = 10.99f,
            Unit = UnitOfMeasurement.kg 
        };
        var mappedDto = new ProductDto 
        { 
            Id = 42, 
            Name = "Test Product",
            Price = 10.99f,
            Unit = UnitOfMeasurement.kg 
        };

        // Setup mocks
        _senderMock.Setup(s => s.Send(It.IsAny<ExistStoresIdQuery>(), cancellationToken))
                   .ReturnsAsync(new CheckedIds { AllExist = true });

        _mapperMock.Setup(m => m.Map<ProductDto, Product>(dto))
                   .Returns(newProduct);

        _productRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Product>(), cancellationToken))
            .Callback<Product, CancellationToken>((p, ct) => p.Id = 42)
            .ReturnsAsync((Product p, CancellationToken ct) => p);

        _mapperMock.Setup(m => m.Map<Product, ProductDto>(newProduct))
                   .Returns(mappedDto);


        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(42);
        result.Name.Should().Be("Test Product");
        result.Price.Should().Be(10.99f);
        result.Unit.Should().Be(UnitOfMeasurement.kg);

        _productRepoMock.Verify(r => r.AddAsync(It.Is<Product>(p => p == newProduct), cancellationToken), Times.Once);
        _productStoreRepoMock.Verify(r => r.AddRangeAsync(It.IsAny<List<ProductStore>>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_ApiException_When_Ids_Not_Exist()
    {
        // Arrange
        var dto = new ProductDto 
        { 
            Name = "Test Product",
            Price = 10.99f,
            Unit = UnitOfMeasurement.kg
        };
        var command = new CreateProductCommand
        {
            DTO = dto,
            Stores = new List<int> { 1, 2 }
        };

        var cancellationToken = CancellationToken.None;

        _senderMock.Setup(s => s.Send(It.IsAny<ExistStoresIdQuery>(), cancellationToken))
                   .ReturnsAsync(new CheckedIds { AllExist = false, FirstMissedId = 1 });

        // Act
        var act = async () => await _handler.Handle(command, cancellationToken);

        // Assert
        var exception = await Assert.ThrowsAsync<ApiException>(act);
        exception.ExceptionType.Should().Be(ApiExceptionType.KeyNotFound);
        exception.Message.Should().Contain("Store with id 1 not found.");
    }
}
