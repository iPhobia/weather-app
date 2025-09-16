using MediatR;
using WeatherApp.Api.Features.WeatherForecast.Infrastructure;

namespace WeatherApp.Api.Features.WeatherForecast;

public record GetWeatherForecastQuery(int ForecastDays, string City) : IRequest<IEnumerable<WeatherForecastDto?>>;

public class GetWeatherForecastQueryHandler(
    IEnumerable<IWeatherForecastApiHttpClient> apiHttpClients,
    ILogger<GetWeatherForecastQueryHandler> logger)
    : IRequestHandler<GetWeatherForecastQuery, IEnumerable<WeatherForecastDto?>>
{
    public async Task<IEnumerable<WeatherForecastDto?>> Handle(GetWeatherForecastQuery request, CancellationToken ct)
    {
        var tasks = apiHttpClients.Select(x => x.GetWeatherForecast(request.City, request.ForecastDays, ct));

        var forecasts = await Task.WhenAll(tasks.Select(HandleException));

        return forecasts.Where(f => f != null);
    }

    // does not matter if task fails -> user will see response
    private async Task<WeatherForecastDto?> HandleException(Task<WeatherForecastDto> task)
    {
        try
        {
            return await task;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occured during api request");
            return null;
        }
    }
}