using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Security.Tokens;
using System.Text;
using System.Xml;
using System.IdentityModel.Tokens;
using System.Configuration;
using System.Reflection;
using SMEndpointAddress = System.ServiceModel.EndpointAddress;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings.Properties.Security
{
    public class IssuedTokenParametersElementTests
    {
        [Fact]
        public void TokenType_SetNullOrEmpty_SetsEmptyString()
        {
            var element = new IssuedTokenParametersElement();
            element.TokenType = null;
            Assert.Equal(string.Empty, element.TokenType);
            element.TokenType = "";
            Assert.Equal(string.Empty, element.TokenType);
        }

        [Fact]
        public void KeySize_SetValue_ReturnsValue()
        {
            var element = new IssuedTokenParametersElement();
            element.KeySize = 128;
            Assert.Equal(128, element.KeySize);
        }

        [Fact]
        public void KeyType_SetValue_ReturnsValue()
        {
            var element = new IssuedTokenParametersElement();
            var keyTypeValue = element.KeyType; // get default value
            element.KeyType = keyTypeValue;
            Assert.Equal(keyTypeValue, element.KeyType);
        }

        [Fact]
        public void UseStrTransform_SetValue_ReturnsValue()
        {
            var element = new IssuedTokenParametersElement();
            element.UseStrTransform = true;
            Assert.True(element.UseStrTransform);
        }

        [Fact]
        public void ApplyConfiguration_ThrowsOnNull()
        {
            var element = new IssuedTokenParametersElement();
            Assert.Throws<ArgumentNullException>(() => element.ApplyConfiguration(null));
        }

        [Fact]
        public void ApplyConfiguration_SetsParameters()
        {
            var element = new IssuedTokenParametersElement
            {
                KeyType = SecurityKeyType.SymmetricKey,
                TokenType = "urn:test-token",
                DefaultMessageSecurityVersion = new SecurityElementBase().MessageSecurityVersion
            };

            var parameters = new IssuedSecurityTokenParameters();
            element.ApplyConfiguration(parameters);

            Assert.Equal(SecurityKeyType.SymmetricKey, parameters.KeyType);
            Assert.Equal("urn:test-token", parameters.TokenType);
            Assert.Equal(element.DefaultMessageSecurityVersion, parameters.DefaultMessageSecurityVersion);
        }

        [Fact]
        public void ApplyConfiguration_IssuerOrMetadata_ThrowsPlatformNotSupported()
        {
            var element = new IssuedTokenParametersElement();
            element.Issuer.Address = new Uri("http://issuer");
            var parameters = new IssuedSecurityTokenParameters();
            Assert.Throws<PlatformNotSupportedException>(() => element.ApplyConfiguration(parameters));

            var element2 = new IssuedTokenParametersElement();
            element2.IssuerMetadata.Address = new Uri("http://issuer-metadata");
            Assert.Throws<PlatformNotSupportedException>(() => element2.ApplyConfiguration(parameters));
        }

        [Fact]
        public void Copy_CopiesCollectionsAndProperties()
        {
            var src = new IssuedTokenParametersElement
            {
                KeySize = 256,
                KeyType = SecurityKeyType.SymmetricKey,
                TokenType = "urn:copy",
                DefaultMessageSecurityVersion = new SecurityElementBase().MessageSecurityVersion,
                UseStrTransform = true
            };
            src.ClaimTypeRequirements.Add(new ClaimTypeElement("ct1", false));
            src.AdditionalRequestParameters.Add(new XmlElementElement(new XmlDocument().CreateElement("extra")));
            src.Issuer.Address = new Uri("http://issuer");
            src.IssuerMetadata.Address = new Uri("http://issuer-metadata");

            var dest = new IssuedTokenParametersElement();
            dest.Copy(src);

            Assert.Equal(256, dest.KeySize);
            Assert.Equal(SecurityKeyType.SymmetricKey, dest.KeyType);
            Assert.Equal("urn:copy", dest.TokenType);
            Assert.Equal(src.DefaultMessageSecurityVersion, dest.DefaultMessageSecurityVersion);
            Assert.True(dest.UseStrTransform);
            Assert.Single(dest.ClaimTypeRequirements);
            Assert.Single(dest.AdditionalRequestParameters);
            Assert.NotNull(dest.Issuer.Address);
            Assert.NotNull(dest.IssuerMetadata.Address);
        }

        [Fact]
        public void Create_WithTemplateOnly_SetsTemplateKeyType()
        {
            var element = new IssuedTokenParametersElement();
            var result = element.Create(createTemplateOnly: true, templateKeyType: SecurityKeyType.BearerKey);
            Assert.Equal(SecurityKeyType.BearerKey, result.KeyType);
        }

        [Fact]
        public void Create_ApplyConfiguration_PopulatesParameters()
        {
            var element = new IssuedTokenParametersElement
            {
                KeyType = SecurityKeyType.SymmetricKey,
                TokenType = "urn:test",
                DefaultMessageSecurityVersion = new SecurityElementBase().MessageSecurityVersion
            };
            var result = element.Create(createTemplateOnly: false, templateKeyType: SecurityKeyType.BearerKey);
            Assert.Equal(SecurityKeyType.SymmetricKey, result.KeyType);
            Assert.Equal("urn:test", result.TokenType);
            Assert.Equal(element.DefaultMessageSecurityVersion, result.DefaultMessageSecurityVersion);
        }

        [Fact]
        public void InitializeFrom_SetsProperties()
        {
            var parameters = new IssuedSecurityTokenParameters
            {
                KeyType = SecurityKeyType.SymmetricKey,
                TokenType = "urn:init",
                IssuerAddress = new SMEndpointAddress("http://issuer")
            };
            parameters.DefaultMessageSecurityVersion = new SecurityElementBase().MessageSecurityVersion;

            var element = new IssuedTokenParametersElement();
            element.InitializeFrom(parameters, initializeNestedBindings: false);

            Assert.Equal(SecurityKeyType.SymmetricKey, element.KeyType);
            Assert.Equal("urn:init", element.TokenType);
            Assert.NotNull(element.Issuer.Address);
            Assert.Equal(parameters.DefaultMessageSecurityVersion, element.DefaultMessageSecurityVersion);
        }

        [Fact]
        public void InitializeFrom_Null_Throws()
        {
            var element = new IssuedTokenParametersElement();
            Assert.Throws<ArgumentNullException>(() => element.InitializeFrom(null!, initializeNestedBindings: true));
        }

        [Fact]
        public void SerializeToXmlElement_WritesAlternativeCommentWhenOptionalPresent()
        {
            var element = new IssuedTokenParametersElement();
            var alt = new IssuedTokenParametersElement();
            alt.TokenType = "urn:alt";
            element.OptionalIssuedTokenParameters.Add(alt);

            var sb = new StringBuilder();
            using var writer = XmlWriter.Create(new StringWriter(sb), new XmlWriterSettings { OmitXmlDeclaration = true });
            var method = typeof(IssuedTokenParametersElement).GetMethod("SerializeToXmlElement", BindingFlags.NonPublic | BindingFlags.Instance);
            var result = (bool)method!.Invoke(element, new object[] { writer, "issuedTokenParameters" })!;
            writer.Flush();
            var xml = sb.ToString();

            Assert.True(result);
            Assert.Contains("alternativeIssuedTokenParameters", xml, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Unmerge_CopiesOptionalIssuedTokenParametersReference()
        {
            var src = new IssuedTokenParametersElement();
            var alt = new IssuedTokenParametersElement();
            src.OptionalIssuedTokenParameters.Add(alt);
            var dest = new IssuedTokenParametersElement();

            var method = typeof(IssuedTokenParametersElement).GetMethod("Unmerge", BindingFlags.NonPublic | BindingFlags.Instance);
            method!.Invoke(dest, new object[] { src, null!, ConfigurationSaveMode.Minimal });

            Assert.Single(dest.OptionalIssuedTokenParameters);
        }
    }
}
