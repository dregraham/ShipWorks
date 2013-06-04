using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Stores.Platforms.Ebay.Requests;
using ShipWorks.Stores.Platforms.Ebay.Authorization;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Stores.Platforms.Ebay.Requests.Fulfillment;

namespace ShipWorks.Tests.Stores.eBay.Requests.Fulfillment
{
    [TestClass]
    public class EbayItemTransactionRequestTest
    {
        private EbayItemTransactionRequest testObject;

        public EbayItemTransactionRequestTest()
        {
            testObject = new EbayItemTransactionRequest(new TokenData());
        }


        [TestMethod]
        public void GetEbayCallName_ReturnsGetItemTransactions_Test()
        {
            Assert.AreEqual("GetItemTransactions", testObject.GetEbayCallName());
        }

        [TestMethod]
        public void GetEbayRequest_ReturnsGetItemTransactionRequestType_Test()
        {
            Assert.IsInstanceOfType(testObject.GetEbayRequest(), typeof(GetItemTransactionsRequestType));
        }

        [TestMethod]
        public void GetEbayRequest_ReturnsGetItemRequestType_WithNullDetailLevel_Test()
        {
            GetItemTransactionsRequestType request = (GetItemTransactionsRequestType)testObject.GetEbayRequest();
            Assert.IsNull(request.DetailLevel);
        }
        
    }
}
