using EasyCaching.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MinimalApiStarter.Endpoints;

public static class Endpoints
{
    public static void UseMyEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/weatherforecast", ForecastEndpointHandler)
            .WithName("GetWeatherForecast")
            .AllowAnonymous();
        
        app.MapGet("/api/getInfo", () =>
        {
            return $"server responded {DateTime.Now.ToLongTimeString()}";
        });

        app.MapGet("/api/test", ([FromQuery] int param) =>
        {
            return $"you entered {param}";
        });

        app.MapGet("/api/secret", [Authorize] () => { return "secret"; });

        app.MapGet("/api/admin", [Authorize(Roles = "admin")] () => { return "admin secret"; });

        app.MapGet("/api/admin2", [Authorize(Roles = "admin2")] () => { return "admin 2 secret"; });
    }

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private static async Task<IEnumerable<WeatherForecast>> ForecastEndpointHandler(HttpContext context, IEasyCachingProvider _provider)
    {
        {
            if (await _provider.ExistsAsync("forecast"))
            {
                var cachedForecast = await _provider.GetAsync<WeatherForecast[]>("forecast");
                return cachedForecast.Value;
            }

            var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateTime.Now.AddDays(index),
                    Random.Shared.Next(-20, 55),
                    Summaries[Random.Shared.Next(Summaries.Length)]
                )).ToArray();

            await _provider.SetAsync("forecast", forecast, TimeSpan.FromMinutes(1));

            return forecast;
        }
    }
}