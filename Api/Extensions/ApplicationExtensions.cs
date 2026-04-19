using FluentValidation;
using WildTracker.Application.Interfaces;
using WildTracker.Application.Services;
using WildTracker.Application.Validators;

namespace WildTracker.API.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAnimalService, AnimalService>();
        services.AddScoped<ISightingReportService, SightingReportService>();
        services.AddScoped<IObservationNoteService, ObservationNoteService>();
        services.AddScoped<IStatsService, StatsService>();
        services.AddScoped<IUserService, UserService>();

        services.AddValidatorsFromAssemblyContaining<CreateAnimalValidator>();

        return services;
    }
}
