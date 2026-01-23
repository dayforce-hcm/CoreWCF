using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Globalization;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings.Properties.Security
{
    public class BasicHttpMessageSecurityElementTests
    {
        [Fact]
        public void ClientCredentialType_Default_IsUserName()
        {
            var element = new BasicHttpMessageSecurityElement();
            Assert.Equal(BasicHttpMessageCredentialType.UserName, element.ClientCredentialType);
        }

        [Fact]
        public void ClientCredentialType_SetValue_ReturnsValue()
        {
            var element = new BasicHttpMessageSecurityElement();
            element.ClientCredentialType = BasicHttpMessageCredentialType.Certificate;
            Assert.Equal(BasicHttpMessageCredentialType.Certificate, element.ClientCredentialType);
        }

        [Fact]
        public void ApplyConfiguration_ThrowsOnNull()
        {
            var element = new BasicHttpMessageSecurityElement();
            Assert.Throws<ArgumentNullException>(() => element.ApplyConfiguration(null));
        }

        [Fact]
        public void ApplyConfiguration_SetsClientCredentialType()
        {
            var element = new BasicHttpMessageSecurityElement();
            var security = new BasicHttpMessageSecurity();
            element.ClientCredentialType = BasicHttpMessageCredentialType.Certificate;
            element.ApplyConfiguration(security);
            Assert.Equal(BasicHttpMessageCredentialType.Certificate, security.ClientCredentialType);
        }

        [Theory]
        [InlineData("Default")]
        [InlineData("Basic256")]
        [InlineData("Basic256Sha256")]
        [InlineData("TripleDes")]
        public void ApplyConfiguration_SetsAlgorithmSuite(string securityAlgorithmSuiteString)
        {
            var converter = new SecurityAlgorithmSuiteConverter();
            SecurityAlgorithmSuite securityAlgorithmSuite = (SecurityAlgorithmSuite) converter.ConvertFrom(null, CultureInfo.InvariantCulture, securityAlgorithmSuiteString);
            var element = new BasicHttpMessageSecurityElement();
            element.AlgorithmSuite = securityAlgorithmSuite;
            Assert.Equal(securityAlgorithmSuite, element.AlgorithmSuite);
        }
    }
}
