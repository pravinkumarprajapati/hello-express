using TMS.Domain.Common;
using TMS.Domain.Enums;

namespace TMS.Domain.Entities;

/// <summary>
/// Represents a trainer skill with department, proficiency and flags used by the assignment engine.
/// </summary>
public class TrainerSkill : BaseAuditableEntity
{
    public Guid TrainerId { get; set; }
    public string SkillName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public ProficiencyLevel ProficiencyLevel { get; set; }
    public bool IsExaminer { get; set; }
    public bool IsAuditor { get; set; }
    public bool IsCrossSkilled { get; set; }
}
