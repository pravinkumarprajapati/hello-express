using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using TMS.Api.Security;

namespace TMS.IntegrationTests;

public class AuthenticatedFlowIntegrationTests
{
    [Fact]
    public async Task ManagerUser_ShouldSatisfy_ManagerPolicy()
    {
        ServiceProvider serviceProvider = BuildAuthorizationProvider();
        var authorizationService = serviceProvider.GetRequiredService<IAuthorizationService>();

        var principal = BuildPrincipal("manager.user@company.com", "Manager");
        AuthorizationResult result = await authorizationService.AuthorizeAsync(principal, null, "ManagerPolicy");

        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task TrainerUser_ShouldNotSatisfy_AdminPolicy()
    {
        ServiceProvider serviceProvider = BuildAuthorizationProvider();
        var authorizationService = serviceProvider.GetRequiredService<IAuthorizationService>();

        var principal = BuildPrincipal("trainer.user@company.com", "Trainer");
        AuthorizationResult result = await authorizationService.AuthorizeAsync(principal, null, "AdminPolicy");

        Assert.False(result.Succeeded);
    }

    private static ServiceProvider BuildAuthorizationProvider()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddTmsAuthorizationPolicies();
        services.AddAuthorizationCore();
        return services.BuildServiceProvider();
    }

    private static ClaimsPrincipal BuildPrincipal(string userName, string role)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Role, role)
        };

        var identity = new ClaimsIdentity(claims, "TestAuthType");
        return new ClaimsPrincipal(identity);
    }
}
