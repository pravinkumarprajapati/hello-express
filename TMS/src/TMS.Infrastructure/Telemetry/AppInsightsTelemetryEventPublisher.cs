using Microsoft.ApplicationInsights;
using TMS.Application.Contracts;

namespace TMS.Infrastructure.Telemetry;

/// <summary>
/// Publishes custom business telemetry events to Application Insights.
/// </summary>
public class AppInsightsTelemetryEventPublisher : ITelemetryEventPublisher
{
    private readonly TelemetryClient _telemetryClient;

    public AppInsightsTelemetryEventPublisher(TelemetryClient telemetryClient)
    {
        _telemetryClient = telemetryClient;
    }

    public void TrackEvent(string eventName, IDictionary<string, string>? properties = null)
    {
        _telemetryClient.TrackEvent(eventName, properties);
    }
}
