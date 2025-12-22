using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings
{
    public class HttpBindingBaseElementTests
    {
        private class TestHttpBindingBaseElement : HttpBindingBaseElement
        {
            public TestHttpBindingBaseElement(string name) : base(name) { }
            public override Binding CreateBinding() => null;
        }

        [Fact]
        public void CanInstantiateHttpBindingBaseElement()
        {
            var element = new TestHttpBindingBaseElement("test");
            Assert.NotNull(element);
            Assert.Equal("test", element.Name);
        }

        [Fact]
        public void AllowCookies_ThrowsPlatformNotSupported()
        {
            var element = new TestHttpBindingBaseElement("test");
            Assert.Throws<PlatformNotSupportedException>(() => _ = element.AllowCookies);
        }

        [Fact]
        public void BypassProxyOnLocal_ThrowsPlatformNotSupported()
        {
            var element = new TestHttpBindingBaseElement("test");
            Assert.Throws<PlatformNotSupportedException>(() => _ = element.BypassProxyOnLocal);
        }

        [Fact]
        public void HostNameComparisonMode_DefaultIsStrongWildcard()
        {
            var element = new TestHttpBindingBaseElement("test");
            Assert.Equal(HostNameComparisonMode.StrongWildcard, element.HostNameComparisonMode);
        }

        [Fact]
        public void HostNameComparisonMode_CanSetAndGet()
        {
            var element = new TestHttpBindingBaseElement("test");
            element.HostNameComparisonMode = HostNameComparisonMode.Exact;
            Assert.Equal(HostNameComparisonMode.Exact, element.HostNameComparisonMode);
        }

        [Fact]
        public void MaxBufferPoolSize_DefaultIs524288L()
        {
            var element = new TestHttpBindingBaseElement("test");
            Assert.Equal(524288L, element.MaxBufferPoolSize);
        }

        [Fact]
        public void MaxBufferPoolSize_CanSetAndGet()
        {
            var element = new TestHttpBindingBaseElement("test");
            element.MaxBufferPoolSize = 1024L;
            Assert.Equal(1024L, element.MaxBufferPoolSize);
        }

        [Fact]
        public void MaxBufferSize_DefaultIs65536()
        {
            var element = new TestHttpBindingBaseElement("test");
            Assert.Equal(65536, element.MaxBufferSize);
        }

        [Fact]
        public void MaxBufferSize_CanSetAndGet()
        {
            var element = new TestHttpBindingBaseElement("test");
            element.MaxBufferSize = 2048;
            Assert.Equal(2048, element.MaxBufferSize);
        }

        [Fact]
        public void MaxReceivedMessageSize_DefaultIs65536()
        {
            var element = new TestHttpBindingBaseElement("test");
            Assert.Equal(65536L, element.MaxReceivedMessageSize);
        }

        [Fact]
        public void MaxReceivedMessageSize_CanSetAndGet()
        {
            var element = new TestHttpBindingBaseElement("test");
            element.MaxReceivedMessageSize = 4096L;
            Assert.Equal(4096L, element.MaxReceivedMessageSize);
        }

        [Fact]
        public void MessageEncoding_DefaultIsText()
        {
            var element = new TestHttpBindingBaseElement("test");
            Assert.Equal("Text", element.MessageEncoding);
        }

        [Fact]
        public void MessageEncoding_CanSetAndGet()
        {
            var element = new TestHttpBindingBaseElement("test");
            element.MessageEncoding = "Binary";
            Assert.Equal("Binary", element.MessageEncoding);
        }

        [Fact]
        public void ProxyAddress_DefaultIsNull()
        {
            var element = new TestHttpBindingBaseElement("test");
            Assert.Null(element.ProxyAddress);
        }

        [Fact]
        public void ProxyAddress_CanSetAndGet()
        {
            var element = new TestHttpBindingBaseElement("test");
            var uri = new Uri("http://proxy.local:8080");
            element.ProxyAddress = uri;
            Assert.Equal(uri, element.ProxyAddress);
        }

        [Fact]
        public void TextEncoding_DefaultIsUtf8()
        {
            var element = new TestHttpBindingBaseElement("test");
            Assert.Equal(Encoding.UTF8.WebName, element.TextEncoding.WebName);
        }

        [Fact]
        public void TextEncoding_CanSetAndGet()
        {
            var element = new TestHttpBindingBaseElement("test");
            element.TextEncoding = Encoding.Unicode;
            Assert.Equal(Encoding.Unicode.WebName, element.TextEncoding.WebName);
        }

        [Fact]
        public void TransferMode_DefaultIsBuffered()
        {
            var element = new TestHttpBindingBaseElement("test");
            Assert.Equal(TransferMode.Buffered, element.TransferMode);
        }

        [Fact]
        public void TransferMode_CanSetAndGet()
        {
            var element = new TestHttpBindingBaseElement("test");
            element.TransferMode = TransferMode.Streamed;
            Assert.Equal(TransferMode.Streamed, element.TransferMode);
        }

        [Fact]
        public void UseDefaultWebProxy_DefaultIsTrue()
        {
            var element = new TestHttpBindingBaseElement("test");
            Assert.True(element.UseDefaultWebProxy);
        }

        [Fact]
        public void UseDefaultWebProxy_CanSetAndGet()
        {
            var element = new TestHttpBindingBaseElement("test");
            element.UseDefaultWebProxy = false;
            Assert.False(element.UseDefaultWebProxy);
        }

        [Fact]
        public void ReaderQuotas_DefaultInstancePresent()
        {
            var element = new TestHttpBindingBaseElement("test");
            Assert.NotNull(element.ReaderQuotas);
        }
    }
}
