using TMS.Domain.Common;
using TMS.Domain.Enums;

namespace TMS.Domain.Entities;

/// <summary>
/// Represents trainer leave fetched from HRMS.
/// </summary>
public class LeaveRecord : BaseAuditableEntity
{
    public Guid TrainerId { get; set; }
    public LeaveType LeaveType { get; set; }
    public DateTime StartOnUtc { get; set; }
    public DateTime EndOnUtc { get; set; }

    public bool Overlaps(DateTime startOnUtc, DateTime endOnUtc)
    {
        return StartOnUtc < endOnUtc && EndOnUtc > startOnUtc;
    }
}
