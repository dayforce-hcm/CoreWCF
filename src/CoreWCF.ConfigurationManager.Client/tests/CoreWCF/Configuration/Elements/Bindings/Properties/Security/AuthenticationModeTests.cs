using Xunit;
using CoreWCF.ConfigurationManager.Client;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings.Properties.Security
{
    public class AuthenticationModeTests
    {
        [Theory]
        [InlineData(AuthenticationMode.CertificateOverTransport)]
        [InlineData(AuthenticationMode.IssuedTokenForCertificate)]
        [InlineData(AuthenticationMode.IssuedTokenForSslNegotiated)]
        [InlineData(AuthenticationMode.IssuedTokenOverTransport)]
        [InlineData(AuthenticationMode.SecureConversation)]
        [InlineData(AuthenticationMode.UserNameOverTransport)]
        [InlineData(AuthenticationMode.SspiNegotiatedOverTransport)]
        public void Enum_Values_AreDefined(AuthenticationMode mode)
        {
            Assert.True(System.Enum.IsDefined(typeof(AuthenticationMode), mode));
        }
    }
}
