using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Close;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.ShipEngine.DTOs;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.ShipEngine
{
    public class ShipEngineTrackingServiceTest : IDisposable
    {
        readonly AutoMock mock;

        private const string trackingUrl = "https://www.shipworks.com";
        private const ApiLogSource apiLogSource = ApiLogSource.FedEx;
        private readonly ShipmentEntity shipment;
        private const string carrierCode = "fedex";
        private readonly Mock<ICarrierShipmentAdapter> shipmentAdapter;
        private readonly Mock<IShipEngineWebClient> seClient;
        private readonly Mock<IShipEngineTrackingResultFactory> trackingResultFactory;
        private readonly TrackingResult factoryResult = new TrackingResult();
        
        public ShipEngineTrackingServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shipment = new ShipmentEntity() { TrackingNumber = "123" };
            shipmentAdapter = mock.Mock<ICarrierShipmentAdapter>();
            mock.Mock<ICarrierShipmentAdapterFactory>()
                .Setup(f => f.Get(shipment))
                .Returns(shipmentAdapter.Object);

            seClient = mock.Mock<IShipEngineWebClient>();
            trackingResultFactory = mock.Mock<IShipEngineTrackingResultFactory>();
            trackingResultFactory
                .Setup(f => f.Create(It.IsAny<TrackingInformation>()))
                .Returns(factoryResult);

                
        }

        [Fact]
        public async Task TrackShiment_CallsTrack_WithId_WhenSELabelIdHasValue()
        {
            var seLabelId = "foo";
            var testObject = mock.Create<ShipEngineTrackingService>();
            shipmentAdapter.SetupGet(s => s.ShipEngineLabelId).Returns(seLabelId);

            await testObject.TrackShipment(shipment, apiLogSource, carrierCode, trackingUrl).ConfigureAwait(false);

            seClient.Verify(c=>c.Track(seLabelId, apiLogSource));
        }

        [Fact]
        public async Task TrackShiment_CallsTrack_WithoutId_WhenSELabelIdIsEmpty()
        {
            var testObject = mock.Create<ShipEngineTrackingService>();
            shipmentAdapter.SetupGet(s => s.ShipEngineLabelId).Returns(string.Empty);

            await testObject.TrackShipment(shipment, apiLogSource, carrierCode, trackingUrl).ConfigureAwait(false);

            seClient.Verify(c => c.Track(carrierCode, shipment.TrackingNumber, apiLogSource), Times.Once);
        }

        [Theory]
        [InlineData("UN", 3)]
        [InlineData("DE", 0)]
        public async Task TrackShipment_ReturnsTrackingUrl_WhenNoTracking(string resultStatusCode, int resultEventCount)
        {
            var seLabelId = "foo";
            var testObject = mock.Create<ShipEngineTrackingService>();
            shipmentAdapter.SetupGet(s => s.ShipEngineLabelId).Returns(seLabelId);

            seClient.Setup(c => c.Track(seLabelId, apiLogSource))
                .ReturnsAsync(new TrackingInformation()
                {
                    StatusCode = resultStatusCode,
                    Events = Enumerable.Repeat(new TrackEvent(), resultEventCount).ToList()
                });

            var result = await testObject.TrackShipment(shipment, apiLogSource, carrierCode, trackingUrl).ConfigureAwait(false);

            Assert.NotEqual(factoryResult, result);
            Assert.Contains(trackingUrl, result.Summary);
            Assert.Contains("Click here to view tracking information online", result.Summary);
        }

        [Fact]
        public async Task TrackShipment_ReturnsTrackingInfo_FromResultFactory_WhenHasTracking()
        {
            var seLabelId = "foo";
            var testObject = mock.Create<ShipEngineTrackingService>();
            shipmentAdapter.SetupGet(s => s.ShipEngineLabelId).Returns(seLabelId);

            var trackingInfo = new TrackingInformation()
            {
                StatusCode = "DE",
                Events = Enumerable.Repeat(new TrackEvent(), 1).ToList()
            };
            seClient.Setup(c => c.Track(seLabelId, apiLogSource))
                .ReturnsAsync(trackingInfo);

            var result = await testObject.TrackShipment(shipment, apiLogSource, carrierCode, trackingUrl).ConfigureAwait(false);

            Assert.Equal(factoryResult, result);
            trackingResultFactory.Verify(f => f.Create(trackingInfo), Times.Once);
        }

        [Fact]
        public async Task TrackShipment_ReturnstrackingUrl_WhenExceptionIsThrown()
        {
            var seLabelId = "foo";
            var testObject = mock.Create<ShipEngineTrackingService>();
            shipmentAdapter.SetupGet(s => s.ShipEngineLabelId).Throws(new Exception());

            var result = await testObject.TrackShipment(shipment, apiLogSource, carrierCode, trackingUrl).ConfigureAwait(false);

            Assert.NotEqual(factoryResult, result);
            Assert.Contains(trackingUrl, result.Summary);
            Assert.Contains("Click here to view tracking information online", result.Summary);
        }

        public void Dispose()
        {
            ((IDisposable) mock).Dispose();
        }
    }
}
