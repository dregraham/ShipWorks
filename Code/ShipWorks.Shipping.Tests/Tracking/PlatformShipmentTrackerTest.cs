using Autofac.Extras.Moq;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using RestSharp;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Shipping.Tracking.DTO;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Tests.Tracking
{
    public class PlatformShipmentTrackerTest : IDisposable
    {
        private readonly AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks();

        private const string fakedWarehouseId = "whID";
        
        public PlatformShipmentTrackerTest()
        {
            mock.Mock<IConfigurationData>()
                .Setup(c => c.FetchReadOnly())
                .Returns(new ConfigurationEntity { WarehouseID = fakedWarehouseId });
        }

        [Fact]
        public async Task TrackShipments_SendsShipmentsToTrack()
        {
            var trackingRepository = mock.Mock<ITrackingRepository>();
            trackingRepository.SetupSequence(r => r.FetchShipmentsToTrack())
                .ReturnsAsync(new[]
                {
                    new ShipmentEntity
                    {
                        TrackingNumber = "t1",
                        ShipmentTypeCode = ShipmentTypeCode.FedEx,
                    },
                    new ShipmentEntity
                    {
                        TrackingNumber = "t2",
                        ShipmentTypeCode = ShipmentTypeCode.FedEx
                    }
                })
                .ReturnsAsync(new[]
                {
                    new ShipmentEntity
                    {
                        TrackingNumber = "t3",
                        ShipmentTypeCode = ShipmentTypeCode.FedEx
                    }
                })
                .ReturnsAsync(new ShipmentEntity[] { });


            mock.Mock<IPlatformShipmentTrackerClient>()
                .Setup(c => c.SendShipment(AnyString, AnyString, AnyString))
                .ReturnsAsync(GenericResult.FromSuccess(mock.Mock<IRestResponse>().Object));

            var testObject = mock.Create<PlatformShipmentTracker>();
            await testObject.TrackShipments(new CancellationToken()).ConfigureAwait(false);
            
            mock.Mock<IPlatformShipmentTrackerClient>().Verify(c=>c.SendShipment(AnyString, AnyString, AnyString), Times.Exactly(3));
            mock.Mock<ITrackingRepository>().Verify(r=>r.FetchShipmentsToTrack(), Times.Exactly(3));
        }

        [Theory]
        [InlineData(true, 1)]
        [InlineData(false, 0)]
        public async Task TrackShipments_MarksShipmentSent_WhenTrackingSentSuccessfully(bool sucessfullySentToPlatform, int timesMarkedAsSent)
        {
            var trackingRepository = mock.Mock<ITrackingRepository>();
            trackingRepository.SetupSequence(r => r.FetchShipmentsToTrack())
                .ReturnsAsync(new[]
                {
                    new ShipmentEntity
                    {
                        TrackingNumber = "t1",
                        ShipmentTypeCode = ShipmentTypeCode.FedEx,
                    }
                })
                .ReturnsAsync(new ShipmentEntity[] { });
            mock.Mock<IPlatformShipmentTrackerClient>()
                .Setup(c => c.SendShipment(AnyString, AnyString, AnyString))
                .ReturnsAsync(sucessfullySentToPlatform
                    ? GenericResult.FromSuccess(mock.Mock<IRestResponse>().Object)
                    : GenericResult.FromError<IRestResponse>(new Exception()));

            var testObject = mock.Create<PlatformShipmentTracker>();
            await testObject.TrackShipments(new CancellationToken()).ConfigureAwait(false);
            
            mock.Mock<ITrackingRepository>().Verify(r=>r.MarkAsSent(AnyShipment), Times.Exactly(timesMarkedAsSent));
        }

        [Theory]
        [InlineData(ShipmentTypeCode.FedEx, "fedex")]
        [InlineData(ShipmentTypeCode.Endicia, "usps")]
        [InlineData(ShipmentTypeCode.Usps, "usps")]
        [InlineData(ShipmentTypeCode.UpsWorldShip, "ups")]
        [InlineData(ShipmentTypeCode.UpsOnLineTools, "ups")]
        public async Task TrackShipments_CorrectDataSentToHub(ShipmentTypeCode shipmentTypeCode, string carrierNameToSendToPlatform)
        {
            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            shipmentTypeManager
                .Setup(s => s.IsPostal(AnyShipmentTypeCode))
                .Returns<ShipmentTypeCode>(ShipmentTypeManager.IsPostal);
            shipmentTypeManager
                .Setup(s=>s.IsUps(AnyShipmentTypeCode))
                .Returns<ShipmentTypeCode>(ShipmentTypeManager.IsUps);
            
            var trackingRepository = mock.Mock<ITrackingRepository>();
            trackingRepository.SetupSequence(r => r.FetchShipmentsToTrack())
                .ReturnsAsync(new[]
                {
                    new ShipmentEntity
                    {
                        TrackingNumber = "t3",
                        ShipmentTypeCode = shipmentTypeCode
                    }
                })
                .ReturnsAsync(new ShipmentEntity[] { });

            var testObject = mock.Create<PlatformShipmentTracker>();
            await testObject.TrackShipments(new CancellationToken()).ConfigureAwait(false);
            
            mock.Mock<IPlatformShipmentTrackerClient>().Verify(c=>c.SendShipment("t3", carrierNameToSendToPlatform, fakedWarehouseId));
        }

        [Fact]
        public async Task PopulateLatestTracking_NotificationsSaved()
        {
            DateTime yesterday = DateTime.Now.AddDays(-1);
            DateTime now = DateTime.Now;

            mock.Mock<ITrackingRepository>().Setup(r => r.GetLatestNotificationDate())
                .ReturnsAsync(yesterday);

            var trackingNotification = new TrackingNotification();
            mock.Mock<IPlatformShipmentTrackerClient>()
                .Setup(c => c.GetTracking(fakedWarehouseId, yesterday))
                .ReturnsAsync(new[] { trackingNotification, trackingNotification });

            var testObject = mock.Create<PlatformShipmentTracker>();
            testObject.PopulateLatestTracking(new CancellationToken());

            mock.Mock<ITrackingRepository>().Verify(r=>r.SaveNotification(trackingNotification), Times.Exactly(2));
        }

        [Fact]
        public async Task PopulateLatestTracking_FetchesNextPage_WithMostRecentDate()
        {
            DateTime yesterday = DateTime.Now.AddDays(-1);
            DateTime now = DateTime.Now;

            mock.Mock<ITrackingRepository>().Setup(r => r.GetLatestNotificationDate())
                .ReturnsAsync(yesterday);

            var trackingNotification = new TrackingNotification { HubTimestamp = now };
            mock.Mock<IPlatformShipmentTrackerClient>()
                .Setup(c => c.GetTracking(fakedWarehouseId, yesterday))
                .ReturnsAsync(new[] { trackingNotification, trackingNotification });
            
            var testObject = mock.Create<PlatformShipmentTracker>();
            testObject.PopulateLatestTracking(new CancellationToken());

            mock.Mock<IPlatformShipmentTrackerClient>().Verify(c=>c.GetTracking(fakedWarehouseId, yesterday), Times.Once);
            mock.Mock<IPlatformShipmentTrackerClient>().Verify(c=>c.GetTracking(fakedWarehouseId, now), Times.Once);

        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}