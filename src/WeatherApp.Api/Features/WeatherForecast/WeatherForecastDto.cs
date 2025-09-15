using System.Globalization;

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
                DateTime.Parse(dayForecast[i].Astro.Sunset, CultureInfo.InvariantCulture),
                DateTime.Parse(dayForecast[i].Astro.Sunrise, CultureInfo.InvariantCulture)
            ));
        }

        return new WeatherForecastDto
        {
            Source = "weatherapi",
            City = city,
            DailyForecasts = dailyForecasts
        };
    }
}