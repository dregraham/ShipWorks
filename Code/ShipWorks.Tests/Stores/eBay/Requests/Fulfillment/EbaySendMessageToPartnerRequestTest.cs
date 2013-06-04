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
    public class EbaySendMessageToPartnerRequestTest
    {
        private EbaySendMessageToPartnerRequest testObject;

        public EbaySendMessageToPartnerRequestTest()
        {
            testObject = new EbaySendMessageToPartnerRequest(new TokenData());
        }

        [TestMethod]
        public void GetEbayCallName_ReturnsAddMemberMessageAAQToPartner_Test()
        {
            Assert.AreEqual("AddMemberMessageAAQToPartner", testObject.GetEbayCallName());
        }

        [TestMethod]
        public void GetEbayRequest_ReturnsAddMemberMessageAAQToPartnerRequestType_Test()
        {
            Assert.IsInstanceOfType(testObject.GetEbayRequest(), typeof(AddMemberMessageAAQToPartnerRequestType));
        }

        [TestMethod]
        public void GetEbayRequest_ReturnsRequest_WithEmailCopyToSenderSpecified_Test()
        {
            AddMemberMessageAAQToPartnerRequestType request = (AddMemberMessageAAQToPartnerRequestType)testObject.GetEbayRequest();
            Assert.IsTrue(request.MemberMessage.EmailCopyToSenderSpecified);
        }

        [TestMethod]
        public void GetEbayRequest_ReturnsRequest_WithQuestionTypeSpecified_Test()
        {
            AddMemberMessageAAQToPartnerRequestType request = (AddMemberMessageAAQToPartnerRequestType)testObject.GetEbayRequest();
            Assert.IsTrue(request.MemberMessage.QuestionTypeSpecified);
        }
    }
}
