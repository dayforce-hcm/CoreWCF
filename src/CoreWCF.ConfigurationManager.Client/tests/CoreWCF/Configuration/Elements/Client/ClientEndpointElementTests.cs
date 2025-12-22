using System;
using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System.Configuration;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Client
{
    public class ClientEndpointElementTests
    {
        [Fact]
        public void Properties_SetAndGet_ReturnExpectedValues()
        {
            var element = new ClientEndpointElement
            {
                Address = "address",
                BehaviorConfiguration = "behavior",
                Binding = "binding",
                BindingConfiguration = "bindingConfig",
                Contract = "contract",
                EndpointConfiguration = "endpointConfig",
                Kind = "kind",
                Name = "name"
            };

            Assert.Equal("address", element.Address);
            Assert.Equal("behavior", element.BehaviorConfiguration);
            Assert.Equal("binding", element.Binding);
            Assert.Equal("bindingConfig", element.BindingConfiguration);
            Assert.Equal("contract", element.Contract);
            Assert.Equal("endpointConfig", element.EndpointConfiguration);
            Assert.Equal("kind", element.Kind);
            Assert.Equal("name", element.Name);
        }

        [Fact]
        public void CreateClientEndpoint_ReturnsExpectedClientEndpoint()
        {
            var element = new ClientEndpointElement
            {
                Name = "name",
                Address = "address",
                Binding = "binding",
                BindingConfiguration = "bindingConfig",
                Contract = "contract"
            };

            var endpoint = element.CreateClientEndpoint();
            Assert.Equal("name", endpoint.Name);
            Assert.Equal("address", endpoint.Address);
            Assert.Equal("binding", endpoint.Binding);
            Assert.Equal("bindingConfig", endpoint.BindingConfiguration);
            Assert.Equal("contract", endpoint.Contract);
        }


        [Fact]
        public void CreateClientEndpoint_DefaultValues()
        {
            var element = new ClientEndpointElement
            {
                Name = null,
                BehaviorConfiguration = null,
                Binding = null,
                BindingConfiguration = null,
                Contract = null,
                EndpointConfiguration = null,
                Kind = null
            };            

            Assert.Equal(string.Empty, element.Name);
            Assert.Equal(string.Empty, element.BehaviorConfiguration);
            Assert.Equal(string.Empty, element.Binding);
            Assert.Equal(string.Empty, element.BindingConfiguration);
            Assert.Equal(string.Empty, element.Contract);
            Assert.Equal(string.Empty, element.EndpointConfiguration);
            Assert.Equal(string.Empty, element.Kind);

            ClientEndpoint endpoint = element.CreateClientEndpoint();
            Assert.Null(endpoint.BehaviorConfiguration);
            Assert.Equal(string.Empty, endpoint.Binding);
            Assert.Equal(string.Empty, endpoint.BindingConfiguration);
            Assert.Equal(string.Empty, endpoint.Contract);
            Assert.Null(endpoint.EndpointConfiguration);
            Assert.Null(endpoint.Kind);
        }
    }
}
