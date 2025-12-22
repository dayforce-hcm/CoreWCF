using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System;
using System.ServiceModel;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings.Properties
{
    public class EndpointAddressElementBaseTests
    {
        [Fact]
        public void Address_SetAndGet_ReturnsValue()
        {
            var element = new TestEndpointAddressElementBase();
            var uri = new Uri("http://localhost");
            element.Address = uri;
            Assert.Equal(uri, element.Address);
        }

        [Fact]
        public void Copy_CopiesAddressAndHeaders()
        {
            var source = new TestEndpointAddressElementBase();
            var target = new TestEndpointAddressElementBase();
            var uri = new Uri("http://localhost");
            source.Address = uri;
            source.Headers.Headers = new System.ServiceModel.Channels.AddressHeaderCollection();
            target.Copy(source);
            Assert.Equal(uri, target.Address);
        }

        private class TestEndpointAddressElementBase : EndpointAddressElementBase
        {
            public TestEndpointAddressElementBase() : base() { }
        }
    }
}
