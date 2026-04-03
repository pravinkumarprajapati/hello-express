namespace TMS.Application.DTOs;

public class NotificationDispatchResultDto
{
    public string EventKey { get; set; } = string.Empty;
    public bool EmailAttempted { get; set; }
    public bool SmsAttempted { get; set; }
    public bool EmailSent { get; set; }
    public bool SmsSent { get; set; }
}
