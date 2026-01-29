using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Globalization;
using SMBasicHttpMessageCredentialType = System.ServiceModel.BasicHttpMessageCredentialType;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings.Properties.Security
{
    public class BasicHttpMessageSecurityElementTests
    {
        [Fact]
        public void ClientCredentialType_Default_IsUserName()
        {
            var element = new BasicHttpMessageSecurityElement();
            Assert.Equal(SMBasicHttpMessageCredentialType.UserName, element.ClientCredentialType);
        }

        [Fact]
        public void ClientCredentialType_SetValue_ReturnsValue()
        {
            var element = new BasicHttpMessageSecurityElement();
            element.ClientCredentialType = SMBasicHttpMessageCredentialType.Certificate;
            Assert.Equal(SMBasicHttpMessageCredentialType.Certificate, element.ClientCredentialType);
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
            var security = new System.ServiceModel.BasicHttpMessageSecurity();
            element.ClientCredentialType = SMBasicHttpMessageCredentialType.Certificate;
            element.ApplyConfiguration(security);
            Assert.Equal(SMBasicHttpMessageCredentialType.Certificate, security.ClientCredentialType);
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
