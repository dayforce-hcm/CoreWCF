using System;
using System.Collections.Generic;
using System.Linq;
using TestWCFService.Models;

namespace TestWCFService
{
    public class WeatherService : IWeatherService
    {
        private readonly WeatherServiceLib _weatherServiceLib = new WeatherServiceLib();
        public IEnumerable<WeatherForecast> GetWeatherforecast()
        {
            var forecast = _weatherServiceLib.GetWeatherforecast().ToArray();
            return forecast;
        }

        public string UpdateForecast(WeatherForecast forecast)
        {
            return _weatherServiceLib.UpdateForecast(forecast);
        }


    }
}
