using Carter;
using Carter.ModelBinding;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;

namespace WeatherApp.Api.Features.WeatherForecast;


public class WeatherForecastModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/weatherforecast", async (
            [AsParameters] WeatherForecastQuery query,
            IMediator mediator,
            HybridCache cache,
            IConfiguration config,
            IValidator<WeatherForecastQuery> validator,
            CancellationToken ct) =>
        {
            var validationResult = validator.Validate(query);

            if (!validationResult.IsValid)
            {
                var problemDetails = new ValidationProblemDetails(validationResult.GetValidationProblems())
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "One or more validation errors occurred."
                };

                return Results.BadRequest(problemDetails);
            }

            var cacheKey = $"forecast:{query.City}:{query.ForecastDays}";

            var forecast = await cache.GetOrCreateAsync(
                cacheKey,
                async token => await mediator.Send(new GetWeatherForecastQuery(query.ForecastDays, query.City), ct),
                options: new HybridCacheEntryOptions
                {
                    Expiration = TimeSpan.FromMinutes(config.GetValue<long>("CacheConfiguration:WeatherForecastCacheTTL"))
                },
                cancellationToken: ct);

            return Results.Ok(forecast);
        });
    }
}