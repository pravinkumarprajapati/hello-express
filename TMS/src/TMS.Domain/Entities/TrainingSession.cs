using TMS.Domain.Common;

namespace TMS.Domain.Entities;

/// <summary>
/// Represents a single training session/module that requires expert and observer trainer assignments.
/// </summary>
public class TrainingSession : BaseAuditableEntity
{
    public string SessionCode { get; set; } = string.Empty;
    public string DomainName { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public bool IsOutstation { get; set; }
    public bool IsSingleModuleOvertimeAllowed { get; set; }
    public DateTime StartOnUtc { get; set; }
    public DateTime EndOnUtc { get; set; }

    public double DurationHours => (EndOnUtc - StartOnUtc).TotalHours;
}
