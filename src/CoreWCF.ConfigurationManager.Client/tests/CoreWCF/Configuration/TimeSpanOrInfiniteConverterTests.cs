using System;
using System.ComponentModel;
using System.Globalization;
using CoreWCF.ConfigurationManager.Client;
using Xunit;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration
{
    public class TimeSpanOrInfiniteConverterTests
    {
        [Fact]
        public void CanConvertFrom_StringType_ReturnsTrue()
        {
            var converter = new TimeSpanOrInfiniteConverter();
            Assert.True(converter.CanConvertFrom(null, typeof(string)));
        }

        [Fact]
        public void ConvertFrom_InfiniteString_ReturnsTimeSpanMaxValue()
        {
            var converter = new TimeSpanOrInfiniteConverter();
            var result = converter.ConvertFrom(null, CultureInfo.InvariantCulture, "Infinite");
            Assert.Equal(TimeSpan.MaxValue, result);
        }

        [Fact]
        public void ConvertFrom_NormalTimeSpanString_ReturnsTimeSpan()
        {
            var converter = new TimeSpanOrInfiniteConverter();
            var result = (TimeSpan)converter.ConvertFrom(null, CultureInfo.InvariantCulture, "00:01:30");
            Assert.Equal(TimeSpan.FromMinutes(1.5), result);
        }

        [Fact]
        public void ConvertFrom_InvalidType_ThrowsInvalidCast()
        {
            var converter = new TimeSpanOrInfiniteConverter();
            Assert.Throws<InvalidCastException>(() => converter.ConvertFrom(null, CultureInfo.InvariantCulture, 123));
        }

        [Fact]
        public void ConvertTo_InfiniteTimeSpan_ReturnsInfiniteString()
        {
            var converter = new TimeSpanOrInfiniteConverter();
            var result = converter.ConvertTo(null, CultureInfo.InvariantCulture, TimeSpan.MaxValue, typeof(string));
            Assert.Equal("Infinite", result);
        }

        [Fact]
        public void ConvertTo_NormalTimeSpan_ReturnsString()
        {
            var converter = new TimeSpanOrInfiniteConverter();
            var ts = TimeSpan.FromSeconds(5);
            var result = converter.ConvertTo(null, CultureInfo.InvariantCulture, ts, typeof(string));
            Assert.Equal(ts.ToString(), result);
        }

        [Fact]
        public void ConvertTo_InvalidValueType_ThrowsArgumentException()
        {
            var converter = new TimeSpanOrInfiniteConverter();
            Assert.Throws<ArgumentException>(() => converter.ConvertTo(null, CultureInfo.InvariantCulture, "not-timespan", typeof(string)));
        }
    }
}
