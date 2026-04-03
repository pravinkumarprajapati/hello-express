using TMS.Domain.Entities;

namespace TMS.Application.Contracts;

/// <summary>
/// Repository abstraction for trainer profile persistence.
/// </summary>
public interface ITrainerRepository
{
    Task<IReadOnlyCollection<TrainerProfile>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TrainerProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
