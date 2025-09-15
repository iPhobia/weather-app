using System.ComponentModel.DataAnnotations;

public record WeatherApiOptions
{
    [Required, Url]
    public string ForecastEndpoint { get; init; }

    [Required]
    public string ApiKey { get; init; }
}