// using System.Threading.Tasks;
// using Application.Features.GeneralPropose.Helpers;
// using Domain.Models;
// using Domain.Enums;
// using Domain.Exceptions;
// using Microsoft.AspNetCore.Mvc;
// using Presentation.Controllers;

// namespace Presentation.Tests.Controllers
// {
//     public partial class StorageControllerTests
//     {
//         [Fact]
//         public async Task EditProduct_WhenProductExists_ReturnsOk()
//         {
//             // Arrange
//             var productDto = new Application.Features.Storage.DTOs.ProductDto 
//             { Id = 1, Name = "Test", Price = 0.99f, Unit = UnitOfMeasurement.kg};
//             _helperMock
//                 .Setup(h => h.EditAsync(productDto, It.IsAny<CancellationToken>()))
//                 .Returns(Task.CompletedTask);

//             // Act
//             var result = await _controller.EditProduct(productDto);

//             // Assert
//             Assert.IsType<OkResult>(result);
//         }

//         [Fact]
//         public async Task EditProduct_WhenProductNotFound_ReturnsNotFound()
//         {
//             // Arrange
//             var productDto = new Application.Features.Storage.DTOs.ProductDto 
//             { Id = 99, Name = "Test", Price = 0.99f, Unit = UnitOfMeasurement.kg};
//             _helperMock
//                 .Setup(h => h.EditAsync(productDto, It.IsAny<CancellationToken>()))
//                 .ThrowsAsync(new ApiException(ApiExceptionType.KeyNotFound, "Product not found.", new KeyNotFoundException()));

//             // Act
//             var result = await _controller.EditProduct(productDto);

//             // Assert
//             var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
//             Assert.Equal("Product not found.", notFoundResult.Value);
//         }
//     }
// }
