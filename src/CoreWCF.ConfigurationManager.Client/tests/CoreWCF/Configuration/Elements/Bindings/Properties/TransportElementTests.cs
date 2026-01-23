using System;
using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System.ServiceModel.Channels;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings.Properties
{
    public class TransportElementTests
    {
        [Fact]
        public void Properties_SetAndGet_ReturnsValues()
        {
            var element = new TestTransportElement();
            element.ManualAddressing = true;
            element.MaxBufferPoolSize = 1000;
            element.MaxReceivedMessageSize = 2000;
            Assert.True(element.ManualAddressing);
            Assert.Equal(1000, element.MaxBufferPoolSize);
            Assert.Equal(2000, element.MaxReceivedMessageSize);
        }

        [Fact]
        public void ApplyConfiguration_SetsBindingValues()
        {
            var element = new TestTransportElement
            {
                ManualAddressing = true,
                MaxBufferPoolSize = 1234,
                MaxReceivedMessageSize = 5678
            };
            var binding = new CustomTransportBindingElement();

            element.ApplyConfiguration(binding);

            Assert.True(binding.ManualAddressing);
            Assert.Equal(1234, binding.MaxBufferPoolSize);
            Assert.Equal(5678, binding.MaxReceivedMessageSize);
        }

        [Fact]
        public void CopyFrom_CopiesValues()
        {
            var source = new TestTransportElement
            {
                ManualAddressing = true,
                MaxBufferPoolSize = 111,
                MaxReceivedMessageSize = 222
            };
            var target = new TestTransportElement();

            target.CopyFrom(source);

            Assert.True(target.ManualAddressing);
            Assert.Equal(111, target.MaxBufferPoolSize);
            Assert.Equal(222, target.MaxReceivedMessageSize);
        }

        [Fact]
        public void CreateBindingElement_UsesDefaultAndAppliesConfiguration()
        {
            var element = new TestTransportElement
            {
                ManualAddressing = true,
                MaxBufferPoolSize = 333,
                MaxReceivedMessageSize = 444
            };

            var bindingElement = element.CreateBindingElement();
            var binding = Assert.IsType<CustomTransportBindingElement>(bindingElement);
            Assert.True(binding.ManualAddressing);
            Assert.Equal(333, binding.MaxBufferPoolSize);
            Assert.Equal(444, binding.MaxReceivedMessageSize);
        }

        [Fact]
        public void InitializeFrom_SetsElementPropertiesFromBinding()
        {
            var binding = new CustomTransportBindingElement
            {
                ManualAddressing = true,
                MaxBufferPoolSize = 555,
                MaxReceivedMessageSize = 666
            };

            var element = new TestTransportElement();
            element.InitializeFrom(binding);

            Assert.True(element.ManualAddressing);
            Assert.Equal(555, element.MaxBufferPoolSize);
            Assert.Equal(666, element.MaxReceivedMessageSize);
        }

        private class TestTransportElement : TransportElement
        {
            public override Type BindingElementType => typeof(CustomTransportBindingElement);
            protected override TransportBindingElement CreateDefaultBindingElement()
            {
                return new CustomTransportBindingElement();
            }
        }

        private class CustomTransportBindingElement : TransportBindingElement
        {
            public override BindingElement Clone() => this;
            public override string Scheme => "custom";
            public new bool ManualAddressing { get => base.ManualAddressing; set => base.ManualAddressing = value; }
            public new long MaxBufferPoolSize { get => base.MaxBufferPoolSize; set => base.MaxBufferPoolSize = value; }
            public new long MaxReceivedMessageSize { get => base.MaxReceivedMessageSize; set => base.MaxReceivedMessageSize = value; }
        }
    }
}
