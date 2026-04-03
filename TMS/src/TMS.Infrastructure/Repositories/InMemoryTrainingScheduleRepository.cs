using TMS.Application.Contracts;
using TMS.Domain.Entities;
using TMS.Domain.Enums;

namespace TMS.Infrastructure.Repositories;

/// <summary>
/// Temporary in-memory schedule and leave repository used for module 2/3 development.
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

    private static readonly List<TrainingAssignment> Assignments =
    [
        new TrainingAssignment
        {
            SessionId = Guid.Parse("f10c88db-148f-4bfe-b554-359eecf5a572"),
            TrainerId = Guid.Parse("d6a9f5f3-95bb-46f9-b71e-f2763d4dce1b"),
            Role = TrainingRole.Expert,
            CreatedOnUtc = DateTime.UtcNow
        }
    ];

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

    public Task<IReadOnlyCollection<TrainingSession>> GetSessionsByTrainerAsync(Guid trainerId, CancellationToken cancellationToken = default)
    {
        HashSet<Guid> sessionIds = Assignments
            .Where(x => x.TrainerId == trainerId)
            .Select(x => x.SessionId)
            .ToHashSet();

        IReadOnlyCollection<TrainingSession> sessions = Sessions
            .Where(x => sessionIds.Contains(x.Id))
            .ToArray();

        return Task.FromResult(sessions);
    }

    public Task UpsertLeavesAsync(IEnumerable<LeaveRecord> leaves, CancellationToken cancellationToken = default)
    {
        foreach (LeaveRecord leave in leaves)
        {
            LeaveRecord? existing = Leaves.FirstOrDefault(x => x.TrainerId == leave.TrainerId && x.StartOnUtc == leave.StartOnUtc && x.EndOnUtc == leave.EndOnUtc);
            if (existing is null)
            {
                Leaves.Add(leave);
            }
        }

        return Task.CompletedTask;
    }

    public Task ClearAssignmentsForSessionAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        Assignments.RemoveAll(x => x.SessionId == sessionId);
        return Task.CompletedTask;
    }
}
