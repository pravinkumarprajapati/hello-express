using TMS.Application.Contracts;
using TMS.Application.DTOs;
using TMS.Domain.Entities;

namespace TMS.Application.Services;

/// <summary>
/// Synchronizes leave data from HRMS and triggers reassignments for impacted sessions.
/// </summary>
public class LeaveSyncService : ILeaveSyncService
{
    private readonly IHrmsLeaveProvider _hrmsLeaveProvider;
    private readonly ITrainingScheduleRepository _trainingScheduleRepository;
    private readonly IReassignmentService _reassignmentService;

    public LeaveSyncService(
        IHrmsLeaveProvider hrmsLeaveProvider,
        ITrainingScheduleRepository trainingScheduleRepository,
        IReassignmentService reassignmentService)
    {
        _hrmsLeaveProvider = hrmsLeaveProvider;
        _trainingScheduleRepository = trainingScheduleRepository;
        _reassignmentService = reassignmentService;
    }

    public async Task<LeaveSyncResultDto> SyncLeavesAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<LeaveRecord> latestLeaves = await _hrmsLeaveProvider.GetLeavesAsync(cancellationToken);
        await _trainingScheduleRepository.UpsertLeavesAsync(latestLeaves, cancellationToken);

        IReadOnlyCollection<TrainingAssignment> assignments = await _trainingScheduleRepository.GetAssignmentsAsync(cancellationToken);

        var impactedSessionIds = new HashSet<Guid>();
        foreach (TrainingAssignment assignment in assignments)
        {
            IReadOnlyCollection<TrainingSession> sessions = await _trainingScheduleRepository
                .GetSessionsByTrainerAsync(assignment.TrainerId, cancellationToken);

            TrainingSession? assignedSession = sessions.FirstOrDefault(x => x.Id == assignment.SessionId);
            if (assignedSession is null)
            {
                continue;
            }

            bool conflictsWithLeave = latestLeaves.Any(leave =>
                leave.TrainerId == assignment.TrainerId
                && leave.Overlaps(assignedSession.StartOnUtc, assignedSession.EndOnUtc));

            if (conflictsWithLeave)
            {
                impactedSessionIds.Add(assignedSession.Id);
            }
        }

        int reassignments = 0;
        foreach (Guid sessionId in impactedSessionIds)
        {
            bool reassigned = await _reassignmentService.ReassignSessionAsync(sessionId, cancellationToken);
            if (reassigned)
            {
                reassignments++;
            }
        }

        return new LeaveSyncResultDto
        {
            LeavesFetched = latestLeaves.Count,
            ImpactedSessions = impactedSessionIds.Count,
            ReassignmentsTriggered = reassignments,
            SessionIds = impactedSessionIds.ToArray()
        };
    }
}
