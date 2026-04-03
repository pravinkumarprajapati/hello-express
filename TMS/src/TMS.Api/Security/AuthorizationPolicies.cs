using Microsoft.AspNetCore.Authorization;

namespace TMS.Api.Security;

/// <summary>
/// Centralized role-based authorization policy registration for TMS.
/// </summary>
public static class AuthorizationPolicies
{
    public static IServiceCollection AddTmsAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("TrainerPolicy", policy => policy.RequireRole("Trainer", "Manager", "Admin"));
            options.AddPolicy("ManagerPolicy", policy => policy.RequireRole("Manager", "Admin"));
            options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
        });

        return services;
    }
}
