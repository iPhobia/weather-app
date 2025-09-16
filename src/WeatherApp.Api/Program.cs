using Carter;
using FluentValidation;
using WeatherApp.Api.Features.WeatherForecast;
using WeatherApp.Api.Features.WeatherForecast.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHybridCache();
builder.Services.AddScoped<IValidator<WeatherForecastQuery>, WeatherForecastQueryValidator>();
builder.Services.AddProblemDetails();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Configuration.AddEnvironmentVariables();


var app = builder.Build();

app.UseHttpsRedirection();

app.MapCarter();
app.UseExceptionHandler();
app.Run();
