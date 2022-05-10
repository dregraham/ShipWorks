using Autofac.Extras.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Threading.Tasks;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Shipping.Tracking.DTO;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Tests.Tracking
{
    public class PlatformShipmentTrackerClientTest : IDisposable
    {
        AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks();

        [Fact]
        public async Task SendShipment_CreatesRequestWithFactory()
        {
            var testObject = mock.Create<PlatformShipmentTrackerClient>();
            await testObject.SendShipment("t", "c", "w").ConfigureAwait(false);
            mock.Mock<IWarehouseRequestFactory>()
                .Verify(f=>f.Create(WarehouseEndpoints.Tracking, Method.POST, 
                    It.Is<TrackingRequest>(r=>r.TrackingNumber=="t" && r.CarrierCode=="c" && r.WarehouseId=="w")));
        }

        [Fact]
        public async Task SendShipment_MakesRequestFromFactory()
        {
            var testObject = mock.Create<PlatformShipmentTrackerClient>();
            var request = mock.FromFactory<IWarehouseRequestFactory>().Mock(f => f.Create(AnyString, Method.POST, AnyObject));
            
            await testObject.SendShipment("t", "c", "w").ConfigureAwait(false);

            mock.Mock<IWarehouseRequestClient>()
                .Verify(c=>c.MakeRequest(request.Object, "TrackingSendShipment"), Times.Once);
        }

        [Fact]
        public async Task GetShipments_CreatesRequestWithFactory()
        {
            DateTime lastUpdateDate = DateTime.Now;
            
            var testObject = mock.Create<PlatformShipmentTrackerClient>();
            await testObject.GetTracking("w", lastUpdateDate).ConfigureAwait(false);
            mock.Mock<IWarehouseRequestFactory>()
                .Verify(f => f.Create(WarehouseEndpoints.GetTrackingUpdatesAfter(lastUpdateDate), Method.GET, null), Times.Once);
        }

        [Fact]
        public async Task GetTracking_MakesRequestFromFactory()
        { 
            DateTime lastUpdateDate = DateTime.Now;

            var testObject = mock.Create<PlatformShipmentTrackerClient>();
            var request = mock.FromFactory<IWarehouseRequestFactory>().Mock(f => f.Create(AnyString, Method.GET, null));
            
            await testObject.GetTracking("w", lastUpdateDate).ConfigureAwait(false);
            
            mock.Mock<IWarehouseRequestClient>()
                .Verify(c=>c.MakeRequest<List<TrackingNotification>>(request.Object, "GetTracking"), Times.Once);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}