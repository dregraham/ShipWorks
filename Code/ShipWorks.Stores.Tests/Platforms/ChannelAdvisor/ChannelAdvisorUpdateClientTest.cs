using System;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ChannelAdvisor;
using ShipWorks.Stores.Platforms.ChannelAdvisor.DTO;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.ChannelAdvisor
{
    public class ChannelAdvisorUpdateClientTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ChannelAdvisorStoreEntity store;
        private readonly ChannelAdvisorShipment shipment;
        private readonly Mock<IChannelAdvisorClient> soapClient;

        public ChannelAdvisorUpdateClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            store = new ChannelAdvisorStoreEntity();
            shipment = new ChannelAdvisorShipment()
            {
                ShippedDateUtc = new DateTime(2017, 1, 1),
                ShippingCarrier = "carrier",
                ShippingClass = "class",
                TrackingNumber = "track this!"
            };
            soapClient = mock.CreateMock<IChannelAdvisorClient>();

            mock.MockFunc<ChannelAdvisorStoreEntity, IChannelAdvisorClient>(soapClient);
        }

        [Fact]
        public void UploadShipmentDetails_DelegatesToSoapClient_WhenStoreHasEmptyRefreshToken()
        {
            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            testObject.UploadShipmentDetails(store, shipment, 1234);

            soapClient.Verify(c=>c.UploadShipmentDetails(1234,shipment.ShippedDateUtc, "carrier", "class", "track this!"), Times.Once);
        }

        [Fact]
        public void UploadShipmentDetails_DoesNotDelegateToSoapClient_WhenStoreHasRefreshToken()
        {
            store.RefreshToken = "ha";
            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            testObject.UploadShipmentDetails(store, shipment, 1234);

            soapClient.Verify(c => c.UploadShipmentDetails(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void UploadShipmentDetails_DelegatesToRestClient_WhenStoreHasRefreshToken()
        {
            store.RefreshToken = "ha";
            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            testObject.UploadShipmentDetails(store, shipment, 1234);

            mock.Mock<IChannelAdvisorRestClient>().Verify(c => c.UploadShipmentDetails(shipment, "ha", "1234"),
                Times.Once);
        }

        [Fact]
        public void UploadShipmentDetails_DoesNotDelegateToRestClient_WhenStoreHasEmptyRefreshToken()
        {
            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            testObject.UploadShipmentDetails(store, shipment, 1234);

            mock.Mock<IChannelAdvisorRestClient>().Verify(c => c.UploadShipmentDetails(
                It.IsAny<ChannelAdvisorShipment>(),
                It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}