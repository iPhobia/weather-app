using System.Text.Json.Serialization;

namespace WeatherApp.Api.Features.WeatherForecast.Infrastructure.WeatherApi;

public record WeatherApiForecastResponse
{
    [JsonPropertyName("forecast")]
    public WeatherApiForecastDto Forecast { get; set; }
}

public record WeatherApiForecastDto
{
    [JsonPropertyName("forecastday")]
    public WeatherApiForecastDayDto[] ForecastDay { get; set; }
}


public record WeatherApiForecastDayDto
{
    [JsonPropertyName("date")]
    public DateOnly Date { get; set; }

    [JsonPropertyName("day")]
    public WeatherApiDayDto Day { get; init; }

    [JsonPropertyName("astro")]
    public WeatherApiAstroDto Astro { get; init; }
}

public record WeatherApiDayDto
{
    [JsonPropertyName("condition")]
    public WeatherApiConditionDto WeatherCondition { get; init; }

    [JsonPropertyName("maxtemp_c")]
    public decimal TemperatureMax { get; init; }

    [JsonPropertyName("mintemp_c")]
    public decimal TemperatureMin { get; init; }

    [JsonPropertyName("avghumidity")]
    public decimal Humidity { get; init; }

    [JsonPropertyName("maxwind_kph")]
    public decimal WindSpeedMax { get; init; }
}

public record WeatherApiAstroDto
{
    [JsonPropertyName("sunrise")]
    public string Sunrise { get; init; }

    [JsonPropertyName("sunset")]
    public string Sunset { get; init; }
}

public record WeatherApiConditionDto
{
    [JsonPropertyName("text")]
    public string Text { get; set; }
}