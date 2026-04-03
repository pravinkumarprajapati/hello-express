using Microsoft.ApplicationInsights;

namespace TMS.Api.Middleware;

/// <summary>
/// Adds request-level telemetry enrichment for API calls.
/// </summary>
public class TelemetryEnrichmentMiddleware
{
    private readonly RequestDelegate _next;

    public TelemetryEnrichmentMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, TelemetryClient telemetryClient)
    {
        var properties = new Dictionary<string, string>
        {
            ["Path"] = context.Request.Path,
            ["Method"] = context.Request.Method,
            ["User"] = context.User.Identity?.Name ?? "Anonymous"
        };

        telemetryClient.TrackEvent("Api.Request.Started", properties);
        await _next(context);

        properties["StatusCode"] = context.Response.StatusCode.ToString();
        telemetryClient.TrackEvent("Api.Request.Completed", properties);
    }
}
