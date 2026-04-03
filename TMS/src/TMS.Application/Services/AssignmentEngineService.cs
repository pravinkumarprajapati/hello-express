using TMS.Application.Contracts;
using TMS.Application.DTOs;
using TMS.Domain.Entities;
using TMS.Domain.Enums;

namespace TMS.Application.Services;

/// <summary>
/// Applies assignment policies and business constraints for assigning expert and observer trainers.
/// </summary>
public class AssignmentEngineService : IAssignmentEngineService
{
    private const int MaxConsecutiveDays = 7;
    private static readonly TimeSpan MinimumRestGap = TimeSpan.FromHours(12);

    private readonly ITrainingScheduleRepository _trainingScheduleRepository;
    private readonly ITrainerRepository _trainerRepository;
    private readonly ITelemetryEventPublisher _telemetryEventPublisher;

    public AssignmentEngineService(
        ITrainingScheduleRepository trainingScheduleRepository,
        ITrainerRepository trainerRepository,
        ITelemetryEventPublisher telemetryEventPublisher)
    {
        _trainingScheduleRepository = trainingScheduleRepository;
        _trainerRepository = trainerRepository;
        _telemetryEventPublisher = telemetryEventPublisher;
    }

    public async Task<AutoAssignmentResultDto> AutoAssignAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        _telemetryEventPublisher.TrackEvent("AssignmentEngine.Started", new Dictionary<string, string> { ["SessionId"] = sessionId.ToString() });

        TrainingSession? session = await _trainingScheduleRepository.GetSessionByIdAsync(sessionId, cancellationToken);
        if (session is null)
        {
            return BuildFailure(sessionId, "NotFound", "Training session was not found.");
        }

        if (session.DurationHours > 8.5 && !session.IsSingleModuleOvertimeAllowed)
        {
            return BuildFailure(sessionId, "DurationExceeded", "Training session exceeds 8.5 hours without overtime allowance.");
        }

        IReadOnlyCollection<HolidayCalendarDay> holidays = await _trainingScheduleRepository.GetHolidaysAsync(cancellationToken);
        if (holidays.Any(x => x.Date == DateOnly.FromDateTime(session.StartOnUtc)))
        {
            return BuildFailure(sessionId, "HolidayBlocked", "Training cannot be assigned on a holiday.");
        }

        IReadOnlyCollection<TrainerProfile> trainers = await _trainerRepository.GetAllAsync(cancellationToken);
        IReadOnlyCollection<LeaveRecord> leaves = await _trainingScheduleRepository.GetLeavesAsync(cancellationToken);
        IReadOnlyCollection<TrainingAssignment> assignments = await _trainingScheduleRepository.GetAssignmentsAsync(cancellationToken);

        List<TrainerProfile> eligibleTrainers = trainers
            .Where(trainer => IsTrainerAvailable(trainer, session, leaves, assignments))
            .ToList();

        TrainerProfile? expert = eligibleTrainers
            .Where(trainer => HasDomainSkill(trainer, session.DomainName, requireAdvanced: true))
            .OrderByDescending(trainer => trainer.SeniorityInYears)
            .FirstOrDefault();

        if (expert is null)
        {
            return BuildFailure(sessionId, "NoExpert", "No eligible expert trainer found.");
        }

        TrainerProfile? observer = eligibleTrainers
            .Where(trainer => trainer.Id != expert.Id)
            .Where(trainer => HasDomainSkill(trainer, session.DomainName, requireAdvanced: false) || trainer.Skills.Any())
            .OrderByDescending(trainer => trainer.Skills.Count)
            .FirstOrDefault();

        if (observer is null)
        {
            return BuildFailure(sessionId, "NoObserver", "No eligible observer trainer found.");
        }

        var newAssignments = new[]
        {
            new TrainingAssignment
            {
                SessionId = session.Id,
                TrainerId = expert.Id,
                Role = TrainingRole.Expert
            },
            new TrainingAssignment
            {
                SessionId = session.Id,
                TrainerId = observer.Id,
                Role = TrainingRole.Observer
            }
        };

        await _trainingScheduleRepository.SaveAssignmentsAsync(newAssignments, cancellationToken);

        _telemetryEventPublisher.TrackEvent("AssignmentEngine.Assigned", new Dictionary<string, string>
        {
            ["SessionId"] = sessionId.ToString(),
            ["ExpertTrainerId"] = expert.Id.ToString(),
            ["ObserverTrainerId"] = observer.Id.ToString()
        });

        return new AutoAssignmentResultDto
        {
            SessionId = sessionId,
            IsAssigned = true,
            Status = "Assigned",
            Message = "Expert and observer were assigned successfully.",
            ExpertTrainerId = expert.Id,
            ObserverTrainerId = observer.Id
        };
    }

    private static bool HasDomainSkill(TrainerProfile trainer, string domainName, bool requireAdvanced)
    {
        return trainer.Skills.Any(skill =>
            string.Equals(skill.Department, domainName, StringComparison.OrdinalIgnoreCase)
            && (!requireAdvanced || skill.ProficiencyLevel >= ProficiencyLevel.Advanced));
    }

    private static bool IsTrainerAvailable(
        TrainerProfile trainer,
        TrainingSession session,
        IReadOnlyCollection<LeaveRecord> leaves,
        IReadOnlyCollection<TrainingAssignment> assignments)
    {
        bool hasLeaveConflict = leaves.Any(leave =>
            leave.TrainerId == trainer.Id
            && leave.Overlaps(session.StartOnUtc, session.EndOnUtc));

        if (hasLeaveConflict)
        {
            return false;
        }

        IReadOnlyCollection<TrainingAssignment> trainerAssignments = assignments
            .Where(assignment => assignment.TrainerId == trainer.Id)
            .ToArray();

        bool restGapViolation = trainerAssignments.Any(existing =>
            Math.Abs((existing.CreatedOnUtc - session.StartOnUtc).TotalHours) < MinimumRestGap.TotalHours);

        if (restGapViolation)
        {
            return false;
        }

        int consecutiveDays = trainerAssignments
            .Select(assignment => DateOnly.FromDateTime(assignment.CreatedOnUtc))
            .Distinct()
            .Count();

        return consecutiveDays < MaxConsecutiveDays;
    }

    private static AutoAssignmentResultDto BuildFailure(Guid sessionId, string status, string message)
    {
        return new AutoAssignmentResultDto
        {
            SessionId = sessionId,
            IsAssigned = false,
            Status = status,
            Message = message
        };
    }
}
