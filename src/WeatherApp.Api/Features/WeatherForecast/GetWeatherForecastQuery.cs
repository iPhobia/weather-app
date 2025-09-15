using MediatR;

public record GetWeatherForecastQuery(int ForecastDays, string City) : IRequest<WeatherForecastDto[]>;

public class GetWeatherForecastQueryHandler(IEnumerable<IWeatherForecastApiHttpClient> apiHttpClients) 
    : IRequestHandler<GetWeatherForecastQuery, WeatherForecastDto[]>
{
    public async Task<WeatherForecastDto[]> Handle(GetWeatherForecastQuery request, CancellationToken ct)
    {
        return await Task.WhenAll(apiHttpClients.Select(x => x.GetWeatherForecast(request.City, request.ForecastDays, ct)));
    }
}