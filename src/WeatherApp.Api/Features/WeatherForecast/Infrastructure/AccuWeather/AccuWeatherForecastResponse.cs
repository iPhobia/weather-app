
public record AccuWeatherForecastResponse
{
    public AccuWeatherDailyForecastDto[] DailyForecasts { get; init; }
}

public record AccuWeatherDailyForecastDto
{
    public DateTime Date { get; init; }
    public AccuWeatherSunDto Sun { get; init; }
    public AccuWeatherTemperatureDto Temperature { get; init; }
    public AccuWeatherDayDto Day { get; init; }
}

public record AccuWeatherTemperatureDto
{
    public AccuWeatherValueDto Maximum { get; init; }
    public AccuWeatherValueDto Minimum { get; init; }
}

public record struct AccuWeatherValueDto
{
    public decimal Value { get; init; }
}

public record AccuWeatherDayDto
{
    public string IconPhrase { get; init; }
    public AccuWeatherRelativeHumidityDto RelativeHumidity { get; init; }
    public AccuWeatherWindDto Wind { get; init; }
}

public record AccuWeatherWindDto
{
    public AccutWeatherWindSpeed Speed { get; init; }
}

public record AccuWeatherSunDto
{
    public DateTimeOffset Rise { get; init; }
    public DateTimeOffset Set { get; init; }
}

public record AccuWeatherRelativeHumidityDto
{
    public int Average { get; init; }
}

public record AccutWeatherWindSpeed
{
    public decimal Value { get; init; }
}