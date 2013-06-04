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
    public class EbayLeaveFeedbackRequestTest
    {
        private EbayLeaveFeedbackRequest testObject;

        public EbayLeaveFeedbackRequestTest()
        {
            testObject = new EbayLeaveFeedbackRequest(new TokenData());
        }

        [TestMethod]
        public void GetEbayCallName_ReturnsLeaveFeedback_Test()
        {
            Assert.AreEqual("LeaveFeedback", testObject.GetEbayCallName());
        }

        [TestMethod]
        public void GetEbayRequest_ReturnsLeaveFeedbackRequestType_Test()
        {
            Assert.IsInstanceOfType(testObject.GetEbayRequest(), typeof(LeaveFeedbackRequestType));
        }

        [TestMethod]
        public void GetEbayRequest_ReturnsLeaveFeedbackRequestType_WithCommentTypeSpecified_Test()
        {
            LeaveFeedbackRequestType request = (LeaveFeedbackRequestType)testObject.GetEbayRequest();
            Assert.IsTrue(request.CommentTypeSpecified);
        }

    }
}
