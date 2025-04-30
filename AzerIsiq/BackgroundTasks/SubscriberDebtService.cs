using AzerIsiq.Data;
using Microsoft.EntityFrameworkCore;

namespace AzerIsiq.Extensions.BackgroundTasks;

public class SubscriberDebtService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SubscriberDebtService> _logger;

    private const int TargetStatus = 5;
    private const decimal DebtIncrement = 0.10m;
    private const int CounterIncrement = 20;
    private static readonly TimeSpan DelayInterval = TimeSpan.FromHours(1);

    public SubscriberDebtService(IServiceProvider serviceProvider, ILogger<SubscriberDebtService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("SubscriberDebtService started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var subscribersToUpdate = await db.Subscribers
                    .Include(s => s.Counters)
                    .Where(s => s.Status == TargetStatus)
                    .ToListAsync(stoppingToken);

                if (subscribersToUpdate.Any())
                {
                    _logger.LogInformation("Found {Count} subscribers with Status == 5", subscribersToUpdate.Count);
                }

                foreach (var subscriber in subscribersToUpdate)
                {
                    _logger.LogInformation("Before: Subscriber {Id} - Debt: {Debt}", subscriber.Id, subscriber.Debt);

                    subscriber.Debt += DebtIncrement;

                    foreach (var counter in subscriber.Counters)
                    {
                        var oldValue = counter.CurrentValue;
                        counter.CurrentValue += CounterIncrement;

                        _logger.LogInformation("Updated Counter {CounterId} for Subscriber {SubscriberId}: {OldValue} -> {NewValue}",
                            counter.Id, subscriber.Id, oldValue, counter.CurrentValue);
                    }

                    _logger.LogInformation("After: Subscriber {Id} - New Debt: {NewDebt}", subscriber.Id, subscriber.Debt);
                }

                await db.SaveChangesAsync(stoppingToken);
                _logger.LogInformation("Debt and counter values updated and changes saved.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating subscriber debts and counters.");
            }

            await Task.Delay(DelayInterval, stoppingToken);
        }
    }
}
