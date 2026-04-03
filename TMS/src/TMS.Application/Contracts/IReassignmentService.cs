namespace TMS.Application.Contracts;

public interface IReassignmentService
{
    Task<bool> ReassignSessionAsync(Guid sessionId, CancellationToken cancellationToken = default);
}
