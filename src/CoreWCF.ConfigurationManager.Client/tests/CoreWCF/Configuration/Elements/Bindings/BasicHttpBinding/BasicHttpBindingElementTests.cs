using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings
{
    public class BasicHttpBindingElementTests
    {
        [Fact]
        public void CanInstantiateBasicHttpBindingElement_DefaultConstructor()
        {
            var element = new BasicHttpBindingElement();
            Assert.NotNull(element);
        }

        [Fact]
        public void CanInstantiateBasicHttpBindingElement_WithName()
        {
            var element = new BasicHttpBindingElement("testName");
            Assert.NotNull(element);
            Assert.Equal("testName", element.Name);
        }

        [Fact]
        public void SecurityProperty_IsNotNull()
        {
            var element = new BasicHttpBindingElement();
            Assert.NotNull(element.Security);
        }

        [Fact]
        public void CreateBinding_ReturnsBasicHttpBinding()
        {
            var element = new BasicHttpBindingElement("TestBindingName");
            var binding = element.CreateBinding();
            Assert.IsType<System.ServiceModel.BasicHttpBinding>(binding);
            Assert.Equal("TestBindingName", binding.Name);
        }
    }
}
