using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

// API docs https://developer.accuweather.com/core-weather/location-key-daily#1-day-by-location-key
namespace WeatherApp.Api.Features.WeatherForecast.Infrastructure.AccuWeather;

public class AccuWeatherHttpClient : IWeatherForecastApiHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly AccuWeatherApiOptions _accuWeatherApiOptions;
    private static readonly ConcurrentDictionary<string, string> _locationKeys = new();

    public AccuWeatherHttpClient(
        HttpClient httpClient,
        IOptionsSnapshot<AccuWeatherApiOptions> accuWeatherApiOptions)
    {
        _accuWeatherApiOptions = accuWeatherApiOptions.Value;

        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accuWeatherApiOptions.Value.ApiKey);

        // AccuWeather documentation suggests to use response compression, but .net autodecompression does not work for some reason
        // httpClient.DefaultRequestHeaders
        //     .AcceptEncoding
        //     .Add(new StringWithQualityHeaderValue("gzip"));

        _httpClient = httpClient;
    }

    public async Task<WeatherForecastDto> GetWeatherForecast(string city, int forecastDays, CancellationToken ct = default)
    {
        var queryParams = new Dictionary<string, string?>
        {
            ["details"] = "true",
            ["metric"] = "true",
        };

        // ugly workaround
        var days = forecastDays == 1 ? "1day" : "5day";

        var location = await GetLocationKey(city, ct);

        var url = QueryHelpers.AddQueryString($"{_accuWeatherApiOptions.ForecastEndpoint}/{days}/{location}",
            queryParams);

        var response = await _httpClient.GetAsync(url, ct);

        var deserializedResponse = await JsonSerializer.DeserializeAsync<AccuWeatherForecastResponse>(await response.Content.ReadAsStreamAsync(ct),
            cancellationToken: ct);

        return WeatherForecastDto.MapFrom(
            deserializedResponse!,
            city);
    }

    private async ValueTask<string> GetLocationKey(string city, CancellationToken ct)
    {
        var keyExists = _locationKeys.TryGetValue(city, out var k);

        if (keyExists)
            return k;

        var url = $"{_accuWeatherApiOptions.LocationEndpoint}?q={city}";

        var response = await _httpClient.GetAsync(url, ct);

        response.EnsureSuccessStatusCode();

        using var jDoc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync(ct),
            cancellationToken: ct);

        var locKey = jDoc.RootElement[0]
            .GetProperty("Key")
            .GetString();

        _locationKeys.TryAdd(city, locKey);

        return locKey;
    }
}