using System;
using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System.Configuration;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Client
{
    public class ClientEndpointElementCollectionTests
    {
        [Fact]
        public void Collection_IsInstantiableAndEmptyByDefault()
        {
            var collection = new ClientEndpointElementCollection();
            Assert.NotNull(collection);
            Assert.IsAssignableFrom<ConfigurationElementCollection>(collection);
            Assert.Equal(0, collection.Count);
        }
    }
}
