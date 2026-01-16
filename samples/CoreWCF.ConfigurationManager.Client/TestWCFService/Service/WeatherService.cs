using CoreWCF;

namespace TestWCFService.Service
{
    [ServiceContract]
    public interface IWeatherService
    {
        [OperationContract]
        public IEnumerable<WeatherForecast> GetWeatherforecast();

        [OperationContract]
        public string UpdateForecast(WeatherForecast forecast);
    }

    public class WeatherService: IWeatherService
    {
        private string[] Summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

        public IEnumerable<WeatherForecast> GetWeatherforecast()
        {
            var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)).ToShortDateString(),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
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
