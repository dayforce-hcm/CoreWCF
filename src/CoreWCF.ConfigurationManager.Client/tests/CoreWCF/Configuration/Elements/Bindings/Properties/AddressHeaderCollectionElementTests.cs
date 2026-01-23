using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System.ServiceModel.Channels;
using System.Xml;
using System;
using System.IO;
using System.Reflection;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings.Properties
{
    public class AddressHeaderCollectionElementTests
    {
        [Fact]
        public void DefaultConstructor_InitializesHeaders()
        {
            var element = new AddressHeaderCollectionElement();
            Assert.NotNull(element.Headers);
        }

        [Fact]
        public void Headers_SetNull_ReplacesWithEmptyCollection()
        {
            var element = new AddressHeaderCollectionElement();
            element.Headers = null;
            Assert.NotNull(element.Headers);
            Assert.Empty(element.Headers);
        }

        [Fact]
        public void Copy_CopiesHeaders()
        {
            var source = new AddressHeaderCollectionElement();
            var header = new AddressHeaderCollection(new[] { AddressHeader.CreateAddressHeader("h","urn:test","v") });
            source.Headers = header;
            var target = new AddressHeaderCollectionElement();
            target.Copy(source);
            Assert.Equal(header, target.Headers);
        }

        [Fact]
        public void Copy_SourceNull_Throws()
        {
            var target = new AddressHeaderCollectionElement();
            Assert.Throws<ArgumentNullException>(() => target.Copy(null));
        }

        [Fact]
        public void Copy_DoesNotCopy_WhenSourceHasDefaultOrigin()
        {
            var source = new AddressHeaderCollectionElement(); // value origin remains Default
            var initial = new AddressHeaderCollection(new[] { AddressHeader.CreateAddressHeader("x","urn","1") });
            var target = new AddressHeaderCollectionElement { Headers = initial };

            target.Copy(source);

            Assert.Same(initial, target.Headers);
        }

        [Fact]
        public void InitializeFrom_SetsHeaders()
        {
            var element = new AddressHeaderCollectionElement();
            var header = new AddressHeaderCollection(new[] { AddressHeader.CreateAddressHeader("h","urn:test","v") });
            element.InitializeFrom(header);
            Assert.Equal(header, element.Headers);
        }

        [Fact]
        public void DeserializeElement_LoadsSingleHeader()
        {
            var xml = "<h xmlns=\"urn:test\"><v/></h>";
            using var sr = new StringReader(xml);
            using var xr = XmlReader.Create(sr, new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Fragment });
            xr.Read();
            var elem = new AddressHeaderCollectionElement();
            var mi = typeof(AddressHeaderCollectionElement).GetMethod("DeserializeElement", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(mi);
            mi.Invoke(elem, new object[] { xr, false });

            Assert.NotNull(elem.Headers);
            Assert.Single(elem.Headers);
        }

        [Fact]
        public void SerializeToXmlElement_WithEmptyHeaders_ReturnsFalse()
        {
            var elem = new AddressHeaderCollectionElement();
            var mi = typeof(AddressHeaderCollectionElement).GetMethod("SerializeToXmlElement", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(mi);
            using var sw = new StringWriter();
            using var xw = XmlWriter.Create(sw, new XmlWriterSettings { OmitXmlDeclaration = true, ConformanceLevel = ConformanceLevel.Fragment });
            var result = (bool)mi.Invoke(elem, new object[] { xw, "headers" });
            Assert.False(result);
            Assert.Equal(string.Empty, sw.ToString());
        }

        [Fact]
        public void SerializeToXmlElement_WithHeaders_WritesContent()
        {
            var headers = new AddressHeaderCollection(new[] { AddressHeader.CreateAddressHeader("h","urn:test","v") });
            var elem = new AddressHeaderCollectionElement { Headers = headers };
            var mi = typeof(AddressHeaderCollectionElement).GetMethod("SerializeToXmlElement", BindingFlags.Instance | BindingFlags.NonPublic);
            using var sw = new StringWriter();
            using var xw = XmlWriter.Create(sw, new XmlWriterSettings { OmitXmlDeclaration = true, ConformanceLevel = ConformanceLevel.Fragment });
            var result = (bool)mi.Invoke(elem, new object[] { xw, "headers" });
            xw.Flush();
            Assert.True(result);
            var output = sw.ToString();
            Assert.Contains("<headers", output);
            Assert.Contains("h", output);
        }

        [Fact]
        public void SerializeToXmlElement_NullWriter_ReturnsTrueWhenHasHeaders()
        {
            var headers = new AddressHeaderCollection(new[] { AddressHeader.CreateAddressHeader("h","urn:test","v") });
            var elem = new AddressHeaderCollectionElement { Headers = headers };
            var mi = typeof(AddressHeaderCollectionElement).GetMethod("SerializeToXmlElement", BindingFlags.Instance | BindingFlags.NonPublic);
            var result = (bool)mi.Invoke(elem, new object[] { null, "headers" });
            Assert.True(result);
        }
    }
}
