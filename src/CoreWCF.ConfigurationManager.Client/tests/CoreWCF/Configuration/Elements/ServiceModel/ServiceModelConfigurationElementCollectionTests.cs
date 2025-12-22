using System;
using System.Configuration;
using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System.Collections;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.ServiceModel
{
    // Minimal concrete implementation for testing
    internal class TestServiceModelConfigurationElementCollection : ServiceModelConfigurationElementCollection<TestElement>
    {
        public TestServiceModelConfigurationElementCollection() : base() { }
        public TestServiceModelConfigurationElementCollection(ConfigurationElementCollectionType type, string name) : base(type, name) { }
        public TestServiceModelConfigurationElementCollection(ConfigurationElementCollectionType type, string name, IComparer comparer) : base(type, name, comparer) { }
        protected override object GetElementKey(ConfigurationElement element) => element.GetHashCode();
        // Expose CreateNewElement for testing
        public ConfigurationElement CallCreateNewElement() => CreateNewElement();
    }

    internal class TestElement : ConfigurationElement { }

    public class ServiceModelConfigurationElementCollectionTests
    {
        [Fact]
        public void CreateNewElement_ReturnsCorrectType()
        {
            var collection = new TestServiceModelConfigurationElementCollection();
            var element = collection.CallCreateNewElement();
            Assert.IsType<TestElement>(element);
        }
    }
}
