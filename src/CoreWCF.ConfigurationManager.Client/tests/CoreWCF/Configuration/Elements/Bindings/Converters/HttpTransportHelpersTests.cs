using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Xunit;
using CoreWCF.ConfigurationManager.Client;
using SMHttpClientCredentialType = System.ServiceModel.HttpClientCredentialType;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings.Converters
{
    public class HttpTransportHelpersTests
    {
        [Fact]
        public void ConfigureTransportProtectionAndAuthentication_SetsRequireClientCertificate()
        {
            var https = new HttpsTransportBindingElement();
            var security = new System.ServiceModel.HttpTransportSecurity { ClientCredentialType = SMHttpClientCredentialType.Certificate };
            HttpTransportHelpers.ConfigureTransportProtectionAndAuthentication(https, security);
            Assert.True(https.RequireClientCertificate);
        }

        [Fact]
        public void ConfigureTransportAuthentication_ThrowsOnCertificateCredentialType()
        {
            var http = new HttpTransportBindingElement();
            var security = new System.ServiceModel.HttpTransportSecurity { ClientCredentialType = SMHttpClientCredentialType.Certificate };
            Assert.Throws<InvalidOperationException>(() => HttpTransportHelpers.ConfigureTransportAuthentication(http, security));
        }

        [Theory]
        [InlineData(SMHttpClientCredentialType.None, AuthenticationSchemes.Anonymous)]
        [InlineData(SMHttpClientCredentialType.Basic, AuthenticationSchemes.Basic)]
        [InlineData(SMHttpClientCredentialType.Digest, AuthenticationSchemes.Digest)]
        [InlineData(SMHttpClientCredentialType.Ntlm, AuthenticationSchemes.Ntlm)]
        [InlineData(SMHttpClientCredentialType.Windows, AuthenticationSchemes.Negotiate)]
        [InlineData(SMHttpClientCredentialType.InheritedFromHost, AuthenticationSchemes.None)]
        public void ConfigureTransportAuthentication_SetsAuthenticationScheme(SMHttpClientCredentialType credentialType, AuthenticationSchemes expectedScheme)
        {
            var http = new HttpTransportBindingElement();
            var security = new System.ServiceModel.HttpTransportSecurity { ClientCredentialType = credentialType };
            if (credentialType != SMHttpClientCredentialType.Certificate)
            {
                HttpTransportHelpers.ConfigureTransportAuthentication(http, security);
                Assert.Equal(expectedScheme, http.AuthenticationScheme);
            }
        }

        [Fact]
        public void DisableTransportAuthentication_SetsAnonymousScheme()
        {
            var http = new HttpTransportBindingElement();
            HttpTransportHelpers.DisableTransportAuthentication(http);
            Assert.Equal(AuthenticationSchemes.Anonymous, http.AuthenticationScheme);
        }
    }
}
