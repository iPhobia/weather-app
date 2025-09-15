
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

public class WeatherApiHttpClient(
    HttpClient httpClient,
    IOptionsSnapshot<WeatherApiOptions> weatherApiOptions)
    : IWeatherForecastApiHttpClient
{
    public async Task<WeatherForecastDto> GetWeatherForecast(string city, int forecastDays, CancellationToken ct = default)
    {
        var queryParams = new Dictionary<string, string?>
        {
            ["key"] = weatherApiOptions.Value.ApiKey,
            ["q"] = city,
            ["days"] = forecastDays.ToString(),
            ["aqi"] = "no",
            ["alerts"] = "no"
        };
        var url = QueryHelpers.AddQueryString(weatherApiOptions.Value.ForecastEndpoint, queryParams);

        var response = await httpClient.GetAsync(url, ct);

        var deserializedResponse  = await JsonSerializer.DeserializeAsync<WeatherApiForecastResponse>(
            await response.Content.ReadAsStreamAsync(ct), cancellationToken: ct);

        return WeatherForecastDto.MapFrom(
            deserializedResponse!,
            city);
    }
}