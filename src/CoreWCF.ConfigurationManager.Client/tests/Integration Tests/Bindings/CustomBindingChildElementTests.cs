using CoreWCF.ConfigurationManager.Client;
using Microsoft.Extensions.DependencyInjection;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using Xunit;

namespace CoreWCF.ConfigurationManager.Client.Tests.IntegrationTests.Bindings
{
    public class CustomBindingChildElementTests : TestBase
    {
        private const string EndpointAddress = "http://localhost:8080/TestService";
        private const string BindingType = "customBinding";
        private const string BindingName = "TestCustomBindingWithChildren";
        private const string Contract = "CoreWCF.ConfigurationManager.Client.Tests.IntegrationTests.ISomeService";
        private const string EndpointName = "CustomChildEndpoint";

        [Theory]
        [InlineData("compositeDuplex")]
        [InlineData("pnrpPeerResolver")]
        [InlineData("reliableSession")]
        [InlineData("windowsStreamSecurity")]
        [InlineData("sslStreamSecurity")]
        [InlineData("transactionFlow")]
        public void CustomBinding_WithNotSupportedElements_Throws(string sectionName)
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
                    .StartElement("customBinding")
                        .StartBinding(BindingName)
                            .AddElement(sectionName)
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

        [Fact]
        public void CustomBinding_WithSecuritySection_Empty_ThrowsConfigurationError()
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
                    .StartElement("customBinding")
                        .StartBinding(BindingName)
                            .AddElement("security")
                        .CloseBinding()
                    .EndElement("customBinding")
                .CloseBindingsSection()
                .EndConfig()
                .ToString();

            using (var tempFile = TemporaryFileStream.Create(config))
            {
                var provider = CreateProvider(tempFile.Name);
                Assert.Throws<System.ComponentModel.InvalidEnumArgumentException>(() =>
                {
                    var endpointBuilder = provider.GetRequiredService<IServiceEndpointBuilder>();
                });
            }
        }

        [Theory]
        [InlineData("IssuedTokenForCertificate")]
        [InlineData("IssuedTokenForSslNegotiated")]
        [InlineData("SecureConversation")]
        [InlineData("SspiNegotiatedOverTransport")]
        public void CustomBinding_WithSecuritySection_WithUnsupportedAuthenticationMode_Throws(string authenticationMode)
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
                    .StartElement("customBinding")
                        .StartBinding(BindingName)
                            .AddElement("security",
                                ("authenticationMode", authenticationMode))
                        .CloseBinding()
                    .EndElement("customBinding")
                .CloseBindingsSection()
                .EndConfig()
                .ToString();

            using (var tempFile = TemporaryFileStream.Create(config))
            {
                var provider = CreateProvider(tempFile.Name);
                Assert.Throws<System.ComponentModel.InvalidEnumArgumentException>(() =>
                {
                    var endpointBuilder = provider.GetRequiredService<IServiceEndpointBuilder>();
                });
            }
        }

        [Theory]
        [InlineData("CertificateOverTransport")]
        [InlineData("IssuedTokenOverTransport")]
        [InlineData("UserNameOverTransport")]
        public void CustomBinding_WithSecuritySection_WithSupportedAuthenticationMode_Ok(string authenticationMode)
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
                    .StartElement("customBinding")
                        .StartBinding(BindingName)
                            .AddElement("security",
                                ("authenticationMode", authenticationMode))
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

                var securityElement = customBinding.Elements.Find<SecurityBindingElement>();
                Assert.NotNull(securityElement);
            }
        }

        [Theory]
        [InlineData("WSSecurity11WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11")]
        [InlineData("WSSecurity10WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11BasicSecurityProfile10")]
        [InlineData("WSSecurity11WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11BasicSecurityProfile10")]
        public void CustomBinding_WithSecuritySection_AllAttributesWithMessageSecurityVersion(string messageSecurityVersion)
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
                    .StartElement("customBinding")
                        .StartBinding(BindingName)
                            .StartElement("security",
                                ("authenticationMode", "CertificateOverTransport"),
                                ("defaultAlgorithmSuite", "Basic256"),
                                ("includeTimestamp", "true"),
                                ("keyEntropyMode", "CombinedEntropy"),
                                ("messageSecurityVersion", messageSecurityVersion),
                                ("requireDerivedKeys", "true"),
                                ("requireSecurityContextCancellation", "true"),
                                ("requireSignatureConfirmation", "true"),
                                ("securityHeaderLayout", "Lax")
                            )
                            .EndElement("security")
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

                var securityElement = customBinding.Elements.Find<SecurityBindingElement>();
                Assert.NotNull(securityElement);

                Assert.IsType<TransportSecurityBindingElement>(securityElement);

                Assert.Equal(SecurityAlgorithmSuite.Basic256, securityElement.DefaultAlgorithmSuite);
                Assert.True(securityElement.IncludeTimestamp);
                Assert.Equal(SecurityKeyEntropyMode.CombinedEntropy, securityElement.KeyEntropyMode);
                Assert.Equal(SecurityHeaderLayout.Lax, securityElement.SecurityHeaderLayout);

                Assert.Equal(messageSecurityVersion, securityElement.MessageSecurityVersion.ToString());


            }
        }

        [Theory]
        [InlineData("ClientEntropy")]
        [InlineData("ServerEntropy")]
        [InlineData("CombinedEntropy")]
        public void CustomBinding_WithSecuritySection_AllAttributesWithKeyEntropyMode(string keyEntropyMode)
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
                    .StartElement("customBinding")
                        .StartBinding(BindingName)
                            .StartElement("security",
                                ("authenticationMode", "CertificateOverTransport"),
                                ("defaultAlgorithmSuite", "Basic256"),
                                ("includeTimestamp", "true"),
                                ("keyEntropyMode", keyEntropyMode),
                                //("messageSecurityVersion", messageSecurityVersion),
                                ("requireDerivedKeys", "true"),
                                ("requireSecurityContextCancellation", "true"),
                                ("requireSignatureConfirmation", "true"),
                                ("securityHeaderLayout", "Lax")
                            )
                            .EndElement("security")
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

                var securityElement = customBinding.Elements.Find<SecurityBindingElement>();
                Assert.NotNull(securityElement);

                Assert.IsType<TransportSecurityBindingElement>(securityElement);

                Assert.Equal(SecurityAlgorithmSuite.Basic256, securityElement.DefaultAlgorithmSuite);
                Assert.True(securityElement.IncludeTimestamp);
                Assert.Equal(keyEntropyMode, securityElement.KeyEntropyMode.ToString());
                Assert.Equal(SecurityHeaderLayout.Lax, securityElement.SecurityHeaderLayout);

            }
        }

        [Theory]
        [InlineData("Strict")]
        [InlineData("Lax")]
        [InlineData("LaxTimestampFirst")]
        [InlineData("LaxTimestampLast")]
        public void CustomBinding_WithSecuritySection_AllAttributesWithSecurityHeaderLauout(string securityHeaderLayout)
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
                    .StartElement("customBinding")
                        .StartBinding(BindingName)
                            .StartElement("security",
                                ("authenticationMode", "CertificateOverTransport"),
                                ("defaultAlgorithmSuite", "Basic256"),
                                ("includeTimestamp", "true"),
                                ("requireDerivedKeys", "true"),
                                ("requireSecurityContextCancellation", "true"),
                                ("requireSignatureConfirmation", "true"),
                                ("securityHeaderLayout", securityHeaderLayout)
                            )
                            .EndElement("security")
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

                var securityElement = customBinding.Elements.Find<SecurityBindingElement>();
                Assert.NotNull(securityElement);

                Assert.IsType<TransportSecurityBindingElement>(securityElement);

                Assert.Equal(SecurityAlgorithmSuite.Basic256, securityElement.DefaultAlgorithmSuite);
                Assert.True(securityElement.IncludeTimestamp);
                Assert.Equal(securityHeaderLayout, securityElement.SecurityHeaderLayout.ToString());

            }
        }

        [Fact]
        public void CustomBinding_WithbinaryMessageEncodingAndHttpTransport_CreatedOk()
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
                    .StartElement("customBinding")
                        .StartBinding(BindingName)
                            .StartElement("binaryMessageEncoding")
                                .StartElement("readerQuotas", 
                                    ("maxArrayLength", "2147483647"),
                                    ("maxStringContentLength", "3000000")
                                )
                                .EndElement("readerQuotas")
                            .EndElement("binaryMessageEncoding")
                            .StartElement("httpTransport",  
                                ("maxBufferSize", "2147483647"),
                                ("maxReceivedMessageSize", "2147483647")
                            )
                            .EndElement("httpTransport")
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

                var binaryMessageEncodingElement = customBinding.Elements.Find<BinaryMessageEncodingBindingElement>();
                Assert.NotNull(binaryMessageEncodingElement);
                Assert.Equal(2147483647, binaryMessageEncodingElement.ReaderQuotas.MaxArrayLength);
                Assert.Equal(3000000, binaryMessageEncodingElement.ReaderQuotas.MaxStringContentLength);

                var transportElement = customBinding.Elements.Find<HttpTransportBindingElement>();
                Assert.NotNull(transportElement);
                Assert.Equal(2147483647, transportElement.MaxBufferSize);
                Assert.Equal(2147483647, transportElement.MaxReceivedMessageSize);
            }
        }

        [Fact]
        public void CustomBinding_WithbinaryMessageEncodingAndHttpsTransport_CreatedOk()
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
                    .StartElement("customBinding")
                        .StartBinding(BindingName)
                            .StartElement("binaryMessageEncoding")
                                .StartElement("readerQuotas",
                                    ("maxArrayLength", "2147483647"),
                                    ("maxStringContentLength", "3000000")
                                )
                                .EndElement("readerQuotas")
                            .EndElement("binaryMessageEncoding")
                            .StartElement("httpsTransport",
                                ("maxBufferSize", "2147483647"),
                                ("maxReceivedMessageSize", "2147483647")
                            )
                            .EndElement("httpsTransport")
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

                var binaryMessageEncodingElement = customBinding.Elements.Find<BinaryMessageEncodingBindingElement>();
                Assert.NotNull(binaryMessageEncodingElement);
                Assert.Equal(2147483647, binaryMessageEncodingElement.ReaderQuotas.MaxArrayLength);
                Assert.Equal(3000000, binaryMessageEncodingElement.ReaderQuotas.MaxStringContentLength);

                var transportElement = customBinding.Elements.Find<HttpsTransportBindingElement>();
                Assert.NotNull(transportElement);
                Assert.Equal(2147483647, transportElement.MaxBufferSize);
                Assert.Equal(2147483647, transportElement.MaxReceivedMessageSize);
            }
        }


        [Fact]
        public void CustomBinding_WithbinaryMessageEncodingAndTextMessageEncoding_CreatedOk()
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
                    .StartElement("customBinding")
                        .StartBinding(BindingName)
                            .StartElement("textMessageEncoding",
                                ("messageVersion", "Soap11")
                             )
                            .EndElement("textMessageEncoding")
                            .StartElement("httpsTransport")
                            .EndElement("httpsTransport")
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

                var textMessageEncodingElement = customBinding.Elements.Find<TextMessageEncodingBindingElement>();
                Assert.NotNull(textMessageEncodingElement);
                Assert.Equal(MessageVersion.Soap11, textMessageEncodingElement.MessageVersion);

                var transportElement = customBinding.Elements.Find<HttpsTransportBindingElement>();
                Assert.NotNull(transportElement);
            }
        }

    }
}