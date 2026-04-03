using TMS.Domain.Common;

namespace TMS.Domain.Entities;

/// <summary>
/// Represents holiday calendar day that blocks scheduling.
/// </summary>
public class HolidayCalendarDay : BaseAuditableEntity
{
    public DateOnly Date { get; set; }
    public string HolidayName { get; set; } = string.Empty;
}
