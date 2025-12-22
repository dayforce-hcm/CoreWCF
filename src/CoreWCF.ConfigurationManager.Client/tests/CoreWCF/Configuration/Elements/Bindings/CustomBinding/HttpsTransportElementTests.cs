using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System.ServiceModel.Channels;
using System.Security.Authentication.ExtendedProtection;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings
{
    public class HttpsTransportElementTests
    {
        [Fact]
        public void CanInstantiateHttpsTransportElement()
        {
            var element = new HttpsTransportElement();
            Assert.NotNull(element);
        }

        [Fact]
        public void RequireClientCertificate_DefaultIsFalse()
        {
            var element = new HttpsTransportElement();
            Assert.False(element.RequireClientCertificate);
        }

        [Fact]
        public void BindingElementType_IsHttpsTransportBindingElement()
        {
            var element = new HttpsTransportElement();
            Assert.Equal(typeof(HttpsTransportBindingElement), element.BindingElementType);
        }

        [Fact]
        public void CreateDefaultBindingElement_ReturnsHttpsTransportBindingElement()
        {
            var element = new TestHttpsTransportElement();
            var bindingElement = element.CreateDefault();
            Assert.IsType<HttpsTransportBindingElement>(bindingElement);
        }

        [Fact]
        public void ApplyConfiguration_SetsRequireClientCertificate()
        {
            var element = new HttpsTransportElement
            {
                RequireClientCertificate = true
            };
            var binding = new HttpsTransportBindingElement();
            element.ApplyConfiguration(binding);
            Assert.True(binding.RequireClientCertificate);
        }

        [Fact]
        public void CopyFrom_CopiesRequireClientCertificate()
        {
            var source = new HttpsTransportElement
            {
                RequireClientCertificate = true
            };

            var target = new HttpsTransportElement();
            target.CopyFrom(source);

            Assert.True(target.RequireClientCertificate);
        }

        [Fact]
        public void InitializeFrom_SetsRequireClientCertificate_WhenNonDefault()
        {
            var binding = new HttpsTransportBindingElement
            {
                RequireClientCertificate = true
            };

            // Ensure binding has a non-default ExtendedProtectionPolicy and element has a destination instance
            binding.ExtendedProtectionPolicy = new ExtendedProtectionPolicy(PolicyEnforcement.Always);

            var element = new HttpsTransportElement();
            Assert.NotNull(element.ExtendedProtectionPolicy);

            element.InitializeFrom(binding);

            Assert.True(element.RequireClientCertificate);
        }

        private class TestHttpsTransportElement : HttpsTransportElement
        {
            public TransportBindingElement CreateDefault()
            {
                return base.CreateDefaultBindingElement();
            }
        }
    }
}
