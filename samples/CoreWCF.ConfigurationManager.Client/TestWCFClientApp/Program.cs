using CoreWCF.ConfigurationManager.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ServiceReference;
using System;

namespace TestWCFClientApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .Build();

            var provider = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddClientModelServices()
                .AddClientModelConfigurationManagerFile(Path.Combine(AppContext.BaseDirectory, "app.config"))
                .AddSingleton<IWeatherService, WeatherServiceClient>((provider) => {
                    var endpointBuilder = provider.GetService<IServiceEndpointBuilder>();
                    var endpoint = endpointBuilder.ConstructServiceEndPoint("WeatherService");
                    return new WeatherServiceClient(endpoint.Binding, endpoint.Address);
                })
                .BuildServiceProvider();

            Console.WriteLine("WeatherForecast Net 8 Client");
            var weatherServiceClient = provider.GetService<IWeatherService>();
            var request = new GetWeatherforecastRequest();
            var result = weatherServiceClient?.GetWeatherforecastAsync(request).GetAwaiter().GetResult();
            
            foreach(var forecast in result?.GetWeatherforecastResult)
            {
                Console.WriteLine($"Date: {forecast.Date}, TemperatureC: {forecast.TemperatureC}, Summary: {forecast.Summary}");
            }

            var updateResult = weatherServiceClient.UpdateForecastAsync(new UpdateForecastRequest()
            {
                forecast = new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(2)).ToShortDateString(),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = "Cloudy"
                }
            }).GetAwaiter().GetResult();
            Console.WriteLine($"{updateResult.UpdateForecastResult}");

        }
    }
}