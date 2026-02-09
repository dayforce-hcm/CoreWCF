using System.Collections.Generic;
using System.ServiceModel;
using TestWCFServiceLib.Models;

namespace TestWCFServiceLib.Service
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
