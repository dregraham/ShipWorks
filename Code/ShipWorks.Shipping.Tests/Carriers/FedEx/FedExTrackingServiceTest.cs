using System;
using System.Collections.Generic;
using System.Text;
using Autofac.Extras.Moq;
using Castle.Core.Internal;
using Moq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api.Fims;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx
{

    public class FedExTrackingServiceTest : IDisposable
    {
        readonly AutoMock mock;

        public FedExTrackingServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void TrackShipment_UsesFims_WhenFimsShipment()
        {
            mock.Mock<IFedExUtility>()
                .Setup(u => u.IsFimsService(It.IsAny<FedExServiceType>()))
                .Returns(true);
            var shipment = new ShipmentEntity() { FedEx = new FedExShipmentEntity() };
            var trackingResult = new TrackingResult();

            Mock<IFimsShippingClerk> fimsClerkMock = mock.Mock<IFimsShippingClerk>();
            fimsClerkMock.Setup(c => c.Track(shipment))
                .Returns(trackingResult);

            var testObject = mock.Create<FedExTrackingService>();
            var result = testObject.TrackShipment(shipment, "foo");

            fimsClerkMock.Verify(c => c.Track(shipment), Times.Once);

            Assert.Equal(trackingResult, result);
        }

        [Fact]
        public void TrackShipment_UsesShipEngine_WhenNotFims()
        {
            mock.Mock<IFedExUtility>()
                .Setup(u => u.IsFimsService(It.IsAny<FedExServiceType>()))
                .Returns(false);
            var shipment = new ShipmentEntity() { FedEx = new FedExShipmentEntity() };
            var trackingResult = new TrackingResult();
            string trackingUrl = "foo";

            var shipEngineTrackingServiceMock = mock.Mock<IShipEngineTrackingService>();
            shipEngineTrackingServiceMock
                .Setup(s => s.TrackShipment(shipment, ApiLogSource.FedEx, "fedex", trackingUrl))
                .ReturnsAsync(trackingResult);

            var testObject = mock.Create<FedExTrackingService>();

            var result = testObject.TrackShipment(shipment, trackingUrl);

            shipEngineTrackingServiceMock.Verify(s => s.TrackShipment(shipment, ApiLogSource.FedEx, "fedex", trackingUrl), Times.Once);
            Assert.Equal(trackingResult, result);
        }
        public void Dispose()
        {
            ((IDisposable) mock).Dispose();
        }
    }
}
