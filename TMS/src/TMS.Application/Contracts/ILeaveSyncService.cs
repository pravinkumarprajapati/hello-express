using TMS.Application.DTOs;

namespace TMS.Application.Contracts;

public interface ILeaveSyncService
{
    Task<LeaveSyncResultDto> SyncLeavesAsync(CancellationToken cancellationToken = default);
}
