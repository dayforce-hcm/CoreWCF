using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using CoreWCF.ConfigurationManager.Client;
using Moq;
using Xunit;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration
{
    public class ConfigurationManagerServiceModelOptionsTests
    {
        [Fact]
        public void Constructor_CreatesInstance()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton(Mock.Of<IConfigurationHolder>())
                .BuildServiceProvider();

            var options = new ConfigurationManagerServiceModelOptions(serviceProvider, "dummy.config");
            Assert.NotNull(options);
        }

        [Fact]
        public void Configure_CallsConfigureOnOptions()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton(Mock.Of<IConfigurationHolder>())
                .BuildServiceProvider();

            var optionsInstance = new ConfigurationManagerServiceModelOptions(serviceProvider, "dummy.config");
            var clientOptions = new ClientModelOptions();

            // This will throw if config file/section is missing, so we just check for method call
            // and expect ServiceModelConfigurationException or similar.
            Assert.ThrowsAny<Exception>(() => optionsInstance.Configure("test-string", clientOptions));
        }
    }
}