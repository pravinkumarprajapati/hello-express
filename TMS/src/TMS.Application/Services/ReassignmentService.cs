using TMS.Application.Contracts;

namespace TMS.Application.Services;

/// <summary>
/// Handles session reassignment when existing trainer allocations become invalid due to leave updates.
/// </summary>
public class ReassignmentService : IReassignmentService
{
    private readonly ITrainingScheduleRepository _trainingScheduleRepository;
    private readonly IAssignmentEngineService _assignmentEngineService;

    public ReassignmentService(
        ITrainingScheduleRepository trainingScheduleRepository,
        IAssignmentEngineService assignmentEngineService)
    {
        _trainingScheduleRepository = trainingScheduleRepository;
        _assignmentEngineService = assignmentEngineService;
    }

    public async Task<bool> ReassignSessionAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        await _trainingScheduleRepository.ClearAssignmentsForSessionAsync(sessionId, cancellationToken);
        var result = await _assignmentEngineService.AutoAssignAsync(sessionId, cancellationToken);
        return result.IsAssigned;
    }
}
