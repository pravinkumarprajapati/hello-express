using Microsoft.EntityFrameworkCore;
using TMS.Application.Contracts;
using TMS.Domain.Entities;
using TMS.Infrastructure.Persistence;

namespace TMS.Infrastructure.Repositories.Sql;

/// <summary>
/// SQL implementation for notification repository.
/// </summary>
public class SqlNotificationRepository : INotificationRepository
{
    private readonly TmsDbContext _dbContext;

    public SqlNotificationRepository(TmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<NotificationConfiguration?> GetConfigurationAsync(string eventKey, CancellationToken cancellationToken = default)
        => _dbContext.NotificationConfigurations.FirstOrDefaultAsync(x => x.EventKey == eventKey && !x.IsDeleted, cancellationToken)!;

    public async Task UpsertConfigurationAsync(NotificationConfiguration configuration, CancellationToken cancellationToken = default)
    {
        NotificationConfiguration? existing = await _dbContext.NotificationConfigurations
            .FirstOrDefaultAsync(x => x.EventKey == configuration.EventKey && !x.IsDeleted, cancellationToken);

        if (existing is null)
        {
            await _dbContext.NotificationConfigurations.AddAsync(configuration, cancellationToken);
        }
        else
        {
            existing.EnableEmail = configuration.EnableEmail;
            existing.EnableSms = configuration.EnableSms;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<NotificationTemplate>> GetTemplatesAsync(string eventKey, CancellationToken cancellationToken = default)
        => await _dbContext.NotificationTemplates.Where(x => x.EventKey == eventKey && !x.IsDeleted).ToArrayAsync(cancellationToken);

    public async Task SaveTemplateAsync(NotificationTemplate template, CancellationToken cancellationToken = default)
    {
        NotificationTemplate? existing = await _dbContext.NotificationTemplates
            .FirstOrDefaultAsync(x => x.EventKey == template.EventKey && x.Channel == template.Channel && !x.IsDeleted, cancellationToken);

        if (existing is null)
        {
            await _dbContext.NotificationTemplates.AddAsync(template, cancellationToken);
        }
        else
        {
            existing.Subject = template.Subject;
            existing.Body = template.Body;
            existing.IsEnabled = template.IsEnabled;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveLogAsync(NotificationLog log, CancellationToken cancellationToken = default)
    {
        await _dbContext.NotificationLogs.AddAsync(log, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
