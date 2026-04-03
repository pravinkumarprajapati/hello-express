using Microsoft.Extensions.DependencyInjection;
using TMS.Application.Contracts;
using TMS.Infrastructure.Repositories;

namespace TMS.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<ITrainerRepository, InMemoryTrainerRepository>();
        services.AddSingleton<ITrainingScheduleRepository, InMemoryTrainingScheduleRepository>();
        services.AddSingleton<IHrmsLeaveProvider, InMemoryHrmsLeaveProvider>();
        return services;
    }
}
