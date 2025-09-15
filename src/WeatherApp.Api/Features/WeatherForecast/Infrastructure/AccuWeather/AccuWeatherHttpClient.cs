using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

public class AccuWeatherHttpClient : IWeatherForecastApiHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly AccuWeatherApiOptions _accuWeatherApiOptions;

    public AccuWeatherHttpClient(
        HttpClient httpClient,
        IOptionsSnapshot<AccuWeatherApiOptions> accuWeatherApiOptions)
    {
        _accuWeatherApiOptions = accuWeatherApiOptions.Value;

        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accuWeatherApiOptions.Value.ApiKey);

        _httpClient = httpClient;
    }

    public async Task<WeatherForecastDto> GetWeatherForecast(string city, int forecastDays, CancellationToken ct = default)
    {
        var queryParams = new Dictionary<string, string?>
        {
            ["details"] = "true",
            ["metric"] = "true",
        };

        var days = forecastDays == 1 ? "1day" : "7day";

        var location = await GetLocationKey(city, ct);

        var url = QueryHelpers.AddQueryString($"{_accuWeatherApiOptions.ForecastEndpoint}/{days}/{location}",
            queryParams);

        var response = await _httpClient.GetAsync(url, ct);

        var deserializedResponse = await JsonSerializer.DeserializeAsync<AccuWeatherForecastResponse>(
            await response.Content.ReadAsStreamAsync(ct), cancellationToken: ct);

        return WeatherForecastDto.MapFrom(
            deserializedResponse!,
            city);
    }

    private async ValueTask<string> GetLocationKey(string city, CancellationToken ct)
    {
        var url = $"{_accuWeatherApiOptions.LocationEndpoint}?q={city}";

        var response = await _httpClient.GetAsync(url, ct);

        using var jDoc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync(ct),
            cancellationToken: ct);

        return jDoc.RootElement[0]
            .GetProperty("Key")
            .GetString();
    }
}