using TMS.Application.Contracts;
using TMS.Application.Services;
using TMS.Domain.Entities;
using TMS.Infrastructure.Repositories;

namespace TMS.UnitTests;

public class LeaveSyncServiceTests
{
    [Fact]
    public async Task SyncLeavesAsync_WithConflictingAssignments_ShouldTriggerReassignment()
    {
        var hrmsProvider = new InMemoryHrmsLeaveProvider();
        var scheduleRepository = new InMemoryTrainingScheduleRepository();
        var reassignmentService = new StubReassignmentService();
        var service = new LeaveSyncService(hrmsProvider, scheduleRepository, reassignmentService, new NoOpTelemetryEventPublisher());

        var result = await service.SyncLeavesAsync();

        Assert.True(result.LeavesFetched > 0);
        Assert.True(result.ImpactedSessions > 0);
        Assert.True(result.ReassignmentsTriggered > 0);
    }

    private sealed class StubReassignmentService : IReassignmentService
    {
        public Task<bool> ReassignSessionAsync(Guid sessionId, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(true);
        }
    }
}
