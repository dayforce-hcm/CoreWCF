using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System.ServiceModel.Channels;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings
{
    public class CustomBindingCollectionElementTests
    {
        [Fact]
        public void CanInstantiateCustomBindingCollectionElement()
        {
            var element = new CustomBindingCollectionElement();
            Assert.NotNull(element);
        }
    }
}
