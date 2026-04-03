using TMS.Domain.Entities;

namespace TMS.Application.Contracts;

/// <summary>
/// Repository abstraction for schedule, leave and assignment operations used by the assignment engine.
/// </summary>
public interface ITrainingScheduleRepository
{
    Task<TrainingSession?> GetSessionByIdAsync(Guid sessionId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<HolidayCalendarDay>> GetHolidaysAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<LeaveRecord>> GetLeavesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TrainingAssignment>> GetAssignmentsAsync(CancellationToken cancellationToken = default);
    Task SaveAssignmentsAsync(IEnumerable<TrainingAssignment> assignments, CancellationToken cancellationToken = default);
}
