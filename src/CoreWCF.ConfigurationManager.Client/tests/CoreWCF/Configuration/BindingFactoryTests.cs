using CoreWCF.Configuration;
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
        [InlineData("basicHttpBinding", typeof(System.ServiceModel.BasicHttpBinding))]
        [InlineData("netTcpBinding", typeof(System.ServiceModel.NetTcpBinding))]
        [InlineData("wsHttpBinding", typeof(System.ServiceModel.WSHttpBinding))]
        [InlineData("netHttpBinding", typeof(System.ServiceModel.NetHttpBinding))]
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
