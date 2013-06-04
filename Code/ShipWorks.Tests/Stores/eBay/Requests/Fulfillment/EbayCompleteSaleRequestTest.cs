using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.Requests;
using ShipWorks.Stores.Platforms.Ebay.Authorization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Stores.Platforms.Ebay.Requests.Fulfillment;

namespace ShipWorks.Tests.Stores.eBay.Requests.Fulfillment
{
    [TestClass]
    public class EbayCompleteSaleRequestTest
    {
        private EbayCompleteSaleRequest testObject;


        public EbayCompleteSaleRequestTest()
        {
            testObject = new EbayCompleteSaleRequest(new TokenData());
        }

        [TestMethod]
        public void GetEbayCallName_ReturnsCompleteSale_Test()
        {
            Assert.AreEqual("CompleteSale", testObject.GetEbayCallName());
        }

        [TestMethod]
        public void GetEbayRequest_ReturnsCompleteSaleRequestType_Test()
        {
            Assert.IsInstanceOfType(testObject.GetEbayRequest(), typeof(CompleteSaleRequestType));
        }

        [TestMethod]
        public void GetEbayRequest_ReturnsCompleteSaleRequest_WithShipment_Test()
        {
            CompleteSaleRequestType request = (CompleteSaleRequestType)testObject.GetEbayRequest();
            Assert.IsNotNull(request.Shipment);
        }

    }
}
