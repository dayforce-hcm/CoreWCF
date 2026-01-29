using System.Linq;
using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System;
using CoreWCF.Configuration;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings.Properties.Security
{
    public class ClaimTypeElementCollectionTests
    {
        [Fact]
        public void Add_AddsElementToCollection()
        {
            var collection = new ClaimTypeElementCollection();
            var element = new ClaimTypeElement("type", true);
            collection.Add(element);
            Assert.Contains(element, collection.OfType<ClaimTypeElement>());
        }

        [Fact]
        public void GetElementKey_ReturnsClaimType()
        {
            var collection = new ClaimTypeElementCollection();
            var element = new ClaimTypeElement("type", true);
            var key = collection.GetType().GetMethod("GetElementKey", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(collection, new object[] { element });
            Assert.Equal("type", key);
        }

        [Fact]
        public void GetElementKey_ThrowsOnNull()
        {
            var collection = new ClaimTypeElementCollection();
            var ex = Assert.Throws<System.Reflection.TargetInvocationException>(() =>
                collection.GetType().GetMethod("GetElementKey", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(collection, new object[] { null })
            );
            Assert.IsType<ArgumentNullException>(ex.InnerException);
        }
    }
}
