using System;
using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System.ServiceModel.Channels;
using System.Xml;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Configuration
{
    public class XmlDictionaryReaderQuotasElementTests
    {
        [Fact]
        public void Properties_SetAndGet()
        {
            var element = new XmlDictionaryReaderQuotasElement();
            element.MaxDepth = 10;
            element.MaxStringContentLength = 20;
            element.MaxArrayLength = 30;
            element.MaxBytesPerRead = 40;
            element.MaxNameTableCharCount = 50;
            Assert.Equal(10, element.MaxDepth);
            Assert.Equal(20, element.MaxStringContentLength);
            Assert.Equal(30, element.MaxArrayLength);
            Assert.Equal(40, element.MaxBytesPerRead);
            Assert.Equal(50, element.MaxNameTableCharCount);
        }

        [Fact]
        public void ApplyConfiguration_SetsReaderQuotas()
        {
            var element = new XmlDictionaryReaderQuotasElement();
            element.MaxDepth = 10;
            element.MaxStringContentLength = 20;
            element.MaxArrayLength = 30;
            element.MaxBytesPerRead = 40;
            element.MaxNameTableCharCount = 50;
            var quotas = new System.Xml.XmlDictionaryReaderQuotas();
            element.ApplyConfiguration(quotas);
            Assert.Equal(10, quotas.MaxDepth);
            Assert.Equal(20, quotas.MaxStringContentLength);
            Assert.Equal(30, quotas.MaxArrayLength);
            Assert.Equal(40, quotas.MaxBytesPerRead);
            Assert.Equal(50, quotas.MaxNameTableCharCount);
        }

        [Fact]
        public void Clone_ReturnsClonedQuotas()
        {
            var element = new XmlDictionaryReaderQuotasElement();
            element.MaxDepth = 10;
            var clone = element.Clone();
            Assert.Equal(10, clone.MaxDepth);
        }

        [Fact]
        public void InitializeFrom_SetsPropertiesFromQuotas()
        {
            var quotas = new System.Xml.XmlDictionaryReaderQuotas();
            quotas.MaxDepth = 15;
            quotas.MaxStringContentLength = 25;
            quotas.MaxArrayLength = 35;
            quotas.MaxBytesPerRead = 45;
            quotas.MaxNameTableCharCount = 55;
            var element = new XmlDictionaryReaderQuotasElement();
            element.InitializeFrom(quotas);
            Assert.Equal(15, element.MaxDepth);
            Assert.Equal(25, element.MaxStringContentLength);
            Assert.Equal(35, element.MaxArrayLength);
            Assert.Equal(45, element.MaxBytesPerRead);
            Assert.Equal(55, element.MaxNameTableCharCount);
        }
    }
}
