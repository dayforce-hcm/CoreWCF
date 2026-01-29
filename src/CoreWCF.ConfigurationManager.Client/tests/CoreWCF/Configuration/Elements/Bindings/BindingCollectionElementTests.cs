using Xunit;
using CoreWCF.ConfigurationManager.Client;
using CoreWCF.Configuration;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings
{
    public class BindingCollectionElementTests
    {
        private class TestBindingCollectionElement : BindingCollectionElement { }

        [Fact]
        public void CanInstantiateBindingCollectionElement_Derived()
        {
            var element = new TestBindingCollectionElement();
            Assert.NotNull(element);
        }
    }
}
