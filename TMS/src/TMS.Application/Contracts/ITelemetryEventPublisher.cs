namespace TMS.Application.Contracts;

public interface ITelemetryEventPublisher
{
    void TrackEvent(string eventName, IDictionary<string, string>? properties = null);
}
