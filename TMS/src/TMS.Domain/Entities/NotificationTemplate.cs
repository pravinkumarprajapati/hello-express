using TMS.Domain.Common;
using TMS.Domain.Enums;

namespace TMS.Domain.Entities;

/// <summary>
/// Stores admin-editable notification template content.
/// </summary>
public class NotificationTemplate : BaseAuditableEntity
{
    public string EventKey { get; set; } = string.Empty;
    public NotificationChannel Channel { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public bool IsEnabled { get; set; } = true;
}
