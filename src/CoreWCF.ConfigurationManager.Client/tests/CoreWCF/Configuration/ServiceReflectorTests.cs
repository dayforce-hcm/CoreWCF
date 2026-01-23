using System;
using CoreWCF.ConfigurationManager.Client;
using Xunit;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration
{
    public class ServiceReflectorTests
    {
        [Fact]
        public void ResolveTypeFromName_ReturnsType()
        {
            var type = ServiceReflector.ResolveTypeFromName(typeof(string).FullName);
            Assert.Equal(typeof(string), type);
        }
    }
}
