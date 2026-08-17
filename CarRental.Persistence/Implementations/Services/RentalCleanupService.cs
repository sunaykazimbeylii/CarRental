using CarRental.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CarRental.Persistence.Services;

public class RentalCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public RentalCleanupService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                var context = scope.ServiceProvider
                    .GetRequiredService<AppDbContext>();

                var now = DateTime.UtcNow;

                await context.Rentals
                    .Where(x =>
                        x.EndDate < now &&
                        !x.IsDeleted)
                    .ExecuteUpdateAsync(
                        setters => setters
                            .SetProperty(x => x.IsDeleted, true)
                            .SetProperty(x => x.UpdatedAt, now),
                        stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Rental cleanup xətası: {ex.Message}");
            }

            await Task.Delay(
                TimeSpan.FromDays(1),
                stoppingToken);
        }
    }
}