using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using TMS.Api.Security;

namespace TMS.IntegrationTests;

public class AuthorizationPolicyIntegrationTests
{
    [Theory]
    [InlineData("TrainerPolicy", new[] { "Trainer", "Manager", "Admin" })]
    [InlineData("ManagerPolicy", new[] { "Manager", "Admin" })]
    [InlineData("AdminPolicy", new[] { "Admin" })]
    public async Task Policies_ShouldContainExpectedRoles(string policyName, string[] expectedRoles)
    {
        var services = new ServiceCollection();
        services.AddTmsAuthorizationPolicies();

        IServiceProvider provider = services.BuildServiceProvider();
        var policyProvider = provider.GetRequiredService<IAuthorizationPolicyProvider>();

        AuthorizationPolicy? policy = await policyProvider.GetPolicyAsync(policyName);

        Assert.NotNull(policy);
        var roles = policy!.Requirements
            .OfType<RolesAuthorizationRequirement>()
            .SelectMany(r => r.AllowedRoles)
            .Distinct()
            .OrderBy(x => x)
            .ToArray();

        Assert.Equal(expectedRoles.OrderBy(x => x).ToArray(), roles);
    }
}
