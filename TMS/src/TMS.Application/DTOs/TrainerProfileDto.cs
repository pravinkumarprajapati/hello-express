namespace TMS.Application.DTOs;

public class TrainerProfileDto
{
    public Guid Id { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string CoreCompetencies { get; set; } = string.Empty;
    public string AreasOfExpertise { get; set; } = string.Empty;
    public string BaseLocation { get; set; } = string.Empty;
    public int SeniorityInYears { get; set; }
    public bool IsAepAuthorized { get; set; }
    public IReadOnlyCollection<TrainerSkillDto> Skills { get; set; } = Array.Empty<TrainerSkillDto>();
}
