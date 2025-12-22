using Xunit;
using CoreWCF.ConfigurationManager.Client;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings
{
    public class BasicHttpBindingCollectionElementTests
    {
        [Fact]
        public void CanInstantiateBasicHttpBindingCollectionElement()
        {
            var element = new BasicHttpBindingCollectionElement();
            Assert.NotNull(element);
        }
    }
}
