using System.Text;
using TMS.Application.Contracts;
using TMS.Application.DTOs;
using TMS.Domain.Entities;

namespace TMS.Application.Services;

/// <summary>
/// Builds instructor utilization reports and export payloads.
/// </summary>
public class ReportService : IReportService
{
    private readonly ITrainerRepository _trainerRepository;
    private readonly ITrainingScheduleRepository _trainingScheduleRepository;

    public ReportService(ITrainerRepository trainerRepository, ITrainingScheduleRepository trainingScheduleRepository)
    {
        _trainerRepository = trainerRepository;
        _trainingScheduleRepository = trainingScheduleRepository;
    }

    public async Task<IReadOnlyCollection<UtilizationReportRowDto>> GetUtilizationReportAsync(UtilizationReportFilterDto filter, CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<TrainerProfile> trainers = await _trainerRepository.GetAllAsync(cancellationToken);
        IReadOnlyCollection<TrainingAssignment> assignments = await _trainingScheduleRepository.GetAssignmentsAsync(cancellationToken);
        IReadOnlyCollection<TrainingSession> sessions = await _trainingScheduleRepository.GetAllSessionsAsync(cancellationToken);
        IReadOnlyCollection<LeaveRecord> leaves = await _trainingScheduleRepository.GetLeavesAsync(cancellationToken);

        IEnumerable<TrainerProfile> query = trainers;
        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            query = query.Where(x => x.FullName.Contains(filter.Name, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(filter.Department))
        {
            query = query.Where(x => x.Department.Equals(filter.Department, StringComparison.OrdinalIgnoreCase));
        }

        DateTime fromOnUtc = filter.FromOnUtc ?? DateTime.UtcNow.Date.AddDays(-30);
        DateTime toOnUtc = filter.ToOnUtc ?? DateTime.UtcNow.Date.AddDays(1);

        var rows = new List<UtilizationReportRowDto>();

        foreach (TrainerProfile trainer in query)
        {
            HashSet<Guid> sessionIds = assignments
                .Where(x => x.TrainerId == trainer.Id)
                .Select(x => x.SessionId)
                .ToHashSet();

            IReadOnlyCollection<TrainingSession> trainerSessions = sessions
                .Where(x => sessionIds.Contains(x.Id) && x.StartOnUtc >= fromOnUtc && x.EndOnUtc <= toOnUtc)
                .ToArray();

            int leavesAvailed = leaves.Count(x =>
                x.TrainerId == trainer.Id
                && x.StartOnUtc >= fromOnUtc
                && x.EndOnUtc <= toOnUtc);

            rows.Add(new UtilizationReportRowDto
            {
                TrainerId = trainer.Id,
                TrainerName = trainer.FullName,
                Department = trainer.Department,
                TrainingHours = trainerSessions.Sum(x => x.DurationHours),
                LeavesAvailed = leavesAvailed,
                WeekOffsGiven = 8,
                CompOffsGiven = 0,
                PendingCompOffs = 0,
                ProjectHours = 0,
                LearningHours = 0,
                StandbyDays = 0,
                AdminDays = 0
            });
        }

        return rows;
    }

    public async Task<byte[]> ExportUtilizationExcelAsync(UtilizationReportFilterDto filter, CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<UtilizationReportRowDto> rows = await GetUtilizationReportAsync(filter, cancellationToken);

        var builder = new StringBuilder();
        builder.AppendLine("TrainerName,Department,TrainingHours,LeavesAvailed,WeekOffsGiven,CompOffsGiven,PendingCompOffs,ProjectHours,LearningHours,StandbyDays,AdminDays");

        foreach (UtilizationReportRowDto row in rows)
        {
            builder.AppendLine($"{row.TrainerName},{row.Department},{row.TrainingHours},{row.LeavesAvailed},{row.WeekOffsGiven},{row.CompOffsGiven},{row.PendingCompOffs},{row.ProjectHours},{row.LearningHours},{row.StandbyDays},{row.AdminDays}");
        }

        return Encoding.UTF8.GetBytes(builder.ToString());
    }
}
