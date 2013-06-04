using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Stores.Platforms.Ebay.Requests;
using ShipWorks.Stores.Platforms.Ebay.Authorization;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Stores.Platforms.Ebay.Requests.Download;

namespace ShipWorks.Tests.Stores.eBay.Requests.Download
{
    /// <summary>
    /// Summary description for EbayTransactionRequestTest
    /// </summary>
    [TestClass]
    public class EbayTransactionRequestTest
    {
        EbayTransactionRequest testObject;

        public EbayTransactionRequestTest()
        {
            testObject = new EbayTransactionRequest(new TokenData());
        }

        [TestMethod]
        public void GetEbayCallName_ReturnsGetSellerTransactions_Test()
        {
            string callName = testObject.GetEbayCallName();
            Assert.AreEqual("GetSellerTransactions", callName);            
        }

        [TestMethod]
        public void GetEbayRequest_ReturnsGetSellerTransactionsRequestType_Test()
        {
            AbstractRequestType request = testObject.GetEbayRequest();
            Assert.IsInstanceOfType(request, typeof(GetSellerTransactionsRequestType));
        }


    }
}
