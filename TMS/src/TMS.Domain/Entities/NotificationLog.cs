using TMS.Domain.Common;
using TMS.Domain.Enums;

namespace TMS.Domain.Entities;

/// <summary>
/// Stores notification delivery attempt audit details.
/// </summary>
public class NotificationLog : BaseAuditableEntity
{
    public string EventKey { get; set; } = string.Empty;
    public NotificationChannel Channel { get; set; }
    public string Recipient { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string ProviderResponse { get; set; } = string.Empty;
}
