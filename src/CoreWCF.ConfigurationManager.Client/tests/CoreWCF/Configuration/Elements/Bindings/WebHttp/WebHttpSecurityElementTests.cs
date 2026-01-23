using System;
using Xunit;
using CoreWCF.ConfigurationManager.Client;

namespace CoreWCF.ConfigurationManager.Elements.Bindings.WebHttp.Tests
{
    public class WebHttpSecurityElementTests
    {
        [Fact]
        public void CanInstantiateWebHttpSecurityElement()
        {
            var element = new WebHttpSecurityElement();
            Assert.NotNull(element);
        }

        [Fact]
        public void CanSetAndGetMode()
        {
            var element = new WebHttpSecurityElement();
            element.Mode = WebHttpSecurityMode.Transport;
            Assert.Equal(WebHttpSecurityMode.Transport, element.Mode);
        }

        [Fact]
        public void ApplyConfiguration_ThrowsOnNull()
        {
            var element = new WebHttpSecurityElement();
            Assert.Throws<ArgumentNullException>(() => element.ApplyConfiguration(null));
        }
    }
}
