using TMS.Application.Contracts;

namespace TMS.Infrastructure.Repositories;

/// <summary>
/// Placeholder Azure Communication Services SMS adapter.
/// </summary>
public class AzureCommunicationSmsSender : ISmsSender
{
    public Task<(bool IsSuccess, string ProviderResponse)> SendAsync(string to, string message, CancellationToken cancellationToken = default)
    {
        return Task.FromResult((true, "Simulated ACS SMS delivery"));
    }
}
