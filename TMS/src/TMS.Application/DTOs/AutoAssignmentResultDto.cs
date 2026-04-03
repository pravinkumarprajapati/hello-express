namespace TMS.Application.DTOs;

public class AutoAssignmentResultDto
{
    public Guid SessionId { get; set; }
    public bool IsAssigned { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public Guid? ExpertTrainerId { get; set; }
    public Guid? ObserverTrainerId { get; set; }
}
