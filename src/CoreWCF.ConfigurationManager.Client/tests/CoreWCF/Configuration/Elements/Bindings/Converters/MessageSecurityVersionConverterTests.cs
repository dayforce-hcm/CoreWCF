using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using Xunit;
using CoreWCF.ConfigurationManager;
using CoreWCF.ConfigurationManager.Client;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings.Converters
{
    public class MessageSecurityVersionConverterTests
    {
        private readonly MessageSecurityVersionConverter _converter = new MessageSecurityVersionConverter();

        public static TheoryData<string, MessageSecurityVersion> ConvertFromData => new TheoryData<string, MessageSecurityVersion>
        {
            { ConfigurationStrings.WSSecurity11WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11, MessageSecurityVersion.WSSecurity11WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11 },
            { ConfigurationStrings.WSSecurity10WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11BasicSecurityProfile10, MessageSecurityVersion.WSSecurity10WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11BasicSecurityProfile10 },
            { ConfigurationStrings.WSSecurity11WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11BasicSecurityProfile10, MessageSecurityVersion.WSSecurity11WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11BasicSecurityProfile10 },
            { ConfigurationStrings.WSSecurity11WSTrust13WSSecureConversation13WSSecurityPolicy12BasicSecurityProfile10, MessageSecurityVersion.WSSecurity11WSTrust13WSSecureConversation13WSSecurityPolicy12BasicSecurityProfile10 },
            { ConfigurationStrings.Default, MessageSecurityVersion.WSSecurity11WSTrust13WSSecureConversation13WSSecurityPolicy12BasicSecurityProfile10 }
        };

        [Theory]
        [MemberData(nameof(ConvertFromData))]
        public void ConvertFrom_ValidString_ReturnsExpectedVersion(string input, MessageSecurityVersion expected)
        {
            var result = _converter.ConvertFrom(input);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertFrom_InvalidString_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _converter.ConvertFrom("invalid"));
        }

        [Fact]
        public void ConvertFrom_NonString_DelegatesToBase_Throws()
        {
            // Base TypeConverter does not support conversion from int
            Assert.Throws<NotSupportedException>(() => _converter.ConvertFrom(42));
        }

        public static TheoryData<MessageSecurityVersion, string> ConvertToData => new TheoryData<MessageSecurityVersion, string>
        {
            { MessageSecurityVersion.WSSecurity11WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11, ConfigurationStrings.WSSecurity11WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11 },
            { MessageSecurityVersion.WSSecurity10WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11BasicSecurityProfile10, ConfigurationStrings.WSSecurity10WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11BasicSecurityProfile10 },
            { MessageSecurityVersion.WSSecurity11WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11BasicSecurityProfile10, ConfigurationStrings.WSSecurity11WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11BasicSecurityProfile10 },
            { MessageSecurityVersion.WSSecurity11WSTrust13WSSecureConversation13WSSecurityPolicy12BasicSecurityProfile10, ConfigurationStrings.WSSecurity11WSTrust13WSSecureConversation13WSSecurityPolicy12BasicSecurityProfile10 }
        };

        [Theory]
        [MemberData(nameof(ConvertToData))]
        public void ConvertTo_ValidVersion_ReturnsExpectedString(MessageSecurityVersion input, string expected)
        {
            var result = _converter.ConvertTo(input, typeof(string));
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertTo_UnrecognizedVersion_Throws()
        {
            // Find a static MessageSecurityVersion different from the ones explicitly handled by the converter
            var recognized = new[]
            {
                MessageSecurityVersion.WSSecurity11WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11,
                MessageSecurityVersion.WSSecurity10WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11BasicSecurityProfile10,
                MessageSecurityVersion.WSSecurity11WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11BasicSecurityProfile10,
                MessageSecurityVersion.WSSecurity11WSTrust13WSSecureConversation13WSSecurityPolicy12BasicSecurityProfile10
            };

            var unrecognized = typeof(MessageSecurityVersion)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(p => p.PropertyType == typeof(MessageSecurityVersion))
                .Select(p => (MessageSecurityVersion)p.GetValue(null))
                .FirstOrDefault(v => v != null && !recognized.Contains(v));

            if (unrecognized is null)
            {
                // Environment doesn't provide any extra variant; nothing to assert for throw in this TFM
                return;
            }

            Assert.Throws<ArgumentOutOfRangeException>(() => _converter.ConvertTo(unrecognized, typeof(string)));
        }

        [Fact]
        public void ConvertTo_InvalidValueType_DelegatesToBase_ReturnsString()
        {
            // base TypeConverter.ConvertTo will format non-null value to string when destinationType is string
            var result = _converter.ConvertTo(123, typeof(string));
            Assert.Equal("123", result);
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

        [Fact]
        public void ConvertTo_NonStringDestination_DelegatesToBase_Throws()
        {
            Assert.Throws<NotSupportedException>(() => _converter.ConvertTo(MessageSecurityVersion.WSSecurity11WSTrust13WSSecureConversation13WSSecurityPolicy12BasicSecurityProfile10, typeof(int)));
        }
    }
}
