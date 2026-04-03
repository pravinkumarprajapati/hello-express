using TMS.Application.Contracts;

namespace TMS.Infrastructure.Repositories;

/// <summary>
/// Placeholder SendGrid adapter.
/// </summary>
public class SendGridEmailSender : IEmailSender
{
    public Task<(bool IsSuccess, string ProviderResponse)> SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        return Task.FromResult((true, "Simulated SendGrid delivery"));
    }
}
