using TMS.Application.Contracts;

namespace TMS.Api.BackgroundServices;

/// <summary>
/// Runs periodic leave synchronization every 60 seconds.
/// </summary>
public class LeaveSyncBackgroundService : BackgroundService
{
    private static readonly TimeSpan SyncInterval = TimeSpan.FromSeconds(60);
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<LeaveSyncBackgroundService> _logger;

    public LeaveSyncBackgroundService(IServiceScopeFactory serviceScopeFactory, ILogger<LeaveSyncBackgroundService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using IServiceScope scope = _serviceScopeFactory.CreateScope();
                var leaveSyncService = scope.ServiceProvider.GetRequiredService<ILeaveSyncService>();
                var result = await leaveSyncService.SyncLeavesAsync(stoppingToken);

                _logger.LogInformation(
                    "Leave sync completed. LeavesFetched={LeavesFetched}, ImpactedSessions={ImpactedSessions}, ReassignmentsTriggered={ReassignmentsTriggered}",
                    result.LeavesFetched,
                    result.ImpactedSessions,
                    result.ReassignmentsTriggered);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Leave sync background job failed.");
            }

            await Task.Delay(SyncInterval, stoppingToken);
        }
    }
}
