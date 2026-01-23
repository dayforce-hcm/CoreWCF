using Xunit;
using CoreWCF.ConfigurationManager.Client;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings.Converters
{
    public class WebHttpSecurityModeHelperTests
    {
        [Theory]
        [InlineData(WebHttpSecurityMode.None, true)]
        [InlineData(WebHttpSecurityMode.Transport, true)]
        [InlineData(WebHttpSecurityMode.TransportCredentialOnly, true)]
        [InlineData((WebHttpSecurityMode)999, false)]
        public void IsDefined_ReturnsExpected(WebHttpSecurityMode value, bool expected)
        {
            Assert.Equal(expected, WebHttpSecurityModeHelper.IsDefined(value));
        }
    }
}
