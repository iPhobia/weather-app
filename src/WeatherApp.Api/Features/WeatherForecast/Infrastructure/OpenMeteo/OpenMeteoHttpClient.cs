
using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

public class OpenMeteoHttpClient(
    HttpClient httpClient,
    IOptionsSnapshot<OpenMeteoApiOptions> openMeteoOptions) 
    : IWeatherForecastApiHttpClient
{
    public async Task<WeatherForecastDto> GetWeatherForecast(string city, DateOnly date, CancellationToken ct = default)
    {
        var geocodeData = await GetCityGeocode(city);

        var queryParams = new Dictionary<string, string?>
        {
            ["latitude"] = geocodeData.Latitude,
            ["longitude"] = geocodeData.Longitude,
            ["start_date"] = date.ToString("yyyy-MM-dd"),
            ["end_date"] = date.ToString("yyyy-MM-dd"),
            ["daily"] = "weather_code,temperature_2m_max,temperature_2m_min,relative_humidity_2m_mean,sunrise,sunset,wind_speed_10m_max",
            ["timezone"] = "auto"
        };
        var url = QueryHelpers.AddQueryString(openMeteoOptions.Value.WeatherForecastEndpoint, queryParams);

        var response = await httpClient.GetAsync(url, ct);

        var deserializedResponse  = await JsonSerializer.DeserializeAsync<OpenMeteoWeatherForecastResponse>(
            await response.Content.ReadAsStreamAsync(ct), cancellationToken: ct);

        var dayForecast = deserializedResponse!.Daily;

        return new WeatherForecastDto(
            "https://open-meteo.com", 
            date, 
            city,
            dayForecast.Humidity[0].ToString($"{0}'%'"),
            (int)dayForecast.TemperatureMax[0],
            (int)dayForecast.TemperatureMin[0],
            dayForecast.WeatherCode[0].ToString(),
            dayForecast.WindSpeedMax[0].ToString($"{0}km/h"),
            DateTime.Parse(dayForecast.Sunset[0]),
            DateTime.Parse(dayForecast.Sunrise[0]));
    }

    private async ValueTask<GeocodeData> GetCityGeocode(string city)
    {
        var queryParams = $"?name={city}&count=1";
        var response = await httpClient.GetAsync(openMeteoOptions.Value.GeocodingEndpoint + queryParams);

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var jDoc = await JsonDocument.ParseAsync(stream);

        var parsedResult = jDoc.RootElement
            .GetProperty("results")[0];

        var lat = parsedResult.GetProperty("latitude")
            .GetDecimal()
            .ToString(CultureInfo.InvariantCulture);
        var lon = parsedResult.GetProperty("longitude")
            .GetDecimal()
            .ToString(CultureInfo.InvariantCulture); 

        return new GeocodeData(lat, lon);
    }
}