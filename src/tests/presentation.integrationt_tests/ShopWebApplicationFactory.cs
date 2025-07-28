using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Persistence.Contexts;

namespace Presentation.IntegrationTests;

public class ShopWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the real DbContext registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ShopDbContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            // Add DbContext using in-memory DB
            services.AddDbContext<ShopDbContext>(options =>
            {
                options.UseInMemoryDatabase("ShopTestDb");
            });


            // Build service provider and ensure DB is created
            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ShopDbContext>();
                db.Database.EnsureCreated();
            }
        });
    }
}
