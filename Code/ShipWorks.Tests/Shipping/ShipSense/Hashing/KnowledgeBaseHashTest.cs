﻿using System.Collections.Generic;
using System.Xml.Linq;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.ShipSense.Hashing;

namespace ShipWorks.Tests.Shipping.ShipSense.Hashing
{
    public class KnowledgebaseHashTest
    {
        private KnowledgebaseHash testObject;

        private OrderEntity order;
        private List<ShipSenseOrderItemKey> keys;
        
        public KnowledgebaseHashTest()
        {
            order = new OrderEntity();
            order.StoreID = 1006;

            keys = new List<ShipSenseOrderItemKey>();

            testObject = new KnowledgebaseHash();
        }

        [Fact]
        public void ComputeHash_ReturnsInvalidResult_WhenUniquenessKeyIsNotFound()
        {
            ShipSenseOrderItemKey invalidKey = new ShipSenseOrderItemKey { Quantity = 3 };
            keys = new List<ShipSenseOrderItemKey>
            {
                invalidKey
            };

            KnowledgebaseHashResult result = testObject.ComputeHash(keys);

            Assert.False(result.IsValid);
        }


        [Fact]
        public void ComputeHash_WithSingleKey()
        {
            ShipSenseOrderItemKey key = new ShipSenseOrderItemKey { Quantity = 3 };
            key.Add("Code", "Code-1");
            key.Add("SKU", "ABC123");

            keys = new List<ShipSenseOrderItemKey>{ key };
            
            KnowledgebaseHashResult result = testObject.ComputeHash(keys);

            Assert.Equal("hZjpDRmimvKCHMvXJ8TuqDrr86dCZ3/yrTmxaaLoEnk=", result.HashValue);
        }

        [Fact]
        public void ComputeHash_WithMultipleKeys()
        {
            ShipSenseOrderItemKey key1 = new ShipSenseOrderItemKey { Quantity = 3 };
            key1.Add("Code", "Code-1");
            key1.Add("SKU", "ABC123");

            ShipSenseOrderItemKey key2 = new ShipSenseOrderItemKey { Quantity = 4 };
            key2.Add("Code", "Code-2");
            key2.Add("SKU", "XYZ789");

            keys = new List<ShipSenseOrderItemKey> { key1, key2 };

            KnowledgebaseHashResult result = testObject.ComputeHash(keys);

            Assert.Equal("ks2iA3JFLyllOGXXYo1FmYWuBQJbVddkk82xXfL3+YI=", result.HashValue);
        }

        [Fact]
        public void ComputeHash_ReturnsSameResult_WhenKeyValuesAreSame_AndQuantitiesAreSame()
        {
            ShipSenseOrderItemKey key1 = new ShipSenseOrderItemKey { Quantity = 3 };
            key1.Add("Code", "Code-1");
            key1.Add("SKU", "ABC123");

            ShipSenseOrderItemKey key2 = new ShipSenseOrderItemKey { Quantity = 3 };
            key2.Add("Code", "Code-1");
            key2.Add("SKU", "ABC123");

            KnowledgebaseHashResult result1 = testObject.ComputeHash(new List<ShipSenseOrderItemKey> { key1 });
            KnowledgebaseHashResult result2 = testObject.ComputeHash(new List<ShipSenseOrderItemKey> { key2 });

            Assert.Equal(result1.HashValue, result2.HashValue);
        }

        [Fact]
        public void ComputeHash_ReturnsDifferentResults_WhenKeyValuesAreSame_AndQuantitiesDiffer()
        {
            ShipSenseOrderItemKey key1 = new ShipSenseOrderItemKey { Quantity = 3 };
            key1.Add("Code", "Code-1");
            key1.Add("SKU", "ABC123");

            ShipSenseOrderItemKey key2 = new ShipSenseOrderItemKey { Quantity = 4 };
            key2.Add("Code", "Code-1");
            key2.Add("SKU", "ABC123");
            
            KnowledgebaseHashResult result1 = testObject.ComputeHash(new List<ShipSenseOrderItemKey> { key1 });
            KnowledgebaseHashResult result2 = testObject.ComputeHash(new List<ShipSenseOrderItemKey> { key2 });

            Assert.NotEqual(result1.HashValue, result2.HashValue);
        }

        [Fact]
        public void ComputeHash_ReturnsInvalidHashResult_WhenKeyCollectionIsEmpty()
        {
            KnowledgebaseHashResult result = testObject.ComputeHash(new List<ShipSenseOrderItemKey>());

            Assert.False(result.IsValid);
        }
    }
}
