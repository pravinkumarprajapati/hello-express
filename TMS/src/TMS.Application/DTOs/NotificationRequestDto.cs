namespace TMS.Application.DTOs;

public class NotificationRequestDto
{
    public string EventKey { get; set; } = string.Empty;
    public string RecipientEmail { get; set; } = string.Empty;
    public string RecipientPhone { get; set; } = string.Empty;
    public string TemplateData { get; set; } = string.Empty;
}
