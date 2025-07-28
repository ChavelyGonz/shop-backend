using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.Contexts;

namespace Infrastructure.Persistence.Services
{
    public class DeleteOldPurchasesService : BackgroundService
    {
        private readonly ShopDbContext _context;

        public DeleteOldPurchasesService(ShopDbContext context)
        {
            _context = context;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var tenYearsAgo = DateTime.UtcNow.AddYears(-10);
                
                var oldPurchases = await _context.Purchases
                    .Where(p => p.When <= tenYearsAgo)
                    .ToListAsync();
                
                if (oldPurchases.Any())
                {
                    _context.Purchases.RemoveRange(oldPurchases);
                    await _context.SaveChangesAsync();
                }

                await Task.Delay(TimeSpan.FromDays(30), stoppingToken);
            }
        }
    }
}