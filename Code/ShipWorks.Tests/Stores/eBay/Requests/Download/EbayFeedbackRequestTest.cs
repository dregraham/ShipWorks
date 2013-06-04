using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Stores.Platforms.Ebay.Requests;
using ShipWorks.Stores.Platforms.Ebay.Authorization;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Stores.Platforms.Ebay.Requests.Download;

namespace ShipWorks.Tests.Stores.eBay.Requests.Download
{
    [TestClass]
    public class EbayFeedbackRequestTest 
    {
        private EbayFeedbackRequest testObject;

        public EbayFeedbackRequestTest()
        {
            testObject = new EbayFeedbackRequest(new TokenData());
        }


        [TestMethod]
        public void GetEbayCallName_ReturnsGetFeedback_Test()
        {
            Assert.AreEqual("GetFeedback", testObject.GetEbayCallName());
        }

        [TestMethod]
        public void GetEbayRequest_ReturnsGetFeedbackRequestType_Test()
        {
            Assert.IsInstanceOfType(testObject.GetEbayRequest(), typeof(GetFeedbackRequestType));
        }

    }
}
