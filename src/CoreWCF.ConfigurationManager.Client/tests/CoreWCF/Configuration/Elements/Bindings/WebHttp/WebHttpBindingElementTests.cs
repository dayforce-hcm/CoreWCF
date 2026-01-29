using System;
using System.Text;
using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System.ServiceModel;
using System.Configuration;
using SMTransferMode = System.ServiceModel.TransferMode;
using SMHostNameComparisonMode = System.ServiceModel.HostNameComparisonMode;

namespace CoreWCF.ConfigurationManager.Elements.Bindings.WebHttp.Tests
{
    public class WebHttpBindingElementTests
    {
        [Fact]
        public void CanInstantiateWebHttpBindingElement_DefaultConstructor()
        {
            var element = new WebHttpBindingElement();
            Assert.NotNull(element);
        }

        [Fact]
        public void CanSetAndGetProperties()
        {
            var element = new WebHttpBindingElement();
            element.AllowCookies = true;
            element.BypassProxyOnLocal = true;
            element.HostNameComparisonMode = SMHostNameComparisonMode.Exact;
            element.MaxBufferPoolSize = 1234;
            element.MaxBufferSize = 5678;
            element.MaxReceivedMessageSize = 4321;
            element.ProxyAddress = new Uri("http://localhost");
            element.TransferMode = SMTransferMode.Streamed;
            element.UseDefaultWebProxy = true;
            element.WriteEncoding = Encoding.UTF8;
            element.ContentTypeMapper = "System.String";
            element.CrossDomainScriptAccessEnabled = true;

            Assert.True(element.AllowCookies);
            Assert.True(element.BypassProxyOnLocal);
            Assert.Equal(SMHostNameComparisonMode.Exact, element.HostNameComparisonMode);
            Assert.Equal(1234, element.MaxBufferPoolSize);
            Assert.Equal(5678, element.MaxBufferSize);
            Assert.Equal(4321, element.MaxReceivedMessageSize);
            Assert.Equal(new Uri("http://localhost"), element.ProxyAddress);
            Assert.Equal(SMTransferMode.Streamed, element.TransferMode);
            Assert.True(element.UseDefaultWebProxy);
            Assert.Equal(Encoding.UTF8, element.WriteEncoding);
            Assert.Equal("System.String", element.ContentTypeMapper);
            Assert.True(element.CrossDomainScriptAccessEnabled);
        }

        [Fact]
        public void CreateBinding_ReturnsWebHttpBinding()
        {
            var element = new WebHttpBindingElement();
            element.Name = "TestBinding"; // Set a valid name
            var binding = element.CreateBinding();
            Assert.IsType<CoreWCF.ConfigurationManager.Client.WebHttpBinding>(binding);
        }

        [Fact]
        public void GetContentTypeMapper_InvalidType_Throws()
        {
            Assert.Throws<ConfigurationErrorsException>(() => WebHttpBindingElement.GetContentTypeMapper("System.String"));
        }
    }
}
