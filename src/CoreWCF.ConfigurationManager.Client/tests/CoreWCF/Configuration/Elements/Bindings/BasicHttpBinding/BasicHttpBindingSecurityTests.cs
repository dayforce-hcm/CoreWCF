using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System.ServiceModel;
using SMHttpClientCredentialType = System.ServiceModel.HttpClientCredentialType;
using SMBasicHttpMessageCredentialType = System.ServiceModel.BasicHttpMessageCredentialType;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings
{
    public class BasicHttpBindingSecurityTests
    {
        [Fact]
        public void SecurityElement_DefaultValues_AreCorrect()
        {
            var bindingElement = new BasicHttpBindingElement();
            var security = bindingElement.Security;
            Assert.NotNull(security);
            Assert.Equal(BasicHttpSecurityMode.None, security.Mode);
            Assert.NotNull(security.Transport);
            Assert.NotNull(security.Message);
        }

        [Fact]
        public void SecurityElement_SetAllProperties_ValuesAreSet()
        {
            var bindingElement = new BasicHttpBindingElement();
            var security = bindingElement.Security;
            security.Mode = BasicHttpSecurityMode.TransportWithMessageCredential;
            security.Transport.ClientCredentialType = SMHttpClientCredentialType.Windows;
            security.Transport.Realm = "TestRealm";
            security.Message.ClientCredentialType = SMBasicHttpMessageCredentialType.Certificate;

            Assert.Equal(BasicHttpSecurityMode.TransportWithMessageCredential, security.Mode);
            Assert.Equal(SMHttpClientCredentialType.Windows, security.Transport.ClientCredentialType);
            Assert.Equal("TestRealm", security.Transport.Realm);
            Assert.Equal(SMBasicHttpMessageCredentialType.Certificate, security.Message.ClientCredentialType);
        }

        [Fact]
        public void SecurityElement_ApplyConfiguration_SetsPropertiesOnBinding()
        {
            var bindingElement = new BasicHttpBindingElement("TestBindingName");
            var security = bindingElement.Security;
            security.Mode = BasicHttpSecurityMode.Transport;
            security.Transport.ClientCredentialType = SMHttpClientCredentialType.Basic;
            security.Transport.Realm = "RealmValue";
            security.Message.ClientCredentialType = SMBasicHttpMessageCredentialType.UserName;

            var binding = (System.ServiceModel.BasicHttpBinding)bindingElement.CreateBinding();
            Assert.Equal(BasicHttpSecurityMode.Transport, binding.Security.Mode);
            Assert.Equal(SMHttpClientCredentialType.Basic, binding.Security.Transport.ClientCredentialType);
            Assert.Equal(SMBasicHttpMessageCredentialType.UserName, binding.Security.Message.ClientCredentialType);
        }
    }
}
