
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers;
using Application.Features.Storage.DTOs;
using Application.Features.Storage.Commands;
using Domain.Enums;
using Application.Features.GeneralPropose.Helpers;
using Domain.Models;
using System.Threading;
using System.Threading.Tasks;

public partial class StorageControllerTests
{
    [Fact]
    public async Task AddProduct_ValidCommand_ReturnsOkWithCreatedProduct()
    {
        #region arrange
        var mockMediator = new Mock<ISender>();
        var helperMock = new Mock<Helper<Product>>(null, null, null);
        var controller = new StorageController(mockMediator.Object, helperMock.Object);

        var fakeProduct = new ProductDto
        {
            Id = 1,
            Name = "Test Product", 
            Price = 10.0f, 
            Unit = UnitOfMeasurement.kg
        };
        var command = new CreateProductCommand 
        {
            DTO = new ProductDto
            {
                Name = "Test Product",
                Price = 10.0f,
                Unit = UnitOfMeasurement.kg,
            },
            Stores = new List<int> { 1, 2 }
        };

        mockMediator
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeProduct);
        #endregion
        #region act
        var result = await controller.AddProduct(command);
        #endregion
        #region assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedProduct = Assert.IsType<ProductDto>(okResult.Value);
        Assert.Equal(fakeProduct.Id, returnedProduct.Id);
        Assert.Equal(fakeProduct.Name, returnedProduct.Name);
        Assert.Equal(fakeProduct.Price, returnedProduct.Price);
        Assert.Equal(fakeProduct.Unit, returnedProduct.Unit);
        mockMediator.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        #endregion
    }
}
