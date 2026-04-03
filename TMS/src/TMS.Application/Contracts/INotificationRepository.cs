using TMS.Domain.Entities;

namespace TMS.Application.Contracts;

public interface INotificationRepository
{
    Task<NotificationConfiguration?> GetConfigurationAsync(string eventKey, CancellationToken cancellationToken = default);
    Task UpsertConfigurationAsync(NotificationConfiguration configuration, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<NotificationTemplate>> GetTemplatesAsync(string eventKey, CancellationToken cancellationToken = default);
    Task SaveTemplateAsync(NotificationTemplate template, CancellationToken cancellationToken = default);
    Task SaveLogAsync(NotificationLog log, CancellationToken cancellationToken = default);
}
