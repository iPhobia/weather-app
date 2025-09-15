public record WeatherForecastDto(
    DateOnly Date,
    string City,
    string Country,
    string Humidity,
    int TemperatureC,
    int TemperatureF);