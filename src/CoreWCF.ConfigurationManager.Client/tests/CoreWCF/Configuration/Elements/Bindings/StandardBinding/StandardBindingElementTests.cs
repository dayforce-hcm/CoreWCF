using System;
using System.ServiceModel.Channels;
using Xunit;
using System.Configuration;
using CoreWCF.ConfigurationManager.Client;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings
{
    internal class TestStandardBindingElement : StandardBindingElement
    {
        public TestStandardBindingElement() : base() { }
        public TestStandardBindingElement(string name) : base(name) { }
        public override Binding CreateBinding() => null;
    }

    public class StandardBindingElementTests
    {
        [Fact]
        public void Name_Property_SetAndGet()
        {
            var element = new TestStandardBindingElement();
            element.Name = "TestName";
            Assert.Equal("TestName", element.Name);
            element.Name = null;
            Assert.Equal(string.Empty, element.Name);
        }

        [Fact]
        public void TimeoutProperties_SetAndGet()
        {
            var element = new TestStandardBindingElement();
            var ts = TimeSpan.FromSeconds(10);
            element.CloseTimeout = ts;
            element.OpenTimeout = ts;
            element.ReceiveTimeout = ts;
            element.SendTimeout = ts;
            Assert.Equal(ts, element.CloseTimeout);
            Assert.Equal(ts, element.OpenTimeout);
            Assert.Equal(ts, element.ReceiveTimeout);
            Assert.Equal(ts, element.SendTimeout);
        }

        [Fact]
        public void CreateBinding_ReturnsNull()
        {
            var element = new TestStandardBindingElement();
            Assert.Null(element.CreateBinding());
        }
    }
}
