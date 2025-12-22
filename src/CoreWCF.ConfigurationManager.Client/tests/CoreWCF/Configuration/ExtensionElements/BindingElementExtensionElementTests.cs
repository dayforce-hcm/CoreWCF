using System;
using System.ServiceModel.Channels;
using CoreWCF.ConfigurationManager.Client;
using Xunit;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.ExtensionElements
{
    public class TestBindingElementExtensionElement : BindingElementExtensionElement
    {
        public override Type BindingElementType => typeof(TextMessageEncodingBindingElement);
        protected internal override BindingElement CreateBindingElement() => new TextMessageEncodingBindingElement();
    }

    public class BindingElementExtensionElementTests
    {
        [Fact]
        public void CreateBindingElement_ReturnsBindingElement()
        {
            var ext = new TestBindingElementExtensionElement();
            var bindingElement = ext.CreateBindingElement();
            Assert.IsType<TextMessageEncodingBindingElement>(bindingElement);
        }

        [Fact]
        public void BindingElementType_ReturnsType()
        {
            var ext = new TestBindingElementExtensionElement();
            Assert.Equal(typeof(TextMessageEncodingBindingElement), ext.BindingElementType);
        }
    }
}
