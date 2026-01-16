using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.ServiceModel;
using System.Threading.Tasks;
using CoreWCF.ConfigurationManager.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestWCFSuite.Framework;
using WeatherServiceReference;
using Xunit.v3;
using Xunit;

namespace TestWCFSuite;

// Define a collection for the fixture
[CollectionDefinition("WCFClient test collection")]
public class WebFrameworkCollection : global::Xunit.ICollectionFixture<WebFrameworkFixture> { }

[Collection("WCFClient test collection")]
public class IntegrationTests 
{
    private readonly WebFrameworkFixture webFrameworkFixture;
    private readonly ITestOutputHelper m_output;

    public IntegrationTests(WebFrameworkFixture webFrameworkFixture, ITestOutputHelper output)
    {
        this.webFrameworkFixture = webFrameworkFixture;
        m_output = output;
    }

    [Fact]
    public async Task TestWCFService(/* xUnit1041: Remove ITestOutputHelper from constructor, use method parameter instead if needed */)
    {
        Assert.True(webFrameworkFixture.IsReady());

        var endpointName = "WeatherService";
        var appConfigContent = BuildConfig(endpointName: endpointName);

        using (var tempFile = TemporaryFileStream.Create(appConfigContent))
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .Build();

            var provider = new ServiceCollection()
                .AddSingleton(configuration)
                .AddClientModelServices()
                .AddClientModelConfigurationManagerFile(tempFile.Name)
                .AddSingleton<IWeatherService, WeatherServiceClient>((provider) => {
                    var endpointBuilder = provider.GetService<IServiceEndpointBuilder>();
                    var endpoint = endpointBuilder.ConstructServiceEndPoint(endpointName);
                    return new WeatherServiceClient(endpoint.Binding, endpoint.Address);
                })
                .BuildServiceProvider();

            m_output.WriteLine("WeatherForecast Net 8 Client instance");
            var weatherServiceClient = provider.GetService<IWeatherService>();
            Assert.NotNull(weatherServiceClient);
            var request = new GetWeatherforecastRequest();
            var result = await weatherServiceClient.GetWeatherforecastAsync(request);
            Assert.NotNull(result);

            var updateResult = await weatherServiceClient.UpdateForecastAsync(new UpdateForecastRequest()
            {
                forecast = new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(2)).ToShortDateString(),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = "Cloudy"
                }
            });
            Assert.NotNull(updateResult);
            Assert.True(updateResult.UpdateForecastResult.Length > 0);
        }
        
    }

    private string BuildConfig(
        string endpointName = "WeatherService",
        string endpointAddress = "https://localhost:7055/WeatherService.svc",
        string bindingType = "customBinding",
        string bindingName = "BinaryBindingSSL",
        string contract = "WeatherServiceReference.IWeatherService"
        )
    {
         string config = new ServiceModelConfigBuilder()
                .StartConfig()
                .StartClientSection()
                .AddEndpoint(
                    address: endpointAddress,
                    binding: bindingType,
                    contract: contract,
                    bindingConfiguration: bindingName,
                    name: endpointName
                )
                .CloseClientSection()
                .StartBindingsSection()
                    .StartElement("customBinding")
                        .StartBinding(bindingName)
                            .StartElement("binaryMessageEncoding")
                                .AddElement("readerQuotas",
                                    ("maxArrayLength", "2147483647"),
                                    ("maxStringContentLength", "300000")
                                )
                            .EndElement("binaryMessageEncoding")
                            .StartElement("httpsTransport",
                                ("maxBufferSize", "2147483647"),
                                ("maxReceivedMessageSize", "2147483647")
                            )
                            .EndElement("httpsTransport")
                        .CloseBinding()
                    .EndElement("customBinding")
                .CloseBindingsSection()                
                .EndConfig()
                .ToString();
        return config;
    }
}
