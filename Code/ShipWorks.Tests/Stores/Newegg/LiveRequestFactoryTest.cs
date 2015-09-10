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

        public LiveRequestFactoryTest()
        {
            testObject = new LiveRequestFactory();
        }

        [Fact]
        public void CreateReportStatusRequest_ReturnsStatusRequest_Test()
        {
            Credentials credentials = new Credentials(string.Empty, string.Empty, NeweggChannelType.US);
            Assert.IsAssignableFrom<StatusRequest>(testObject.CreateReportStatusRequest(credentials));
        }


        [Fact]
        public void CreateCheckCredentialRequest_ReturnsCheckCredentialRequest_Test()
        {
            Assert.IsAssignableFrom<CheckCredentialsRequest>(testObject.CreateCheckCredentialRequest());
        }


        [Fact]
        public void CreateDownloadOrderRequest_ReturnsOrdersRequest_Test()
        {
            Credentials credentials = new Credentials(string.Empty, string.Empty, NeweggChannelType.US);
            Assert.IsAssignableFrom<DownloadOrdersRequest>(testObject.CreateDownloadOrderRequest(credentials));
        }

        [Fact]
        public void CreateCancelOrderRequest_ReturnsCancelOrdersRequest_Test()
        {
            Credentials credentials = new Credentials(string.Empty, string.Empty, NeweggChannelType.US);
            Assert.IsAssignableFrom<CancelOrderRequest>(testObject.CreateCancelOrderRequest(credentials));
        }

        [Fact]
        public void CreateShippingRequest_ReturnsShippingRequest_Test()
        {
            Credentials credentials = new Credentials(string.Empty, string.Empty, NeweggChannelType.US);
            Assert.IsAssignableFrom<ShippingRequest>(testObject.CreateShippingRequest(credentials));
        }

        [Fact]
        public void CreateRemoveItemsRequest_ReturnsRemoveItemsRequest_Test()
        {
            Credentials credentials = new Credentials(string.Empty, string.Empty, NeweggChannelType.US);
            Assert.IsAssignableFrom<RemoveItemRequest>(testObject.CreateRemoveItemRequest(credentials));
        }
    }
}
