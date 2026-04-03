using TMS.Application.Contracts;
using TMS.Application.Services;
using TMS.Domain.Entities;
using TMS.Infrastructure.Repositories;

namespace TMS.UnitTests;

public class AssignmentEngineServiceTests
{
    [Fact]
    public async Task AutoAssignAsync_WithValidSession_ShouldAssignExpertAndObserver()
    {
        var service = new AssignmentEngineService(new InMemoryTrainingScheduleRepository(), new InMemoryTrainerRepository(), new NoOpTelemetryEventPublisher());

        var result = await service.AutoAssignAsync(Guid.Parse("f10c88db-148f-4bfe-b554-359eecf5a572"));

        Assert.True(result.IsAssigned);
        Assert.Equal("Assigned", result.Status);
        Assert.NotNull(result.ExpertTrainerId);
        Assert.NotNull(result.ObserverTrainerId);
        Assert.NotEqual(result.ExpertTrainerId, result.ObserverTrainerId);
    }

    [Fact]
    public async Task AutoAssignAsync_WithUnknownSession_ShouldReturnNotFound()
    {
        var service = new AssignmentEngineService(new InMemoryTrainingScheduleRepository(), new InMemoryTrainerRepository(), new NoOpTelemetryEventPublisher());

        var result = await service.AutoAssignAsync(Guid.NewGuid());

        Assert.False(result.IsAssigned);
        Assert.Equal("NotFound", result.Status);
    }

    [Fact]
    public async Task AutoAssignAsync_WithOverDurationWithoutOverride_ShouldFail()
    {
        var service = new AssignmentEngineService(new OversizedSessionRepository(), new InMemoryTrainerRepository(), new NoOpTelemetryEventPublisher());

        var result = await service.AutoAssignAsync(OversizedSessionRepository.SessionId);

        Assert.False(result.IsAssigned);
        Assert.Equal("DurationExceeded", result.Status);
    }

    private sealed class OversizedSessionRepository : ITrainingScheduleRepository
    {
        internal static readonly Guid SessionId = Guid.Parse("bd75045d-a58d-4375-a453-8c370fcd552a");

        public Task<TrainingSession?> GetSessionByIdAsync(Guid sessionId, CancellationToken cancellationToken = default)
        {
            if (sessionId != SessionId)
            {
                return Task.FromResult<TrainingSession?>(null);
            }

            return Task.FromResult<TrainingSession?>(new TrainingSession
            {
                Id = SessionId,
                DomainName = "Aircraft Training",
                StartOnUtc = DateTime.UtcNow.Date.AddHours(8),
                EndOnUtc = DateTime.UtcNow.Date.AddHours(20),
                IsSingleModuleOvertimeAllowed = false
            });
        }

        public Task<IReadOnlyCollection<HolidayCalendarDay>> GetHolidaysAsync(CancellationToken cancellationToken = default)
            => Task.FromResult((IReadOnlyCollection<HolidayCalendarDay>)Array.Empty<HolidayCalendarDay>());

        public Task<IReadOnlyCollection<TrainingSession>> GetAllSessionsAsync(CancellationToken cancellationToken = default)
            => Task.FromResult((IReadOnlyCollection<TrainingSession>)Array.Empty<TrainingSession>());


        public Task<IReadOnlyCollection<LeaveRecord>> GetLeavesAsync(CancellationToken cancellationToken = default)
            => Task.FromResult((IReadOnlyCollection<LeaveRecord>)Array.Empty<LeaveRecord>());

        public Task<IReadOnlyCollection<TrainingAssignment>> GetAssignmentsAsync(CancellationToken cancellationToken = default)
            => Task.FromResult((IReadOnlyCollection<TrainingAssignment>)Array.Empty<TrainingAssignment>());

        public Task SaveAssignmentsAsync(IEnumerable<TrainingAssignment> assignments, CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public Task<IReadOnlyCollection<TrainingSession>> GetSessionsByTrainerAsync(Guid trainerId, CancellationToken cancellationToken = default)
            => Task.FromResult((IReadOnlyCollection<TrainingSession>)Array.Empty<TrainingSession>());

        public Task UpsertLeavesAsync(IEnumerable<LeaveRecord> leaves, CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public Task ClearAssignmentsForSessionAsync(Guid sessionId, CancellationToken cancellationToken = default)
            => Task.CompletedTask;
    }
}
