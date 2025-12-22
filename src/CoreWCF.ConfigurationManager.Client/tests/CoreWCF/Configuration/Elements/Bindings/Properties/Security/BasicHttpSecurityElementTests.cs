using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System;
using System.ServiceModel;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings.Properties.Security
{
    public class BasicHttpSecurityElementTests
    {
        [Fact]
        public void Mode_Default_IsNone()
        {
            var element = new BasicHttpSecurityElement();
            Assert.Equal(BasicHttpSecurityMode.None, element.Mode);
        }

        [Fact]
        public void Mode_SetValue_ReturnsValue()
        {
            var element = new BasicHttpSecurityElement();
            element.Mode = BasicHttpSecurityMode.Transport;
            Assert.Equal(BasicHttpSecurityMode.Transport, element.Mode);
        }

        [Fact]
        public void ApplyConfiguration_ThrowsOnNull()
        {
            var element = new BasicHttpSecurityElement();
            Assert.Throws<ArgumentNullException>(() => element.ApplyConfiguration(null));
        }
    }
}
