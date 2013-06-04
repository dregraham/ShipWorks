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
    public class EbayUserNotesRequestTest
    {
        private EbayUserNotesRequest testObject;

        public EbayUserNotesRequestTest()
        {
            testObject = new EbayUserNotesRequest(new TokenData());
        }


        [TestMethod]
        public void GetEbayCallName_ReturnsSetUserNotes_Test()
        {
            Assert.AreEqual("SetUserNotes", testObject.GetEbayCallName());
        }

        [TestMethod]
        public void GetEbayRequest_ReturnsSetUserNotesRequestType_Test()
        {
            Assert.IsInstanceOfType(testObject.GetEbayRequest(), typeof(SetUserNotesRequestType));
        }

        [TestMethod]
        public void GetEbayRequest_ReturnsSetUserNotesRequestType_WithActionSpecified_Test()
        {
            SetUserNotesRequestType request = (SetUserNotesRequestType)testObject.GetEbayRequest();
            Assert.IsTrue(request.ActionSpecified);
        }

        [TestMethod]
        public void GetEbayRequest_ReturnsSetUserNotestRequestType_WithActionAsAddUpdate_Test()
        {
            SetUserNotesRequestType request = (SetUserNotesRequestType)testObject.GetEbayRequest();
            Assert.AreEqual(SetUserNotesActionCodeType.AddOrUpdate, request.Action);
        }
    }
}
