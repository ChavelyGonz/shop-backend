
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers;
using Domain.Enums;
using Application.Features.GeneralPropose.Helpers;
using Domain.Models;
using Application.Features.Storage.DTOs;
using Application.Features.Storage.Commands;
using Application.Features.Storage.Queries;

public partial class StorageControllerTests
{
    [Fact]
    public async Task GetAllProducts_ShouldReturnOk_WithList()
    {
        // Arrange
        var mockSender = new Mock<ISender>();
        var expected = new List<ProductDto>
        {
            new ProductDto { Id = 11, Name = "Product 11", Price = 99.99f, Unit = UnitOfMeasurement.kg }
        };
        mockSender.Setup(s => s.Send(It.IsAny<GetAllProductsQuery>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(expected);

        var helperMock = new Mock<Helper<Product>>(null, null, null);
        var controller = new StorageController(mockSender.Object, helperMock.Object);

        // Act
        var result = await controller.GetAllProducts(1, 10);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var value = Assert.IsAssignableFrom<List<ProductDto>>(okResult.Value);
        Assert.Single(value);
    }
}
