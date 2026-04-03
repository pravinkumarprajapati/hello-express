namespace TMS.Application.Contracts;

public interface ISmsSender
{
    Task<(bool IsSuccess, string ProviderResponse)> SendAsync(string to, string message, CancellationToken cancellationToken = default);
}
