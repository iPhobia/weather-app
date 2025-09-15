using System.ComponentModel.DataAnnotations;

public class OpenMeteoApiOptions
{
    [Required, Url]
    public string WeatherForecastEndpoint { get; init; }

    [Required, Url]
    public string GeocodingEndpoint { get; init; }
}