using System.ComponentModel.DataAnnotations;

namespace WeatherApp.Api.Features.WeatherForecast.Infrastructure.WeatherApi;

public record WeatherApiOptions
{
    [Required, Url]
    public string ForecastEndpoint { get; init; }

    [Required]
    public string ApiKey { get; init; }
}