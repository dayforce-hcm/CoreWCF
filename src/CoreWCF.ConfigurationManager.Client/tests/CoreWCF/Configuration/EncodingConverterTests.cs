using System;
using System.ComponentModel;
using System.Text;
using CoreWCF.ConfigurationManager.Client;
using Xunit;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration
{
    public class EncodingConverterTests
    {
        [Fact]
        public void CanConvertFrom_String_ReturnsTrue()
        {
            var converter = new EncodingConverter();
            Assert.True(converter.CanConvertFrom(null, typeof(string)));
        }

        [Fact]
        public void CanConvertTo_InstanceDescriptor_ReturnsTrue()
        {
            var converter = new EncodingConverter();
            Assert.True(converter.CanConvertTo(null, typeof(System.ComponentModel.Design.Serialization.InstanceDescriptor)));
        }

        [Fact]
        public void ConvertFrom_Utf8String_ReturnsUtf8Encoding()
        {
            var converter = new EncodingConverter();
            var encoding = converter.ConvertFrom(null, null, "utf-8") as Encoding;
            Assert.Equal(Encoding.UTF8, encoding);
        }

        [Fact]
        public void ConvertFrom_OtherEncoding_UsesGetEncoding()
        {
            var converter = new EncodingConverter();
            var encoding = converter.ConvertFrom(null, null, "utf-16") as Encoding;
            Assert.Equal(Encoding.GetEncoding("utf-16"), encoding);
        }

        [Fact]
        public void ConvertFrom_NonString_DelegatesToBase()
        {
            var converter = new EncodingConverter();
            Assert.Throws<NotSupportedException>(() => converter.ConvertFrom(null, null, 123));
        }

        [Fact]
        public void ConvertTo_Encoding_ReturnsHeaderName()
        {
            var converter = new EncodingConverter();
            var encoding = Encoding.UTF8;
            var headerName = converter.ConvertTo(null, null, encoding, typeof(string));
            Assert.Equal(encoding.HeaderName, headerName);
        }

        [Fact]
        public void ConvertTo_NonStringDestination_DelegatesToBase()
        {
            var converter = new EncodingConverter();
            var encoding = Encoding.UTF8;
            Assert.Throws<NotSupportedException>(() => converter.ConvertTo(null, null, encoding, typeof(int)));
        }
    }
}
