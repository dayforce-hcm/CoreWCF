using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using CoreWCF.ConfigurationManager.Client;
using Moq;
using Xunit;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration
{
    public class ServiceEndpointBuilderTests
    {
        [Fact]
        public void Constructor_SetsFields()
        {
            var configHolder = Mock.Of<IConfigurationHolder>();
            var options = Options.Create(new ClientModelOptions());
            var builder = new ServiceEndpointBuilder(configHolder, options);
            Assert.NotNull(builder);
        }

        [Fact]
        public void ConstructServiceEndPoint_ThrowsForUnknownEndpoint()
        {
            var configHolder = Mock.Of<IConfigurationHolder>(h =>
                h.ClientEndpoints == new HashSet<ClientEndpoint>());
            var options = Options.Create(new ClientModelOptions());
            var builder = new ServiceEndpointBuilder(configHolder, options);

            Assert.Throws<ArgumentException>(() => builder.ConstructServiceEndPoint("unknown"));
        }

        // Additional tests for successful endpoint construction would require more setup/mocking.
    }
}