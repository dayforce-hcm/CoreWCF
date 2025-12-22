using System;
using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System.ServiceModel.Channels;
using System.ServiceModel;

namespace CoreWCF.ConfigurationManager.Elements.Bindings.WebHttp.Tests
{
    public class WebHttpSecurityTests
    {
        [Fact]
        public void CanInstantiateWebHttpSecurity_Defaults()
        {
            var security = new WebHttpSecurity();
            Assert.NotNull(security.Transport);
            Assert.Equal(WebHttpSecurityMode.None, security.Mode);
        }

        [Fact]
        public void CanSetModeAndTransport()
        {
            var security = new WebHttpSecurity();
            security.Mode = WebHttpSecurityMode.Transport;
            Assert.Equal(WebHttpSecurityMode.Transport, security.Mode);
            var transport = new HttpTransportSecurity();
            security.Transport = transport;
            Assert.Equal(transport, security.Transport);
        }

        [Fact]
        public void SettingInvalidMode_Throws()
        {
            var security = new WebHttpSecurity();
            Assert.Throws<ArgumentOutOfRangeException>(() => security.Mode = (WebHttpSecurityMode)999);
        }
    }
}
