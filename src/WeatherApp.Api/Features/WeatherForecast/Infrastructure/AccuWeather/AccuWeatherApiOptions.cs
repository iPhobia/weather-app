using System.ComponentModel.DataAnnotations;

namespace WeatherApp.Api.Features.WeatherForecast.Infrastructure.AccuWeather;

public record AccuWeatherApiOptions
{
    [Required, Url]
    public string ForecastEndpoint { get; init; }

    [Required, Url]
    public string LocationEndpoint { get; init; }

    [Required]
    public string ApiKey { get; init; }
}