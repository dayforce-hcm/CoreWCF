using Xunit;
using CoreWCF.ConfigurationManager.Client;

namespace CoreWCF.ConfigurationManager.Elements.Bindings.WebHttp.Tests
{
    public class WebHttpSecurityModeTests
    {
        [Theory]
        [InlineData(WebHttpSecurityMode.None)]
        [InlineData(WebHttpSecurityMode.Transport)]
        [InlineData(WebHttpSecurityMode.TransportCredentialOnly)]
        public void WebHttpSecurityMode_Values_AreDefined(WebHttpSecurityMode mode)
        {
            Assert.True(System.Enum.IsDefined(typeof(WebHttpSecurityMode), mode));
        }
    }
}
