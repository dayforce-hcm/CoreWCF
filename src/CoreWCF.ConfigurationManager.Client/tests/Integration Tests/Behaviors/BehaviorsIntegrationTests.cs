using CoreWCF.ConfigurationManager.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ServiceModel;
using Xunit;
using System.ServiceModel.Channels;

namespace CoreWCF.ConfigurationManager.Client.Tests.IntegrationTests.Behaviors
{
    public class BehaviorsIntegrationTests : TestBase
    {
        private const string EndpointAddress = "http://localhost:8080/TestService";
        private const string BindingType = "customBinding";
        private const string BindingName = "TestCustomBinding";
        private const string Contract = "CoreWCF.ConfigurationManager.Client.Tests.IntegrationTests.ISomeService";
        private const string EndpointName = "DummyEndpoint";
        
        [Fact]
        public void CustomBinding_WithBehaviors_NotSupported_ProcessedOk()
        {
            string config = new ServiceModelConfigBuilder()
                .StartConfig()
                .StartClientSection()
                .AddEndpoint(
                    address: EndpointAddress,
                    binding: BindingType,
                    contract: Contract,
                    bindingConfiguration: BindingName,
                    name: EndpointName,
                    behaviorConfiguration: "TestEndpointBehavior"
                )
                .CloseClientSection()
                .StartBindingsSection()
                    .StartElement("customBinding")
                        .StartBinding(BindingName)
                            .StartElement("binaryMessageEncoding")
                                .AddElement("readerQuotas",
                                    ("maxArrayLength", "2147483647"),
                                    ("maxStringContentLength", "300000")                                    
                                )
                            .EndElement("binaryMessageEncoding")
                            .StartElement("httpTransport",
                                ("maxBufferSize", "2147483647"),
                                ("maxReceivedMessageSize", "2147483647")
                            )
                            .EndElement("httpTransport")
                        .CloseBinding()
                    .EndElement("customBinding")
                .CloseBindingsSection()
                .StartElement("behaviors")
                    .StartElement("endpointBehaviors")
                        .StartElement("behavior", 
                            ("name", "TestEndpointBehavior"))
                            .AddElement("dataContractSerializer",
                                ("maxItemsInObjectGraph", "2147483647")
                            )                            
                        .EndElement("behavior")
                    .EndElement("endpointBehaviors")
                .EndElement("behaviors")
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
                Assert.Empty(endpoint.EndpointBehaviors); // Behaviors are not applied automatically in CoreWCF    
            }
        }

    }
}
