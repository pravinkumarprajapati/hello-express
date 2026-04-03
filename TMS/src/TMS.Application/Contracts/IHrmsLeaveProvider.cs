using TMS.Domain.Entities;

namespace TMS.Application.Contracts;

/// <summary>
/// Provides leave data pulled from HRMS integration.
/// </summary>
public interface IHrmsLeaveProvider
{
    Task<IReadOnlyCollection<LeaveRecord>> GetLeavesAsync(CancellationToken cancellationToken = default);
}
