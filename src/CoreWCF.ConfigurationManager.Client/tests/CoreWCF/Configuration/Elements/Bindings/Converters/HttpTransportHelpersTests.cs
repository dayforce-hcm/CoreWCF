using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Xunit;
using CoreWCF.ConfigurationManager.Client;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings.Converters
{
    public class HttpTransportHelpersTests
    {
        [Fact]
        public void ConfigureTransportProtectionAndAuthentication_SetsRequireClientCertificate()
        {
            var https = new HttpsTransportBindingElement();
            var security = new HttpTransportSecurity { ClientCredentialType = HttpClientCredentialType.Certificate };
            HttpTransportHelpers.ConfigureTransportProtectionAndAuthentication(https, security);
            Assert.True(https.RequireClientCertificate);
        }

        [Fact]
        public void ConfigureTransportAuthentication_ThrowsOnCertificateCredentialType()
        {
            var http = new HttpTransportBindingElement();
            var security = new HttpTransportSecurity { ClientCredentialType = HttpClientCredentialType.Certificate };
            Assert.Throws<InvalidOperationException>(() => HttpTransportHelpers.ConfigureTransportAuthentication(http, security));
        }

        [Theory]
        [InlineData(HttpClientCredentialType.None, AuthenticationSchemes.Anonymous)]
        [InlineData(HttpClientCredentialType.Basic, AuthenticationSchemes.Basic)]
        [InlineData(HttpClientCredentialType.Digest, AuthenticationSchemes.Digest)]
        [InlineData(HttpClientCredentialType.Ntlm, AuthenticationSchemes.Ntlm)]
        [InlineData(HttpClientCredentialType.Windows, AuthenticationSchemes.Negotiate)]
        [InlineData(HttpClientCredentialType.InheritedFromHost, AuthenticationSchemes.None)]
        public void ConfigureTransportAuthentication_SetsAuthenticationScheme(HttpClientCredentialType credentialType, AuthenticationSchemes expectedScheme)
        {
            var http = new HttpTransportBindingElement();
            var security = new HttpTransportSecurity { ClientCredentialType = credentialType };
            if (credentialType != HttpClientCredentialType.Certificate)
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
