using CoreWCF.ConfigurationManager.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ServiceModel;
using Xunit;

namespace CoreWCF.ConfigurationManager.Client.Tests.IntegrationTests.Bindings
{
    public class BasicHttpBindingAllAttributesTests : TestBase
    {
        private const string EndpointAddress = "http://localhost:8080/TestService";
        private const string BindingType = "basicHttpBinding";
        private const string BindingName = "TestHttpBinding";
        private const string Contract = "CoreWCF.ConfigurationManager.Client.Tests.IntegrationTests.ISomeService";
        private const string EndpointName = "TestClientEndpoint";
        
        [Fact]
        public void BasicHttpBinding_AllAttributes_AreAppliedCorrectly()
        {
            // Set test values for all supported attributes
            string closeTimeout = "00:01:11";           //"00:01:00" is the default
            string openTimeout = "00:01:11";            //"00:01:00" is the default
            string receiveTimeout = "00:11:11";         //"00:10:00" is the default
            string sendTimeout = "00:01:11";            //"00:01:00" is the default
            bool allowCookies = true;                   // false is the default
            bool bypassProxyOnLocal = true;             // false is the default
            string hostNameComparisonMode = "Exact";    // "StrongWildcard" is the default
            long maxBufferPoolSize = 12345;             // 524288 is the default
            int maxBufferSize = 12345;                  // 65536 is the default
            long maxReceivedMessageSize = 12345;        // 65536 is the default
            string messageEncoding = "Mtom";            // "Text" is the default
            string proxyAddress = "http://proxy:8081/";  // null is the default
            string textEncoding = "UTF-16";              // "utf-8" is the default
            string transferMode = "Streamed";           //"Buffered" is the default
            bool useDefaultWebProxy = false;            // true is the default

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
                .StartHttpBindingSection()
                .StartBinding(BindingName,
                        ("closeTimeout", closeTimeout),
                        ("openTimeout", openTimeout),
                        ("receiveTimeout", receiveTimeout),
                        ("sendTimeout", sendTimeout),
                        ("allowCookies", allowCookies.ToString().ToLower()),
                        ("bypassProxyOnLocal", bypassProxyOnLocal.ToString().ToLower()),
                        ("hostNameComparisonMode", hostNameComparisonMode),
                        ("maxBufferPoolSize", maxBufferPoolSize.ToString()),
                        ("maxBufferSize", maxBufferSize.ToString()),
                        ("maxReceivedMessageSize", maxReceivedMessageSize.ToString()),
                        ("messageEncoding", messageEncoding),
                        ("proxyAddress", proxyAddress),
                        ("textEncoding", textEncoding),
                        ("transferMode", transferMode),
                        ("useDefaultWebProxy", useDefaultWebProxy.ToString().ToLower())
                    )
                .CloseBinding()
                .CloseHttpBindingSection()
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
                var basicHttpBinding = endpoint.Binding as BasicHttpBinding;
                Assert.NotNull(basicHttpBinding);
                Assert.Equal(maxBufferSize, basicHttpBinding.MaxBufferSize);
                Assert.Equal(maxReceivedMessageSize, basicHttpBinding.MaxReceivedMessageSize);
                #region UnsupportedProperties in CoreWCF
                // should return default value,
                // false is the default
                Assert.False(basicHttpBinding.AllowCookies);

                // should return default value,
                // false is the default
                Assert.False(basicHttpBinding.BypassProxyOnLocal);
#if NETFRAMEWORK
                // should return default value
                // "StrongWildcard" is the default
                Assert.Equal("StrongWildcard", basicHttpBinding.HostNameComparisonMode.ToString());
#endif
                #endregion
                Assert.Equal(maxBufferPoolSize, basicHttpBinding.MaxBufferPoolSize);
                Assert.Equal(messageEncoding, basicHttpBinding.MessageEncoding.ToString());
                Assert.Equal(proxyAddress, basicHttpBinding.ProxyAddress?.ToString());
                Assert.Equal(textEncoding, basicHttpBinding.TextEncoding.WebName, ignoreCase: true);
                Assert.Equal(transferMode, basicHttpBinding.TransferMode.ToString());
                Assert.Equal(useDefaultWebProxy, basicHttpBinding.UseDefaultWebProxy);
                Assert.Equal(closeTimeout, basicHttpBinding.CloseTimeout.ToString());
                Assert.Equal(openTimeout, basicHttpBinding.OpenTimeout.ToString());
                Assert.Equal(receiveTimeout, basicHttpBinding.ReceiveTimeout.ToString());
                Assert.Equal(sendTimeout, basicHttpBinding.SendTimeout.ToString());
            }
        }

        [Fact]
        public void BasicHttpBinding_DefaultValues_OnlyRequiredAttributes_AreAppliedCorrectly()
        {
            // Set test values for all supported attributes
            string closeTimeout = "00:01:00";           //"00:01:00" is the default
            string openTimeout = "00:01:00";            //"00:01:00" is the default
            string receiveTimeout = "00:10:00";         //"00:10:00" is the default
            string sendTimeout = "00:01:00";            //"00:01:00" is the default
            bool allowCookies = false;                   // false is the default
            bool bypassProxyOnLocal = false;             // false is the default
            string hostNameComparisonMode = "StrongWildcard";    // "StrongWildcard" is the default
            long maxBufferPoolSize = 524288L;             // 524288 is the default
            int maxBufferSize = 65536;                  // 65536 is the default
            long maxReceivedMessageSize = 65536;        // 65536 is the default
            string messageEncoding = "Text";            // "Text" is the default
            string proxyAddress = null;                 // null is the default
            string textEncoding = "utf-8";              // "utf-8" is the default
            string transferMode = "Buffered";           //"Buffered" is the default
            bool useDefaultWebProxy = true;            // true is the default

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
                .StartHttpBindingSection()
                .StartBinding(BindingName, ("messageEncoding", messageEncoding))
                .CloseBinding()
                .CloseHttpBindingSection()
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
                var basicHttpBinding = endpoint.Binding as BasicHttpBinding;
                Assert.NotNull(basicHttpBinding);
                Assert.Equal(maxBufferSize, basicHttpBinding.MaxBufferSize);
                Assert.Equal(maxReceivedMessageSize, basicHttpBinding.MaxReceivedMessageSize);

                #region UnsupportedProperties in CoreWCF
                // should return default value,
                // false is the default
                Assert.False(basicHttpBinding.AllowCookies);

                // should return default value,
                // false is the default
                Assert.False(basicHttpBinding.BypassProxyOnLocal);


#if NETFRAMEWORK
                // should return default value
                // "StrongWildcard" is the default
                Assert.Equal("StrongWildcard", basicHttpBinding.HostNameComparisonMode.ToString());
#endif
                #endregion
                Assert.Equal(maxBufferPoolSize, basicHttpBinding.MaxBufferPoolSize);
                Assert.Equal(messageEncoding, basicHttpBinding.MessageEncoding.ToString());
                Assert.Equal(proxyAddress, basicHttpBinding.ProxyAddress?.ToString());
                Assert.Equal(textEncoding, basicHttpBinding.TextEncoding.WebName, ignoreCase: true);
                Assert.Equal(transferMode, basicHttpBinding.TransferMode.ToString());
                Assert.Equal(useDefaultWebProxy, basicHttpBinding.UseDefaultWebProxy);
                Assert.Equal(closeTimeout, basicHttpBinding.CloseTimeout.ToString());
                Assert.Equal(openTimeout, basicHttpBinding.OpenTimeout.ToString());
                Assert.Equal(receiveTimeout, basicHttpBinding.ReceiveTimeout.ToString());
                Assert.Equal(sendTimeout, basicHttpBinding.SendTimeout.ToString());
            }
        }

        [Fact]
        public void BasicHttpBinding_EmptyName_ShouldThrow()
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
                .StartHttpBindingSection()
                    .StartBinding()
                    .CloseBinding()
                .CloseHttpBindingSection()
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
        public void BasicHttpBinding_ReaderQuotas_AllAttributes_AreAppliedCorrectly()
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
                .StartHttpBindingSection()
                .StartBinding(BindingName)
                    .AddElement("readerQuotas",
                        ("maxDepth", maxDepth),
                        ("maxStringContentLength", maxStringContentLength),
                        ("maxArrayLength", maxArrayLength),
                        ("maxBytesPerRead", maxBytesPerRead),
                        ("maxNameTableCharCount", maxNameTableCharCount)
                    )
                .CloseBinding()
                .CloseHttpBindingSection()
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
                var basicHttpBinding = endpoint.Binding as BasicHttpBinding;
                Assert.NotNull(basicHttpBinding);
                var quotas = basicHttpBinding.ReaderQuotas;
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
