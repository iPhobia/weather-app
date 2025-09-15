using System.Text.Json.Serialization;

public record OpenMeteoWeatherForecastResponse
{
    [JsonPropertyName("daily")]
    public OpenMeteoDaily Daily { get; init; }
}

public record OpenMeteoDaily
{
    [JsonPropertyName("time")]
    public DateOnly[] Time { get; set; }

    [JsonPropertyName("weather_code")]
    public WeatherCode[] WeatherCode { get; init; } = [];

    [JsonPropertyName("temperature_2m_max")]
    public decimal[] TemperatureMax { get; init; } = [];

    [JsonPropertyName("temperature_2m_min")]
    public decimal[] TemperatureMin { get; init; } = [];

    [JsonPropertyName("relative_humidity_2m_mean")]
    public decimal[] Humidity { get; init; } = [];

    [JsonPropertyName("sunrise")]
    public DateTimeOffset[] Sunrise { get; init; } = [];

    [JsonPropertyName("sunset")]
    public DateTimeOffset[] Sunset { get; init; } = [];

    [JsonPropertyName("wind_speed_10m_max")]
    public decimal[] WindSpeedMax { get; init; } = [];
}
    