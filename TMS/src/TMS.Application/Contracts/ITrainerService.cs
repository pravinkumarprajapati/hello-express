using TMS.Application.DTOs;

namespace TMS.Application.Contracts;

public interface ITrainerService
{
    Task<IReadOnlyCollection<TrainerProfileDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TrainerProfileDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
