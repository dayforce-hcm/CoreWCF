using System;
using System.Configuration;
using System.Xml;
using CoreWCF.ConfigurationManager.Client;
using Xunit;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.ExtensionElements
{
    public class DummyExtensionElement2 : ServiceModelExtensionElement
    {
        public bool DefaultInitialized { get; private set; }
        protected override void InitializeDefault() { DefaultInitialized = true; }
    }

    public class ServiceModelExtensionElementTests
    {
        [Fact]
        public void CopyFrom_ThrowsIfReadOnlyOrNull()
        {
            var element = new DummyExtensionElement2();
            Assert.Throws<ArgumentNullException>(() => element.CopyFrom(null));
            element.SetReadOnlyInternal();
            Assert.Throws<ConfigurationErrorsException>(() => element.CopyFrom(new DummyExtensionElement2()));
        }

        [Fact]
        public void Properties_GetSet_Works()
        {
            var element = new DummyExtensionElement2();
            element.ExtensionCollectionName = "collection";
            element.ConfigurationElementName = "element";
            Assert.Equal("collection", element.ExtensionCollectionName);
            Assert.Equal("element", element.ConfigurationElementName);
        }

        [Fact]
        public void InternalInitializeDefault_CallsInitializeDefault()
        {
            var element = new DummyExtensionElement2();
            element.InternalInitializeDefault();
            Assert.True(element.DefaultInitialized);
        }

        [Fact]
        public void IsModifiedInternal_And_ResetModifiedInternal()
        {
            var element = new DummyExtensionElement2();
            Assert.False(element.IsModifiedInternal());
            element.ResetModifiedInternal();
            Assert.False(element.IsModifiedInternal());
        }

        [Fact]
        public void SetReadOnlyInternal_MakesReadOnly()
        {
            var element = new DummyExtensionElement2();
            element.SetReadOnlyInternal();
            Assert.True(element.IsReadOnly());
        }

        [Fact]
        public void CopyFrom_ValidScenario_DoesNotThrow()
        {
            var element1 = new DummyExtensionElement2();
            var element2 = new DummyExtensionElement2();
            element1.CopyFrom(element2);
        }

        [Fact]
        public void DeserializeInternal_DoesNotThrow()
        {
            var element = new DummyExtensionElement2();
            var xml = "<DummyExtensionElement2 />";
            using var reader = XmlReader.Create(new System.IO.StringReader(xml));
            reader.Read();
            element.DeserializeInternal(reader, false);
        }
    }
}
