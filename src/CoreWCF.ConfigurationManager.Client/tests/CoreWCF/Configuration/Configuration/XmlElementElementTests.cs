using System;
using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System.Xml;
using System.IO;
using System.Text;
using System.Configuration;
using System.Reflection;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Configuration
{
    public class XmlElementElementTests
    {
        [Fact]
        public void Constructor_Default_SetsXmlElementNull()
        {
            var element = new XmlElementElement();
            Assert.Null(element.XmlElement);
        }

        [Fact]
        public void Constructor_WithXmlElement_SetsXmlElement()
        {
            var doc = new XmlDocument();
            var xmlElem = doc.CreateElement("Test");
            var element = new XmlElementElement(xmlElem);
            Assert.Equal(xmlElem, element.XmlElement);
        }

        [Fact]
        public void Copy_CopiesXmlElement()
        {
            var doc = new XmlDocument();
            var xmlElem = doc.CreateElement("Test");
            var source = new XmlElementElement(xmlElem);
            var target = new XmlElementElement();
            target.Copy(source);
            Assert.Equal(xmlElem.OuterXml, target.XmlElement.OuterXml);
        }

        [Fact]
        public void Copy_ThrowsIfSourceNull()
        {
            var element = new XmlElementElement();
            Assert.Throws<ArgumentNullException>(() => element.Copy(null));
        }

        [Fact]
        public void Copy_ThrowsIfReadOnly()
        {
            var doc = new XmlDocument();
            var xmlElem = doc.CreateElement("Test");
            var readonlyTarget = new XmlElementElement();
            // Set read-only via protected base method using reflection
            var setReadOnly = typeof(ConfigurationElement).GetMethod("SetReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(setReadOnly);
            setReadOnly.Invoke(readonlyTarget, null);
            var source = new XmlElementElement(xmlElem);
            Assert.Throws<ConfigurationErrorsException>(() => readonlyTarget.Copy(source));
        }

        [Fact]
        public void DeserializeElement_SetsXmlElementFromReader()
        {
            var xml = "<Node attr=\"1\"><Child /></Node>";
            using var stringReader = new StringReader(xml);
            using var reader = XmlReader.Create(stringReader, new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Fragment });
            reader.Read(); // position at first node
            var element = new XmlElementElement();
            var method = typeof(XmlElementElement).GetMethod("DeserializeElement", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(method);
            method.Invoke(element, new object[] { reader, false });
            Assert.NotNull(element.XmlElement);
            Assert.Equal(xml, element.XmlElement.OuterXml);
        }

        [Fact]
        public void SerializeToXmlElement_NoElement_ReturnsFalse()
        {
            var element = new XmlElementElement();
            var method = typeof(XmlElementElement).GetMethod("SerializeToXmlElement", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(method);
            using var sw = new StringWriter();
            using var xw = XmlWriter.Create(sw, new XmlWriterSettings { OmitXmlDeclaration = true, ConformanceLevel = ConformanceLevel.Fragment });
            var result = (bool)method.Invoke(element, new object[] { xw, ConfigurationStrings.XmlElement });
            Assert.False(result);
        }

        [Fact]
        public void SerializeToXmlElement_WritesNode_DefaultName()
        {
            var doc = new XmlDocument();
            var xmlElem = doc.CreateElement("Item");
            xmlElem.SetAttribute("a", "b");
            var element = new XmlElementElement(xmlElem);

            var method = typeof(XmlElementElement).GetMethod("SerializeToXmlElement", BindingFlags.Instance | BindingFlags.NonPublic);
            using var sw = new StringWriter();
            using var xw = XmlWriter.Create(sw, new XmlWriterSettings { OmitXmlDeclaration = true, ConformanceLevel = ConformanceLevel.Fragment });
            var result = (bool)method.Invoke(element, new object[] { xw, ConfigurationStrings.XmlElement });
            xw.Flush();
            Assert.True(result);
            Assert.Equal(xmlElem.OuterXml, sw.ToString());
        }

        [Fact]
        public void SerializeToXmlElement_WritesNode_WithWrapper()
        {
            var doc = new XmlDocument();
            var xmlElem = doc.CreateElement("Item");
            var child = doc.CreateElement("Child");
            xmlElem.AppendChild(child);
            var element = new XmlElementElement(xmlElem);
            var method = typeof(XmlElementElement).GetMethod("SerializeToXmlElement", BindingFlags.Instance | BindingFlags.NonPublic);
            using var sw = new StringWriter();
            using var xw = XmlWriter.Create(sw, new XmlWriterSettings { OmitXmlDeclaration = true, ConformanceLevel = ConformanceLevel.Fragment });
            var result = (bool)method.Invoke(element, new object[] { xw, "Wrapper" });
            xw.Flush();
            Assert.True(result);
            Assert.Equal("<Wrapper>" + xmlElem.OuterXml + "</Wrapper>", sw.ToString());
        }

        [Fact]
        public void SerializeToXmlElement_NullWriter_ReturnsTrueAndDoesNotThrow()
        {
            var doc = new XmlDocument();
            var xmlElem = doc.CreateElement("X");
            var element = new XmlElementElement(xmlElem);
            var method = typeof(XmlElementElement).GetMethod("SerializeToXmlElement", BindingFlags.Instance | BindingFlags.NonPublic);
            var result = (bool)method.Invoke(element, new object[] { null, ConfigurationStrings.XmlElement });
            Assert.True(result);
        }

        [Fact]
        public void PostDeserialize_ThrowsWhenXmlElementNull()
        {
            var element = new XmlElementElement();
            var method = typeof(XmlElementElement).GetMethod("PostDeserialize", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(method);
            Assert.Throws<TargetInvocationException>(() => method.Invoke(element, null));
            try
            {
                method.Invoke(element, null);
            }
            catch (TargetInvocationException ex)
            {
                Assert.IsType<ConfigurationErrorsException>(ex.InnerException);
            }
        }

        [Fact]
        public void PostDeserialize_SucceedsWhenXmlElementPresent()
        {
            var doc = new XmlDocument();
            var xmlElem = doc.CreateElement("Ok");
            var element = new XmlElementElement(xmlElem);
            var method = typeof(XmlElementElement).GetMethod("PostDeserialize", BindingFlags.Instance | BindingFlags.NonPublic);
            method.Invoke(element, null);
        }

        [Fact]
        public void ResetInternal_CopiesFromSource()
        {
            var doc = new XmlDocument();
            var srcElem = doc.CreateElement("Src");
            var dst = new XmlElementElement();
            var src = new XmlElementElement(srcElem);
            dst.ResetInternal(src);
            Assert.NotNull(dst.XmlElement);
            Assert.Equal(srcElem.OuterXml, dst.XmlElement.OuterXml);
        }
    }
}
