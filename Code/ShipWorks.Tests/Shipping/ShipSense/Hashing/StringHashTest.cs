﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.ShipSense.Hashing;

namespace ShipWorks.Tests.Shipping.ShipSense.Hashing
{
    [TestClass]
    public class StringHashTest
    {
        private StringHash testObject;
        private string stringToHash;

        [TestInitialize]
        public void Initialize()
        {
            stringToHash = string.Empty;
            testObject = new KnowledgebaseHash();
        }

        [TestMethod]
        public void ComputeHash_WithSingleItem_Test()
        {
            stringToHash = "{\"StoreID\":2005,\"Packages\":[{\"Length\":11.0,\"Width\":22.0,\"Height\":5.0,\"Weight\":1.9,\"ApplyAdditionalWeight\":false,\"AdditionalWeight\":0.0}],\"CustomsItems\":[]}";

            string hash = testObject.Hash(stringToHash, "2005");

            Assert.AreEqual("X63+ia+sW5olNgkpyRA2yXf9LeSmNAZwTeiHCQMx9ZM=", hash);
        }
    }
}
