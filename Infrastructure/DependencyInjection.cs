using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WildTracker.Domain.Repositories;
using WildTracker.Infrastructure.Persistence;
using WildTracker.Infrastructure.Persistence.Repositories;

namespace WildTracker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

        services.AddScoped<IAnimalRepository, AnimalRepository>();
        services.AddScoped<ISightingReportRepository, SightingReportRepository>();
        services.AddScoped<IObservationNoteRepository, ObservationNoteRepository>();
        services.AddScoped<AppUserRepository, AppUserRepository>();

        return services;
    }
}
