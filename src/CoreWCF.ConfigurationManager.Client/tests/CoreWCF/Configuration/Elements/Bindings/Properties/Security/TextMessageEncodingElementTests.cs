using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings.Properties.Security
{
    public class TextMessageEncodingElementTests
    {
        [Fact]
        public void MaxReadPoolSize_SetValue_ReturnsValue()
        {
            var element = new TextMessageEncodingElement();
            element.MaxReadPoolSize = 10;
            Assert.Equal(10, element.MaxReadPoolSize);
        }

        [Fact]
        public void MaxWritePoolSize_SetValue_ReturnsValue()
        {
            var element = new TextMessageEncodingElement();
            element.MaxWritePoolSize = 10;
            Assert.Equal(10, element.MaxWritePoolSize);
        }

        [Fact]
        public void MessageVersion_SetValue_ReturnsValue()
        {
            var element = new TextMessageEncodingElement();
            var version = MessageVersion.Default;
            element.MessageVersion = version;
            Assert.Equal(version, element.MessageVersion);
        }

        [Fact]
        public void WriteEncoding_SetValue_ReturnsValue()
        {
            var element = new TextMessageEncodingElement();
            var encoding = Encoding.UTF8;
            element.WriteEncoding = encoding;
            Assert.Equal(encoding, element.WriteEncoding);
        }

        [Fact]
        public void BindingElementType_IsTextMessageEncodingBindingElement()
        {
            var element = new TextMessageEncodingElement();
            Assert.Equal(typeof(TextMessageEncodingBindingElement), element.BindingElementType);
        }

        [Fact]
        public void CreateBindingElement_AppliesConfiguration_MessageAndEncoding()
        {
            var element = new TextMessageEncodingElement
            {
                MessageVersion = MessageVersion.Soap12WSAddressing10,
                WriteEncoding = Encoding.Unicode // supported
            };

            var binding = (TextMessageEncodingBindingElement)element.CreateBindingElement();
            Assert.Equal(MessageVersion.Soap12WSAddressing10, binding.MessageVersion);
            Assert.Equal(Encoding.Unicode, binding.WriteEncoding);
        }

        [Fact]
        public void ApplyConfiguration_SetsBindingProperties_MessageAndEncoding()
        {
            var element = new TextMessageEncodingElement
            {
                MessageVersion = MessageVersion.Soap11,
                WriteEncoding = Encoding.UTF8 // supported
            };
            var binding = new TextMessageEncodingBindingElement();
            element.ApplyConfiguration(binding);
            Assert.Equal(MessageVersion.Soap11, binding.MessageVersion);
            Assert.Equal(Encoding.UTF8, binding.WriteEncoding);
        }

        [Fact]
        public void CopyFrom_CopiesAllProperties()
        {
            var source = new TextMessageEncodingElement
            {
                MaxReadPoolSize = 7,
                MaxWritePoolSize = 8,
                MessageVersion = MessageVersion.Soap12,
                WriteEncoding = Encoding.UTF8
            };
            var target = new TextMessageEncodingElement();
            target.CopyFrom(source);
            Assert.Equal(7, target.MaxReadPoolSize);
            Assert.Equal(8, target.MaxWritePoolSize);
            Assert.Equal(MessageVersion.Soap12, target.MessageVersion);
            Assert.Equal(Encoding.UTF8, target.WriteEncoding);
        }

        [Fact]
        public void InitializeFrom_SetsValuesWhenNonDefault_MessageAndEncoding()
        {
            var binding = new TextMessageEncodingBindingElement
            {
                MessageVersion = MessageVersion.Soap12,
                WriteEncoding = Encoding.BigEndianUnicode, // supported
                ReaderQuotas = new XmlDictionaryReaderQuotas()
            };

            var element = new TextMessageEncodingElement();
            element.InitializeFrom(binding);

            Assert.Equal(MessageVersion.Soap12, element.MessageVersion);
            Assert.Equal(Encoding.BigEndianUnicode, element.WriteEncoding);
        }
    }
}
