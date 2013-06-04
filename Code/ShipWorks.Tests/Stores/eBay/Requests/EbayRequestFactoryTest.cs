using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Stores.Platforms.Ebay.Requests;
using ShipWorks.Stores.Platforms.Ebay.Authorization;
using ShipWorks.Stores.Platforms.Ebay.Requests.Download;
using ShipWorks.Stores.Platforms.Ebay.Requests.Fulfillment;
using ShipWorks.Stores.Platforms.Ebay.Requests.Authorization;
using ShipWorks.Stores.Platforms.Ebay.Requests.System;

namespace ShipWorks.Tests.Stores.eBay.Requests
{
    /// <summary>
    /// Summary description for EbayRequestFactoryTest
    /// </summary>
    [TestClass]
    public class EbayRequestFactoryTest
    {
        EbayRequestFactory testObject;

        public EbayRequestFactoryTest()
        {
            testObject = new EbayRequestFactory();
        }

        [TestMethod]
        public void CreateUserInfoRequest_ReturnsEbayUserInfoRequest_Test()
        {
            IUserInfoRequest request = testObject.CreateUserInfoRequest(new TokenData());
            Assert.IsInstanceOfType(request, typeof(EbayUserInfoRequest));
        }

        [TestMethod]
        public void CreateTangoAuthorizationRequest_ReturnsEbayTangoAuthorizationRequest_Test()
        {
            ITangoAuthorizationRequest request = testObject.CreateTangoAuthorizationRequest(string.Empty);
            Assert.IsInstanceOfType(request, typeof(EbayTangoAuthorizationRequest));
        }

        [TestMethod]
        public void CreateTimeRequest_ReturnsEbayTimeRequest_Test()
        {
            ITimeRequest request = testObject.CreateTimeRequest(new TokenData());
            Assert.IsInstanceOfType(request, typeof(EbayTimeRequest));
        }

        [TestMethod]
        public void CreateTransactionRequest_ReturnsEbayTransactionRequest_Test()
        {
            ITransactionRequest request = testObject.CreateTransactionRequest(new TokenData());
            Assert.IsInstanceOfType(request, typeof(EbayTransactionRequest));
        }

        [TestMethod]
        public void CreateCombinedPaymentRequest_ReturnsEbayCombinedPaymentRequest_Test()
        {
            ICombinedPaymentRequest request = testObject.CreateCombinedPaymentRequest(new TokenData());
            Assert.IsInstanceOfType(request, typeof(EbayCombinedPaymentRequest));
        }

        [TestMethod]
        public void CreateSellingRequest_ReturnsEbaySellingRequest_Test()
        {
            ISellingRequest request = testObject.CreateSellingRequest(new TokenData());
            Assert.IsInstanceOfType(request, typeof(EbaySellingRequest));
        }

        [TestMethod]
        public void CreateFeedbackRequest_ReturnsEbayFeedbackRequest_Test()
        {
            IFeedbackRequest request = testObject.CreateFeedbackRequest(new TokenData());
            Assert.IsInstanceOfType(request, typeof(EbayFeedbackRequest));
        }

        [TestMethod]
        public void CreateItemRequest_ReturnsEbayItemRequest_Test()
        {
            IItemRequest request = testObject.CreateItemRequest(new TokenData());
            Assert.IsInstanceOfType(request, typeof(EbayItemRequest));
        }

        [TestMethod]
        public void CreateItemTransactionRequest_ReturnsEbayItemTransactionRequest_Test()
        {
            IItemTransactionRequest request = testObject.CreateItemTransactionRequest(new TokenData());
            Assert.IsInstanceOfType(request, typeof(EbayItemTransactionRequest));
        }

        [TestMethod]
        public void CreateLeaveFeedbackRequest_ReturnsEbayLeaveFeedbackRequest_Test()
        {
            ILeaveFeedbackRequest request = testObject.CreateLeaveFeedbackRequest(new TokenData());
            Assert.IsInstanceOfType(request, typeof(EbayLeaveFeedbackRequest));
        }

        [TestMethod]
        public void CreateSendPartnerMessageRequest_ReturnsEbaySendMessageRequest_Test()
        {
            ISendPartnerMessageRequest request = testObject.CreateSendPartnerMessageRequest(new TokenData());
            Assert.IsInstanceOfType(request, typeof(EbaySendMessageToPartnerRequest));
        }

        [TestMethod]
        public void CreateCompleteSaleRequest_ReturnsEbayCompleteSaleRequest_Test()
        {
            ICompleteSaleRequest request = testObject.CreateCompleteSaleRequest(new TokenData());
            Assert.IsInstanceOfType(request, typeof(EbayCompleteSaleRequest));
        }

        [TestMethod]
        public void CreateUserNotesRequest_ReturnsEbayUserNotesRequest_Test()
        {
            IUserNotesRequest request = testObject.CreateUserNotesRequest(new TokenData());
            Assert.IsInstanceOfType(request, typeof(EbayUserNotesRequest));
        }

        [TestMethod]
        public void CreateCombineOrdersRequest_ReturnsEbayCombineOrdersRequest_Test()
        {
            ICombineOrdersRequest request = testObject.CreateCombineOrdersRequest(new TokenData());
            Assert.IsInstanceOfType(request, typeof(EbayCombineOrdersRequest));
        }
    }
}
