using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System.ServiceModel.Channels;
using System.Xml;
using System.IO;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings.Properties
{
    public class BinaryMessageEncodingElementTests
    {
        [Fact]
        public void Properties_SetAndGet_ReturnsValues()
        {
            var element = new BinaryMessageEncodingElement();
            element.MaxReadPoolSize = 10;
            element.MaxWritePoolSize = 20;
            element.MaxSessionSize = 30;
            element.CompressionFormat = CompressionFormat.GZip;
            Assert.Equal(10, element.MaxReadPoolSize);
            Assert.Equal(20, element.MaxWritePoolSize);
            Assert.Equal(30, element.MaxSessionSize);
            Assert.Equal(CompressionFormat.GZip, element.CompressionFormat);
        }

        [Fact]
        public void CopyFrom_CopiesProperties()
        {
            var source = new BinaryMessageEncodingElement();
            source.MaxReadPoolSize = 5;
            source.MaxWritePoolSize = 6;
            source.MaxSessionSize = 7;
            source.CompressionFormat = CompressionFormat.Deflate;
            var target = new BinaryMessageEncodingElement();
            target.CopyFrom(source);
            Assert.Equal(5, target.MaxReadPoolSize);
            Assert.Equal(6, target.MaxWritePoolSize);
            Assert.Equal(7, target.MaxSessionSize);
            Assert.Equal(CompressionFormat.Deflate, target.CompressionFormat);
        }

        [Fact]
        public void ApplyConfiguration_SetsBindingValues()
        {
            var element = new BinaryMessageEncodingElement
            {
                MaxReadPoolSize = 11,
                MaxWritePoolSize = 22,
                MaxSessionSize = 33,
                CompressionFormat = CompressionFormat.GZip
            };
            element.ReaderQuotas.MaxDepth = 7;
            var binding = new BinaryMessageEncodingBindingElement();
            element.ApplyConfiguration(binding);

            Assert.Equal(11, binding.MaxReadPoolSize);
            Assert.Equal(22, binding.MaxWritePoolSize);
            Assert.Equal(33, binding.MaxSessionSize);
            Assert.Equal(CompressionFormat.GZip, binding.CompressionFormat);
            Assert.Equal(7, binding.ReaderQuotas.MaxDepth);
        }

        [Fact]
        public void InitializeFrom_SetsElementFromBindingExceptReaderQuotas()
        {
            var binding = new BinaryMessageEncodingBindingElement
            {
                MaxReadPoolSize = 15,
                MaxWritePoolSize = 25,
                MaxSessionSize = 35,
                CompressionFormat = CompressionFormat.Deflate
            };
            binding.ReaderQuotas.MaxDepth = 9;

            var element = new BinaryMessageEncodingElement();
            element.InitializeFrom(binding);

            Assert.Equal(15, element.MaxReadPoolSize);
            Assert.Equal(25, element.MaxWritePoolSize);
            Assert.Equal(35, element.MaxSessionSize);
            Assert.Equal(CompressionFormat.Deflate, element.CompressionFormat);
            // InitializeFrom calls ReaderQuotas.ApplyConfiguration(binding.ReaderQuotas), which applies element quotas to binding, not vice versa.
            // ReaderQuotas on element remain at defaults (0 means use default).
            Assert.Equal(0, element.ReaderQuotas.MaxDepth);
        }

        [Fact]
        public void CreateBindingElement_UsesApplyConfiguration()
        {
            var element = new BinaryMessageEncodingElement
            {
                MaxReadPoolSize = 12,
                MaxWritePoolSize = 23,
                MaxSessionSize = 34,
                CompressionFormat = CompressionFormat.GZip
            };
            element.ReaderQuotas.MaxBytesPerRead = 321;

            var bindingElement = element.CreateBindingElement();
            var binding = Assert.IsType<BinaryMessageEncodingBindingElement>(bindingElement);
            Assert.Equal(12, binding.MaxReadPoolSize);
            Assert.Equal(23, binding.MaxWritePoolSize);
            Assert.Equal(34, binding.MaxSessionSize);
            Assert.Equal(CompressionFormat.GZip, binding.CompressionFormat);
            Assert.Equal(321, binding.ReaderQuotas.MaxBytesPerRead);
        }
    }
}
