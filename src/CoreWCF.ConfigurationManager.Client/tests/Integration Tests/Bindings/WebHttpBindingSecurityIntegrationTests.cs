using CoreWCF.ConfigurationManager.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.ServiceModel;
using Xunit;

namespace CoreWCF.ConfigurationManager.Client.Tests.IntegrationTests.Bindings
{
    public class WebHttpBindingSecurityIntegrationTests : TestBase
    {
        private const string EndpointAddress = "http://localhost:8080/TestService";
        private const string BindingType = "webHttpBinding";
        private const string BindingName = "TestWebHttpBindingSecurity";
        private const string Contract = "CoreWCF.ConfigurationManager.Client.Tests.IntegrationTests.ISomeService";
        private const string EndpointName = "TestClientEndpointWebHttpSecurity";

        [Theory]
        [InlineData("None")]
        [InlineData("Transport")]
        [InlineData("TransportCredentialOnly")]
        public void WebHttpBinding_Security_AllAttributes_AreAppliedCorrectly(string securityMode)
        {
            string transportClientCredentialType = "Windows";
            string transportProxyCredentialType = "None"; // default in element, not supported by binding

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
                    .StartSecurity(securityMode)
                        .StartTransport(transportClientCredentialType)
                        .CloseTransport()
                    .CloseSecurity()
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
                var security = webHttpBinding.Security;
                Assert.NotNull(security);
                Assert.Equal(securityMode, security.Mode.ToString());
                Assert.Equal(transportClientCredentialType, security.Transport.ClientCredentialType.ToString());
                // ProxyCredentialType is not supported by WebHttpBinding, element default remains None
                Assert.Equal(transportProxyCredentialType, security.Transport.ProxyCredentialType.ToString());
            }
        }

        [Fact]
        public void WebHttpBinding_Security_DefaultValues_AreAppliedCorrectly()
        {
            // Default values for security attributes
            string securityMode = "None";
            string transportClientCredentialType = "None";
            string transportProxyCredentialType = "None";

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
                    .StartSecurity(securityMode)
                        .StartTransport(transportClientCredentialType)
                        .CloseTransport()
                    .CloseSecurity()
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
                var security = webHttpBinding.Security;
                Assert.NotNull(security);
                Assert.Equal(securityMode, security.Mode.ToString());
                Assert.Equal(transportClientCredentialType, security.Transport.ClientCredentialType.ToString());
                Assert.Equal(transportProxyCredentialType, security.Transport.ProxyCredentialType.ToString());
            }
        }

        [Theory]
        [InlineData("Basic")]
        [InlineData("Certificate")]
        [InlineData("Digest")]
        [InlineData("None")]
        [InlineData("Ntlm")]
        [InlineData("Windows")]
        public void WebHttpBinding_Security_TransportCredentialTypeValues_AreAppliedCorrectly(string transportClientCredentialType)
        {
            // Default values for security attributes
            string securityMode = "None";
            string transportProxyCredentialType = "None";

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
                    .StartSecurity(securityMode)
                        .StartTransport(transportClientCredentialType)
                        .CloseTransport()
                    .CloseSecurity()
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
                var security = webHttpBinding.Security;
                Assert.NotNull(security);
                Assert.Equal(securityMode, security.Mode.ToString());
                Assert.Equal(transportClientCredentialType, security.Transport.ClientCredentialType.ToString());
                Assert.Equal(transportProxyCredentialType, security.Transport.ProxyCredentialType.ToString());
            }
        }

        [Theory]
        [InlineData("None")]
        [InlineData("Transport")]
        [InlineData("TransportCredentialOnly")]
        public void WebHttpBinding_Security_SectionOnly_AllAttributes(string securityMode)
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
                .StartBinding(BindingName)
                    .StartSecurity(securityMode)
                    .CloseSecurity()
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
                var security = webHttpBinding.Security;
                Assert.NotNull(security);
                Assert.Equal(securityMode, security.Mode.ToString());
            }
        }

        [Theory]
        [InlineData("None")]
        [InlineData("Transport")]
        [InlineData("TransportCredentialOnly")]
        public void WebHttpBinding_Security_TransportProxyCredentialType_IsNotSupported_Throws(string securityMode)
        {
            string transportClientCredentialType = "Windows";
            string transportProxyCredentialType = "Basic"; // forcing explicit proxyCredentialType which should be unsupported

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
                    .StartSecurity(securityMode)
                        .StartTransport(transportClientCredentialType, transportProxyCredentialType)
                        .CloseTransport()
                    .CloseSecurity()
                .CloseBinding()
                .CloseWebHttpBindingSection()
                .CloseBindingsSection()
                .EndConfig()
                .ToString();

            using (var tempFile = TemporaryFileStream.Create(config))
            {
                var provider = CreateProvider(tempFile.Name);
                Assert.Throws<ConfigurationErrorsException>(() => provider.GetRequiredService<IServiceEndpointBuilder>());
            }
        }

        [Theory]
        [InlineData("NonSupportedSecurityMode")]
        public void WebHttpBinding_Security_NonSupportedMode_Throws(string securityMode)
        {
            string transportClientCredentialType = "None";

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
                    .StartSecurity(securityMode)
                        .StartTransport(transportClientCredentialType)
                        .CloseTransport()
                    .CloseSecurity()
                .CloseBinding()
                .CloseWebHttpBindingSection()
                .CloseBindingsSection()
                .EndConfig()
                .ToString();

            using (var tempFile = TemporaryFileStream.Create(config))
            {
                var provider = CreateProvider(tempFile.Name);
                Assert.Throws<ConfigurationErrorsException>(() => provider.GetRequiredService<IServiceEndpointBuilder>());
            }
        }
    }
}
