using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System.Security.Authentication.ExtendedProtection;
using System.Collections.Generic;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings.Properties.Security
{
    public class ExtendedProtectionPolicyElementTests
    {
        [Fact]
        public void Default_Properties_AreSet()
        {
            var element = new ExtendedProtectionPolicyElement();
            Assert.Equal(PolicyEnforcement.Never, element.PolicyEnforcement);
            Assert.Equal(ProtectionScenario.TransportSelected, element.ProtectionScenario);
            Assert.NotNull(element.CustomServiceNames);
        }

        [Fact]
        public void BuildPolicy_Never_ReturnsPolicyWithNever()
        {
            var element = new ExtendedProtectionPolicyElement();
            element.PolicyEnforcement = PolicyEnforcement.Never;
            var policy = element.BuildPolicy();
            Assert.Equal(PolicyEnforcement.Never, policy.PolicyEnforcement);
        }
    }
}
