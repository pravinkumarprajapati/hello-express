using Microsoft.EntityFrameworkCore;
using TMS.Application.Contracts;
using TMS.Domain.Entities;
using TMS.Infrastructure.Persistence;

namespace TMS.Infrastructure.Repositories.Sql;

/// <summary>
/// SQL implementation for training schedule repository.
/// </summary>
public class SqlTrainingScheduleRepository : ITrainingScheduleRepository
{
    private readonly TmsDbContext _dbContext;

    public SqlTrainingScheduleRepository(TmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<TrainingSession?> GetSessionByIdAsync(Guid sessionId, CancellationToken cancellationToken = default)
        => _dbContext.TrainingSessions.FirstOrDefaultAsync(x => x.Id == sessionId && !x.IsDeleted, cancellationToken)!;

    public async Task<IReadOnlyCollection<TrainingSession>> GetAllSessionsAsync(CancellationToken cancellationToken = default)
        => await _dbContext.TrainingSessions.Where(x => !x.IsDeleted).ToArrayAsync(cancellationToken);

    public async Task<IReadOnlyCollection<HolidayCalendarDay>> GetHolidaysAsync(CancellationToken cancellationToken = default)
        => await _dbContext.HolidayCalendarDays.Where(x => !x.IsDeleted).ToArrayAsync(cancellationToken);

    public async Task<IReadOnlyCollection<LeaveRecord>> GetLeavesAsync(CancellationToken cancellationToken = default)
        => await _dbContext.LeaveRecords.Where(x => !x.IsDeleted).ToArrayAsync(cancellationToken);

    public async Task<IReadOnlyCollection<TrainingAssignment>> GetAssignmentsAsync(CancellationToken cancellationToken = default)
        => await _dbContext.TrainingAssignments.Where(x => !x.IsDeleted).ToArrayAsync(cancellationToken);

    public async Task SaveAssignmentsAsync(IEnumerable<TrainingAssignment> assignments, CancellationToken cancellationToken = default)
    {
        await _dbContext.TrainingAssignments.AddRangeAsync(assignments, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<TrainingSession>> GetSessionsByTrainerAsync(Guid trainerId, CancellationToken cancellationToken = default)
    {
        Guid[] sessionIds = await _dbContext.TrainingAssignments
            .Where(x => x.TrainerId == trainerId && !x.IsDeleted)
            .Select(x => x.SessionId)
            .Distinct()
            .ToArrayAsync(cancellationToken);

        return await _dbContext.TrainingSessions
            .Where(x => sessionIds.Contains(x.Id) && !x.IsDeleted)
            .ToArrayAsync(cancellationToken);
    }

    public async Task UpsertLeavesAsync(IEnumerable<LeaveRecord> leaves, CancellationToken cancellationToken = default)
    {
        foreach (LeaveRecord leave in leaves)
        {
            bool exists = await _dbContext.LeaveRecords.AnyAsync(x => x.TrainerId == leave.TrainerId && x.StartOnUtc == leave.StartOnUtc && x.EndOnUtc == leave.EndOnUtc && !x.IsDeleted, cancellationToken);
            if (!exists)
            {
                await _dbContext.LeaveRecords.AddAsync(leave, cancellationToken);
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ClearAssignmentsForSessionAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        List<TrainingAssignment> assignments = await _dbContext.TrainingAssignments
            .Where(x => x.SessionId == sessionId && !x.IsDeleted)
            .ToListAsync(cancellationToken);

        assignments.ForEach(x =>
        {
            x.IsDeleted = true;
            x.DeletedOnUtc = DateTime.UtcNow;
        });

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
