using TMS.Domain.Common;
using TMS.Domain.Enums;

namespace TMS.Domain.Entities;

/// <summary>
/// Represents trainer profile information synchronized from HRMS and enriched in TMS.
/// </summary>
public class TrainerProfile : BaseAuditableEntity
{
    public string EmployeeCode { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public GenderType Gender { get; set; }
    public string Department { get; set; } = string.Empty;
    public string CoreCompetencies { get; set; } = string.Empty;
    public string AreasOfExpertise { get; set; } = string.Empty;
    public string BaseLocation { get; set; } = string.Empty;
    public int SeniorityInYears { get; set; }
    public bool IsAepAuthorized { get; set; }

    public ICollection<TrainerSkill> Skills { get; set; } = new List<TrainerSkill>();

    public string FullName => $"{FirstName} {LastName}".Trim();
}
