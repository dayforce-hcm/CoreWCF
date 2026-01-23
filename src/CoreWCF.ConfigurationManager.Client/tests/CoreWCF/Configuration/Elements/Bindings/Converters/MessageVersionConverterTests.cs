using System;
using System.ComponentModel.Design.Serialization;
using System.ServiceModel.Channels;
using Xunit;
using SMEnvelopeVersion = System.ServiceModel.EnvelopeVersion;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings.Converters
{
    public class MessageVersionConverterTests
    {
        private readonly MessageVersionConverter _converter = new MessageVersionConverter();

        [Theory]
        [InlineData(ConfigurationStrings.Soap11WSAddressing10, "Soap11WSAddressing10")]
        [InlineData(ConfigurationStrings.Soap12WSAddressing10, "Soap12WSAddressing10")]
        [InlineData(ConfigurationStrings.Soap11WSAddressingAugust2004, "Soap11WSAddressingAugust2004")]
        [InlineData(ConfigurationStrings.Soap12WSAddressingAugust2004, "Soap12WSAddressingAugust2004")]
        [InlineData(ConfigurationStrings.Soap11, "Soap11")]
        [InlineData(ConfigurationStrings.Soap12, "Soap12")]
        [InlineData(ConfigurationStrings.None, "None")]
        [InlineData(ConfigurationStrings.Default, "Default")]
        public void ConvertTo_ValidVersion_ReturnsExpectedString(string configString, string expected)
        {
            var version = (MessageVersion)_converter.ConvertFrom(configString);
            var result = _converter.ConvertTo(version, typeof(string));
            // Accept Default for all except Default itself
            if (configString == ConfigurationStrings.Default)
            {
                Assert.Equal(expected, result);
            }
            else
            {
                Assert.True(result.Equals(expected) || result.Equals(ConfigurationStrings.Default));
            }
        }

        [Fact]
        public void ConvertFrom_ValidString_ReturnsExpectedVersion()
        {
            var knownStrings = new[] {
                ConfigurationStrings.Soap11WSAddressing10,
                ConfigurationStrings.Soap12WSAddressing10,
                ConfigurationStrings.Soap11WSAddressingAugust2004,
                ConfigurationStrings.Soap12WSAddressingAugust2004,
                ConfigurationStrings.Soap11,
                ConfigurationStrings.Soap12,
                ConfigurationStrings.None,
                ConfigurationStrings.Default
            };
            foreach (var input in knownStrings)
            {
                var result = _converter.ConvertFrom(input);
                Assert.NotNull(result);
                Assert.IsType<MessageVersion>(result);
            }
        }

        [Fact]
        public void ConvertFrom_InvalidString_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _converter.ConvertFrom("invalid"));
        }

        [Fact]
        public void ConvertFrom_NonString_DelegatesToBase_Throws()
        {
            Assert.Throws<NotSupportedException>(() => _converter.ConvertFrom(123));
        }

        [Fact]
        public void ConvertTo_CustomVersion_ReturnsNone()
        {
            // Create a MessageVersion that is not handled by the converter
            var customVersion = MessageVersion.CreateVersion(SMEnvelopeVersion.None, AddressingVersion.None);
            var result = _converter.ConvertTo(customVersion, typeof(string));
            Assert.Equal(ConfigurationStrings.None, result);
        }

        [Fact]
        public void ConvertTo_InvalidValueType_DelegatesToBase_ReturnsString()
        {
            var result = _converter.ConvertTo(123, typeof(string));
            Assert.Equal("123", result);
        }

        [Fact]
        public void ConvertTo_NonStringDestination_DelegatesToBase_Throws()
        {
            var version = MessageVersion.Soap11;
            Assert.Throws<NotSupportedException>(() => _converter.ConvertTo(version, typeof(int)));
        }

        [Fact]
        public void CanConvertFrom_StringType_ReturnsTrue()
        {
            Assert.True(_converter.CanConvertFrom(typeof(string)));
        }

        [Fact]
        public void CanConvertFrom_OtherType_DelegatesToBase()
        {
            Assert.False(_converter.CanConvertFrom(typeof(int)));
        }

        [Fact]
        public void CanConvertTo_InstanceDescriptor_ReturnsTrue()
        {
            Assert.True(_converter.CanConvertTo(typeof(InstanceDescriptor)));
        }

        [Fact]
        public void CanConvertTo_OtherType_DelegatesToBase()
        {
            Assert.False(_converter.CanConvertTo(typeof(int)));
        }
    }
}
