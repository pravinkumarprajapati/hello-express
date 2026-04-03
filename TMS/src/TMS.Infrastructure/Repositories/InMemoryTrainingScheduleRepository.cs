using TMS.Application.Contracts;
using TMS.Domain.Entities;
using TMS.Domain.Enums;

namespace TMS.Infrastructure.Repositories;

/// <summary>
/// Temporary in-memory schedule and leave repository used for module 2 development.
/// </summary>
public class InMemoryTrainingScheduleRepository : ITrainingScheduleRepository
{
    private static readonly List<TrainingSession> Sessions =
    [
        new TrainingSession
        {
            Id = Guid.Parse("f10c88db-148f-4bfe-b554-359eecf5a572"),
            SessionCode = "TS-APR-001",
            DomainName = "Aircraft Training",
            Location = "Delhi",
            IsOutstation = false,
            IsSingleModuleOvertimeAllowed = false,
            StartOnUtc = DateTime.UtcNow.Date.AddHours(9),
            EndOnUtc = DateTime.UtcNow.Date.AddHours(16)
        },
        new TrainingSession
        {
            Id = Guid.Parse("a7a94e50-fb15-4389-bca1-2bb5c767af19"),
            SessionCode = "TS-APR-002",
            DomainName = "Safety",
            Location = "Mumbai",
            IsOutstation = true,
            IsSingleModuleOvertimeAllowed = true,
            StartOnUtc = DateTime.UtcNow.Date.AddDays(1).AddHours(8),
            EndOnUtc = DateTime.UtcNow.Date.AddDays(1).AddHours(18)
        }
    ];

    private static readonly List<HolidayCalendarDay> Holidays =
    [
        new HolidayCalendarDay
        {
            Date = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(10)),
            HolidayName = "National Holiday"
        }
    ];

    private static readonly List<LeaveRecord> Leaves =
    [
        new LeaveRecord
        {
            TrainerId = Guid.Parse("61719f66-42e6-4e2b-8ab8-535974f68e30"),
            LeaveType = LeaveType.Casual,
            StartOnUtc = DateTime.UtcNow.Date.AddDays(2),
            EndOnUtc = DateTime.UtcNow.Date.AddDays(2).AddHours(23)
        }
    ];

    private static readonly List<TrainingAssignment> Assignments = [];

    public Task<TrainingSession?> GetSessionByIdAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        TrainingSession? session = Sessions.FirstOrDefault(x => x.Id == sessionId);
        return Task.FromResult(session);
    }

    public Task<IReadOnlyCollection<HolidayCalendarDay>> GetHolidaysAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult((IReadOnlyCollection<HolidayCalendarDay>)Holidays);
    }

    public Task<IReadOnlyCollection<LeaveRecord>> GetLeavesAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult((IReadOnlyCollection<LeaveRecord>)Leaves);
    }

    public Task<IReadOnlyCollection<TrainingAssignment>> GetAssignmentsAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult((IReadOnlyCollection<TrainingAssignment>)Assignments);
    }

    public Task SaveAssignmentsAsync(IEnumerable<TrainingAssignment> assignments, CancellationToken cancellationToken = default)
    {
        Assignments.AddRange(assignments);
        return Task.CompletedTask;
    }
}
