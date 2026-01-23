using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System.ServiceModel;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings
{
    public class HttpTransportSecurityElementTests
    {
        [Fact]
        public void Default_ClientCredentialType_IsNone()
        {
            var element = new HttpTransportSecurityElement();
            Assert.Equal(HttpClientCredentialType.None, element.ClientCredentialType);
        }

        [Fact]
        public void Properties_SetAndGet_ReturnsValues()
        {
            var element = new HttpTransportSecurityElement();
            element.ClientCredentialType = HttpClientCredentialType.Basic;
            element.Realm = "realm";
            Assert.Equal(HttpClientCredentialType.Basic, element.ClientCredentialType);
            Assert.Equal("realm", element.Realm);
        }

        [Fact]
        public void Realm_SetNullOrEmpty_NormalizesToEmptyString()
        {
            var element = new HttpTransportSecurityElement();
            element.Realm = null;
            Assert.Equal(string.Empty, element.Realm);
            element.Realm = "";
            Assert.Equal(string.Empty, element.Realm);
        }

        [Fact]
        public void ApplyConfiguration_ThrowsOnNullSecurity()
        {
            var element = new HttpTransportSecurityElement();
            Assert.Throws<System.ArgumentNullException>(() => element.ApplyConfiguration(null));
        }

        [Fact]
        public void ApplyConfiguration_SetsSecurityProperties()
        {
            var element = new HttpTransportSecurityElement();
            var security = new HttpTransportSecurity();
            element.ClientCredentialType = HttpClientCredentialType.Windows;
            element.ApplyConfiguration(security);
            Assert.Equal(HttpClientCredentialType.Windows, security.ClientCredentialType);
        }
    }
}
