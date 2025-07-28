
using Microsoft.Extensions.Logging;
using Application.Features.Storage.Queries;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories;
using Application.Features.Storage.DTOs;
using Application.Features.GeneralPropose.Specifications;

public class GetAllProductsQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnMappedProductDtos_WithPagination()
    {
        // Arrange
        var pageNumber = 2;
        var pageSize = 5;

        var products = new List<Product>
        {
            new Product { Id = 11, Name = "Product 11", Price = 99.99f, Unit = UnitOfMeasurement.kg },
            new Product { Id = 12, Name = "Product 12", Price = 109.99f, Unit = UnitOfMeasurement.kg }
        };

        var mockRepo = new Mock<IEntityReadRepository<Product>>();
        mockRepo
            .Setup(r => r.ListAsync(It.IsAny<PaginateSpecification<Product>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(m => m.Map<List<ProductDto>>(products))
                  .Returns(new List<ProductDto>
                  {
                      new ProductDto { Id = 11, Name = "Product 11", Price = 99.99f, Unit = UnitOfMeasurement.kg },
                      new ProductDto { Id = 12, Name = "Product 12", Price = 109.99f, Unit = UnitOfMeasurement.kg }
                  });

        var mockLogger = new Mock<ILogger<GetAllProductsQueryHandler>>();

        var handler = new GetAllProductsQueryHandler(mockRepo.Object, mockMapper.Object, mockLogger.Object);

        // Act
        var result = await handler.Handle(new GetAllProductsQuery(pageNumber, pageSize), CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(11, result[0].Id);
        Assert.Equal("Product 11", result[0].Name);
        Assert.Equal(12, result[1].Id);
        Assert.Equal("Product 12", result[1].Name);
    }
}
