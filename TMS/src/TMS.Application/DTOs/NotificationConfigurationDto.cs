namespace TMS.Application.DTOs;

public class NotificationConfigurationDto
{
    public string EventKey { get; set; } = string.Empty;
    public bool EnableEmail { get; set; }
    public bool EnableSms { get; set; }
}
