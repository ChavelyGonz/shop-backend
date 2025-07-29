// using System.Threading.Tasks;
// using Application.Features.GeneralPropose.Helpers;
// using Domain.Enums;
// using Domain.Exceptions;
// using Domain.Models;
// using Microsoft.AspNetCore.Mvc;
// using Presentation.Controllers;

// namespace Presentation.Tests.Controllers
// {
//     public partial class StorageControllerTests
//     {
//         private readonly Mock<Helper<Product>> _helperMock;
//         private readonly StorageController _controller;

//         public StorageControllerTests()
//         {
//             _helperMock = new Mock<Helper<Product>>(null, null, null);
//             _controller = new StorageController(null, _helperMock.Object);
//         }

//         [Fact]
//         public async Task DeleteProductById_WhenProductExists_ReturnsOk()
//         {
//             // Arrange
//             var productId = 1;
//             _helperMock
//                 .Setup(h => h.DeleteAsync(productId, It.IsAny<CancellationToken>()))
//                 .Returns(Task.CompletedTask);

//             // Act
//             var result = await _controller.DeleteProductById(productId);

//             // Assert
//             Assert.IsType<OkResult>(result);
//         }

//         [Fact]
//         public async Task DeleteProductById_WhenProductNotFound_ReturnsNotFound()
//         {
//             // Arrange
//             var productId = 99;
//             _helperMock
//                 .Setup(h => h.DeleteAsync(productId, It.IsAny<CancellationToken>()))
//                 .ThrowsAsync(new ApiException(ApiExceptionType.KeyNotFound, "Product not found.", new KeyNotFoundException()));

//             // Act
//             var result = await _controller.DeleteProductById(productId);

//             // Assert
//             var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
//             Assert.Equal("Product not found.", notFoundResult.Value);
//         }
//     }
// }
