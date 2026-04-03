using TMS.Application.Contracts;
using TMS.Domain.Entities;
using TMS.Domain.Enums;

namespace TMS.Infrastructure.Repositories;

/// <summary>
/// In-memory repository for notification templates, configurations and logs.
/// </summary>
public class InMemoryNotificationRepository : INotificationRepository
{
    private static readonly List<NotificationTemplate> Templates =
    [
        new NotificationTemplate
        {
            EventKey = "AssignmentChanged",
            Channel = NotificationChannel.Email,
            Subject = "Training Plan Updated",
            Body = "Your schedule changed: {{data}}",
            IsEnabled = true
        },
        new NotificationTemplate
        {
            EventKey = "AssignmentChanged",
            Channel = NotificationChannel.Sms,
            Subject = string.Empty,
            Body = "Plan changed: {{data}}",
            IsEnabled = true
        }
    ];

    private static readonly List<NotificationConfiguration> Configurations =
    [
        new NotificationConfiguration
        {
            EventKey = "AssignmentChanged",
            EnableEmail = true,
            EnableSms = true
        }
    ];

    private static readonly List<NotificationLog> Logs = [];

    public Task<NotificationConfiguration?> GetConfigurationAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        NotificationConfiguration? config = Configurations.FirstOrDefault(x => x.EventKey == eventKey);
        return Task.FromResult(config);
    }

    public Task UpsertConfigurationAsync(NotificationConfiguration configuration, CancellationToken cancellationToken = default)
    {
        NotificationConfiguration? existing = Configurations.FirstOrDefault(x => x.EventKey == configuration.EventKey);
        if (existing is null)
        {
            Configurations.Add(configuration);
        }
        else
        {
            existing.EnableEmail = configuration.EnableEmail;
            existing.EnableSms = configuration.EnableSms;
        }

        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<NotificationTemplate>> GetTemplatesAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<NotificationTemplate> templates = Templates.Where(x => x.EventKey == eventKey).ToArray();
        return Task.FromResult(templates);
    }

    public Task SaveTemplateAsync(NotificationTemplate template, CancellationToken cancellationToken = default)
    {
        NotificationTemplate? existing = Templates.FirstOrDefault(x => x.Id == template.Id ||
            (x.EventKey == template.EventKey && x.Channel == template.Channel));

        if (existing is null)
        {
            Templates.Add(template);
        }
        else
        {
            existing.Subject = template.Subject;
            existing.Body = template.Body;
            existing.IsEnabled = template.IsEnabled;
        }

        return Task.CompletedTask;
    }

    public Task SaveLogAsync(NotificationLog log, CancellationToken cancellationToken = default)
    {
        Logs.Add(log);
        return Task.CompletedTask;
    }
}
