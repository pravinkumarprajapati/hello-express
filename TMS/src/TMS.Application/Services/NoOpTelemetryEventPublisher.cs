using TMS.Application.Contracts;

namespace TMS.Application.Services;

/// <summary>
/// Default no-op telemetry publisher used for local/test scenarios.
/// </summary>
public class NoOpTelemetryEventPublisher : ITelemetryEventPublisher
{
    public void TrackEvent(string eventName, IDictionary<string, string>? properties = null)
    {
    }
}
