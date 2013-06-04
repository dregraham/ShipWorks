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
    [TestClass]
    public class EbayCombinedPaymentRequestTest
    {
        EbayCombinedPaymentRequest testObject;

        public EbayCombinedPaymentRequestTest()
        {
            testObject = new EbayCombinedPaymentRequest(new TokenData());
        }

        [TestMethod]
        public void GetEbayCallName_ReturnsGetOrders_Test()
        {
            Assert.AreEqual("GetOrders", testObject.GetEbayCallName());
        }

        [TestMethod]
        public void GetEbayRequest_ReturnsGetOrdersRequestType_Test()
        {
            Assert.IsInstanceOfType(testObject.GetEbayRequest(), typeof(GetOrdersRequestType));
        }
    }
}
