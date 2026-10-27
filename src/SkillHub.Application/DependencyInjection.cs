using Microsoft.Extensions.DependencyInjection;
using SkillHub.Application.Interface;
using SkillHub.Application.Service;

namespace SkillHub.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IWorkshopService, WorkshopService>();
        services.AddScoped<IEnrollmentService, EnrollmentService>();

        return services;
    }
}
