using System;
using System.ServiceModel.Channels;
using CoreWCF.ConfigurationManager.Client;
using Xunit;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration
{
    public class ClientEndpointConfigurationBuilderTests
    {
        [Fact]
        public void Constructor_SetsEndpointType()
        {
            var serviceType = typeof(string);
            var builder = new ClientEndpointConfigurationBuilder(serviceType);
            Assert.NotNull(builder);
        }

        [Fact]
        public void Configure_AddsDelegateToList()
        {
            var builder = new ClientEndpointConfigurationBuilder(typeof(string));
            Action<ClientEndpointConfigurationBuilder> configDelegate = b => { };

            builder.Configure(configDelegate);

            var field = typeof(ClientEndpointConfigurationBuilder)
                .GetField("_configDelegates", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var delegates = (System.Collections.IList)field.GetValue(builder);

            Assert.Single(delegates);
            Assert.Equal(configDelegate, delegates[0]);
        }

        [Fact]
        public void AddClientEndpoint_AddsEndpointToList()
        {
            var builder = new ClientEndpointConfigurationBuilder(typeof(string));
            var contractType = typeof(int);
            var binding = new CustomBinding();
            var address = new Uri("http://localhost/test");

            builder.AddClientEndpoint(contractType, binding, address);

            var field = typeof(ClientEndpointConfigurationBuilder)
                .GetField("_endpoints", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var endpoints = (System.Collections.IList)field.GetValue(builder);

            Assert.Single(endpoints);

            var endpoint = endpoints[0];
            var contractProp = endpoint.GetType().GetProperty("Contract");
            var bindingProp = endpoint.GetType().GetProperty("Binding");
            var addressProp = endpoint.GetType().GetProperty("Address");

            Assert.Equal(contractType, contractProp.GetValue(endpoint));
            Assert.Equal(binding, bindingProp.GetValue(endpoint));
            Assert.Equal(address, addressProp.GetValue(endpoint));
        }
    }
}
