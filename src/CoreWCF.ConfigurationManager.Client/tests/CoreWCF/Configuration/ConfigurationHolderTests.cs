using System;
using System.ServiceModel.Channels;
using CoreWCF.ConfigurationManager.Client;
using Xunit;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration
{
    public class ConfigurationHolderTests
    {
        [Fact]
        public void AddBinding_StoresBinding()
        {
            var holder = new ConfigurationHolder(new DummyProvider(), new DummyFactory());
            var binding = new CustomBinding();
            holder.AddBinding(binding);
            var resolved = holder.ResolveBinding("customBinding", binding.Name);
            Assert.Equal(binding, resolved);
        }

        private class DummyProvider : IServiceProvider { public object GetService(Type t) => null; }
        private class DummyFactory : IBindingFactory { public Binding Create(string t) => new CustomBinding(); }
    }
}
