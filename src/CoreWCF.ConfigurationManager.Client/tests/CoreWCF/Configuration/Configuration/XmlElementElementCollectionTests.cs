using System;
using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System.Xml;
using System.Configuration;
using System.Reflection;
using System.IO;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Configuration
{
    public class XmlElementElementCollectionTests
    {
        [Fact]
        public void Add_AddsElementToCollection()
        {
            var collection = new XmlElementElementCollection();
            var doc = new XmlDocument();
            var xmlElem = doc.CreateElement("Test");
            var element = new XmlElementElement(xmlElem);
            collection.Add(element);
            Assert.Single(collection);
            var found = false;
            foreach (XmlElementElement item in collection)
            {
                Assert.NotNull(item.XmlElement);
                Assert.Equal(xmlElem.OuterXml, item.XmlElement.OuterXml);
                found = true;
            }
            Assert.True(found);
        }

        [Fact]
        public void GetElementKey_ReturnsOuterXml()
        {
            var collection = new XmlElementElementCollection();
            var doc = new XmlDocument();
            var xmlElem = doc.CreateElement("Test");
            var element = new XmlElementElement(xmlElem);
            var key = collection.GetType().GetMethod("GetElementKey", BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(collection, new object[] { element });
            Assert.Equal(xmlElem.OuterXml, key);
        }

        [Fact]
        public void GetElementKey_Null_Throws()
        {
            var collection = new XmlElementElementCollection();
            var method = typeof(XmlElementElementCollection).GetMethod("GetElementKey", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(method);
            Assert.Throws<TargetInvocationException>(() => method!.Invoke(collection, new object[] { null }));
            try
            {
                method!.Invoke(collection, new object[] { null });
            }
            catch (TargetInvocationException ex)
            {
                Assert.IsType<ArgumentNullException>(ex.InnerException);
            }
        }

        [Fact]
        public void Unmerge_AddsOnlyElementsNotInParent()
        {
            var source = new XmlElementElementCollection();
            var parent = new XmlElementElementCollection();
            var dest = new XmlElementElementCollection();
            var doc = new XmlDocument();

            var a = new XmlElementElement(doc.CreateElement("A"));
            var b = new XmlElementElement(doc.CreateElement("B"));
            source.Add(a);
            source.Add(b);
            parent.Add(a); // parent contains A only

            var unmerge = typeof(XmlElementElementCollection).GetMethod("Unmerge", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(unmerge);
            unmerge!.Invoke(dest, new object[] { source, parent, ConfigurationSaveMode.Minimal });

            Assert.Single(dest);
            foreach (XmlElementElement e in dest)
            {
                Assert.Equal("B", e.XmlElement!.Name);
            }
        }

        [Fact]
        public void Unmerge_WithNullParent_AddsAllFromSource()
        {
            var source = new XmlElementElementCollection();
            var dest = new XmlElementElementCollection();
            var doc = new XmlDocument();

            source.Add(new XmlElementElement(doc.CreateElement("X")));
            source.Add(new XmlElementElement(doc.CreateElement("Y")));

            var unmerge = typeof(XmlElementElementCollection).GetMethod("Unmerge", BindingFlags.NonPublic | BindingFlags.Instance);
            unmerge!.Invoke(dest, new object[] { source, null, ConfigurationSaveMode.Full });

            Assert.Equal(2, dest.Count);
        }

        [Fact]
        public void Unmerge_WithNullSource_DoesNothing()
        {
            var dest = new XmlElementElementCollection();
            var unmerge = typeof(XmlElementElementCollection).GetMethod("Unmerge", BindingFlags.NonPublic | BindingFlags.Instance);
            unmerge!.Invoke(dest, new object[] { null, null, ConfigurationSaveMode.Minimal });
            Assert.Equal(0, dest.Count);
        }

        [Fact]
        public void OnDeserializeUnrecognizedElement_AddsElementAndReturnsTrue()
        {
            var collection = new XmlElementElementCollection();
            var method = typeof(XmlElementElementCollection).GetMethod("OnDeserializeUnrecognizedElement", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(method);

            var xml = "<E attr=\"v\" />";
            using var sr = new StringReader(xml);
            using var reader = XmlReader.Create(sr, new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Fragment });
            reader.Read();
            var result = (bool)method!.Invoke(collection, new object[] { "E", reader });
            Assert.True(result);
            Assert.Single(collection);
            foreach (XmlElementElement e in collection)
            {
                Assert.Equal("E", e.XmlElement!.Name);
                Assert.Equal("v", e.XmlElement.GetAttribute("attr"));
            }
        }
    }
}
