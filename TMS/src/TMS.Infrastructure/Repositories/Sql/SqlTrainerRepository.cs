using Microsoft.EntityFrameworkCore;
using TMS.Application.Contracts;
using TMS.Domain.Entities;
using TMS.Infrastructure.Persistence;

namespace TMS.Infrastructure.Repositories.Sql;

/// <summary>
/// SQL implementation for trainer repository.
/// </summary>
public class SqlTrainerRepository : ITrainerRepository
{
    private readonly TmsDbContext _dbContext;

    public SqlTrainerRepository(TmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<TrainerProfile>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Trainers
            .Include(x => x.Skills)
            .Where(x => !x.IsDeleted)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<TrainerProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Trainers
            .Include(x => x.Skills)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
    }
}
