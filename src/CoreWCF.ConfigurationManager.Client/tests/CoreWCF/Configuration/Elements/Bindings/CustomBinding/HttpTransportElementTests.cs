using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Configuration;
using System.Security.Authentication.ExtendedProtection;
using SMTransferMode = System.ServiceModel.TransferMode;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings
{
    public class HttpTransportElementTests
    {
        [Fact]
        public void Properties_SetAndGet_ReturnsValues()
        {
            var element = new TestHttpTransportElement();
            element.AuthenticationScheme = AuthenticationSchemes.Basic;
            element.KeepAliveEnabled = true;
            element.MaxBufferSize = 1234;
            element.Realm = "realm";
            element.TransferMode = SMTransferMode.Streamed;
            Assert.Equal(AuthenticationSchemes.Basic, element.AuthenticationScheme);
            Assert.True(element.KeepAliveEnabled);
            Assert.Equal(1234, element.MaxBufferSize);
            Assert.Equal("realm", element.Realm);
            Assert.Equal(SMTransferMode.Streamed, element.TransferMode);
        }

        [Fact]
        public void BindingElementType_IsHttpTransportBindingElement()
        {
            var element = new TestHttpTransportElement();
            Assert.Equal(typeof(HttpTransportBindingElement), element.BindingElementType);
        }

        [Fact]
        public void CreateDefaultBindingElement_ReturnsHttpTransportBindingElement()
        {
            var element = new TestHttpTransportElement();
            var bindingElement = element.CreateDefault();
            Assert.IsType<HttpTransportBindingElement>(bindingElement);
        }

        [Fact]
        public void Realm_SetNullOrEmpty_NormalizesToEmptyString()
        {
            var element = new TestHttpTransportElement();
            element.Realm = null;
            Assert.Equal(string.Empty, element.Realm);
            element.Realm = "";
            Assert.Equal(string.Empty, element.Realm);
        }

        [Fact]
        public void ApplyConfiguration_SetsProperties_AndHonorsMaxBufferSizeDefaultOrigin()
        {
            var element = new TestHttpTransportElement();
            // Set some values
            element.AuthenticationScheme = AuthenticationSchemes.Ntlm;
            element.KeepAliveEnabled = false;
            element.TransferMode = SMTransferMode.Buffered;
            element.MaxBufferSize = TransportDefaults.MaxBufferSize; // keep default value

            var binding = new HttpTransportBindingElement();
            element.ApplyConfiguration(binding);

            Assert.Equal(AuthenticationSchemes.Ntlm, binding.AuthenticationScheme);
            Assert.False(binding.KeepAliveEnabled);
            Assert.Equal(SMTransferMode.Buffered, binding.TransferMode);
            // Because MaxBufferSize value origin is Default, ApplyConfiguration should NOT set it explicitly
            // so binding should retain its default value
            Assert.Equal(TransportDefaults.MaxBufferSize, binding.MaxBufferSize);
        }

        [Fact]
        public void ApplyConfiguration_SetsMaxBufferSize_WhenNonDefault()
        {
            var element = new TestHttpTransportElement();
            element.MaxBufferSize = 9999; // non-default triggers assignment
            var binding = new HttpTransportBindingElement();
            element.ApplyConfiguration(binding);
            Assert.Equal(9999, binding.MaxBufferSize);
        }

        [Fact]
        public void CopyFrom_CopiesAllSupportedProperties()
        {
            var source = new TestHttpTransportElement
            {
                AuthenticationScheme = AuthenticationSchemes.Digest,
                KeepAliveEnabled = false,
                MaxBufferSize = 4321,
                Realm = "sourceRealm",
                TransferMode = SMTransferMode.Streamed
            };

            var target = new TestHttpTransportElement();
            target.CopyFrom(source);

            Assert.Equal(AuthenticationSchemes.Digest, target.AuthenticationScheme);
            Assert.False(target.KeepAliveEnabled);
            Assert.Equal(4321, target.MaxBufferSize);
            Assert.Equal("sourceRealm", target.Realm);
            Assert.Equal(SMTransferMode.Streamed, target.TransferMode);
        }

        [Fact]
        public void InitializeFrom_SetsPropertiesWhenDifferentFromDefaults()
        {            
            var binding = new HttpTransportBindingElement
            {
                AuthenticationScheme = AuthenticationSchemes.Basic,
                KeepAliveEnabled = false,
                MaxBufferSize = TransportDefaults.MaxBufferSize + 1,
                TransferMode = SMTransferMode.Streamed
            };
            binding.ExtendedProtectionPolicy = new ExtendedProtectionPolicy(PolicyEnforcement.Always);

            Assert.NotNull(binding.ExtendedProtectionPolicy);

            var element = new TestHttpTransportElement();
            // Ensure ExtendedProtectionPolicy is instantiated to avoid NullReference during InitializeFrom
            Assert.NotNull(element.ExtendedProtectionPolicy);

            element.InitializeFrom(binding);

            Assert.Equal(AuthenticationSchemes.Basic, element.AuthenticationScheme);
            Assert.False(element.KeepAliveEnabled);
            Assert.Equal(TransportDefaults.MaxBufferSize + 1, element.MaxBufferSize);
            Assert.Equal(SMTransferMode.Streamed, element.TransferMode);
        }

        [Fact]
        public void ExtendedProtectionPolicy_ApplyCopyInitialize_Mapped()
        {
            var element = new TestHttpTransportElement();

            // Ensure ExtendedProtectionPolicy exists
            Assert.NotNull(element.ExtendedProtectionPolicy);

            // Set a policy on binding and initialize into element
            var binding = new HttpTransportBindingElement();
            binding.ExtendedProtectionPolicy = ConfigurationChannelBindingUtility.BuildPolicy(element.ExtendedProtectionPolicy);
            element.InitializeFrom(binding);

            // Apply configuration to a new binding and ensure policy transferred
            var binding2 = new HttpTransportBindingElement();
            element.ApplyConfiguration(binding2);
            Assert.NotNull(binding2.ExtendedProtectionPolicy);
        }

        private class TestHttpTransportElement : HttpTransportElement 
        { 
            public TransportBindingElement CreateDefault()
            {
                return base.CreateDefaultBindingElement();
            }
        }
    }
}
