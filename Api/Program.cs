using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using WildTracker.API.Extensions;
using WildTracker.API.Middleware;
using WildTracker.Infrastructure;
using WildTracker.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(o =>
        o.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        var allowedOrigins = builder.Configuration
            .GetSection("AllowedOrigins")
            .Get<string[]>() ?? ["http://localhost:5173"];

        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Ensure the database schema exists before the app starts accepting requests.
// Retries handle the race condition where the API starts before PostgreSQL is ready.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    for (var attempt = 0; attempt < 5; attempt++)
    {
        try
        {
            await db.Database.EnsureCreatedAsync();
            break;
        }
        catch (Exception ex)
        {
            if (attempt == 4) throw;
            app.Logger.LogWarning(ex, "Database not ready, retrying in 3 s (attempt {Attempt}/5)…", attempt + 1);
            await Task.Delay(TimeSpan.FromSeconds(3));
        }
    }
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapOpenApi();
app.MapScalarApiReference();
app.UseCors("Frontend");
app.MapControllers();

app.Run();

public partial class Program { }
