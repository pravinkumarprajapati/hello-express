using TMS.Application.Contracts;
using TMS.Application.DTOs;
using TMS.Domain.Entities;
using TMS.Domain.Enums;

namespace TMS.Application.Services;

/// <summary>
/// Sends notifications using template/configuration and records delivery audit logs.
/// </summary>
public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IEmailSender _emailSender;
    private readonly ISmsSender _smsSender;

    public NotificationService(
        INotificationRepository notificationRepository,
        IEmailSender emailSender,
        ISmsSender smsSender)
    {
        _notificationRepository = notificationRepository;
        _emailSender = emailSender;
        _smsSender = smsSender;
    }

    public async Task<NotificationDispatchResultDto> SendAsync(NotificationRequestDto request, CancellationToken cancellationToken = default)
    {
        NotificationConfiguration? configuration = await _notificationRepository.GetConfigurationAsync(request.EventKey, cancellationToken);
        configuration ??= new NotificationConfiguration
        {
            EventKey = request.EventKey,
            EnableEmail = true,
            EnableSms = true
        };

        IReadOnlyCollection<NotificationTemplate> templates = await _notificationRepository.GetTemplatesAsync(request.EventKey, cancellationToken);

        NotificationTemplate? emailTemplate = templates.FirstOrDefault(x => x.Channel == NotificationChannel.Email && x.IsEnabled);
        NotificationTemplate? smsTemplate = templates.FirstOrDefault(x => x.Channel == NotificationChannel.Sms && x.IsEnabled);

        var result = new NotificationDispatchResultDto { EventKey = request.EventKey };

        if (configuration.EnableEmail && !string.IsNullOrWhiteSpace(request.RecipientEmail) && emailTemplate is not null)
        {
            result.EmailAttempted = true;
            string body = ApplyTemplate(emailTemplate.Body, request.TemplateData);
            var response = await _emailSender.SendAsync(request.RecipientEmail, emailTemplate.Subject, body, cancellationToken);
            result.EmailSent = response.IsSuccess;

            await _notificationRepository.SaveLogAsync(new NotificationLog
            {
                EventKey = request.EventKey,
                Channel = NotificationChannel.Email,
                Recipient = request.RecipientEmail,
                Message = body,
                Status = response.IsSuccess ? "Sent" : "Failed",
                ProviderResponse = response.ProviderResponse
            }, cancellationToken);
        }

        if (configuration.EnableSms && !string.IsNullOrWhiteSpace(request.RecipientPhone) && smsTemplate is not null)
        {
            result.SmsAttempted = true;
            string message = ApplyTemplate(smsTemplate.Body, request.TemplateData);
            var response = await _smsSender.SendAsync(request.RecipientPhone, message, cancellationToken);
            result.SmsSent = response.IsSuccess;

            await _notificationRepository.SaveLogAsync(new NotificationLog
            {
                EventKey = request.EventKey,
                Channel = NotificationChannel.Sms,
                Recipient = request.RecipientPhone,
                Message = message,
                Status = response.IsSuccess ? "Sent" : "Failed",
                ProviderResponse = response.ProviderResponse
            }, cancellationToken);
        }

        return result;
    }

    public async Task<IReadOnlyCollection<NotificationTemplateDto>> GetTemplatesAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<NotificationTemplate> templates = await _notificationRepository.GetTemplatesAsync(eventKey, cancellationToken);
        return templates.Select(x => new NotificationTemplateDto
        {
            Id = x.Id,
            EventKey = x.EventKey,
            Channel = x.Channel.ToString(),
            Subject = x.Subject,
            Body = x.Body,
            IsEnabled = x.IsEnabled
        }).ToArray();
    }

    public Task SaveTemplateAsync(NotificationTemplateDto template, CancellationToken cancellationToken = default)
    {
        var entity = new NotificationTemplate
        {
            Id = template.Id == Guid.Empty ? Guid.NewGuid() : template.Id,
            EventKey = template.EventKey,
            Channel = Enum.TryParse<NotificationChannel>(template.Channel, true, out var channel) ? channel : NotificationChannel.Email,
            Subject = template.Subject,
            Body = template.Body,
            IsEnabled = template.IsEnabled
        };

        return _notificationRepository.SaveTemplateAsync(entity, cancellationToken);
    }

    public async Task<NotificationConfigurationDto> GetConfigurationAsync(string eventKey, CancellationToken cancellationToken = default)
    {
        NotificationConfiguration? config = await _notificationRepository.GetConfigurationAsync(eventKey, cancellationToken);
        if (config is null)
        {
            return new NotificationConfigurationDto
            {
                EventKey = eventKey,
                EnableEmail = true,
                EnableSms = true
            };
        }

        return new NotificationConfigurationDto
        {
            EventKey = config.EventKey,
            EnableEmail = config.EnableEmail,
            EnableSms = config.EnableSms
        };
    }

    public Task SaveConfigurationAsync(NotificationConfigurationDto configuration, CancellationToken cancellationToken = default)
    {
        var entity = new NotificationConfiguration
        {
            EventKey = configuration.EventKey,
            EnableEmail = configuration.EnableEmail,
            EnableSms = configuration.EnableSms
        };

        return _notificationRepository.UpsertConfigurationAsync(entity, cancellationToken);
    }

    private static string ApplyTemplate(string templateBody, string templateData)
    {
        return templateBody.Replace("{{data}}", templateData, StringComparison.OrdinalIgnoreCase);
    }
}
