using TMS.Application.DTOs;

namespace TMS.Application.Contracts;

public interface IAssignmentEngineService
{
    Task<AutoAssignmentResultDto> AutoAssignAsync(Guid sessionId, CancellationToken cancellationToken = default);
}
