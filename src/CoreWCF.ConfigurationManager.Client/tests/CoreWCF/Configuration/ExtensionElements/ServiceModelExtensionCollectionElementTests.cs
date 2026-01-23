using CoreWCF.ConfigurationManager.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using Xunit;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.ExtensionElements
{
    public class DummyExtensionElement1 : ServiceModelExtensionElement
    {
        public DummyExtensionElement1() { ConfigurationElementName = "dummy"; }
    }

    // Use a distinct name to avoid collision with DummyExtensionElement2 in ServiceModelExtensionElementTests
    public class ConflictingDummyExtensionElement : ServiceModelExtensionElement
    {
        public ConflictingDummyExtensionElement() { ConfigurationElementName = "conflict"; }
    }

    public class DummyCollection : ServiceModelExtensionCollectionElement<DummyExtensionElement1>
    {
        public DummyCollection(string name) : base(name) { }
        public void CallSetReadOnly() => base.SetReadOnly();
        public void CallSetIsModified() => base.SetIsModified();
        public bool CallIsModified() => base.IsModified();
        public void CallResetModified() => base.ResetModified();
    }

    public class ServiceModelExtensionCollectionElementTests
    {
        [Fact]
        public void Indexer_ByType_ReturnsElement()
        {
            var collection = new DummyCollection("dummyCollection");
            var element = new DummyExtensionElement1();
            collection.Add(element);
            var fetched = collection[typeof(DummyExtensionElement1)];
            Assert.Same(element, fetched);
        }

        [Fact]
        public void Indexer_ByType_NullType_Throws()
        {
            var collection = new DummyCollection("dummyCollection");
            Assert.Throws<ArgumentNullException>(() => { var _ = collection[(Type)null]; });
        }

        [Fact]
        public void Indexer_ByType_InvalidType_Throws()
        {
            var collection = new DummyCollection("dummyCollection");
            Assert.Throws<ArgumentException>(() => { var _ = collection[typeof(ConflictingDummyExtensionElement)]; });
        }

        [Fact]
        public void Contains_Null_Throws()
        {
            var collection = new DummyCollection("dummyCollection");
            Assert.Throws<ArgumentNullException>(() => collection.Contains(null));
        }

        [Fact]
        public void ContainsKey_NullType_Throws()
        {
            var collection = new DummyCollection("dummyCollection");
            Assert.Throws<ArgumentNullException>(() => collection.ContainsKey((Type)null));
        }

        [Fact]
        public void ContainsKey_EmptyName_Throws()
        {
            var collection = new DummyCollection("dummyCollection");
            Assert.Throws<ArgumentNullException>(() => collection.ContainsKey(""));
        }

        [Fact]
        public void Add_ReadOnly_Throws()
        {
            var collection = new DummyCollection("dummyCollection");
            collection.CallSetReadOnly();
            Assert.Throws<ConfigurationErrorsException>(() => collection.Add(new DummyExtensionElement1()));
        }

        [Fact]
        public void Clear_ReadOnly_Throws()
        {
            var collection = new DummyCollection("dummyCollection");
            collection.CallSetReadOnly();
            Assert.Throws<ConfigurationErrorsException>(() => collection.Clear());
        }

        [Fact]
        public void Remove_Null_Throws()
        {
            var collection = new DummyCollection("dummyCollection");
            Assert.Throws<ArgumentNullException>(() => collection.Remove(null));
        }

        [Fact]
        public void ResetModified_ClearsFlag()
        {
            var collection = new DummyCollection("dummyCollection");
            collection.CallSetIsModified();
            var element = new DummyExtensionElement1();
            collection.AddItem(element);
            collection.CallResetModified();
            Assert.False(collection.CallIsModified());
        }

        [Fact]
        public void AddAndContains_Works()
        {
            var collection = new DummyCollection("dummyCollection");
            var element = new DummyExtensionElement1();
            collection.Add(element);
            Assert.Contains(element, collection);
        }

        [Fact]
        public void AddItem_Works()
        {
            var collection = new DummyCollection("dummyCollection");
            var element = new DummyExtensionElement1();
            collection.AddItem(element);
            Assert.Contains(element, collection);
        }

        [Fact]
        public void CanAdd_ReturnsFalseForDuplicate()
        {
            var collection = new DummyCollection("dummyCollection");
            var element = new DummyExtensionElement1();
            collection.Add(element);
            Assert.False(collection.CanAdd(element));
        }

        [Fact]
        public void ContainsKey_ByTypeAndName()
        {
            var collection = new DummyCollection("dummyCollection");
            var element = new DummyExtensionElement1();
            collection.Add(element);
            Assert.True(collection.ContainsKey(typeof(DummyExtensionElement1)));
            Assert.True(collection.ContainsKey("dummy"));
        }

        [Fact]
        public void CollectionElementBaseType_IsCorrect()
        {
            var collection = new DummyCollection("dummyCollection");
            Assert.Equal(typeof(DummyExtensionElement1), collection.CollectionElementBaseType);
        }

        [Fact]
        public void Add_Duplicate_Throws()
        {
            var collection = new DummyCollection("dummyCollection");
            var element = new DummyExtensionElement1();
            collection.Add(element);
            Assert.Throws<ArgumentException>(() => collection.Add(element));
        }

        [Fact]
        public void Add_Null_Throws()
        {
            var collection = new DummyCollection("dummyCollection");
            Assert.Throws<ArgumentNullException>(() => collection.Add(null));
        }

        [Fact]
        public void Remove_RemovesElement()
        {
            var collection = new DummyCollection("dummyCollection");
            var element = new DummyExtensionElement1();
            collection.Add(element);
            var removed = collection.Remove(element);
            Assert.True(removed);
            Assert.Empty(collection);
        }

        [Fact]
        public void Clear_RemovesAll()
        {
            var collection = new DummyCollection("dummyCollection");
            collection.Add(new DummyExtensionElement1());
            collection.Clear();
            Assert.Empty(collection);
        }

        [Fact]
        public void Count_And_Items_Work()
        {
            var collection = new DummyCollection("dummyCollection");
            Assert.Empty(collection);
            Assert.Empty(collection.Items);
            var element = new DummyExtensionElement1();
            collection.Add(element);
            Assert.Single(collection);
            Assert.Single(collection.Items);
        }

        [Fact]
        public void IsReadOnly_ExplicitInterface_IsFalseByDefault()
        {
            var collection = new DummyCollection("dummyCollection");
            var isReadOnly = ((ICollection<DummyExtensionElement1>)collection).IsReadOnly;
            Assert.False(isReadOnly);
        }

        [Fact]
        public void SetReadOnly_MakesCollectionReadOnly()
        {
            var collection = new DummyCollection("dummyCollection");
            collection.CallSetReadOnly();
            var isReadOnly = ((ICollection<DummyExtensionElement1>)collection).IsReadOnly;
            Assert.True(isReadOnly);
        }
    }
}
