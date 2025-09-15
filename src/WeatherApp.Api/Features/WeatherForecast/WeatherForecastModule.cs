using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

public class WeatherForecastModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/weatherforecast", async (
            DateOnly date,
            string city,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetWeatherForecastQuery(date, city), ct);
            return Results.Ok(result);
        });
    }
}