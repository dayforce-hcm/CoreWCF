using CoreWCF.ConfigurationManager.Client;
using Xunit;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.ExtensionElements
{
    public class ExtensionsSectionTests
    {
        [Fact]
        public void BindingElementExtensions_Initialized()
        {
            var section = new ExtensionsSection();
            Assert.NotNull(section.BindingElementExtensions);
        }

        [Fact]
        public void BindingExtensions_Initialized()
        {
            var section = new ExtensionsSection();
            Assert.NotNull(section.BindingExtensions);
        }
    }
}
