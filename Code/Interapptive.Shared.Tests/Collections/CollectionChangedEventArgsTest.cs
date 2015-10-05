using Interapptive.Shared.Collections;
using System;
using Xunit;

namespace Interapptive.Shared.Tests.Collections
{
    public class CollectionChangedEventArgsTest
    {
        [Fact]
        public void Constructor_ThrowsArgumentException_WhenBothOldAndNewItemsAreNull()
        {
            Assert.Throws<ArgumentException>(() => new CollectionChangedEventArgs<string>(null, null, 0));
        }

        [Fact]
        public void Constructor_SetsNewItem_WhenNewItemIsNotNull()
        {
            var testObject = new CollectionChangedEventArgs<string>("Foo", null, 0);
            Assert.Equal("Foo", testObject.NewItem);
        }

        [Fact]
        public void Constructor_SetsOldItem_WhenOldItemIsNotNull()
        {
            var testObject = new CollectionChangedEventArgs<string>(null, "Foo", 0);
            Assert.Equal("Foo", testObject.OldItem);
        }

        [Fact]
        public void Constructor_SetsBothItems_WhenNeitherItemsAreNull()
        {
            var testObject = new CollectionChangedEventArgs<string>("Bar", "Foo", 0);
            Assert.Equal("Bar", testObject.NewItem);
            Assert.Equal("Foo", testObject.OldItem);
        }
    }
}
