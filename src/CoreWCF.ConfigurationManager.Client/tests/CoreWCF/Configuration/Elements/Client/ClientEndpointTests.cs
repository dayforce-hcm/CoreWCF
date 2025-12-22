using Xunit;
using CoreWCF.ConfigurationManager.Client;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Client
{
    public class ClientEndpointTests
    {
        [Fact]
        public void Properties_SetAndGet_ReturnExpectedValues()
        {
            var endpoint = new ClientEndpoint
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

            Assert.Equal("address", endpoint.Address);
            Assert.Equal("behavior", endpoint.BehaviorConfiguration);
            Assert.Equal("binding", endpoint.Binding);
            Assert.Equal("bindingConfig", endpoint.BindingConfiguration);
            Assert.Equal("contract", endpoint.Contract);
            Assert.Equal("endpointConfig", endpoint.EndpointConfiguration);
            Assert.Equal("kind", endpoint.Kind);
            Assert.Equal("name", endpoint.Name);
        }
    }
}
