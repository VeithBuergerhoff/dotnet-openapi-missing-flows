using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options => options.AddDocumentTransformer((document, _, _) =>
{
    var scheme = new OpenApiSecurityScheme()
    {
        BearerFormat = "JSON Web Token",
        Description = "Bearer authentication using a JWT.",
        Scheme = "bearer",
        Flows = new OpenApiOAuthFlows
        {
            Password = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri("https://localhost"),
                Scopes = new Dictionary<string, string>
                {
                    ["api"] = "API access"
                }
            }
        },
        Type = SecuritySchemeType.Http,
        Reference = new()
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme
        }
    };

    document.Components ??= new();
    document.Components.SecuritySchemes ??= new Dictionary<string, OpenApiSecurityScheme>();
    document.Components.SecuritySchemes[scheme.Reference.Id] = scheme;

    // Also register the scheme with the security requirements
    document.SecurityRequirements ??= [];
    document.SecurityRequirements.Add(new() { [scheme] = [] });
    return Task.CompletedTask;
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
