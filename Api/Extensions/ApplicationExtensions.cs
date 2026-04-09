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

        // Scan validators from the Application assembly, not the API assembly
        services.AddValidatorsFromAssemblyContaining<CreateAnimalValidator>();

        return services;
    }
}
