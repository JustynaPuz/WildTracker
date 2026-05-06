using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using WildTracker.Application.Interfaces;
using WildTracker.Application.Services;
using WildTracker.Application.Validators;

namespace WildTracker.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAnimalService, AnimalService>();
        services.AddScoped<ISightingReportService, SightingReportService>();
        services.AddScoped<IObservationNoteService, ObservationNoteService>();
        services.AddScoped<IStatsService, StatsService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddValidatorsFromAssemblyContaining<CreateAnimalValidator>();

        return services;
    }
}
