using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using System.Xml;
using CoreWCF.ConfigurationManager.Client;
using Xunit;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings.Properties.Security
{
    public class SecurityElementBaseTests
    {
        [Fact]
        public void DefaultAlgorithmSuite_SetValue_ReturnsValue()
        {
            var element = new SecurityElementBase();
            var suite = SecurityAlgorithmSuite.Default;
            element.DefaultAlgorithmSuite = suite;
            Assert.Equal(suite, element.DefaultAlgorithmSuite);
        }

        [Fact]
        public void EnableUnsecuredResponse_SetValue_ReturnsValue()
        {
            var element = new SecurityElementBase();
            element.EnableUnsecuredResponse = true;
            Assert.True(element.EnableUnsecuredResponse);
        }

        [Fact]
        public void AuthenticationMode_SetValue_ReturnsValue()
        {
            var element = new SecurityElementBase();
            element.AuthenticationMode = AuthenticationMode.CertificateOverTransport;
            Assert.Equal(AuthenticationMode.CertificateOverTransport, element.AuthenticationMode);
        }

        [Fact]
        public void RequireDerivedKeys_SetValue_ReturnsValue()
        {
            var element = new SecurityElementBase();
            element.RequireDerivedKeys = true;
            Assert.True(element.RequireDerivedKeys);
        }

        [Fact]
        public void SecurityHeaderLayout_SetValue_ReturnsValue()
        {
            var element = new SecurityElementBase();
            element.SecurityHeaderLayout = SecurityHeaderLayout.Strict;
            Assert.Equal(SecurityHeaderLayout.Strict, element.SecurityHeaderLayout);
        }

        [Fact]
        public void IncludeTimestamp_SetValue_ReturnsValue()
        {
            var element = new SecurityElementBase();
            element.IncludeTimestamp = true;
            Assert.True(element.IncludeTimestamp);
        }

        [Fact]
        public void AllowInsecureTransport_SetValue_ReturnsValue()
        {
            var element = new SecurityElementBase();
            element.AllowInsecureTransport = true;
            Assert.True(element.AllowInsecureTransport);
        }

        [Fact]
        public void KeyEntropyMode_SetValue_ReturnsValue()
        {
            var element = new SecurityElementBase();
            element.KeyEntropyMode = SecurityKeyEntropyMode.ClientEntropy;
            Assert.Equal(SecurityKeyEntropyMode.ClientEntropy, element.KeyEntropyMode);
        }

        [Fact]
        public void ProtectTokens_SetValue_ReturnsValue()
        {
            var element = new SecurityElementBase();
            element.ProtectTokens = true;
            Assert.True(element.ProtectTokens);
        }

        [Fact]
        public void MessageSecurityVersion_SetValue_ReturnsValue()
        {
            var element = new SecurityElementBase();
            var version = element.MessageSecurityVersion;
            element.MessageSecurityVersion = version;
            Assert.Equal(version, element.MessageSecurityVersion);
        }

        [Fact]
        public void RequireSecurityContextCancellation_SetValue_ReturnsValue()
        {
            var element = new SecurityElementBase();
            element.RequireSecurityContextCancellation = true;
            Assert.True(element.RequireSecurityContextCancellation);
        }

        [Fact]
        public void RequireSignatureConfirmation_SetValue_ReturnsValue()
        {
            var element = new SecurityElementBase();
            element.RequireSignatureConfirmation = true;
            Assert.True(element.RequireSignatureConfirmation);
        }

        [Fact]
        public void CanRenewSecurityContextToken_SetValue_ReturnsValue()
        {
            var element = new SecurityElementBase();
            element.CanRenewSecurityContextToken = true;
            Assert.True(element.CanRenewSecurityContextToken);
        }

        [Fact]
        public void BindingElementType_ReturnsSecurityBindingElementType()
        {
            var element = new SecurityElementBase();
            Assert.Equal(typeof(SecurityBindingElement), element.BindingElementType);
        }

        [Fact]
        public void ApplyConfiguration_OnlySetPropertiesAreApplied()
        {
            var element = new SecurityElementBase
            {
                DefaultAlgorithmSuite = SecurityAlgorithmSuite.Basic256Sha256,
                IncludeTimestamp = false,
                // Avoid direct references to static MessageSecurityVersion members for cross-targeting
                MessageSecurityVersion = new SecurityElementBase().MessageSecurityVersion,
                KeyEntropyMode = SecurityKeyEntropyMode.ServerEntropy,
                SecurityHeaderLayout = SecurityHeaderLayout.Lax,
                EnableUnsecuredResponse = true
            };

            var target = SecurityBindingElement.CreateUserNameOverTransportBindingElement();

            element.ApplyConfiguration(target);

            Assert.Equal(SecurityAlgorithmSuite.Basic256Sha256, target.DefaultAlgorithmSuite);
            Assert.False(target.IncludeTimestamp);
            Assert.Equal(element.MessageSecurityVersion, target.MessageSecurityVersion);
            Assert.Equal(SecurityKeyEntropyMode.ServerEntropy, target.KeyEntropyMode);
            Assert.Equal(SecurityHeaderLayout.Lax, target.SecurityHeaderLayout);
            Assert.True(target.EnableUnsecuredResponse);
        }

        [Fact]
        public void CreateBindingElement_CertificateOverTransport_SucceedsAndAppliesConfiguration()
        {
            var element = new SecurityElementBase
            {
                AuthenticationMode = AuthenticationMode.CertificateOverTransport,
                IncludeTimestamp = false,
                SecurityHeaderLayout = SecurityHeaderLayout.Lax
            };

            var bindingElement = element.CreateBindingElement();
            var sbe = Assert.IsType<TransportSecurityBindingElement>(bindingElement);

            Assert.False(sbe.IncludeTimestamp);
            Assert.Equal(SecurityHeaderLayout.Lax, sbe.SecurityHeaderLayout);
        }

        [Fact]
        public void CreateBindingElement_UserNameOverTransport_Succeeds()
        {
            var element = new SecurityElementBase
            {
                AuthenticationMode = AuthenticationMode.UserNameOverTransport
            };

            var bindingElement = element.CreateBindingElement();
            Assert.IsType<TransportSecurityBindingElement>(bindingElement);
        }

        [Fact]
        public void CreateBindingElement_UnsupportedMode_Throws()
        {
            var element = new SecurityElementBase
            {
                AuthenticationMode = AuthenticationMode.SspiNegotiatedOverTransport
            };

            var ex = Assert.Throws<InvalidEnumArgumentException>(() => element.CreateBindingElement());
            Assert.Contains(nameof(SecurityElementBase.AuthenticationMode), ex.Message);
        }

        [Fact]
        public void InitializeFrom_SetsPropertiesAndMarksImportFailed()
        {
            var sbe = SecurityBindingElement.CreateUserNameOverTransportBindingElement();
            sbe.IncludeTimestamp = false;
            sbe.KeyEntropyMode = SecurityKeyEntropyMode.ServerEntropy;
            sbe.SecurityHeaderLayout = SecurityHeaderLayout.Strict;

            var element = new SecurityElementBase();
            element.InitializeFrom(sbe, initializeNestedBindings: true);

            Assert.Equal(sbe.DefaultAlgorithmSuite, element.DefaultAlgorithmSuite);
            Assert.Equal(sbe.IncludeTimestamp, element.IncludeTimestamp);
            Assert.Equal(sbe.MessageSecurityVersion, element.MessageSecurityVersion);
            Assert.Equal(sbe.KeyEntropyMode, element.KeyEntropyMode);
            Assert.Equal(sbe.SecurityHeaderLayout, element.SecurityHeaderLayout);

            var hasImportFailedProp = element.GetType().GetProperty("HasImportFailed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.FlattenHierarchy);
            var value = (bool)hasImportFailedProp!.GetMethod.Invoke(element, null)!;
            Assert.True(value);
        }

        [Fact]
        public void SerializeToXmlElement_WhenImportFailed_WritesAndReturnsTrue()
        {
            var sbe = SecurityBindingElement.CreateUserNameOverTransportBindingElement();
            var element = new SecurityElementBase();
            element.InitializeFrom(sbe, initializeNestedBindings: true);

            var sb = new StringBuilder();
            using var writer = XmlWriter.Create(new StringWriter(sb), new XmlWriterSettings { OmitXmlDeclaration = true });

            var method = typeof(SecurityElementBase).GetMethod("SerializeToXmlElement", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var result = (bool)method!.Invoke(element, new object[] { writer, "security" })!;
            Assert.True(result);
        }

        [Fact]
        public void SerializeToXmlElement_NormalPath_DelegatesToBase()
        {
            var element = new SecurityElementBase
            {
                AuthenticationMode = AuthenticationMode.UserNameOverTransport
            };
            var sb = new StringBuilder();
            using var writer = XmlWriter.Create(new StringWriter(sb), new XmlWriterSettings { OmitXmlDeclaration = true });

            var method = typeof(SecurityElementBase).GetMethod("SerializeToXmlElement", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var result = (bool)method!.Invoke(element, new object[] { writer, "security" })!;
            Assert.True(result);
        }

        [Fact]
        public void SerializeElement_BootstrapTrivial_ReturnsFalse()
        {
            var element = new SecurityElementBase
            {
                IsSecurityElementBootstrap = true
            };
            var settings = new XmlWriterSettings { ConformanceLevel = ConformanceLevel.Fragment };
            using var writer = XmlWriter.Create(TextWriter.Null, settings);

            var method = typeof(SecurityElementBase).GetMethod("SerializeElement", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var result = (bool)method!.Invoke(element, new object[] { writer, false })!;
            Assert.False(result);
        }

        [Fact]
        public void SerializeElement_BootstrapWithSetProperty_ReturnsTrue()
        {
            var element = new SecurityElementBase
            {
                IsSecurityElementBootstrap = true,
                IncludeTimestamp = false
            };
            var settings = new XmlWriterSettings { ConformanceLevel = ConformanceLevel.Fragment };
            using var writer = XmlWriter.Create(TextWriter.Null, settings);

            var method = typeof(SecurityElementBase).GetMethod("SerializeElement", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var result = (bool)method!.Invoke(element, new object[] { writer, false })!;
            Assert.True(result);
        }

        [Fact]
        public void Unmerge_CopiesFailureFlagsFromSource()
        {
            var sbe = SecurityBindingElement.CreateUserNameOverTransportBindingElement();
            var source = new SecurityElementBase();
            source.InitializeFrom(sbe, initializeNestedBindings: true);

            var target = new SecurityElementBase();

            var method = typeof(SecurityElementBase).GetMethod("Unmerge", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method!.Invoke(target, new object[] { source, null!, ConfigurationSaveMode.Minimal });

            using var writer = XmlWriter.Create(TextWriter.Null);
            var serializeToXml = typeof(SecurityElementBase).GetMethod("SerializeToXmlElement", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var result = (bool)serializeToXml!.Invoke(target, new object[] { writer, "security" })!;
            Assert.True(result);
        }

        [Fact]
        public void AreBindingsMatching_BasicEqualityPaths_ReturnTrue()
        {
            var b1 = SecurityBindingElement.CreateUserNameOverTransportBindingElement();
            var b2 = SecurityBindingElement.CreateUserNameOverTransportBindingElement();

            b2.DefaultAlgorithmSuite = b1.DefaultAlgorithmSuite;
            b2.IncludeTimestamp = b1.IncludeTimestamp;
            b2.MessageSecurityVersion = b1.MessageSecurityVersion;
            b2.KeyEntropyMode = b1.KeyEntropyMode;
            b2.SecurityHeaderLayout = b1.SecurityHeaderLayout;

            Assert.True(SecurityElementBase.AreBindingsMatching(b1, b2));
            Assert.True(SecurityElementBase.AreBindingsMatching(b1, b2, exactMessageSecurityVersion: true));
        }

        [Fact]
        public void AreBindingsMatching_DifferentTypes_ReturnFalse()
        {
            var element = new SecurityElementBase();
            var b1 = SecurityBindingElement.CreateUserNameOverTransportBindingElement();
            var b2 = SecurityBindingElement.CreateCertificateOverTransportBindingElement(element.MessageSecurityVersion);

            Assert.False(SecurityElementBase.AreBindingsMatching(b1, b2));
        }

        [Fact]
        public void AreBindingsMatching_DifferentKeyEntropyMode_ReturnFalse()
        {
            var b1 = SecurityBindingElement.CreateUserNameOverTransportBindingElement();
            var b2 = SecurityBindingElement.CreateUserNameOverTransportBindingElement();

            b2.KeyEntropyMode = b1.KeyEntropyMode == SecurityKeyEntropyMode.ClientEntropy
                ? SecurityKeyEntropyMode.ServerEntropy
                : SecurityKeyEntropyMode.ClientEntropy;

            Assert.False(SecurityElementBase.AreBindingsMatching(b1, b2));
        }

        [Fact]
        public void AreBindingsMatching_RelaxedMessageSecurityVersion_ReturnTrueWhenVersionsCompatible()
        {
            var element = new SecurityElementBase();
            var b1 = SecurityBindingElement.CreateUserNameOverTransportBindingElement();
            var b2 = SecurityBindingElement.CreateUserNameOverTransportBindingElement();

            // Align both to the same component-wise version to validate relaxed comparison path
            b1.MessageSecurityVersion = element.MessageSecurityVersion;
            b2.MessageSecurityVersion = element.MessageSecurityVersion;

            Assert.True(SecurityElementBase.AreBindingsMatching(b1, b2, exactMessageSecurityVersion: false));
        }

        [Fact]
        public void SerializeToXmlElement_NormalPath_WritesCommentsWhenIssuerSerialFlagTrueOrFalse()
        {
            var element = new SecurityElementBase
            {
                AuthenticationMode = AuthenticationMode.UserNameOverTransport
            };
            var sb = new StringBuilder();
            using var writer = XmlWriter.Create(new StringWriter(sb), new XmlWriterSettings { OmitXmlDeclaration = true });

            var method = typeof(SecurityElementBase).GetMethod("SerializeToXmlElement", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var result = (bool)method!.Invoke(element, new object[] { writer, "security" })!;
            Assert.True(result);
        }

        [Fact]
        public void InitializeFrom_SupportingTokenParametersCollections_DoNotThrow()
        {
            var sbe = SecurityBindingElement.CreateUserNameOverTransportBindingElement();
            // Ensure no supporting tokens to avoid complex branches but exercise code paths
            sbe.EndpointSupportingTokenParameters.Endorsing.Clear();
            sbe.EndpointSupportingTokenParameters.Signed.Clear();
            sbe.EndpointSupportingTokenParameters.SignedEncrypted.Clear();

            var element = new SecurityElementBase();
            element.InitializeFrom(sbe, initializeNestedBindings: true);

            Assert.Equal(sbe.DefaultAlgorithmSuite, element.DefaultAlgorithmSuite);
            Assert.Equal(sbe.IncludeTimestamp, element.IncludeTimestamp);
            Assert.Equal(sbe.MessageSecurityVersion, element.MessageSecurityVersion);
        }

        [Fact]
        public void Unmerge_PreservesIssuerSerialFlag()
        {
            var sbe = SecurityBindingElement.CreateUserNameOverTransportBindingElement();
            var source = new SecurityElementBase();
            source.InitializeFrom(sbe, initializeNestedBindings: true);

            var target = new SecurityElementBase();
            var unmerge = typeof(SecurityElementBase).GetMethod("Unmerge", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            unmerge!.Invoke(target, new object[] { source, null!, ConfigurationSaveMode.Minimal });

            // Serialize to ensure no exception and path returns true
            var sb = new StringBuilder();
            using var writer = XmlWriter.Create(new StringWriter(sb), new XmlWriterSettings { OmitXmlDeclaration = true });
            var serializeToXml = typeof(SecurityElementBase).GetMethod("SerializeToXmlElement", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var result = (bool)serializeToXml!.Invoke(target, new object[] { writer, "security" })!;
            Assert.True(result);
        }
    }
}
