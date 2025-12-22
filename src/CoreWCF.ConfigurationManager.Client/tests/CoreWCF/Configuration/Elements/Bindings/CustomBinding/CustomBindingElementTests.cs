using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System;
using System.ServiceModel.Channels;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings
{
    public class CustomBindingElementTests
    {
        private class TestEncodingElement : BindingElementExtensionElement
        {
            public TestEncodingElement() { ConfigurationElementName = "textMessageEncoding"; }
            public override Type BindingElementType => typeof(TextMessageEncodingBindingElement);
            protected internal override BindingElement CreateBindingElement() => new TextMessageEncodingBindingElement();
        }

        private class TestTransportElement : BindingElementExtensionElement
        {
            public TestTransportElement() { ConfigurationElementName = "httpTransport"; }
            public override Type BindingElementType => typeof(HttpTransportBindingElement);
            protected internal override BindingElement CreateBindingElement() => new HttpTransportBindingElement();
        }

        private class OtherTransportElement : BindingElementExtensionElement
        {
            public OtherTransportElement() { ConfigurationElementName = "tcpTransport"; }
            public override Type BindingElementType => typeof(TcpTransportBindingElement);
            protected internal override BindingElement CreateBindingElement() => new TcpTransportBindingElement();
        }

        private class DummyBinding : Binding
        {
            public override string Scheme => "dummy";
            public override BindingElementCollection CreateBindingElements() => new BindingElementCollection();
        }

        [Fact]
        public void CanInstantiateCustomBindingElement_DefaultConstructor()
        {
            var element = new CustomBindingElement();
            Assert.NotNull(element);
        }

        [Fact]
        public void CanInstantiateCustomBindingElement_WithName()
        {
            var element = new CustomBindingElement("testName");
            Assert.NotNull(element);
            Assert.Equal("testName", element.Name);
        }

        [Fact]
        public void Add_Null_Throws()
        {
            var element = new CustomBindingElement();
            Assert.Throws<ArgumentNullException>(() => element.Add(null));
        }

        [Fact]
        public void Add_DuplicateEncoding_ThrowsConfigurationErrors()
        {
            var element = new CustomBindingElement();
            element.Add(new TestEncodingElement());
            var ex = Assert.Throws<System.Configuration.ConfigurationErrorsException>(() => element.Add(new TestEncodingElement()));
            Assert.Contains("message encoding", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Add_DuplicateTransport_ThrowsConfigurationErrors()
        {
            var element = new CustomBindingElement();
            element.Add(new TestTransportElement());
            var ex = Assert.Throws<System.Configuration.ConfigurationErrorsException>(() => element.Add(new OtherTransportElement()));
            Assert.Contains("transport", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void CanAdd_RejectsNull()
        {
            var element = new CustomBindingElement();
            Assert.Throws<ArgumentNullException>(() => element.CanAdd(null));
        }

        [Fact]
        public void CanAdd_DuplicateEncoding_ReturnsFalse()
        {
            var element = new CustomBindingElement();
            var enc = new TestEncodingElement();
            element.Add(enc);
            Assert.False(element.CanAdd(new TestEncodingElement()));
        }

        [Fact]
        public void CanAdd_DuplicateTransport_ReturnsFalse()
        {
            var element = new CustomBindingElement();
            element.Add(new TestTransportElement());
            Assert.False(element.CanAdd(new OtherTransportElement()));
        }

        [Fact]
        public void ApplyConfiguration_NullBinding_Throws()
        {
            var element = new CustomBindingElement();
            Assert.Throws<ArgumentNullException>(() => element.ApplyConfiguration(null));
        }

        [Fact]
        public void ApplyConfiguration_WrongBindingType_Throws()
        {
            var element = new CustomBindingElement();
            var wrong = new DummyBinding();
            Action act = () => element.ApplyConfiguration(wrong);
            var ex = Assert.Throws<ArgumentException>(act);
            Assert.Contains(nameof(CustomBinding), ex.Message);
        }

        [Fact]
        public void ApplyConfiguration_SetsPropertiesAndElements()
        {
            var element = new CustomBindingElement("myName")
            {
                CloseTimeout = TimeSpan.FromSeconds(1),
                OpenTimeout = TimeSpan.FromSeconds(2),
                ReceiveTimeout = TimeSpan.FromSeconds(3),
                SendTimeout = TimeSpan.FromSeconds(4)
            };
            element.Add(new TestEncodingElement());
            element.Add(new TestTransportElement());

            var binding = new CustomBinding();
            element.ApplyConfiguration(binding);

            Assert.Equal("myName", binding.Name);
            Assert.Equal(TimeSpan.FromSeconds(1), binding.CloseTimeout);
            Assert.Equal(TimeSpan.FromSeconds(2), binding.OpenTimeout);
            Assert.Equal(TimeSpan.FromSeconds(3), binding.ReceiveTimeout);
            Assert.Equal(TimeSpan.FromSeconds(4), binding.SendTimeout);
            Assert.Collection(binding.Elements,
                be => Assert.IsType<TextMessageEncodingBindingElement>(be),
                be => Assert.IsType<HttpTransportBindingElement>(be));
        }

        [Fact]
        public void CreateBinding_ReturnsCustomBindingWithElements()
        {
            var element = new CustomBindingElement("created")
            {
                CloseTimeout = TimeSpan.FromSeconds(5)
            };
            element.Add(new TestEncodingElement());
            element.Add(new TestTransportElement());

            var binding = (CustomBinding)element.CreateBinding();
            Assert.IsType<CustomBinding>(binding);
            Assert.Equal("created", binding.Name);
            Assert.Equal(TimeSpan.FromSeconds(5), binding.CloseTimeout);
            Assert.Collection(binding.Elements,
                be => Assert.IsType<TextMessageEncodingBindingElement>(be),
                be => Assert.IsType<HttpTransportBindingElement>(be));
        }
    }
}
