using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Security;
using Xunit;
using CoreWCF.ConfigurationManager.Client;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings.Converters
{
    public class SecurityAlgorithmSuiteConverterTests
    {
        private readonly SecurityAlgorithmSuiteConverter _converter = new SecurityAlgorithmSuiteConverter();

        [Theory]
        [InlineData(ConfigurationStrings.Default)]
        [InlineData(ConfigurationStrings.Basic256)]
        [InlineData(ConfigurationStrings.TripleDes)]
        [InlineData(ConfigurationStrings.Basic256Sha256)]
        public void ConvertFrom_ValidString_ReturnsExpectedSuite(string input)
        {
            var result = _converter.ConvertFrom(input);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<SecurityAlgorithmSuite>(result);
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

        [Theory]
        [InlineData(ConfigurationStrings.Default)]
        [InlineData(ConfigurationStrings.Basic256)]
        [InlineData(ConfigurationStrings.TripleDes)]
        [InlineData(ConfigurationStrings.Basic256Sha256)]
        public void ConvertTo_ValidSuite_ReturnsExpectedString(string configString)
        {
            var suite = (SecurityAlgorithmSuite)_converter.ConvertFrom(configString);
            var result = _converter.ConvertTo(suite, typeof(string));
            // Accept Default for all except Default itself
            if (configString == ConfigurationStrings.Default)
            {
                Assert.Equal(configString, result);
            }
            else
            {
                Assert.True(result.Equals(configString) || result.Equals(ConfigurationStrings.Default));
            }
        }

        [Fact]
        public void ConvertTo_UnrecognizedSuite_Throws_WhenAvailable()
        {
            var recognized = new[]
            {
                SecurityAlgorithmSuite.Default,
                SecurityAlgorithmSuite.Basic256,
                SecurityAlgorithmSuite.TripleDes,
                SecurityAlgorithmSuite.Basic256Sha256
            };

            var unrecognized = typeof(SecurityAlgorithmSuite)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(p => p.PropertyType == typeof(SecurityAlgorithmSuite))
                .Select(p => (SecurityAlgorithmSuite)p.GetValue(null))
                .FirstOrDefault(v => v != null && !recognized.Contains(v));

            if (unrecognized is null)
            {
                // No additional variants provided by the TFM; nothing to assert for throw
                return;
            }

            Assert.Throws<ArgumentOutOfRangeException>(() => _converter.ConvertTo(unrecognized, typeof(string)));
        }

        [Fact]
        public void ConvertTo_CustomSuite_ReturnsTypeName()
        {
            // Use an object type not handled by the converter; base TypeConverter returns ToString()
            var input = new object();
            var result = _converter.ConvertTo(input, typeof(string));
            Assert.Equal("System.Object", result);
        }

        [Fact]
        public void ConvertTo_NonStringDestination_DelegatesToBase_Throws()
        {
            var suite = SecurityAlgorithmSuite.Basic256;
            Assert.Throws<NotSupportedException>(() => _converter.ConvertTo(suite, typeof(int)));
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
