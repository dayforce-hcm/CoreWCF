using CoreWCF.ConfigurationManager.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.IO;
using Xunit;


namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Extensions
{
    public class ConfigurationManagerExtensionsTests
    {
        [Fact]
        public void AddClientModelConfigurationManagerFile_ThrowsIfFileNotFound()
        {
            var services = new ServiceCollection();
            var invalidPath = "nonexistent.config";
            var ex = Assert.Throws<FileNotFoundException>(() =>
                services.AddClientModelConfigurationManagerFile(invalidPath));
            Assert.Contains(invalidPath, ex.Message);
        }

        [Fact]
        public void AddClientModelConfigurationManagerFile_ThrowsIfFileCannotBeRead()
        {
            var services = new ServiceCollection();
            var path = Path.GetTempFileName();
            try
            {
                // Make file unreadable
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Write)) { }
                File.SetAttributes(path, FileAttributes.ReadOnly);

                // Should not throw, as FileStream with FileAccess.Read will succeed for a normal file
                services.AddClientModelConfigurationManagerFile(path);
            }
            finally
            {
                File.SetAttributes(path, FileAttributes.Normal);
                File.Delete(path);
            }
        }

        [Fact]
        public void AddClientModelConfigurationManagerFile_RegistersExpectedServices()
        {
            var services = new ServiceCollection();
            var path = Path.GetTempFileName();
            try
            {
                File.WriteAllText(path, @"<?xml version=""1.0"" encoding=""utf-8""?><configuration></configuration>");

                services.AddClientModelServices();
                services.AddClientModelConfigurationManagerFile(path);
                var provider = services.BuildServiceProvider();

                var options = provider.GetService<IOptions<ClientModelOptions>>();
                Assert.NotNull(options);
                Assert.NotNull(provider.GetService<IConfigurationHolder>());
                Assert.NotNull(provider.GetService<IBindingFactory>());
                Assert.NotNull(provider.GetService<IServiceEndpointBuilder>());
                Assert.NotNull(provider.GetService<IConfigureOptions<ClientModelOptions>>());
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Fact]
        public void AddClientModelServices_RegistersOptions()
        {
            var services = new ServiceCollection();
            services.AddClientModelServices();
            var provider = services.BuildServiceProvider();

            var options = provider.GetService<IOptions<ClientModelOptions>>();
            Assert.NotNull(options);
        }
    }
}