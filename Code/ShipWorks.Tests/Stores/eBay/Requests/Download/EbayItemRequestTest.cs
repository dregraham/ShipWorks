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
    public class EbayItemRequestTest
    {
        private EbayItemRequest testObject;

        public EbayItemRequestTest()
        {
            testObject = new EbayItemRequest(new TokenData());
        }


        [TestMethod]
        public void GetEbayCallName_ReturnsGetItem_Test()
        {
            Assert.AreEqual("GetItem", testObject.GetEbayCallName());
        }

        [TestMethod]
        public void GetEbayRequest_ReturnsGetItemRequestType_Test()
        {
            Assert.IsInstanceOfType(testObject.GetEbayRequest(), typeof(GetItemRequestType));
        }

        [TestMethod]
        public void GetEbayRequest_ReturnsGetItemRequestType_WithDetailLevel_Test()
        {
            GetItemRequestType request = (GetItemRequestType)testObject.GetEbayRequest();
            Assert.AreEqual(1, request.DetailLevel.Length);
        }

        [TestMethod]
        public void GetEbayRequest_ReturnsGetItemRequestType_WithItemReturnAttributesDetailLevel_Test()
        {
            GetItemRequestType request = (GetItemRequestType)testObject.GetEbayRequest();
            Assert.AreEqual(DetailLevelCodeType.ItemReturnAttributes, request.DetailLevel[0]);
        }
    }
}
