using CoreWCF.ConfigurationManager.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ServiceModel;
using Xunit;
using System.ServiceModel.Channels;

namespace CoreWCF.ConfigurationManager.Client.Tests.IntegrationTests.Bindings
{
    public class CustomBindingIntegrationTests : TestBase
    {
        private const string EndpointAddress = "http://localhost:8080/TestService";
        private const string BindingType = "customBinding";
        private const string BindingName = "TestCustomBinding";
        private const string Contract = "CoreWCF.ConfigurationManager.Client.Tests.IntegrationTests.ISomeService";
        private const string EndpointName = "DummyEndpoint";
        
        [Fact]
        public void CustomBinding_BindingAttributes_AreAppliedCorrectly()
        {
            string closeTimeout = "00:01:11";
            string openTimeout = "00:01:11";
            string receiveTimeout = "00:11:11";
            string sendTimeout = "00:01:11";

            string config = new ServiceModelConfigBuilder()
                .StartConfig()
                .StartClientSection()
                .AddEndpoint(
                    address: EndpointAddress,
                    binding: BindingType,
                    contract: Contract,
                    bindingConfiguration: BindingName,
                    name: EndpointName
                )
                .CloseClientSection()
                .StartBindingsSection()
                    .StartElement("customBinding")
                        .StartBinding(BindingName,
                                ("closeTimeout", closeTimeout),
                                ("openTimeout", openTimeout),
                                ("receiveTimeout", receiveTimeout),
                                ("sendTimeout", sendTimeout)
                            )
                        .CloseBinding()
                    .EndElement("customBinding")
                .CloseBindingsSection()
                .EndConfig()
                .ToString();

            using (var tempFile = TemporaryFileStream.Create(config))
            {
                var provider = CreateProvider(tempFile.Name);
                var endpointBuilder = provider.GetRequiredService<IServiceEndpointBuilder>();
                var endpoint = endpointBuilder.ConstructServiceEndPoint(EndpointName);
                Assert.NotNull(endpoint);

                Assert.Equal(EndpointAddress, endpoint.Address.Uri.ToString());
                Assert.Equal(BindingName, endpoint.Binding.Name, true);
                var customBinding = endpoint.Binding as CustomBinding;
                Assert.NotNull(customBinding);

                Assert.Equal(BindingName, customBinding.Name);
                Assert.Equal(closeTimeout, customBinding.CloseTimeout.ToString());
                Assert.Equal(openTimeout, customBinding.OpenTimeout.ToString());
                Assert.Equal(receiveTimeout, customBinding.ReceiveTimeout.ToString());
                Assert.Equal(sendTimeout, customBinding.SendTimeout.ToString());

            }
        }

        [Fact]
        public void CustomBinding_BindingAttributes_Defaults_AreAppliedCorrectly()
        {
            string closeTimeout = "00:01:00";
            string openTimeout = "00:01:00";
            string receiveTimeout = "00:10:00";
            string sendTimeout = "00:01:00";

            string config = new ServiceModelConfigBuilder()
                .StartConfig()
                .StartClientSection()
                .AddEndpoint(
                    address: EndpointAddress,
                    binding: BindingType,
                    contract: Contract,
                    bindingConfiguration: BindingName,
                    name: EndpointName
                )
                .CloseClientSection()
                .StartBindingsSection()
                    .StartElement("customBinding")
                        .StartBinding(BindingName)
                        .CloseBinding()
                    .EndElement("customBinding")
                .CloseBindingsSection()
                .EndConfig()
                .ToString();

            using (var tempFile = TemporaryFileStream.Create(config))
            {
                var provider = CreateProvider(tempFile.Name);
                var endpointBuilder = provider.GetRequiredService<IServiceEndpointBuilder>();
                var endpoint = endpointBuilder.ConstructServiceEndPoint(EndpointName);
                Assert.NotNull(endpoint);

                Assert.Equal(EndpointAddress, endpoint.Address.Uri.ToString());
                Assert.Equal(BindingName, endpoint.Binding.Name, true);
                var customBinding = endpoint.Binding as CustomBinding;
                Assert.NotNull(customBinding);

                Assert.Equal(BindingName, customBinding.Name);
                Assert.Equal(closeTimeout, customBinding.CloseTimeout.ToString());
                Assert.Equal(openTimeout, customBinding.OpenTimeout.ToString());
                Assert.Equal(receiveTimeout, customBinding.ReceiveTimeout.ToString());
                Assert.Equal(sendTimeout, customBinding.SendTimeout.ToString());
            }
        }

        [Fact]
        public void CustomBinding_EmptyName_ThrowsArgumentException()
        {
            string closeTimeout = "00:01:00";
            string openTimeout = "00:01:00";
            string receiveTimeout = "00:10:00";
            string sendTimeout = "00:01:00";

            string config = new ServiceModelConfigBuilder()
                .StartConfig()
                .StartClientSection()
                .AddEndpoint(
                    address: EndpointAddress,
                    binding: BindingType,
                    contract: Contract,
                    bindingConfiguration: "",
                    name: EndpointName
                )
                .CloseClientSection()
                .StartBindingsSection()
                    .StartElement("customBinding")
                        .StartBinding()
                        .CloseBinding()
                    .EndElement("customBinding")
                .CloseBindingsSection()
                .EndConfig()
                .ToString();

            using (var tempFile = TemporaryFileStream.Create(config))
            {
                var provider = CreateProvider(tempFile.Name);
                Assert.Throws<ArgumentException>(() =>
                {
                    var endpointBuilder = provider.GetRequiredService<IServiceEndpointBuilder>();
                });
            }
        }


        [Fact]
        public void CustomBinding_IncorrectTimeouts_ThrowsConfigurationErrorsException()
        {
            string closeTimeout = "blah-00:01:00";
            string openTimeout = "blah-00:01:00";
            string receiveTimeout = "00:10:00";
            string sendTimeout = "00:01:00";

            string config = new ServiceModelConfigBuilder()
                .StartConfig()
                .StartClientSection()
                .AddEndpoint(
                    address: EndpointAddress,
                    binding: BindingType,
                    contract: Contract,
                    bindingConfiguration: "",
                    name: EndpointName
                )
                .CloseClientSection()
                .StartBindingsSection()
                    .StartElement("customBinding")
                        .StartBinding(BindingName,
                            ("closeTimeout", closeTimeout),
                            ("openTimeout", openTimeout),
                            ("receiveTimeout", receiveTimeout),
                            ("sendTimeout", sendTimeout))
                        .CloseBinding()
                    .EndElement("customBinding")
                .CloseBindingsSection()
                .EndConfig()
                .ToString();

            using (var tempFile = TemporaryFileStream.Create(config))
            {
                var provider = CreateProvider(tempFile.Name);
                Assert.Throws<System.Configuration.ConfigurationErrorsException>(() =>
                {
                    var endpointBuilder = provider.GetRequiredService<IServiceEndpointBuilder>();
                });
            }
        }
    }
}
