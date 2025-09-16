namespace WeatherApp.Api.Features.WeatherForecast;

public record ForecastDailyDto(
    DateOnly Date,
    string HumidityAvg,
    decimal TemperatureMaxC,
    decimal TemperatureMinC,
    string WeatherCondition,
    string WindSpeedMax,
    DateTimeOffset Sunset,
    DateTimeOffset Sunrise);