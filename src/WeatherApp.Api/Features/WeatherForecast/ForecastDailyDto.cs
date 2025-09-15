public record ForecastDailyDto(
    DateOnly Date,
    string HumidityAvg,
    decimal TemperatureMaxC,
    decimal TemperatureMinC,
    string WeatherCondition,
    string WindSpeedMax,
    DateTime Sunset,
    DateTime Sunrise);