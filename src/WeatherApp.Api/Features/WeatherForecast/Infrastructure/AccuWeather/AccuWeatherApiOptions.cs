using System.ComponentModel.DataAnnotations;

public record AccuWeatherApiOptions
{
    [Required, Url]
    public string ForecastEndpoint { get; init; }
    
    [Required, Url]
    public string LocationEndpoint { get; init; }

    [Required]
    public string ApiKey { get; init; }
}