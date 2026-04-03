namespace TMS.Application.DTOs;

public class UtilizationReportRowDto
{
    public Guid TrainerId { get; set; }
    public string TrainerName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public double TrainingHours { get; set; }
    public int LeavesAvailed { get; set; }
    public int WeekOffsGiven { get; set; }
    public int CompOffsGiven { get; set; }
    public int PendingCompOffs { get; set; }
    public double ProjectHours { get; set; }
    public double LearningHours { get; set; }
    public int StandbyDays { get; set; }
    public int AdminDays { get; set; }
}
