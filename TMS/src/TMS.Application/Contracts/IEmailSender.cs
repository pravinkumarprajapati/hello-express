namespace TMS.Application.Contracts;

public interface IEmailSender
{
    Task<(bool IsSuccess, string ProviderResponse)> SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
}
