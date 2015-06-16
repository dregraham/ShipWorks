using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Stores.Platforms.Newegg.Net;
using ShipWorks.Stores.Platforms.Newegg.Net.ReportStatus;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Download;
using ShipWorks.Stores.Platforms.Newegg.Net.CredentialValidation;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Cancellation;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Shipping;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.ItemRemoval;
using ShipWorks.Stores.Platforms.Newegg.Enums;

namespace ShipWorks.Tests.Stores.Newegg
{
    /// <summary>
    /// Summary description for LiveRequestFactoryTest
    /// </summary>
    [TestClass]
    public class LiveRequestFactoryTest
    {
        private LiveRequestFactory testObject;

        [TestInitialize]
        public void Initialize()
        {
            testObject = new LiveRequestFactory();
        }

        [TestMethod]
        public void CreateReportStatusRequest_ReturnsStatusRequest_Test()
        {
            Credentials credentials = new Credentials(string.Empty, string.Empty, NeweggChannelType.Marketplace);
            Assert.IsInstanceOfType(testObject.CreateReportStatusRequest(credentials), typeof(StatusRequest));
        }


        [TestMethod]
        public void CreateCheckCredentialRequest_ReturnsCheckCredentialRequest_Test()
        {
            Assert.IsInstanceOfType(testObject.CreateCheckCredentialRequest(), typeof(CheckCredentialsRequest));
        }


        [TestMethod]
        public void CreateDownloadOrderRequest_ReturnsOrdersRequest_Test()
        {
            Credentials credentials = new Credentials(string.Empty, string.Empty, NeweggChannelType.Marketplace);
            Assert.IsInstanceOfType(testObject.CreateDownloadOrderRequest(credentials), typeof(DownloadOrdersRequest));
        }

        [TestMethod]
        public void CreateCancelOrderRequest_ReturnsCancelOrdersRequest_Test()
        {
            Credentials credentials = new Credentials(string.Empty, string.Empty, NeweggChannelType.Marketplace);
            Assert.IsInstanceOfType(testObject.CreateCancelOrderRequest(credentials), typeof(CancelOrderRequest));
        }

        [TestMethod]
        public void CreateShippingRequest_ReturnsShippingRequest_Test()
        {
            Credentials credentials = new Credentials(string.Empty, string.Empty, NeweggChannelType.Marketplace);
            Assert.IsInstanceOfType(testObject.CreateShippingRequest(credentials), typeof(ShippingRequest));
        }

        [TestMethod]
        public void CreateRemoveItemsRequest_ReturnsRemoveItemsRequest_Test()
        {
            Credentials credentials = new Credentials(string.Empty, string.Empty, NeweggChannelType.Marketplace);
            Assert.IsInstanceOfType(testObject.CreateRemoveItemRequest(credentials), typeof(RemoveItemRequest));
        }
    }
}
