namespace TestWCFClientApp
{
    public class WeatherServiceWrapperBase : IDisposable
    {
        protected WeatherServiceClient weatherServiceClient;

        public WeatherServiceWrapperBase() : this("*") { }

        public WeatherServiceWrapperBase(string endpointConfigurationName)
        {
            weatherServiceClient = new WeatherServiceClient(endpointConfigurationName);
        }

        public WeatherServiceWrapperBase(string endpointConfigurationName, string remoteAddress)
        {
            weatherServiceClient = new WeatherServiceClient(endpointConfigurationName, remoteAddress);
        }

        public WeatherServiceWrapperBase(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress)
        {
            weatherServiceClient = new WeatherServiceClient(binding, remoteAddress);
        }

        public WeatherServiceWrapperBase(System.ServiceModel.Description.ServiceEndpoint endpoint)
        {
            weatherServiceClient = new WeatherServiceClient(endpoint.Binding, endpoint.Address);
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (weatherServiceClient != null)
                    {
                        if (weatherServiceClient is IDisposable)
                            ((IDisposable)weatherServiceClient).Dispose();         // Let's us compile cleanly, even if we're not usign the new wrapping proxy
                    }
                    weatherServiceClient = null;
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public WeatherServiceClient WeatherServiceClient { get { return weatherServiceClient; } }

        public async Task<IEnumerable<WeatherForecast>> GetWeatherforecastAsync()
        {
            return await weatherServiceClient.GetWeatherforecastAsync();
        }

        public async Task<string> UpdateForecastAsync(WeatherForecast forecast)
        {
            return await weatherServiceClient.UpdateForecastAsync(forecast);
        }
    }
}
