using System;
using Xunit;
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
    public class LiveRequestFactoryTest
    {
        private LiveRequestFactory testObject;

        [TestInitialize]
        public void Initialize()
        {
            testObject = new LiveRequestFactory();
        }

        [Fact]
        public void CreateReportStatusRequest_ReturnsStatusRequest_Test()
        {
            Credentials credentials = new Credentials(string.Empty, string.Empty, NeweggChannelType.US);
            Assert.IsInstanceOfType(testObject.CreateReportStatusRequest(credentials), typeof(StatusRequest));
        }


        [Fact]
        public void CreateCheckCredentialRequest_ReturnsCheckCredentialRequest_Test()
        {
            Assert.IsInstanceOfType(testObject.CreateCheckCredentialRequest(), typeof(CheckCredentialsRequest));
        }


        [Fact]
        public void CreateDownloadOrderRequest_ReturnsOrdersRequest_Test()
        {
            Credentials credentials = new Credentials(string.Empty, string.Empty, NeweggChannelType.US);
            Assert.IsInstanceOfType(testObject.CreateDownloadOrderRequest(credentials), typeof(DownloadOrdersRequest));
        }

        [Fact]
        public void CreateCancelOrderRequest_ReturnsCancelOrdersRequest_Test()
        {
            Credentials credentials = new Credentials(string.Empty, string.Empty, NeweggChannelType.US);
            Assert.IsInstanceOfType(testObject.CreateCancelOrderRequest(credentials), typeof(CancelOrderRequest));
        }

        [Fact]
        public void CreateShippingRequest_ReturnsShippingRequest_Test()
        {
            Credentials credentials = new Credentials(string.Empty, string.Empty, NeweggChannelType.US);
            Assert.IsInstanceOfType(testObject.CreateShippingRequest(credentials), typeof(ShippingRequest));
        }

        [Fact]
        public void CreateRemoveItemsRequest_ReturnsRemoveItemsRequest_Test()
        {
            Credentials credentials = new Credentials(string.Empty, string.Empty, NeweggChannelType.US);
            Assert.IsInstanceOfType(testObject.CreateRemoveItemRequest(credentials), typeof(RemoveItemRequest));
        }
    }
}
