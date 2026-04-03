using TMS.Application.Contracts;
using TMS.Domain.Entities;
using TMS.Domain.Enums;

namespace TMS.Infrastructure.Repositories;

/// <summary>
/// Mock HRMS leave provider for module-3 pull synchronization behavior.
/// </summary>
public class InMemoryHrmsLeaveProvider : IHrmsLeaveProvider
{
    public Task<IReadOnlyCollection<LeaveRecord>> GetLeavesAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<LeaveRecord> leaves =
        [
            new LeaveRecord
            {
                TrainerId = Guid.Parse("d6a9f5f3-95bb-46f9-b71e-f2763d4dce1b"),
                LeaveType = LeaveType.Emergency,
                StartOnUtc = DateTime.UtcNow.Date.AddHours(8),
                EndOnUtc = DateTime.UtcNow.Date.AddHours(18)
            }
        ];

        return Task.FromResult(leaves);
    }
}
