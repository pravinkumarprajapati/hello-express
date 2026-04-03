using TMS.Domain.Common;
using TMS.Domain.Enums;

namespace TMS.Domain.Entities;

/// <summary>
/// Represents trainer to session assignment with the requested role.
/// </summary>
public class TrainingAssignment : BaseAuditableEntity
{
    public Guid SessionId { get; set; }
    public Guid TrainerId { get; set; }
    public TrainingRole Role { get; set; }
}
