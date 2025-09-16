using FluentValidation;

namespace WeatherApp.Api.Features.WeatherForecast;

public class WeatherForecastQueryValidator : AbstractValidator<WeatherForecastQuery>
{
    public WeatherForecastQueryValidator()
    {
        RuleFor(x => x.ForecastDays).InclusiveBetween(1, 5)
            .WithMessage("Value must be 1 or 5");
    }
}