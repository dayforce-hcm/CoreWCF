using System;
using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System.Configuration;
using System.Reflection;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Client
{
    public class ClientElementCollectionTests
    {
        [Fact]
        public void Collection_IsInstantiableAndEmptyByDefault()
        {
            var collection = new ClientElementCollection();
            Assert.NotNull(collection);
            Assert.IsAssignableFrom<ConfigurationElementCollection>(collection);
            Assert.Empty(collection);
        }

        [Fact]
        public void CreateNewElement_ReturnsClientElement()
        {
            var collection = new ClientElementCollection();
            var mi = typeof(ClientElementCollection).GetMethod(
                "CreateNewElement",
                BindingFlags.Instance | BindingFlags.NonPublic,
                binder: null,
                types: Type.EmptyTypes,
                modifiers: null);
            Assert.NotNull(mi);
            var result = mi.Invoke(collection, null);
            Assert.IsType<ClientElement>(result);
        }

        [Fact]
        public void GetElementKey_Null_Throws()
        {
            var collection = new ClientElementCollection();
            var mi = typeof(ClientElementCollection).GetMethod("GetElementKey", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(mi);
            Assert.Throws<TargetInvocationException>(() => mi.Invoke(collection, new object[] { null }));
            try
            {
                mi.Invoke(collection, new object[] { null });
            }
            catch (TargetInvocationException ex)
            {
                Assert.IsType<ArgumentNullException>(ex.InnerException);
            }
        }

        [Fact]
        public void GetElementKey_ReturnsClientElementItself()
        {
            var collection = new ClientElementCollection();
            var mi = typeof(ClientElementCollection).GetMethod("GetElementKey", BindingFlags.Instance | BindingFlags.NonPublic);
            var element = new ClientElement();
            var key = mi.Invoke(collection, new object[] { element });
            Assert.Same(element, key);
        }
    }
}
