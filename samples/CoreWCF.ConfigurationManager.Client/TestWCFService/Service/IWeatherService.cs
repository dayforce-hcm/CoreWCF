using System.Collections.Generic;
using System.ServiceModel;
using TestWCFService.Models;

namespace TestWCFService
{
    [ServiceContract]
    public interface IWeatherService
    {
        [OperationContract]
        public IEnumerable<WeatherForecast> GetWeatherforecast();

        [OperationContract]
        public string UpdateForecast(WeatherForecast forecast);
    }    
}
