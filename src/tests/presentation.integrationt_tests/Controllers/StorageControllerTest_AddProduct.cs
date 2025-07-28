
using Domain.Enums;
using Application.Features.Storage.DTOs;
using Application.Features.Storage.Commands;
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
    private readonly HttpClient _client;
    private readonly ShopWebApplicationFactory _factory;

    public StorageControllerTests(ShopWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task AddProduct_ValidCommand_ReturnsOkWithProduct()
    {
        // Arrange
        await ResetDatabaseAsync();

        var command = new CreateProductCommand
        {
            DTO = new ProductDto{
                Name = "Test Product",
                Price = 9.99f,
                Unit = UnitOfMeasurement.kg
            },
            Stores = new List<int>()
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Storage/AddProduct", command);

        // Assert
        response.EnsureSuccessStatusCode();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        options.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        var product = await response.Content.ReadFromJsonAsync<ProductDto>(options);
        product.Should().NotBeNull();
        product!.Name.Should().Be("Test Product");
    }

    private async Task ResetDatabaseAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ShopDbContext>();
        await db.Database.EnsureDeletedAsync();
        await db.Database.EnsureCreatedAsync();
    }
}
