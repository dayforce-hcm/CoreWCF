using System;
using System.Text;
using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System.ServiceModel.Channels;
using System.Xml;

namespace CoreWCF.ConfigurationManager.Elements.Bindings.WebHttp.Tests
{
    public class WebMessageEncodingBindingElementTests
    {
        [Fact]
        public void CanInstantiateWithDefaultConstructor()
        {
            var element = new WebMessageEncodingBindingElement();
            Assert.NotNull(element);
        }

        [Fact]
        public void CanInstantiateWithEncoding()
        {
            var element = new WebMessageEncodingBindingElement(Encoding.UTF8);
            Assert.Equal(Encoding.UTF8, element.WriteEncoding);
        }

        [Fact]
        public void CanSetAndGetProperties()
        {
            var element = new WebMessageEncodingBindingElement();
            element.MaxReadPoolSize = 10;
            element.MaxWritePoolSize = 20;
            element.WriteEncoding = Encoding.ASCII;
            element.CrossDomainScriptAccessEnabled = true;
            Assert.Equal(10, element.MaxReadPoolSize);
            Assert.Equal(20, element.MaxWritePoolSize);
            Assert.Equal(Encoding.ASCII, element.WriteEncoding);
            Assert.True(element.CrossDomainScriptAccessEnabled);
        }

        [Fact]
        public void MessageVersion_NoneIsDefaultAndOnlyAllowed()
        {
            var element = new WebMessageEncodingBindingElement();
            Assert.Equal(MessageVersion.None, element.MessageVersion);
            Assert.Throws<ArgumentException>(() => element.MessageVersion = MessageVersion.Default);
        }

        [Fact]
        public void CloneCreatesCopy()
        {
            var element = new WebMessageEncodingBindingElement(Encoding.UTF8)
            {
                MaxReadPoolSize = 5,
                MaxWritePoolSize = 6,
                CrossDomainScriptAccessEnabled = true
            };
            var clone = (WebMessageEncodingBindingElement)element.Clone();
            Assert.Equal(element.MaxReadPoolSize, clone.MaxReadPoolSize);
            Assert.Equal(element.MaxWritePoolSize, clone.MaxWritePoolSize);
            Assert.Equal(element.WriteEncoding, clone.WriteEncoding);
            Assert.Equal(element.CrossDomainScriptAccessEnabled, clone.CrossDomainScriptAccessEnabled);
        }

        [Fact]
        public void CreateMessageEncoderFactory_ThrowsNotImplemented()
        {
            var element = new WebMessageEncodingBindingElement();
            Assert.Throws<NotImplementedException>(() => element.CreateMessageEncoderFactory());
        }
    }
}
