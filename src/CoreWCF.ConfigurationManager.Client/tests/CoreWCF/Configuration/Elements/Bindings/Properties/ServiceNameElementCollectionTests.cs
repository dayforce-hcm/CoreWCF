using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System;
using System.Reflection;
using CoreWCF.Configuration;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings.Properties
{
    public class ServiceNameElementCollectionTests
    {
        [Fact]
        public void AddAndGetByIndex_WorksCorrectly()
        {
            var collection = new ServiceNameElementCollection();
            var element = new ServiceNameElement { Name = "A" };
            collection.Add(element);
            Assert.Equal(element, collection[0]);
        }

        [Fact]
        public void AddAndGetByName_WorksCorrectly()
        {
            var collection = new ServiceNameElementCollection();
            var element = new ServiceNameElement { Name = "B" };
            collection.Add(element);
            Assert.Equal(element, collection["B"]);
        }

        [Fact]
        public void IndexerSetByIndex_ReplacesExisting()
        {
            var collection = new ServiceNameElementCollection();
            var first = new ServiceNameElement { Name = "C1" };
            var second = new ServiceNameElement { Name = "C2" };
            collection.Add(first);
            collection[0] = second;
            Assert.Equal(second, collection[0]);
            Assert.Null(collection["C1"]);
        }

        [Fact]
        public void IndexerSetByName_ReplacesExisting()
        {
            var collection = new ServiceNameElementCollection();
            var first = new ServiceNameElement { Name = "D" };
            var second = new ServiceNameElement { Name = "D" };
            collection.Add(first);
            collection["D"] = second;
            Assert.Equal(second, collection["D"]);
        }

        [Fact]
        public void IndexOf_ReturnsExpectedIndex()
        {
            var collection = new ServiceNameElementCollection();
            var e1 = new ServiceNameElement { Name = "E1" };
            var e2 = new ServiceNameElement { Name = "E2" };
            collection.Add(e1);
            collection.Add(e2);
            Assert.Equal(0, collection.IndexOf(e1));
            Assert.Equal(1, collection.IndexOf(e2));
        }

        [Fact]
        public void Remove_ByElement_Removes()
        {
            var collection = new ServiceNameElementCollection();
            var element = new ServiceNameElement { Name = "F" };
            collection.Add(element);
            collection.Remove(element);
            Assert.Null(collection["F"]);
        }

        [Fact]
        public void Remove_NullElement_Throws()
        {
            var collection = new ServiceNameElementCollection();
            Assert.Throws<ArgumentNullException>(() => collection.Remove((ServiceNameElement)null));
        }

        [Fact]
        public void Remove_RemovesElement()
        {
            var collection = new ServiceNameElementCollection();
            var element = new ServiceNameElement { Name = "C" };
            collection.Add(element);
            collection.Remove("C");
            Assert.Null(collection["C"]);
        }

        [Fact]
        public void Remove_Nonexistent_DoesNotThrow()
        {
            var collection = new ServiceNameElementCollection();
            collection.Remove("does-not-exist");
            Assert.Empty(collection);
        }

        [Fact]
        public void RemoveAt_RemovesAtIndex()
        {
            var collection = new ServiceNameElementCollection();
            var e1 = new ServiceNameElement { Name = "G1" };
            var e2 = new ServiceNameElement { Name = "G2" };
            collection.Add(e1);
            collection.Add(e2);
            collection.RemoveAt(0);
            Assert.Single(collection);
            Assert.Equal(e2, collection[0]);
        }

        [Fact]
        public void Clear_RemovesAllElements()
        {
            var collection = new ServiceNameElementCollection();
            collection.Add(new ServiceNameElement { Name = "D" });
            collection.Clear();
            Assert.Empty(collection);
        }

        [Fact]
        public void GetElementKey_Null_Throws()
        {
            var collection = new ServiceNameElementCollection();
            var method = typeof(ServiceNameElementCollection).GetMethod("GetElementKey", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(method);
            Assert.Throws<TargetInvocationException>(() => method.Invoke(collection, new object[] { null }));
            // Unwrap to assert inner exception type explicitly
            try
            {
                method.Invoke(collection, new object[] { null });
            }
            catch (TargetInvocationException ex)
            {
                Assert.IsType<ArgumentNullException>(ex.InnerException);
            }
        }

        [Fact]
        public void CreateNewElement_ReturnsServiceNameElement()
        {
            var collection = new ServiceNameElementCollection();
            var method = typeof(ServiceNameElementCollection).GetMethod(
                "CreateNewElement",
                BindingFlags.Instance | BindingFlags.NonPublic,
                binder: null,
                types: Type.EmptyTypes,
                modifiers: null);
            Assert.NotNull(method);
            var created = method.Invoke(collection, null);
            Assert.IsType<ServiceNameElement>(created);
        }
    }
}
