using System;
using System.ServiceModel.Channels;
using CoreWCF.ConfigurationManager.Client;
using Xunit;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration
{
    public class ClientEndpointConfigurationTests
    {
        [Fact]
        public void Properties_SetAndGet_ReturnExpectedValues()
        {
            // Arrange
            var address = new Uri("http://localhost/service");
            var binding = new CustomBinding();
            var contractType = typeof(string);
            var listenUri = new Uri("http://localhost/listen");
            Action<ClientEndpoint> configureEndpoint = ep => ep.Name = "TestEndpoint";

            var config = new ClientEndpointConfiguration
            {
                Address = address,
                Binding = binding,
                Contract = contractType,
                ListenUri = listenUri,
                ConfigureEndpoint = configureEndpoint
            };

            // Assert
            Assert.Equal(address, config.Address);
            Assert.Equal(binding, config.Binding);
            Assert.Equal(contractType, config.Contract);
            Assert.Equal(listenUri, config.ListenUri);
            Assert.Equal(configureEndpoint, config.ConfigureEndpoint);
        }

        [Fact]
        public void ConfigureEndpoint_ActionIsInvoked()
        {
            // Arrange
            var config = new ClientEndpointConfiguration();
            var endpoint = new ClientEndpoint();
            bool invoked = false;

            config.ConfigureEndpoint = ep =>
            {
                invoked = true;
                ep.Name = "Configured";
            };

            // Act
            config.ConfigureEndpoint?.Invoke(endpoint);

            // Assert
            Assert.True(invoked);
            Assert.Equal("Configured", endpoint.Name);
        }
    }
}