using System;
using System.Collections.Generic;
using System.Linq;
using TestWCFServiceLib.Models;

namespace TestWCFServiceLib.BL
{
    public class WeatherServiceLib
    {
        private string[] Summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

        public IEnumerable<WeatherForecast> GetWeatherforecast()
        {
            var rand = new Random(100);
            var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    {
                        Date = DateTime.Now.AddDays(index).ToShortDateString(),
                        TemperatureC = rand.Next(-20, 55),
                        Summary = Summaries[rand.Next(Summaries.Length)]
                    })
                    .ToArray();
            return forecast;
        }

        public string UpdateForecast(WeatherForecast forecast)
        {
            return $"Updated forecast for {forecast.Date} where Temperature was {forecast.TemperatureC}°C and Summary was '{forecast.Summary}'.";
        }
    }
}
