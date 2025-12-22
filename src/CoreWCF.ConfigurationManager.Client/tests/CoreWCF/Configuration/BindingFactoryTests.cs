using CoreWCF.ConfigurationManager.Client;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Xunit;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration
{
    public class BindingFactoryTests
    {
        private readonly BindingFactory _factory = new BindingFactory();

        [Theory]
        [InlineData("basicHttpBinding", typeof(BasicHttpBinding))]
        [InlineData("netTcpBinding", typeof(NetTcpBinding))]
        [InlineData("wsHttpBinding", typeof(WSHttpBinding))]
        [InlineData("netHttpBinding", typeof(NetHttpBinding))]
        [InlineData("customBinding", typeof(CustomBinding))]
        public void Create_ValidBindingType_ReturnsCorrectBinding(string bindingType, Type expectedType)
        {
            var binding = _factory.Create(bindingType);
            Assert.NotNull(binding);
            Assert.IsType(expectedType, binding);
        }

        [Fact]
        public void Create_NullOrEmptyBindingType_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _factory.Create(null));
            Assert.Throws<ArgumentNullException>(() => _factory.Create(string.Empty));
        }

        [Fact]
        public void Create_InvalidBindingType_ThrowsBindingNotFoundException()
        {
            Assert.Throws<BindingNotFoundException>(() => _factory.Create("invalidBinding"));
        }
    }
}
