using System;
using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System.Configuration;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings
{
    public class BindingsSectionTests
    {
        [Fact]
        public void Properties_AreAccessibleAndNotNullByDefault()
        {
            var section = new BindingsSection();
            Assert.NotNull(section.BasicHttpBinding);
            Assert.False(section.BasicHttpBinding.ElementInformation.IsPresent);

            Assert.NotNull(section.WebHttpBinding);
            Assert.False(section.WebHttpBinding.ElementInformation.IsPresent);

            Assert.NotNull(section.CustomBinding);
            Assert.False(section.CustomBinding.ElementInformation.IsPresent);
        }
    }
}
