using TMS.Domain.Common;

namespace TMS.Domain.Entities;

/// <summary>
/// Stores per-event notification toggle configuration controlled by admin.
/// </summary>
public class NotificationConfiguration : BaseAuditableEntity
{
    public string EventKey { get; set; } = string.Empty;
    public bool EnableEmail { get; set; } = true;
    public bool EnableSms { get; set; } = true;
}
