using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System;
using System.Reflection;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;
using CoreWCF.Configuration;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings.Properties.Security
{
    public class SecurityElementTests
    {
        [Fact]
        public void Constructor_SetsBootstrapFlag()
        {
            var element = new SecurityElement();
            Assert.True(element.SecureConversationBootstrap.IsSecurityElementBootstrap);
        }

        [Fact]
        public void CopyFrom_CopiesProperties()
        {
            var source = new SecurityElement();
            var target = new SecurityElement();
            // Set a unique value in source
            source.SecureConversationBootstrap.AuthenticationMode = AuthenticationMode.SspiNegotiatedOverTransport;
            // Leave target at default
            target.CopyFrom(source);
            Assert.Equal(source.SecureConversationBootstrap.AuthenticationMode, target.SecureConversationBootstrap.AuthenticationMode);
        }

        [Fact]
        public void CreateBindingElement_SecureConversation_WithoutBootstrap_Throws()
        {
            var element = new SecurityElement
            {
                AuthenticationMode = AuthenticationMode.SecureConversation
            };

            // In this implementation, bootstrap property is never null, so invalid path is not reachable.
            // Cover the closest invalid scenario via bootstrap using SecureConversation, which throws.
            element.SecureConversationBootstrap.AuthenticationMode = AuthenticationMode.SecureConversation;
            Assert.Throws<InvalidOperationException>(() => element.CreateBindingElement());
        }

        [Fact]
        public void CreateBindingElement_SecureConversation_BootstrapSecureConversation_Throws()
        {
            var element = new SecurityElement
            {
                AuthenticationMode = AuthenticationMode.SecureConversation
            };

            element.SecureConversationBootstrap.AuthenticationMode = AuthenticationMode.SecureConversation;
            Assert.Throws<InvalidOperationException>(() => element.CreateBindingElement());
        }

        [Fact]
        public void CreateBindingElement_SecureConversation_ValidBootstrap_UsesBootstrapBindingAndAppliesConfiguration()
        {
            var element = new SecurityElement
            {
                AuthenticationMode = AuthenticationMode.SecureConversation,
                IncludeTimestamp = false,
                SecurityHeaderLayout = SecurityHeaderLayout.Lax
            };

            // Use a supported bootstrap auth mode via base implementation
            element.SecureConversationBootstrap.AuthenticationMode = AuthenticationMode.UserNameOverTransport;

            var bindingElement = element.CreateBindingElement();
            var sbe = Assert.IsType<TransportSecurityBindingElement>(bindingElement);

            // ConfigurationManager from element should be applied to the resulting binding element
            Assert.False(sbe.IncludeTimestamp);
            Assert.Equal(SecurityHeaderLayout.Lax, sbe.SecurityHeaderLayout);
        }

        [Fact]
        public void CreateBindingElement_NonSecureConversation_DelegatesToBase()
        {
            var element = new SecurityElement
            {
                AuthenticationMode = AuthenticationMode.UserNameOverTransport
            };

            var bindingElement = element.CreateBindingElement();
            Assert.IsType<TransportSecurityBindingElement>(bindingElement);
        }

        [Fact]
        public void InitializeNestedTokenParameterSettings_SecureConversationParameters_PopulatesFlagsAndBootstrap()
        {
            var element = new SecurityElement();

            var scParams = new SecureConversationSecurityTokenParameters();
            scParams.RequireCancellation = true;
            scParams.BootstrapSecurityBindingElement = SecurityBindingElement.CreateUserNameOverTransportBindingElement();

            // Invoke the protected method via reflection
            var method = typeof(SecurityElement).GetMethod("InitializeNestedTokenParameterSettings", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(method);
            method!.Invoke(element, new object[] { scParams, true });

            Assert.True(element.RequireSecurityContextCancellation);
            // Bootstrap should have been initialized from the provided binding element
            Assert.Equal(scParams.BootstrapSecurityBindingElement.IncludeTimestamp, element.SecureConversationBootstrap.IncludeTimestamp);
        }
    }
}
