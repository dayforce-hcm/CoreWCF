using System;
using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System.Configuration;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Client
{
    public class ClientElementTests
    {
        [Fact]
        public void Endpoints_IsAccessibleAndNotNullByDefault()
        {
            var element = new ClientElement();
            Assert.NotNull(element.Endpoints);
            Assert.False(element.Endpoints.ElementInformation.IsPresent);
        }
    }
}
