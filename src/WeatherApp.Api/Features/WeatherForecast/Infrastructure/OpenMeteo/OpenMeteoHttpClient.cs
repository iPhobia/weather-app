
using System.Collections.Concurrent;
using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

// API docs https://open-meteo.com/en/docs
namespace WeatherApp.Api.Features.WeatherForecast.Infrastructure.OpenMeteo;

public class OpenMeteoHttpClient(
    HttpClient httpClient,
    IOptionsSnapshot<OpenMeteoApiOptions> openMeteoOptions)
    : IWeatherForecastApiHttpClient
{
    private static readonly ConcurrentDictionary<string, GeocodeData> _geoData = new();

    public async Task<WeatherForecastDto> GetWeatherForecast(string city, int forecastDays, CancellationToken ct = default)
    {
        var geocodeData = await GetCityGeocode(city, ct);

        var queryParams = new Dictionary<string, string?>
        {
            ["latitude"] = geocodeData.Latitude,
            ["longitude"] = geocodeData.Longitude,
            ["forecast_days"] = forecastDays.ToString(),
            ["daily"] = "weather_code,temperature_2m_max,temperature_2m_min,relative_humidity_2m_mean,sunrise,sunset,wind_speed_10m_max",
            ["timezone"] = "auto"
        };
        var url = QueryHelpers.AddQueryString(openMeteoOptions.Value.ForecastEndpoint, queryParams);

        var response = await httpClient.GetAsync(url, ct);

        response.EnsureSuccessStatusCode();

        var deserializedResponse = await JsonSerializer.DeserializeAsync<OpenMeteoWeatherForecastResponse>(
            await response.Content.ReadAsStreamAsync(ct), cancellationToken: ct);

        return WeatherForecastDto.MapFrom(
            deserializedResponse!,
            city);
    }

    private async ValueTask<GeocodeData> GetCityGeocode(string city, CancellationToken ct)
    {
        var geocodeExists = _geoData.TryGetValue(city, out var geocode);

        if (geocodeExists)
            return geocode;

        var queryParams = $"?name={city}&count=1";
        var response = await httpClient.GetAsync(openMeteoOptions.Value.GeocodingEndpoint + queryParams, ct);

        await using var stream = await response.Content.ReadAsStreamAsync(ct);
        using var jDoc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);

        var parsedResult = jDoc.RootElement
            .GetProperty("results")[0];

        var lat = parsedResult.GetProperty("latitude")
            .GetDecimal()
            .ToString(CultureInfo.InvariantCulture);
        var lon = parsedResult.GetProperty("longitude")
            .GetDecimal()
            .ToString(CultureInfo.InvariantCulture);

        var g = new GeocodeData(lat, lon);

        _geoData.TryAdd(city, geocode);

        return g;
    }
}