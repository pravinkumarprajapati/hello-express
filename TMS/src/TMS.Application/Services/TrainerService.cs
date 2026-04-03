using TMS.Application.Contracts;
using TMS.Application.DTOs;
using TMS.Domain.Entities;

namespace TMS.Application.Services;

/// <summary>
/// Service used by API/UI to retrieve trainer profile data.
/// </summary>
public class TrainerService : ITrainerService
{
    private readonly ITrainerRepository _trainerRepository;

    public TrainerService(ITrainerRepository trainerRepository)
    {
        _trainerRepository = trainerRepository;
    }

    public async Task<IReadOnlyCollection<TrainerProfileDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<TrainerProfile> trainers = await _trainerRepository.GetAllAsync(cancellationToken);

        return trainers
            .Select(MapToDto)
            .ToArray();
    }

    public async Task<TrainerProfileDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        TrainerProfile? trainer = await _trainerRepository.GetByIdAsync(id, cancellationToken);
        return trainer is null ? null : MapToDto(trainer);
    }

    private static TrainerProfileDto MapToDto(TrainerProfile trainer)
    {
        return new TrainerProfileDto
        {
            Id = trainer.Id,
            EmployeeCode = trainer.EmployeeCode,
            FullName = trainer.FullName,
            Email = trainer.Email,
            Department = trainer.Department,
            CoreCompetencies = trainer.CoreCompetencies,
            AreasOfExpertise = trainer.AreasOfExpertise,
            BaseLocation = trainer.BaseLocation,
            SeniorityInYears = trainer.SeniorityInYears,
            IsAepAuthorized = trainer.IsAepAuthorized,
            Skills = trainer.Skills.Select(skill => new TrainerSkillDto
            {
                SkillName = skill.SkillName,
                Department = skill.Department,
                ProficiencyLevel = skill.ProficiencyLevel.ToString(),
                IsExaminer = skill.IsExaminer,
                IsAuditor = skill.IsAuditor,
                IsCrossSkilled = skill.IsCrossSkilled
            }).ToArray()
        };
    }
}
