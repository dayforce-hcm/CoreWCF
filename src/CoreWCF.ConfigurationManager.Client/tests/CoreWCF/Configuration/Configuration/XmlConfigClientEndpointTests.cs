using System;
using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System.ServiceModel.Channels;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Configuration
{
    public class XmlConfigClientEndpointTests
    {
        [Fact]
        public void Constructor_SetsPropertiesCorrectly()
        {
            var contract = typeof(string);
            var binding = new CustomBinding();
            var address = "http://localhost";
            var endpoint = new XmlConfigClientEndpoint(contract, binding, address);
            Assert.Equal(contract, endpoint.Contract);
            Assert.Equal(binding, endpoint.Binding);
            Assert.Equal(address, endpoint.Address);
        }
    }
}
