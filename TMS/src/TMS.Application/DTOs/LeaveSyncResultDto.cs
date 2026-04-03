namespace TMS.Application.DTOs;

public class LeaveSyncResultDto
{
    public int LeavesFetched { get; set; }
    public int ImpactedSessions { get; set; }
    public int ReassignmentsTriggered { get; set; }
    public IReadOnlyCollection<Guid> SessionIds { get; set; } = Array.Empty<Guid>();
}
