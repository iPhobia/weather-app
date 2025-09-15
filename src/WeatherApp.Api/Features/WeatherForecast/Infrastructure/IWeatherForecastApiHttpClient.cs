public interface IWeatherForecastApiHttpClient
{
    Task<WeatherForecastDto> GetWeatherForecast(string city, DateOnly date, CancellationToken ct = default);
}