using Carter;
using MediatR;

public class WeatherForecastModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/weatherforecast", async (
            int forecastDays,
            string city,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetWeatherForecastQuery(forecastDays, city), ct);
            return Results.Ok(result);
        });
    }
}