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
    }
}