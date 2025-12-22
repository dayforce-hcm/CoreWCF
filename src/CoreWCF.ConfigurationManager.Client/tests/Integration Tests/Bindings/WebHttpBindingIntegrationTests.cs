using CoreWCF.ConfigurationManager.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ServiceModel;
using Xunit;

namespace CoreWCF.ConfigurationManager.Client.Tests.IntegrationTests.Bindings
{
    public class WebHttpBindingIntegrationTests : TestBase
    {
        private const string EndpointAddress = "http://localhost:8080/TestService";
        private const string BindingType = "webHttpBinding";
        private const string BindingName = "TestWebHttpBinding";
        private const string Contract = "CoreWCF.ConfigurationManager.Client.Tests.IntegrationTests.ISomeService";
        private const string EndpointName = "TestClientEndpoint";

        [Fact]
        public void WebHttpBinding_AllAttributes_AreAppliedCorrectly()
        {
            string closeTimeout = "00:01:11";
            string openTimeout = "00:01:11";
            string receiveTimeout = "00:11:11";
            string sendTimeout = "00:01:11";
            long maxBufferPoolSize = 12345;
            int maxBufferSize = 23456;
            long maxReceivedMessageSize = 12345;
            string transferMode = "Streamed"; // default Buffered
            string writeEncoding = "UTF-16"; // default utf-8

            // Attributes below are not supported by WebHttpBinding, but they are supported by WebHttpBindingElement
            // which is used to check the ability to read and create WebHttpBinding from configuration
            bool allowCookies = false;   //default false            
            bool bypassProxyOnLocal = false; // default false
            string hostNameComparisonMode = "StrongWildcard"; // default StrongWildcard
            string proxyAddress = "http://localhost:8085/"; // default null
            bool useDefaultWebProxy = true; // default true


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
                    .StartWebHttpBindingSection()
                        .StartBinding(BindingName,
                                ("closeTimeout", closeTimeout),
                                ("openTimeout", openTimeout),
                                ("receiveTimeout", receiveTimeout),
                                ("sendTimeout", sendTimeout),
                        
                                ("maxBufferPoolSize", maxBufferPoolSize.ToString()),
                                ("maxBufferSize", maxBufferSize.ToString()),
                                ("maxReceivedMessageSize", maxReceivedMessageSize.ToString()),
                        
                                ("transferMode", transferMode),
                                ("writeEncoding", writeEncoding),
                                
                                ("allowCookies", allowCookies.ToString().ToLower()),
                                ("bypassProxyOnLocal", bypassProxyOnLocal.ToString().ToLower()),
                                ("hostNameComparisonMode", hostNameComparisonMode.ToString().ToLower()),
                                ("proxyAddress", proxyAddress.ToString().ToLower(System.Globalization.CultureInfo.CurrentCulture)),
                                ("useDefaultWebProxy", useDefaultWebProxy.ToString().ToLower())
                            )
                        .CloseBinding()
                    .CloseWebHttpBindingSection()
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
                
                var webHttpBinding = endpoint.Binding as WebHttpBinding;
                Assert.NotNull(webHttpBinding);
                Assert.Equal(closeTimeout, webHttpBinding.CloseTimeout.ToString());
                Assert.Equal(openTimeout, webHttpBinding.OpenTimeout.ToString());
                Assert.Equal(receiveTimeout, webHttpBinding.ReceiveTimeout.ToString());
                Assert.Equal(sendTimeout, webHttpBinding.SendTimeout.ToString());

                Assert.Equal(maxBufferPoolSize, webHttpBinding.MaxBufferPoolSize);
                Assert.Equal(maxBufferSize, webHttpBinding.MaxBufferSize);
                Assert.Equal(maxReceivedMessageSize, webHttpBinding.MaxReceivedMessageSize);

                Assert.Equal(transferMode, webHttpBinding.TransferMode.ToString());
                Assert.Equal(writeEncoding, webHttpBinding.WriteEncoding.WebName, ignoreCase: true);

                // The attributes below are not supported by WebHttpBinding, so we cannot check them here   
            }
        }

        [Fact]
        public void WebHttpBinding_DefaultValues_OnlyRequiredAttributes_AreAppliedCorrectly()
        {
            string closeTimeout = "00:01:00";
            string openTimeout = "00:01:00";
            string receiveTimeout = "00:10:00";
            string sendTimeout = "00:01:00";
            long maxBufferPoolSize = 524288L;
            long maxReceivedMessageSize = 65536;
            string transferMode = "Buffered";

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
                .StartWebHttpBindingSection()
                .StartBinding(BindingName)
                .CloseBinding()
                .CloseWebHttpBindingSection()
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
                
                var webHttpBinding = endpoint.Binding as WebHttpBinding;
                Assert.NotNull(webHttpBinding);
                Assert.Equal(maxReceivedMessageSize, webHttpBinding.MaxReceivedMessageSize);
                Assert.Equal(maxBufferPoolSize, webHttpBinding.MaxBufferPoolSize);
                Assert.Equal(transferMode, webHttpBinding.TransferMode.ToString());
                Assert.Equal(closeTimeout, webHttpBinding.CloseTimeout.ToString());
                Assert.Equal(openTimeout, webHttpBinding.OpenTimeout.ToString());
                Assert.Equal(receiveTimeout, webHttpBinding.ReceiveTimeout.ToString());
                Assert.Equal(sendTimeout, webHttpBinding.SendTimeout.ToString());
            }
        }

        [Fact]
        public void WebHttpBinding_EmptyName_ShouldThrow()
        {
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
                .StartWebHttpBindingSection()
                    .StartBinding()
                    .CloseBinding()
                .CloseWebHttpBindingSection()
                .CloseBindingsSection()
                .EndConfig()
                .ToString();

            using (var tempFile = TemporaryFileStream.Create(config))
            {
                var provider = CreateProvider(tempFile.Name);
                Assert.Throws<ArgumentException>(() => { var endpointBuilder = provider.GetRequiredService<IServiceEndpointBuilder>(); });
            }
        }

        [Fact]
        public void WebHttpBinding_ReaderQuotas_AllAttributes_AreAppliedCorrectly()
        {
            string maxDepth = "17";
            string maxStringContentLength = "2500";
            string maxArrayLength = "3500";
            string maxBytesPerRead = "450";
            string maxNameTableCharCount = "5500";

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
                .StartWebHttpBindingSection()
                .StartBinding(BindingName)
                    .AddElement("readerQuotas",
                        ("maxDepth", maxDepth),
                        ("maxStringContentLength", maxStringContentLength),
                        ("maxArrayLength", maxArrayLength),
                        ("maxBytesPerRead", maxBytesPerRead),
                        ("maxNameTableCharCount", maxNameTableCharCount)
                    )
                .CloseBinding()
                .CloseWebHttpBindingSection()
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
                var webHttpBinding = endpoint.Binding as WebHttpBinding;
                Assert.NotNull(webHttpBinding);
                var quotas = webHttpBinding.ReaderQuotas;
                Assert.NotNull(quotas);
                Assert.Equal(int.Parse(maxDepth), quotas.MaxDepth);
                Assert.Equal(int.Parse(maxStringContentLength), quotas.MaxStringContentLength);
                Assert.Equal(int.Parse(maxArrayLength), quotas.MaxArrayLength);
                Assert.Equal(int.Parse(maxBytesPerRead), quotas.MaxBytesPerRead);
                Assert.Equal(int.Parse(maxNameTableCharCount), quotas.MaxNameTableCharCount);
            }
        }
    }
}
