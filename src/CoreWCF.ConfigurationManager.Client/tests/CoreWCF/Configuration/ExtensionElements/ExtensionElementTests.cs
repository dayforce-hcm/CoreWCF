using System;
using CoreWCF.ConfigurationManager.Client;
using Xunit;
using System.Reflection;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.ExtensionElements
{
    public class ExtensionElementTests
    {
        [Fact]
        public void Constructor_NameAndType_SetsProperties()
        {
            var ext = new ExtensionElement("myName", "myType,MyAssembly");
            Assert.Equal("myName", ext.Name);
            Assert.Equal("myType,MyAssembly", ext.Type);
            Assert.Equal("myType", ext.TypeName);
        }

        [Fact]
        public void Constructor_Name_ThrowsOnNullOrEmpty()
        {
            Assert.Throws<ArgumentNullException>(() => new ExtensionElement(null));
            Assert.Throws<ArgumentNullException>(() => new ExtensionElement(""));
        }

        [Fact]
        public void Constructor_NameAndType_ThrowsOnNullOrEmptyType()
        {
            Assert.Throws<ArgumentNullException>(() => new ExtensionElement("name", null));
            Assert.Throws<ArgumentNullException>(() => new ExtensionElement("name", ""));
        }

        [Fact]
        public void GetTypeName_ReturnsTypeName()
        {
            var typeName = ExtensionElement.GetTypeName("MyType,MyAssembly");
            Assert.Equal("MyType", typeName);
        }

        [Fact]
        public void GetTypeName_HandlesWhitespace()
        {
            var typeName = ExtensionElement.GetTypeName("  MyType  ,  MyAssembly  ");
            Assert.Equal("MyType", typeName);
        }

        [Fact]
        public void GetTypeName_HandlesNoComma()
        {
            var typeName = ExtensionElement.GetTypeName("MyType");
            Assert.Equal("MyType", typeName);
        }

        [Fact]
        public void NameSetter_EmptyStringAllowed()
        {
            var ext = new ExtensionElement();
            ext.Name = "";
            Assert.Equal("", ext.Name);
        }

        [Fact]
        public void NameSetter_NullSetsEmpty()
        {
            var ext = new ExtensionElement();
            ext.Name = null;
            Assert.Equal("", ext.Name);
        }

        [Fact]
        public void TypeSetter_EmptyStringAllowed()
        {
            var ext = new ExtensionElement();
            ext.Type = "";
            Assert.Equal("", ext.Type);
        }

        [Fact]
        public void TypeSetter_NullSetsEmpty()
        {
            var ext = new ExtensionElement();
            ext.Type = null;
            Assert.Equal("", ext.Type);
        }

        [Fact]
        public void TypeName_CachesValue()
        {
            var ext = new ExtensionElement("name", "type,assembly");
            // Use reflection to set _typeName
            var field = typeof(ExtensionElement).GetField("_typeName", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(ext, "cachedType");
            Assert.Equal("cachedType", ext.TypeName);
        }
    }
}
