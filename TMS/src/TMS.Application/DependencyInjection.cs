using Microsoft.Extensions.DependencyInjection;
using TMS.Application.Contracts;
using TMS.Application.Services;

namespace TMS.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITrainerService, TrainerService>();
        return services;
    }
}
