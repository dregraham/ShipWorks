using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Stores.Platforms.Ebay.Requests;
using ShipWorks.Stores.Platforms.Ebay.Authorization;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Stores.Platforms.Ebay.Requests.Authorization;

namespace ShipWorks.Tests.Stores.eBay.Requests.Authorization
{
    [TestClass]
    public class EbayUserInfoRequestTest
    {
        // There's not too much to test here from a unit perspective since this is interacting 
        // with eBay directly
        EbayUserInfoRequest testObject;

        public EbayUserInfoRequestTest()
        {
            testObject = new EbayUserInfoRequest(new TokenData());
        }

        [TestMethod]
        public void GetEbayCallName_ReturnsGetUser_Test()
        {
            string callName = testObject.GetEbayCallName();
            Assert.AreEqual("GetUser", callName);
        }

        [TestMethod]
        public void GetEbayRequest_ReturnsGetUserRequestType_Test()
        {
            AbstractRequestType request = testObject.GetEbayRequest();
            Assert.IsInstanceOfType(request, typeof(GetUserRequestType));
        }

        [TestMethod]
        public void GetEbayRequest_ReturnsGetUserRequestType_WithDetailLevel_Test()
        {
            GetUserRequestType request = (GetUserRequestType)testObject.GetEbayRequest();

            Assert.AreEqual(1, request.DetailLevel.Length);
            Assert.AreEqual(DetailLevelCodeType.ReturnAll, request.DetailLevel[0]);
        }
    }
}
