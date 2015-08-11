using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Shipping.ShipSense.Hashing;

namespace ShipWorks.Tests.Shipping.ShipSense.Hashing
{
    public class ShipSenseOrderItemKeyTest
    {
        private ShipSenseOrderItemKey testObject;
        
        [Fact]
        public void IsValid_ReturnsFalse_WhenAllEntriesAreBlank_Test()
        {
            testObject = new ShipSenseOrderItemKey { Quantity = 1 };
            testObject.Add("SKU", string.Empty);
            testObject.Add("Code", string.Empty);

            Assert.IsFalse(testObject.IsValid());
        }

        [Fact]
        public void IsValid_ReturnsFalse_WhenDuplicateKeyIsAdded_AndAllEntriesAreBlank_Test()
        {
            testObject = new ShipSenseOrderItemKey { Quantity = 1 };
            testObject.Add("SKU", string.Empty);
            testObject.Add("Code", string.Empty);
            testObject.Add("Code", string.Empty);

            Assert.IsFalse(testObject.IsValid());
        }

        [Fact]
        public void IsValid_ReturnsFalse_WhenNoEntriesHaveBeenAdded_Test()
        {
            testObject = new ShipSenseOrderItemKey { Quantity = 1 };

            Assert.IsFalse(testObject.IsValid());
        }

        [Fact]
        public void IsValid_ReturnsTrue_WhenOneEntryValueIsNotBlank_Test()
        {
            testObject = new ShipSenseOrderItemKey { Quantity = 1 };
            testObject.Add("SKU", string.Empty);
            testObject.Add("Code", "123");

            Assert.IsTrue(testObject.IsValid());
        }

        [Fact]
        public void KeyValue_ContainsAllIdentifierValues_Test()
        {
            testObject = new ShipSenseOrderItemKey { Quantity = 1 };
            testObject.Add("SKU", "ABC");
            testObject.Add("Code", "123");

            string key = testObject.KeyValue;

            Assert.IsTrue(key.Contains("[SKU,ABC]|[Code,123]"));
        }

        [Fact]
        public void KeyValue_ContainsAllIdentifierValues_WhenOneKeyHasBlankValue_Test()
        {
            testObject = new ShipSenseOrderItemKey { Quantity = 1 };
            testObject.Add("SKU", "");
            testObject.Add("Code", "123");

            string key = testObject.KeyValue;

            Assert.IsTrue(key.Contains("[SKU,]|[Code,123]"));
        }

        [Fact]
        public void KeyValue_IsDifferent_BetweenTwoItems_AndEachItemIsMissingABlankValue_ButOtherValuesAreIdentical_Test()
        {
            // Proving that there are not any collisions when order items have a missing value for one field, but 
            // otherwise have identical values. Basically to prove that the scenario produces different keys:
            // Say  item 1 has a missing SKU but the item code is 123 and item 2 (a separate item) has a 
            // missing code but a SKU of 123. This should result in two different the keys will accurately 
            // reflect the items: [SKU,123]|[Code,] for item 1 and [SKU,]|[Code,123] for item 2

            testObject = new ShipSenseOrderItemKey { Quantity = 2.5 };
            testObject.Add("SKU", "123");
            testObject.Add("Code", "");
            testObject.Add("Location", "XYZ");

            string firstKey = testObject.KeyValue;

            testObject = new ShipSenseOrderItemKey { Quantity = 2.5 };
            testObject.Add("SKU", "");
            testObject.Add("Code", "123");
            testObject.Add("Location", "XYZ");

            string secondKey = testObject.KeyValue;

            Assert.AreNotEqual(firstKey, secondKey);
        }
    }
}
