using System;
using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System.Xml;
using System.ServiceModel.Channels;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Configuration
{
    public class XmlElementBackedAddressHeaderTests
    {
        [Fact]
        public void Constructor_SetsNameAndNamespace()
        {
            var doc = new XmlDocument();
            var elem = doc.CreateElement("Test", "urn:test");
            var reader = new XmlNodeReader(elem);
            reader.Read();
            var dictReader = System.Xml.XmlDictionaryReader.CreateDictionaryReader(reader);
            var header = new XmlElementBackedAddressHeader(dictReader);
            Assert.Equal("Test", header.Name);
            Assert.Equal("urn:test", header.Namespace);
        }
    }
}
