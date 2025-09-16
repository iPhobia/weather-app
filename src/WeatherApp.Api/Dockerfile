# 1. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy everything and restore
COPY . .
RUN dotnet restore "./WeatherApp.Api.csproj"

# Build and publish
RUN dotnet publish "./WeatherApp.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false


# 2. Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy published output
COPY --from=build /app/publish .

# Expose port
EXPOSE 8080

# Run app
ENTRYPOINT ["dotnet", "WeatherApp.Api.dll"]