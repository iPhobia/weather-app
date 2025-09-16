using System.ComponentModel.DataAnnotations;

namespace WeatherApp.Api.Features.WeatherForecast.Infrastructure.OpenMeteo;

public record OpenMeteoApiOptions
{
    [Required, Url]
    public string ForecastEndpoint { get; init; }

    [Required, Url]
    public string GeocodingEndpoint { get; init; }
}