using CoreWCF.Configuration;
using CoreWCF.ConfigurationManager.Client;
using Xunit;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.ExtensionElements
{
    public class DummyExtensionElement3 : ServiceModelExtensionElement { }

    public class DummyNamedCollection : NamedServiceModelExtensionCollectionElement<DummyExtensionElement3>
    {
        public DummyNamedCollection(string collectionName, string name) : base(collectionName, name) { }
    }

    public class NamedServiceModelExtensionCollectionElementTests
    {
        [Fact]
        public void NameProperty_SetsAndGets()
        {
            var collection = new DummyNamedCollection("dummyCollection", "dummyName");
            Assert.Equal("dummyName", collection.Name);
            collection.Name = "newName";
            Assert.Equal("newName", collection.Name);
        }
    }
}
