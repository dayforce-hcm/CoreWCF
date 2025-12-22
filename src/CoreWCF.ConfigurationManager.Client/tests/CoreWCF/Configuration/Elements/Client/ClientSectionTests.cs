using Xunit;
using CoreWCF.ConfigurationManager.Client;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Client
{
    public class ClientSectionTests
    {

        [Fact]
        public void ClientEndpoints_IsAccessibleAndNotNullByDefault()
        {
            var section = new ClientSection();
            Assert.NotNull(section.ClientEndpoints);
            Assert.False(section.ClientEndpoints.ElementInformation.IsPresent);
        }
    }
}
