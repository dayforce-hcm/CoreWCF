using System.Configuration;
using CoreWCF.ConfigurationManager.Client;
using Xunit;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.ExtensionElements
{
    public class ExtensionElementCollectionTests
    {
        [Fact]
        public void AddAndGetElementExtension_Works()
        {
            var collection = new ExtensionElementCollection();
            var ext = new ExtensionElement("name", "type,assembly");
            collection.Add(ext);
            var fetched = collection.GetElementExtension("name");
            Assert.Equal(ext, fetched);
        }

        [Fact]
        public void ContainsKey_ReturnsTrueForExistingKey()
        {
            var collection = new ExtensionElementCollection();
            var ext = new ExtensionElement("name", "type,assembly");
            collection.Add(ext);
            Assert.True(collection.ContainsKey("name"));
        }

        [Fact]
        public void Add_DuplicateName_Throws()
        {
            var collection = new ExtensionElementCollection();
            collection.Add(new ExtensionElement("name", "type1,assembly"));
            Assert.Throws<ConfigurationErrorsException>(() => collection.Add(new ExtensionElement("name", "type2,assembly")));
        }
    }
}
