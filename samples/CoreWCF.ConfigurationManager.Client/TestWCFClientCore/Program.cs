using CoreWCF.ConfigurationManager.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestWCFClientLib.BL;
using TestWCFClientLib.WeatherService;

namespace TestWCFClientCoreApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //Reference interface to load Library into the CurrentDomain asseblies list
            IWeatherService weatherService = null;
            var serviceProvider = GetServiceProvider();
            var endpointBuilder = serviceProvider.GetService<IServiceEndpointBuilder>();
            var endpoint = endpointBuilder.ConstructServiceEndPoint("NetCore_BasicHttpBinding_IWeatherService");

            await WrapperBaseUsage(endpoint);
            await DirectClientUsage(endpoint);

        } 
        
        public static ServiceProvider GetServiceProvider()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .Build();

            var provider = new ServiceCollection()
                .AddSingleton(configuration)
                .AddClientModelServices()
                .AddClientModelConfigurationManagerFile(Path.Combine(AppContext.BaseDirectory, "app.config"))                             
                .BuildServiceProvider();
            
            return provider;
        }

        private static async Task WrapperBaseUsage(System.ServiceModel.Description.ServiceEndpoint endpoint)
        {
            using (var weatherServiceWrapper = new WeatherServiceWrapperBase(endpoint))
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

        private static async Task DirectClientUsage(System.ServiceModel.Description.ServiceEndpoint endpoint)
        {
            var weatherServiceClient = new WeatherServiceClient(endpoint.Binding, endpoint.Address);

            var results = await weatherServiceClient.GetWeatherforecastAsync();
            PrintResults(results);
            var updateResult = await weatherServiceClient.UpdateForecastAsync(
                new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(2)).ToShortDateString(),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = "Cloudy"
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