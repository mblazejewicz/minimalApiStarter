
using Microsoft.AspNetCore.Authorization;

public static class MyEndpoints
{
    public static void UseMyEndpoints(this WebApplication app)
    {

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", [AllowAnonymous]() =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/secret", [Authorize]()=>{
    return "secret";
});


app.MapGet("/admin", [Authorize(Roles="admin")]()=>{
    return "admin secret";
});


app.MapGet("/admin2", [Authorize(Roles="admin2")]()=>{
    return "admin 2 secret";
});
    }
    }
