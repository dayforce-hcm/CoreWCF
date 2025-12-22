using System;
using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System.ServiceModel.Channels;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Configuration
{
    public class IXmlConfigClientEndpointTests
    {
        private class TestEndpoint : IXmlConfigClientEndpoint
        {
            public string Address { get; set; }
            public Binding Binding { get; set; }
            public Type Contract { get; set; }
        }

        [Fact]
        public void Properties_SetAndGet()
        {
            var endpoint = new TestEndpoint
            {
                Address = "http://localhost",
                Binding = new CustomBinding(),
                Contract = typeof(string)
            };
            Assert.Equal("http://localhost", endpoint.Address);
            Assert.IsType<CustomBinding>(endpoint.Binding);
            Assert.Equal(typeof(string), endpoint.Contract);
        }
    }
}
