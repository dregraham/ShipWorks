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
    public class EbayCombineOrdersRequestTest
    {
        private EbayCombineOrdersRequest testObject;

        public EbayCombineOrdersRequestTest()
        {
            testObject = new EbayCombineOrdersRequest(new TokenData());
        }


        [TestMethod]
        public void GetEbayCallName_ReturnsAddOrder_Test()
        {
            Assert.AreEqual("AddOrder", testObject.GetEbayCallName());
        }

        [TestMethod]
        public void GetEbayRequest_ReturnsAddOrderRequestType_Test()
        {
            AddOrderRequestType request = (AddOrderRequestType)testObject.GetEbayRequest();
            Assert.IsInstanceOfType(request, typeof(AddOrderRequestType));
        }

        [TestMethod]
        public void GetEbayRequest_ReturnsAddOrderRequestType_WithOrderCreated_Test()
        {
            AddOrderRequestType request = (AddOrderRequestType)testObject.GetEbayRequest();
            Assert.IsNotNull(request.Order);
        }

        [TestMethod]
        public void GetEbayRequest_ReturnsAddOrderRequestType_WithCreatingUserRoleSpecified_Test()
        {
            AddOrderRequestType request = (AddOrderRequestType)testObject.GetEbayRequest();
            Assert.IsTrue(request.Order.CreatingUserRoleSpecified);
        }

        [TestMethod]
        public void GetEbayRequest_ReturnsAddOrderRequestType_WithSellerAsUserRole_Test()
        {
            AddOrderRequestType request = (AddOrderRequestType)testObject.GetEbayRequest();
            Assert.AreEqual(TradingRoleCodeType.Seller, request.Order.CreatingUserRole);
        }

        [TestMethod]
        public void GetEbayRequest_ReturnsAddOrderRequestType_WithCurrencyTotal_Test()
        {
            AddOrderRequestType request = (AddOrderRequestType)testObject.GetEbayRequest();
            Assert.AreEqual(CurrencyCodeType.USD, request.Order.Total.currencyID);
        }

        [TestMethod]
        public void GetEbayRequest_ReturnsAddOrderRequestType_WithShippingType_Test()
        {
            AddOrderRequestType request = (AddOrderRequestType)testObject.GetEbayRequest();
            Assert.AreEqual(ShippingTypeCodeType.Flat, request.Order.ShippingDetails.ShippingType);
        }

        [TestMethod]
        public void GetEbayRequest_ReturnsAddOrderRequestType_WithShippingTypeSpecified_Test()
        {
            AddOrderRequestType request = (AddOrderRequestType)testObject.GetEbayRequest();
            Assert.IsTrue(request.Order.ShippingDetails.ShippingTypeSpecified);
        }
    }
}
