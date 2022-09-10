using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace minimalApiStarter.Endpoints;

public static class Endpoints
{
    public static void UseMyEndpoints(this WebApplication app)
    {
        app.MapGet("/weatherforecast", ForecastEndpointHandler)
            .WithName("GetWeatherForecast")
            .AllowAnonymous();
         
        app.MapGet("/test", ([FromQuery]int param) => { return $"you entered {param}"; });

        app.MapGet("/secret", [Authorize]() => { return "secret"; });

        app.MapGet("/admin", [Authorize(Roles = "admin")]() => { return "admin secret"; });
        
        app.MapGet("/admin2", [Authorize(Roles = "admin2")]() => { return "admin 2 secret"; });
    }

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private static IEnumerable<WeatherForecast> ForecastEndpointHandler(HttpContext context){
        {
            var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateTime.Now.AddDays(index),
                    Random.Shared.Next(-20, 55),
                    Summaries[Random.Shared.Next(Summaries.Length)]
                ));
            
            return forecast;
        }
    }
}