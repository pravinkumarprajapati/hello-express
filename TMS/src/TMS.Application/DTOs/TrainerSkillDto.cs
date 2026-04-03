namespace TMS.Application.DTOs;

public class TrainerSkillDto
{
    public string SkillName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string ProficiencyLevel { get; set; } = string.Empty;
    public bool IsExaminer { get; set; }
    public bool IsAuditor { get; set; }
    public bool IsCrossSkilled { get; set; }
}
