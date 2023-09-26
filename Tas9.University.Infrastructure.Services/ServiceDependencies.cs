using Microsoft.Extensions.DependencyInjection;
using Task9.University.Services.Interfaces;

namespace Task9.University.Infrastructure.Services;
public static class ServiceDependencies
{
    public static IServiceCollection GetServiceDependencies(this IServiceCollection services)
    {
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IGroupService, GroupService>();
        services.AddScoped<ICourseService, CourseService>();

        return services;
    }
}
