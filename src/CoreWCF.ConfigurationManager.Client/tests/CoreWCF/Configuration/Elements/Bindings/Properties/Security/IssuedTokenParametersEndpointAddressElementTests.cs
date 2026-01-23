using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings.Properties.Security
{
    public class IssuedTokenParametersEndpointAddressElementTests
    {
        [Fact]
        public void Binding_SetNullOrEmpty_SetsEmptyString()
        {
            var element = new IssuedTokenParametersEndpointAddressElement();
            element.Binding = null;
            Assert.Equal(string.Empty, element.Binding);
            element.Binding = "";
            Assert.Equal(string.Empty, element.Binding);
        }

        [Fact]
        public void BindingConfiguration_SetNullOrEmpty_SetsEmptyString()
        {
            var element = new IssuedTokenParametersEndpointAddressElement();
            element.BindingConfiguration = null;
            Assert.Equal(string.Empty, element.BindingConfiguration);
            element.BindingConfiguration = "";
            Assert.Equal(string.Empty, element.BindingConfiguration);
        }

        [Fact]
        public void Copy_CopiesProperties()
        {
            var source = new IssuedTokenParametersEndpointAddressElement();
            source.Binding = "binding";
            source.BindingConfiguration = "config";
            var target = new IssuedTokenParametersEndpointAddressElement();
            target.Copy(source);
            Assert.Equal("binding", target.Binding);
            Assert.Equal("config", target.BindingConfiguration);
        }
    }
}
