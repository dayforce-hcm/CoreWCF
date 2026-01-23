using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings
{
    public class BindingNotFoundExceptionTests
    {
        [Fact]
        public void CanInstantiate_DefaultConstructor()
        {
            var ex = new BindingNotFoundException();
            Assert.NotNull(ex);
        }

        [Fact]
        public void CanInstantiate_WithMessage()
        {
            var ex = new BindingNotFoundException("msg");
            Assert.Equal("msg", ex.Message);
        }

        [Fact]
        public void CanInstantiate_WithMessageAndInnerException()
        {
            var inner = new Exception("inner");
            var ex = new BindingNotFoundException("msg", inner);
            Assert.Equal("msg", ex.Message);
            Assert.Equal(inner, ex.InnerException);
        }
    }
}
