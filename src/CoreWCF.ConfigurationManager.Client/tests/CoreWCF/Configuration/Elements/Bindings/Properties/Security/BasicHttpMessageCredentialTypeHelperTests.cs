using Xunit;
using System.ServiceModel;
using CoreWCF.ConfigurationManager.Client;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings.Properties.Security
{
    public class BasicHttpMessageCredentialTypeHelperTests
    {
        [Theory]
        [InlineData(BasicHttpMessageCredentialType.UserName, true)]
        [InlineData(BasicHttpMessageCredentialType.Certificate, true)]
        [InlineData((BasicHttpMessageCredentialType)999, false)]
        public void IsDefined_ReturnsExpected(BasicHttpMessageCredentialType value, bool expected)
        {
            Assert.Equal(expected, BasicHttpMessageCredentialTypeHelper.IsDefined(value));
        }
    }
}
