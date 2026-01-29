using CoreWCF.ConfigurationManager.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.ServiceModel;
using Xunit;

namespace CoreWCF.ConfigurationManager.Client.Tests.IntegrationTests.Bindings
{
    public class BasicHttpBindingSecurityIntegrationTests : TestBase
    {
        private const string EndpointAddress = "http://localhost:8080/TestService";
        private const string BindingType = "basicHttpBinding";
        private const string BindingName = "TestHttpBindingSecurity";
        private const string Contract = "CoreWCF.ConfigurationManager.Client.Tests.IntegrationTests.ISomeService";
        private const string EndpointName = "TestClientEndpointSecurity";

        [Theory]
        [InlineData("None")]
#if NETFRAMEWORK
        [InlineData("Message")]
#endif
        [InlineData("Transport")]
        [InlineData("TransportWithMessageCredential")]
        [InlineData("TransportCredentialOnly")]
        public void BasicHttpBinding_Security_AllAttributes_AreAppliedCorrectly(string securityMode)
        {
            string transportClientCredentialType = "Windows";
            string transportProxyCredentialType = "None";
            string messageClientCredentialType = "UserName";
            string messageAlgorithmSuite = "Default"; // The default value is Basic256
            string defaultMessageAlgorithmSuite = "Basic256";

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
                    .StartSecurity(securityMode)
                        .StartTransport(transportClientCredentialType)
                        .CloseTransport()
                        .AddMessage(messageClientCredentialType, messageAlgorithmSuite)
                    .CloseSecurity()
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
                var basicHttpBinding = endpoint.Binding as System.ServiceModel.BasicHttpBinding;
                Assert.NotNull(basicHttpBinding);
                var security = basicHttpBinding.Security;
                Assert.NotNull(security);
                Assert.Equal(securityMode, security.Mode.ToString());
                Assert.Equal(transportClientCredentialType, security.Transport.ClientCredentialType.ToString());
                Assert.Equal(transportProxyCredentialType, security.Transport.ProxyCredentialType.ToString());
                Assert.Equal(messageClientCredentialType, security.Message.ClientCredentialType.ToString());
                Assert.NotEqual(messageAlgorithmSuite, security.Message.AlgorithmSuite.ToString());
                Assert.Equal(defaultMessageAlgorithmSuite, security.Message.AlgorithmSuite.ToString());
            }
        }


        [Theory]
        [InlineData("Default", "Basic256", "UserName")]
        [InlineData("Basic256", "Basic256", "UserName")]
        [InlineData("Basic256Sha256", "Basic256Sha256", "Certificate")]
        [InlineData("TripleDes", "TripleDes", "Certificate")]
        public void BasicHttpBinding_Security_Message_AllAttributes_AreAppliedCorrectly(string messageAlgorithmSuite, string expectedMessageAlgorithmSuite, string messageClientCredentialType)
        {
            string securityMode = "Transport";
            string transportClientCredentialType = "Windows";
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
                .StartHttpBindingSection()
                .StartBinding(BindingName)
                    .StartSecurity(securityMode)
                        .StartTransport(transportClientCredentialType)
                        .CloseTransport()
                        .AddMessage(messageClientCredentialType, messageAlgorithmSuite)
                    .CloseSecurity()
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
                var basicHttpBinding = endpoint.Binding as System.ServiceModel.BasicHttpBinding;
                Assert.NotNull(basicHttpBinding);
                var security = basicHttpBinding.Security;
                Assert.NotNull(security);
                Assert.Equal(securityMode, security.Mode.ToString());
                Assert.Equal(transportClientCredentialType, security.Transport.ClientCredentialType.ToString());
                Assert.Equal(transportProxyCredentialType, security.Transport.ProxyCredentialType.ToString());
                Assert.Equal(messageClientCredentialType, security.Message.ClientCredentialType.ToString());
                Assert.Equal(expectedMessageAlgorithmSuite, security.Message.AlgorithmSuite.ToString());
            }
        }


        [Fact]
        public void BasicHttpBinding_Security_DefaultValues_AreAppliedCorrectly()
        {
            // Default values for security attributes
            string securityMode = "None";
            string transportClientCredentialType = "None";
            string transportProxyCredentialType = "None";
            string messageClientCredentialType = "UserName";
            string messageAlgorithmSuite = "Basic256";

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
                    .StartSecurity(securityMode)
                        .StartTransport(transportClientCredentialType)
                        .CloseTransport()
                        .AddMessage(messageClientCredentialType, messageAlgorithmSuite)
                    .CloseSecurity()
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
                var basicHttpBinding = endpoint.Binding as System.ServiceModel.BasicHttpBinding;
                Assert.NotNull(basicHttpBinding);
                var security = basicHttpBinding.Security;
                Assert.NotNull(security);
                Assert.Equal(securityMode, security.Mode.ToString());
                Assert.Equal(transportClientCredentialType, security.Transport.ClientCredentialType.ToString());
                Assert.Equal(transportProxyCredentialType, security.Transport.ProxyCredentialType.ToString());
                Assert.Equal(messageClientCredentialType, security.Message.ClientCredentialType.ToString());
                Assert.Equal(messageAlgorithmSuite, security.Message.AlgorithmSuite.ToString());
            }
        }

        [Theory]
        [InlineData("None")]
#if NETFRAMEWORK
        [InlineData("Message")]
#endif
        [InlineData("Transport")]
        [InlineData("TransportWithMessageCredential")]
        [InlineData("TransportCredentialOnly")]
        public void BasicHttpBinding_Security_SectionOnly_AllAttributes(string securityMode)
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
                .StartBinding(BindingName)
                    .StartSecurity(securityMode)
                    .CloseSecurity()
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
                var basicHttpBinding = endpoint.Binding as System.ServiceModel.BasicHttpBinding;
                Assert.NotNull(basicHttpBinding);
                var security = basicHttpBinding.Security;
                Assert.NotNull(security);
                Assert.Equal(securityMode, security.Mode.ToString());
            }
        }


        [Theory]
        [InlineData("None")]
#if NETFRAMEWORK
        [InlineData("Message")]
#endif
        [InlineData("Transport")]
        [InlineData("TransportCredentialOnly")]
        [InlineData("TransportWithMessageCredential")]
        public void BasicHttpBinding_Security_TransportProxyCredentialType_IsNotSupported_Throws(string securityMode)
        {
            string transportClientCredentialType = "Windows";
            string transportProxyCredentialType = "None";
            string messageClientCredentialType = "UserName";
            string messageAlgorithmSuite = "Basic256";

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
                    .StartSecurity(securityMode)
                        .StartTransport(transportClientCredentialType, transportProxyCredentialType)
                        .CloseTransport()
                        .AddMessage(messageClientCredentialType, messageAlgorithmSuite)
                    .CloseSecurity()
                .CloseBinding()
                .CloseHttpBindingSection()
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
