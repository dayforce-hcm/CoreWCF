using System;
using System.Configuration;
using Xunit;
using CoreWCF.ConfigurationManager.Client;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings
{
    public class ServiceModelSectionGroupTests
    {
        [Fact]
        public void Constructor_CreatesInstance()
        {
            var group = new ServiceModelSectionGroup();
            Assert.NotNull(group);
        }

        [Fact]
        public void GetSectionGroup_ThrowsOnNullConfig()
        {
            Assert.Throws<ArgumentNullException>(() => ServiceModelSectionGroup.GetSectionGroup(null));
        }

        // Note: Bindings and Client properties require a real config, which is not trivial to mock for unit tests.
        // These can be covered in integration tests if needed.
    }
}
