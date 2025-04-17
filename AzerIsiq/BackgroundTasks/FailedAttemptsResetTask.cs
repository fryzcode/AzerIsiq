using AzerIsiq.Repository.Interface;

namespace AzerIsiq.Extensions.BackgroundTasks;

public class FailedAttemptsResetTask : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<FailedAttemptsResetTask> _logger;

    public FailedAttemptsResetTask(IServiceProvider serviceProvider, ILogger<FailedAttemptsResetTask> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

                await userRepository.ResetFailedAttemptsAsync(stoppingToken);
                _logger.LogInformation("Failed Attempts successfully reset at {time}", DateTime.Now);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when resetting Failed Attempts.");
            }

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}
