using System.Globalization;
using WeatherApp.Api.Features.WeatherForecast.Infrastructure.AccuWeather;
using WeatherApp.Api.Features.WeatherForecast.Infrastructure.OpenMeteo;
using WeatherApp.Api.Features.WeatherForecast.Infrastructure.WeatherApi;

namespace WeatherApp.Api.Features.WeatherForecast;

public record WeatherForecastDto
{
    public string Source { get; init; }
    public string City { get; init; }
    public ICollection<ForecastDailyDto> DailyForecasts { get; init; } = [];

    public static WeatherForecastDto MapFrom(OpenMeteoWeatherForecastResponse response, string city)
    {
        var dailyForecasts = new List<ForecastDailyDto>();
        var dayForecast = response.Daily;

        for (int i = 0; i < dayForecast.Time.Length; i++)
        {
            dailyForecasts.Add(new ForecastDailyDto(
                dayForecast.Time[i],
                dayForecast.Humidity[i].ToString($"{0}'%'"),
                dayForecast.TemperatureMax[i],
                dayForecast.TemperatureMin[i],
                dayForecast.WeatherCode[i].ToString(),
                dayForecast.WindSpeedMax[i].ToString($"{0}km/h"),
                dayForecast.Sunset[i],
                dayForecast.Sunrise[i]
            ));
        }

        return new WeatherForecastDto
        {
            Source = "open-meteo",
            City = city,
            DailyForecasts = dailyForecasts
        };
    }

    public static WeatherForecastDto MapFrom(WeatherApiForecastResponse response, string city)
    {
        var dailyForecasts = new List<ForecastDailyDto>();
        var dayForecast = response.Forecast.ForecastDay;

        for (int i = 0; i < dayForecast.Length; i++)
        {
            dailyForecasts.Add(new ForecastDailyDto(
                dayForecast[i].Date,
                dayForecast[i].Day.Humidity.ToString($"{0}'%'"),
                dayForecast[i].Day.TemperatureMax,
                dayForecast[i].Day.TemperatureMin,
                dayForecast[i].Day.WeatherCondition.Text,
                dayForecast[i].Day.WindSpeedMax.ToString($"{0}km/h"),
                DateTimeOffset.Parse(dayForecast[i].Astro.Sunset, CultureInfo.InvariantCulture),
                DateTimeOffset.Parse(dayForecast[i].Astro.Sunrise, CultureInfo.InvariantCulture)
            ));
        }

        return new WeatherForecastDto
        {
            Source = "weatherapi",
            City = city,
            DailyForecasts = dailyForecasts
        };
    }

    public static WeatherForecastDto MapFrom(AccuWeatherForecastResponse response, string city)
    {
        var dailyForecasts = new List<ForecastDailyDto>();
        var forecasts = response.DailyForecasts;

        for (int i = 0; i < forecasts.Length; i++)
        {
            dailyForecasts.Add(new ForecastDailyDto(
                DateOnly.FromDateTime(forecasts[i].Date),
                forecasts[i].Day.RelativeHumidity.Average.ToString($"{0}'%'"),
                forecasts[i].Temperature.Maximum.Value,
                forecasts[i].Temperature.Minimum.Value,
                forecasts[i].Day.IconPhrase,
                forecasts[i].Day.Wind.Speed.Value.ToString($"{0}km/h"),
                forecasts[i].Sun.Set,
                forecasts[i].Sun.Rise)
            );
        }

        return new WeatherForecastDto
        {
            Source = "accuweather",
            City = city,
            DailyForecasts = dailyForecasts
        };
    }
}