using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Task9.University.Domain.Interfaces;
using Task9.University.Infrastructure.Data.Repos;

namespace Task9.University.Infrastructure.Data;
public static class DataDependencies
{
    public static IServiceCollection GetRepositoryDependencies(this IServiceCollection services)
    {
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();

        return services;
    }

    public static IServiceCollection AddApplicationDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddSqlServer<AppDbContext>(connectionString);

        return services;
    }
}
