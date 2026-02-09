using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using TestWCFClientLib.BL;
using TestWCFClientLib.WeatherService;

namespace TestWCFClientApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await WrapperBaseUsage("Net472_BasicHttpBinding_IWeatherService");
            //await WrapperBaseUsage(binding, endpointAddress);
            //await DirectClientUsage(binding, endpointAddress);
        }

        

        private static async Task WrapperBaseUsage(string configName)
        {
            using (var weatherServiceWrapper = new WeatherServiceWrapperBase(configName))
            {
                var results = await weatherServiceWrapper.GetWeatherforecastAsync();
                PrintResults(results);
                var updateResult = await weatherServiceWrapper.UpdateForecastAsync(
                    new WeatherForecast
                    {
                        Date = DateTime.Now.ToString(),
                        TemperatureC = 10,
                        Summary = "Sunny"
                    });
                PrintResult($"{updateResult}");
            }
        }
        private static async Task WrapperBaseUsage(Binding binding, EndpointAddress endpointAddress)
        {
            using (var weatherServiceWrapper = new WeatherServiceWrapperBase(binding, endpointAddress))
            {
                var results = await weatherServiceWrapper.GetWeatherforecastAsync();
                PrintResults(results);
                var updateResult = await weatherServiceWrapper.UpdateForecastAsync(
                    new WeatherForecast
                    {
                        Date = DateTime.Now.ToString(),
                        TemperatureC = 10,
                        Summary = "Sunny"
                    });
                PrintResult($"{updateResult}");
            }
        }

        private static async Task DirectClientUsage(Binding binding, EndpointAddress endpointAddress)
        {
            var weatherServiceClient = new WeatherServiceClient(binding, endpointAddress);

            var results = await weatherServiceClient.GetWeatherforecastAsync();
            PrintResults(results);
            var updateResult = await weatherServiceClient.UpdateForecastAsync(
                new WeatherForecast
                {
                    Date = DateTime.Now.ToString(),
                    TemperatureC = 10,
                    Summary = "Sunny"
                });
            PrintResult($"{updateResult}");
        }

        private static void PrintResults(IEnumerable<WeatherForecast> weatherForecasts)
        {
            foreach (WeatherForecast forecast in weatherForecasts)
            {
                PrintResult($"Date: {forecast.Date}, TemperatureC: {forecast.TemperatureC}, Summary: {forecast.Summary}");
            }
        }

        private static void PrintResult(string result)
        {
            Console.WriteLine(result);
        }

    }
}

