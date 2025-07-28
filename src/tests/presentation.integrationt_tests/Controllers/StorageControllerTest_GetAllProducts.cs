using Domain.Enums;
using Application.Features.Storage.DTOs;
using Infrastructure.Persistence.Contexts;
using Presentation.IntegrationTests;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Presentation.IntegrationTests.Controllers;

public partial class StorageControllerTests : IClassFixture<ShopWebApplicationFactory>
{
    [Fact]
    public async Task GetAllProducts_ReturnsOkWithPaginatedList()
    {
        // Arrange
        await ResetDatabaseAsync();

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ShopDbContext>();

        db.Products.Add(new Domain.Models.Product { Name = "Test Product 1", Price = 9.99f, Unit = UnitOfMeasurement.kg });
        db.Products.Add(new Domain.Models.Product { Name = "Test Product 2", Price = 19.99f, Unit = UnitOfMeasurement.kg });
        db.Products.Add(new Domain.Models.Product { Name = "Test Product 3", Price = 29.99f, Unit = UnitOfMeasurement.kg });
        await db.SaveChangesAsync();

        var pageNumber = 1;
        var pageSize = 2;

        // Act
        var response = await _client.GetAsync($"/api/Storage/GetAllProducts?pageNumber={pageNumber}&pageSize={pageSize}");

        // Assert
        response.EnsureSuccessStatusCode();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        options.Converters.Add(new JsonStringEnumConverter());  // handle enum serialization

        var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>(options);

        products.Should().NotBeNull();
        products.Should().HaveCount(2);
        products![0].Name.Should().Be("Test Product 1");
        products[0].Price.Should().Be(9.99f);
        products[0].Unit.Should().Be(UnitOfMeasurement.kg);

        products[1].Name.Should().Be("Test Product 2");
        products[1].Price.Should().Be(19.99f);
        products[1].Unit.Should().Be(UnitOfMeasurement.kg);
    }
}
