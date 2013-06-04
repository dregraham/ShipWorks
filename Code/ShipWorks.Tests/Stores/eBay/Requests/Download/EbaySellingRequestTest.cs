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
    public class EbaySellingRequestTest
    {
        private EbaySellingRequest testObject;

        public EbaySellingRequestTest()
        {
            testObject = new EbaySellingRequest(new TokenData());
        }

        [TestMethod]
        public void GetEbayCallName_ReturnsGetMyeBaySelling_Test()
        {
            Assert.AreEqual("GetMyeBaySelling", testObject.GetEbayCallName());
        }

        [TestMethod]
        public void GetEbayRequest_ReturnsGetMyeBaySellingReqeust_Test()
        {
            Assert.IsInstanceOfType(testObject.GetEbayRequest(), typeof(GetMyeBaySellingRequestType));
        }

        [TestMethod]
        public void MaximumDurationInDays_ReturnsSixty_Test()
        {
            Assert.AreEqual(60, testObject.MaximumDurationInDays);
        }
    }
}
