public interface IWeatherForecastApiHttpClient
{
    Task<WeatherForecastDto> GetWeatherForecast(string city, int forecastDays, CancellationToken ct = default);
}