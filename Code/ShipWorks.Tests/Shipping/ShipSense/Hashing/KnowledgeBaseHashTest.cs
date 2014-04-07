using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.ShipSense.Hashing;

namespace ShipWorks.Tests.Shipping.ShipSense.Hashing
{
    [TestClass]
    public class KnowledgebaseHashTest
    {
        private KnowledgebaseHash testObject;

        private OrderEntity order;
        private List<ShipSenseOrderItemKey> keys;
        
        [TestInitialize]
        public void Initialize()
        {
            order = new OrderEntity();
            order.StoreID = 1006;

            keys = new List<ShipSenseOrderItemKey>();

            testObject = new KnowledgebaseHash();
        }

        [TestMethod]
        public void ComputeHash_ReturnsInvalidResult_WhenUniquenessKeyIsNotFound_Test()
        {
            ShipSenseOrderItemKey invalidKey = new ShipSenseOrderItemKey { Quantity = 3 };
            keys = new List<ShipSenseOrderItemKey>
            {
                invalidKey
            };

            KnowledgebaseHashResult result = testObject.ComputeHash(keys);

            Assert.IsFalse(result.IsValid);
        }


        [TestMethod]
        public void ComputeHash_WithSingleKey_Test()
        {
            ShipSenseOrderItemKey key = new ShipSenseOrderItemKey { Quantity = 3 };
            key.Add("Code", "Code-1");
            key.Add("SKU", "ABC123");

            keys = new List<ShipSenseOrderItemKey>{ key };
            
            KnowledgebaseHashResult result = testObject.ComputeHash(keys);

            Assert.AreEqual("2JzA5V//RUKSN2J5eBE2dsy8O0MqBR5UldufSSPbrIU=", result.HashValue);
        }

        [TestMethod]
        public void ComputeHash_WithMultipleKeys_Test()
        {
            ShipSenseOrderItemKey key1 = new ShipSenseOrderItemKey { Quantity = 3 };
            key1.Add("Code", "Code-1");
            key1.Add("SKU", "ABC123");

            ShipSenseOrderItemKey key2 = new ShipSenseOrderItemKey { Quantity = 4 };
            key2.Add("Code", "Code-2");
            key2.Add("SKU", "XYZ789");

            keys = new List<ShipSenseOrderItemKey> { key1, key2 };

            KnowledgebaseHashResult result = testObject.ComputeHash(keys);

            Assert.AreEqual("3LeCGrCdBjjTNrLSH6wa8y3sC/Bc8EwH8VnJk8VfoGs=", result.HashValue);
        }
        
    }
}
