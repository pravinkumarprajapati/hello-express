using TMS.Application.Contracts;
using TMS.Domain.Entities;
using TMS.Domain.Enums;

namespace TMS.Infrastructure.Repositories;

/// <summary>
/// Temporary in-memory implementation used during Module 1 bootstrap.
/// This will be replaced by EF Core + SQL Server repository in subsequent module.
/// </summary>
public class InMemoryTrainerRepository : ITrainerRepository
{
    private static readonly List<TrainerProfile> Trainers =
    [
        new TrainerProfile
        {
            Id = Guid.Parse("d6a9f5f3-95bb-46f9-b71e-f2763d4dce1b"),
            EmployeeCode = "EMP001",
            FirstName = "Aarav",
            LastName = "Sharma",
            Email = "aarav.sharma@example.com",
            Gender = GenderType.Male,
            Department = "Aircraft Training",
            CoreCompetencies = "A320 Systems",
            AreasOfExpertise = "Hydraulics, Electrical",
            BaseLocation = "Delhi",
            SeniorityInYears = 9,
            IsAepAuthorized = true,
            Skills =
            [
                new TrainerSkill
                {
                    SkillName = "A320 Hydraulics",
                    Department = "Aircraft Training",
                    ProficiencyLevel = ProficiencyLevel.Expert,
                    IsExaminer = true,
                    IsAuditor = false,
                    IsCrossSkilled = false
                },
                new TrainerSkill
                {
                    SkillName = "A320 Electrical",
                    Department = "Aircraft Training",
                    ProficiencyLevel = ProficiencyLevel.Advanced,
                    IsExaminer = false,
                    IsAuditor = true,
                    IsCrossSkilled = true
                }
            ]
        },
        new TrainerProfile
        {
            Id = Guid.Parse("61719f66-42e6-4e2b-8ab8-535974f68e30"),
            EmployeeCode = "EMP002",
            FirstName = "Meera",
            LastName = "Nair",
            Email = "meera.nair@example.com",
            Gender = GenderType.Female,
            Department = "Safety",
            CoreCompetencies = "Safety Procedures",
            AreasOfExpertise = "Emergency Response",
            BaseLocation = "Mumbai",
            SeniorityInYears = 7,
            IsAepAuthorized = false,
            Skills =
            [
                new TrainerSkill
                {
                    SkillName = "Emergency Evacuation",
                    Department = "Safety",
                    ProficiencyLevel = ProficiencyLevel.Expert,
                    IsExaminer = false,
                    IsAuditor = true,
                    IsCrossSkilled = false
                }
            ]
        }
    ];

    public Task<IReadOnlyCollection<TrainerProfile>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult((IReadOnlyCollection<TrainerProfile>)Trainers);
    }

    public Task<TrainerProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        TrainerProfile? trainer = Trainers.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(trainer);
    }
}
