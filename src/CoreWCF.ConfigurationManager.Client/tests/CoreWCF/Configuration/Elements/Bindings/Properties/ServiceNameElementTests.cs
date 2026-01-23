using Xunit;
using CoreWCF.ConfigurationManager.Client;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings.Properties
{
    public class ServiceNameElementTests
    {
        [Fact]
        public void Name_SetAndGet_ReturnsValue()
        {
            var element = new ServiceNameElement();
            element.Name = "TestName";
            Assert.Equal("TestName", element.Name);
            Assert.Equal("TestName", element.Key);
        }
    }
}
