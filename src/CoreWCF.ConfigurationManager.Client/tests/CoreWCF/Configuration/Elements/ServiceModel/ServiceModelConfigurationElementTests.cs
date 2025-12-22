using CoreWCF.ConfigurationManager.Client;
using System;
using System.Configuration;
using System.Diagnostics.Contracts;
using Xunit;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.ServiceModel
{
    // Minimal concrete implementation for testing
    internal class TestServiceModelConfigurationElement : ServiceModelConfigurationElement
    {
        public TestServiceModelConfigurationElement()
        {
            // Add a test property
            Properties.Add(new ConfigurationProperty("TestProperty", typeof(int), 42));
        }
        public int TestProperty
        {
            get => (int)this["TestProperty"];
            set => this["TestProperty"] = value;
        }
        public void CallSetPropertyValueIfNotDefaultValue<T>(string propertyName, T value)
        {
            SetPropertyValueIfNotDefaultValue(propertyName, value);
        }
    }

    public class ServiceModelConfigurationElementTests
    {
        [Fact]
        public void SetPropertyValueIfNotDefaultValue_SetsValue_WhenNotDefault()
        {
            var element = new TestServiceModelConfigurationElement();
            element.CallSetPropertyValueIfNotDefaultValue("TestProperty", 100);
            Assert.Equal(100, element.TestProperty);
        }

        [Fact]
        public void SetPropertyValueIfNotDefaultValue_DoesNotSet_WhenDefault()
        {
            var element = new TestServiceModelConfigurationElement();
            element.CallSetPropertyValueIfNotDefaultValue("TestProperty", 42);
            Assert.Equal(42, element.TestProperty);
        }
    }
}
