using WildTracker.Application;

namespace WildTracker.API.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
        => services.AddApplicationServices();
}
