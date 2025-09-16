# 🌤️ Weather Forecast Aggregator API

A **.NET 9 Minimal API** project that provides consolidated weather forecasts from multiple providers.  
Built for simplicity, speed, and flexibility.  

---

## 🚀 Features

- ✅ Single **GET** endpoint: `/weatherforecast`  
- ✅ Query parameters:
  - `forecastDays` → `1` or `5` (number of days to forecast)  
  - `city` → target city for forecast  
- ✅ Integrates with **3 weather APIs**:
  - [WeatherAPI.com](https://www.weatherapi.com/)  
  - [Open-Meteo.com](https://open-meteo.com/)  
  - [AccuWeather.com](https://www.accuweather.com/)  
- ✅ Aggregates responses into a unified format  

---

## 🔗 Example Request

```http
https://weather-forecast-aggregator.prouddune-5b383109.westeurope.azurecontainerapps.io/weatherforecast?forecastDays=1&city=Odesa
```
Also contains Postman collection for testing
