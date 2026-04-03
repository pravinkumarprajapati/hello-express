using Microsoft.Extensions.DependencyInjection;
using TMS.Application.Contracts;
using TMS.Application.Services;

namespace TMS.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITrainerService, TrainerService>();
        services.AddScoped<IAssignmentEngineService, AssignmentEngineService>();
        services.AddScoped<IReassignmentService, ReassignmentService>();
        services.AddScoped<ILeaveSyncService, LeaveSyncService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddSingleton<ITelemetryEventPublisher, NoOpTelemetryEventPublisher>();
        return services;
    }
}
