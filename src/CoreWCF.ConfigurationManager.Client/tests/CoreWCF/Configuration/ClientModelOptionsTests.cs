using System;
using CoreWCF.ConfigurationManager.Client;
using Xunit;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration
{
    public class ClientModelOptionsTests
    {
        [Fact]
        public void ConfigureClientEndpoint_RegistersDelegate()
        {
            var options = new ClientModelOptions();
            var contractType = typeof(string);

            options.ConfigureClientEndpoint(contractType, builder =>
            {
                builder.AddClientEndpoint(contractType, null, new Uri("http://localhost"));
            });

            // Use reflection to verify that the delegate was registered
            var configBuildersField = typeof(ClientModelOptions)
                .GetField("_configBuilders", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var configBuilders = (System.Collections.IDictionary)configBuildersField.GetValue(options);
            Assert.True(configBuilders.Contains(contractType));

            var builder = configBuilders[contractType];
            var configDelegatesField = builder.GetType()
                .GetField("_configDelegates", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var configDelegates = configDelegatesField.GetValue(builder) as System.Collections.IList;
            Assert.NotNull(configDelegates);
            Assert.Single(configDelegates);
        }
    }
}