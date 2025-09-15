public record WeatherForecastDto(
    string Source,
    DateOnly Date,
    string City,
    string HumidityAvg,
    int TemperatureMaxC,
    int TemperatureMinC,
    string WeatherCondition,
    string WindSpeedMax,
    DateTime Sunset,
    DateTime Sunrise);