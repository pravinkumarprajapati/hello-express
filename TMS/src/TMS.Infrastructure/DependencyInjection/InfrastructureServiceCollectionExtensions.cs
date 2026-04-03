using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TMS.Application.Contracts;
using TMS.Infrastructure.Persistence;
using TMS.Infrastructure.Repositories;
using TMS.Infrastructure.Repositories.Sql;
using TMS.Infrastructure.Telemetry;
using Microsoft.Extensions.DependencyInjection;
using TMS.Application.Contracts;
using TMS.Infrastructure.Repositories;

namespace TMS.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("TmsDb")
            ?? "Server=(localdb)\\MSSQLLocalDB;Database=TmsDb;Trusted_Connection=True;MultipleActiveResultSets=true";

        services.AddDbContext<TmsDbContext>(options => options.UseSqlServer(connectionString));

        bool useInMemoryRepositories = configuration.GetValue("UseInMemoryRepositories", true);

        if (useInMemoryRepositories)
        {
            services.AddSingleton<ITrainerRepository, InMemoryTrainerRepository>();
            services.AddSingleton<ITrainingScheduleRepository, InMemoryTrainingScheduleRepository>();
            services.AddSingleton<IHrmsLeaveProvider, InMemoryHrmsLeaveProvider>();
            services.AddSingleton<INotificationRepository, InMemoryNotificationRepository>();
        }
        else
        {
            services.AddScoped<ITrainerRepository, SqlTrainerRepository>();
            services.AddScoped<ITrainingScheduleRepository, SqlTrainingScheduleRepository>();
            services.AddScoped<INotificationRepository, SqlNotificationRepository>();
            services.AddSingleton<IHrmsLeaveProvider, InMemoryHrmsLeaveProvider>();
        }

        services.AddSingleton<IEmailSender, SendGridEmailSender>();
        services.AddSingleton<ISmsSender, AzureCommunicationSmsSender>();
        services.AddSingleton<ITelemetryEventPublisher, AppInsightsTelemetryEventPublisher>();
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<ITrainerRepository, InMemoryTrainerRepository>();
        return services;
    }
}
