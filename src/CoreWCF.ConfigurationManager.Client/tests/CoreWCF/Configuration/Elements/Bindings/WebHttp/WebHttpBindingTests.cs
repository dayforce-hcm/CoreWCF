using System;
using System.Text;
using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System.ServiceModel.Channels;
using System.Xml;
using System.ServiceModel;
using SMTransferMode = System.ServiceModel.TransferMode;

namespace CoreWCF.ConfigurationManager.Elements.Bindings.WebHttp.Tests
{
    public class WebHttpBindingTests
    {
        [Fact]
        public void CanInstantiateWebHttpBinding_DefaultConstructor()
        {
            var binding = new WebHttpBinding();
            Assert.NotNull(binding);
        }

        [Fact]
        public void CanInstantiateWebHttpBinding_WithSecurityMode()
        {
            var binding = new WebHttpBinding(WebHttpSecurityMode.Transport);
            Assert.Equal(WebHttpSecurityMode.Transport, binding.Security.Mode);
        }

        [Fact]
        public void DefaultValues_AreCorrect()
        {
            var binding = new WebHttpBinding();
            Assert.Equal(65536, binding.MaxReceivedMessageSize);
            Assert.Equal(524288, binding.MaxBufferPoolSize);
            Assert.Equal(SMTransferMode.Buffered, binding.TransferMode);
#if NETFRAMEWORK
            Assert.Equal(Encoding.GetEncoding(1252).WebName, binding.WriteEncoding.WebName); // Windows-1252 is the default
#else
            Assert.Equal(Encoding.GetEncoding(/*utf-8*/65001).WebName, binding.WriteEncoding.WebName); // utf-8 is the default

#endif
            Assert.False(binding.CrossDomainScriptAccessEnabled);
            Assert.Equal(WebHttpSecurityMode.None, binding.Security.Mode);
        }

        [Fact]
        public void CanSetAndGetProperties()
        {
            var binding = new WebHttpBinding();
            binding.MaxBufferPoolSize = 1234;
            binding.MaxBufferSize = 5678;
            binding.MaxReceivedMessageSize = 4321;
            binding.TransferMode = SMTransferMode.Streamed;
            binding.WriteEncoding = Encoding.UTF8;
            binding.CrossDomainScriptAccessEnabled = true;
            Assert.Equal(1234, binding.MaxBufferPoolSize);
            Assert.Equal(5678, binding.MaxBufferSize);
            Assert.Equal(4321, binding.MaxReceivedMessageSize);
            Assert.Equal(SMTransferMode.Streamed, binding.TransferMode);
            Assert.Equal(Encoding.UTF8, binding.WriteEncoding);
            Assert.True(binding.CrossDomainScriptAccessEnabled);
        }

        [Fact]
        public void Setting_InvalidSecurityMode_Throws()
        {
            var binding = new WebHttpBinding();
            Assert.Throws<ArgumentOutOfRangeException>(() => binding.Security.Mode = (WebHttpSecurityMode)999);
        }

        [Fact]
        public void CreateBindingElements_ReturnsBindingElementCollection()
        {
            var binding = new WebHttpBinding();
            var elements = binding.CreateBindingElements();
            Assert.NotNull(elements);
            Assert.True(elements.Count >= 2);
        }

        [Fact]
        public void CreateBindingElements_WithTransport_UsesHttpsTransport()
        {
            var binding = new WebHttpBinding(WebHttpSecurityMode.Transport);
            var elements = binding.CreateBindingElements();
            Assert.NotNull(elements);
            bool hasHttps = false;
            foreach (var el in elements)
            {
                if (el is HttpsTransportBindingElement)
                {
                    hasHttps = true;
                    break;
                }
            }
            Assert.True(hasHttps);
        }

        [Fact]
        public void CreateBindingElements_WithTransportCredentialOnly_UsesHttpTransport()
        {
            var binding = new WebHttpBinding(WebHttpSecurityMode.TransportCredentialOnly);
            var elements = binding.CreateBindingElements();
            Assert.NotNull(elements);
            bool hasHttp = false;
            foreach (var el in elements)
            {
                if (el is HttpTransportBindingElement)
                {
                    hasHttp = true;
                    break;
                }
            }
            Assert.True(hasHttp);
        }

        [Fact]
        public void TransportCredentialOnly_WithCertificateClientCredential_Throws()
        {
            var binding = new WebHttpBinding(WebHttpSecurityMode.TransportCredentialOnly);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
            Assert.Throws<InvalidOperationException>(() => binding.CreateBindingElements());
        }
    }
}
