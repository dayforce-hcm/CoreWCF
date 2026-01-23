using Xunit;
using CoreWCF.ConfigurationManager.Client;

namespace CoreWCF.ConfigurationManager.Elements.Bindings.WebHttp.Tests
{
    public class WebHttpBindingCollectionElementTests
    {
        [Fact]
        public void CanInstantiateWebHttpBindingCollectionElement()
        {
            var element = new WebHttpBindingCollectionElement();
            Assert.NotNull(element);
        }
    }
}
