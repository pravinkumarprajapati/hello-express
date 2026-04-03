using TMS.Application.DTOs;

namespace TMS.Application.Contracts;

public interface INotificationService
{
    Task<NotificationDispatchResultDto> SendAsync(NotificationRequestDto request, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<NotificationTemplateDto>> GetTemplatesAsync(string eventKey, CancellationToken cancellationToken = default);
    Task SaveTemplateAsync(NotificationTemplateDto template, CancellationToken cancellationToken = default);
    Task<NotificationConfigurationDto> GetConfigurationAsync(string eventKey, CancellationToken cancellationToken = default);
    Task SaveConfigurationAsync(NotificationConfigurationDto configuration, CancellationToken cancellationToken = default);
}
