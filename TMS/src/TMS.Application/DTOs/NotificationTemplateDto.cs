namespace TMS.Application.DTOs;

public class NotificationTemplateDto
{
    public Guid Id { get; set; }
    public string EventKey { get; set; } = string.Empty;
    public string Channel { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
}
