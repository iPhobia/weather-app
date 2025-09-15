using MediatR;

public record GetWeatherForecastQuery(DateOnly Date, string City) : IRequest<WeatherForecastDto>;

public class GetWeatherForecastQueryHandler() : IRequestHandler<GetWeatherForecastQuery, WeatherForecastDto>
{
    public async Task<WeatherForecastDto> Handle(GetWeatherForecastQuery request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}