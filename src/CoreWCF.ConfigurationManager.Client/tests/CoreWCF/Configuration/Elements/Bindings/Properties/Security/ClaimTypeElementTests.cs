using Xunit;
using CoreWCF.ConfigurationManager.Client;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings.Properties.Security
{
    public class ClaimTypeElementTests
    {
        [Fact]
        public void Constructor_SetsProperties()
        {
            var claimType = "type";
            var isOptional = true;
            var element = new ClaimTypeElement(claimType, isOptional);
            Assert.Equal(claimType, element.ClaimType);
            Assert.Equal(isOptional, element.IsOptional);
        }

        [Fact]
        public void ClaimType_SetNullOrEmpty_SetsEmptyString()
        {
            var element = new ClaimTypeElement();
            element.ClaimType = null;
            Assert.Equal(string.Empty, element.ClaimType);
            element.ClaimType = "";
            Assert.Equal(string.Empty, element.ClaimType);
        }

        [Fact]
        public void IsOptional_DefaultIsFalse()
        {
            var element = new ClaimTypeElement();
            Assert.False(element.IsOptional);
        }
    }
}
