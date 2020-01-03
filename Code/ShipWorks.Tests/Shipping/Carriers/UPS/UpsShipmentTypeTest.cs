using System;
using Xunit;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Tests.Shared;
using ShipWorks.Data.Model.EntityClasses;
using Autofac.Extras.Moq;

namespace ShipWorks.Tests.Shipping.Carriers.UPS
{
    public class UpsShipmentTypeTest : IDisposable
    {
        private AutoMock mock;

        public UpsShipmentTypeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void SupportsMultiplePackages_ReturnsTrue()
        {
            UpsOltShipmentType testObject = mock.Create<UpsOltShipmentType>();
            Assert.True(testObject.SupportsMultiplePackages);
        }

        [Fact]
        public void GetCarrierTrackingUrl_ReturnsEmptyStringWhenNoTrackingNumber()
        {
            var shipment = new ShipmentEntity
            {
                TrackingNumber = "",
                Processed = true
            };

            UpsOltShipmentType testObject = mock.Create<UpsOltShipmentType>();

            var trackingUrl = testObject.GetCarrierTrackingUrl(shipment);

            Assert.Empty(trackingUrl);
        }

        [Fact]
        public void GetCarrierTrackingUrl_ReturnsCorrectUrlWhenNotEmpty()
        {
            var shipment = new ShipmentEntity
            {
                TrackingNumber = "foo",
                Processed = true
            };

            UpsOltShipmentType testObject = mock.Create<UpsOltShipmentType>();

            var trackingUrl = testObject.GetCarrierTrackingUrl(shipment);

            Assert.Equal("https://www.ups.com/track?loc=en_US&tracknum=foo", trackingUrl);
        }

        [Fact]
        public void GetCarrierTrackingUrl_ReturnsEmptyStringWhenShipmentNotProcessed()
        {
            var shipment = new ShipmentEntity
            {
                TrackingNumber = "foo",
                Processed = false
            };

            UpsOltShipmentType testObject = mock.Create<UpsOltShipmentType>();

            var trackingUrl = testObject.GetCarrierTrackingUrl(shipment);

            Assert.Empty(trackingUrl);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
