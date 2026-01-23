using System;
using System.Configuration;
using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System.Collections;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.ServiceModel
{
    internal class TestEnhancedElement : ConfigurationElement { }
    internal class TestServiceModelEnhancedConfigurationElementCollection : ServiceModelEnhancedConfigurationElementCollection<TestEnhancedElement>
    {
        public TestServiceModelEnhancedConfigurationElementCollection(string name) : base(name) { }
        protected override object GetElementKey(ConfigurationElement element) => element.GetHashCode();
        // Expose CreateNewElement for testing
        public ConfigurationElement CallCreateNewElement() => CreateNewElement();
    }

    public class ServiceModelEnhancedConfigurationElementCollectionTests
    {
        [Fact]
        public void CreateNewElement_ReturnsCorrectType()
        {
            var collection = new TestServiceModelEnhancedConfigurationElementCollection("testElement");
            var element = collection.CallCreateNewElement();
            Assert.IsType<TestEnhancedElement>(element);
        }
    }
}
