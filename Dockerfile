FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["src/WeatherApp.Api/WeatherApp.Api.csproj", "WeatherApp.Api/"]
RUN dotnet restore "WeatherApp.Api/WeatherApp.Api.csproj"

COPY src/WeatherApp.Api/ WeatherApp.Api/
WORKDIR "/src/WeatherApp.Api"
RUN dotnet build "WeatherApp.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WeatherApp.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeatherApp.Api.dll"]