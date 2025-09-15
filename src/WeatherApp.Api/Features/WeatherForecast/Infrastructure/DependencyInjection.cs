public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddHttpClient<OpenMeteoHttpClient>();
        serviceCollection.AddScoped<IWeatherForecastApiHttpClient, OpenMeteoHttpClient>();
        serviceCollection.AddOptions<OpenMeteoApiOptions>()
            .Bind(configuration.GetSection("OpenMeteoApi"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        serviceCollection.AddHttpClient<WeatherApiHttpClient>();
        serviceCollection.AddScoped<IWeatherForecastApiHttpClient, WeatherApiHttpClient>();
        serviceCollection.AddOptions<WeatherApiOptions>()
            .Bind(configuration.GetSection("WeatherApi"))
            .ValidateDataAnnotations()
            .ValidateOnStart();    
            
        serviceCollection.AddHttpClient<AccuWeatherHttpClient>();
        serviceCollection.AddScoped<IWeatherForecastApiHttpClient, AccuWeatherHttpClient>();
        serviceCollection.AddOptions<AccuWeatherApiOptions>()
            .Bind(configuration.GetSection("AccuWeatherApi"))
            .ValidateDataAnnotations()
            .ValidateOnStart();    
    }
}