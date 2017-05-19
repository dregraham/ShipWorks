using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating
{
    public class TelemetricUpsLocalRateClientTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ShipmentEntity shipment;
        private readonly Mock<IUpsRateClient> localRateClient;

        public TelemetricUpsLocalRateClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            localRateClient = mock.CreateMock<IUpsRateClient>();
            Mock<IIndex<UpsRatingMethod, IUpsRateClient>> repo = mock.CreateMock<IIndex<UpsRatingMethod, IUpsRateClient>>();
            repo.Setup(x => x[UpsRatingMethod.LocalOnly])
                .Returns(localRateClient.Object);
            mock.Provide(repo.Object);

            shipment = new ShipmentEntity()
            {
                OriginPostalCode = "12345",
                OriginStateProvCode = "MO",
                ShipPostalCode = "67890",
                ShipStateProvCode = "MO",
                Ups = new UpsShipmentEntity()
                {
                    Packages = {new UpsPackageEntity {DimsLength = 1, DimsHeight = 1, DimsWidth = 1, Weight = 1}}
                }
            };
        }

        [Fact]
        public void GetRates_AddsShipmentPropertiesToTelemetryEvent()
        {
            localRateClient.Setup(c => c.GetRates(shipment))
                .Returns(GenericResult.FromSuccess(new List<UpsServiceRate>()));

            TelemetricUpsLocalRateClient testObject = mock.Create<TelemetricUpsLocalRateClient>();
            testObject.GetRates(shipment);
            
            mock.Mock<ITrackedDurationEvent>().Verify(e => e.AddProperty("Shipment.Origin.StateProvince", shipment.OriginStateProvCode));
            mock.Mock<ITrackedDurationEvent>().Verify(e => e.AddProperty("Shipment.Origin.PostalCode", shipment.OriginPostalCode));
            mock.Mock<ITrackedDurationEvent>().Verify(e => e.AddProperty("Shipment.Destination.StateProvince", shipment.ShipStateProvCode));
            mock.Mock<ITrackedDurationEvent>().Verify(e => e.AddProperty("Shipment.Destination.PostalCode", shipment.ShipPostalCode));
        }

        [Fact]
        public void GetRates_AddsRateResultProperties_WhenRateResultIsEmpty()
        {
            localRateClient.Setup(c => c.GetRates(shipment))
                .Returns(GenericResult.FromSuccess(new List<UpsServiceRate>()));

            TelemetricUpsLocalRateClient testObject = mock.Create<TelemetricUpsLocalRateClient>();
            testObject.GetRates(shipment);

            mock.Mock<ITrackedDurationEvent>().Verify(e => e.AddProperty("Results.Quantity", 0.ToString()));
            mock.Mock<ITrackedDurationEvent>().Verify(e => e.AddProperty("Results.AvailableServices", string.Empty));
        }

        [Fact]
        public void GetRates_AddsRateResultProperties_WhenRateResultHasOneResult()
        {
            List<UpsServiceRate> result = new List<UpsServiceRate>
            {
                new UpsLocalServiceRate(UpsServiceType.UpsGround, "abcd", 12, "123")
            };

            localRateClient.Setup(c => c.GetRates(shipment))
                .Returns(GenericResult.FromSuccess(result));

            TelemetricUpsLocalRateClient testObject = mock.Create<TelemetricUpsLocalRateClient>();
            testObject.GetRates(shipment);

            mock.Mock<ITrackedDurationEvent>().Verify(e => e.AddProperty("Results.Quantity", 1.ToString()));
            mock.Mock<ITrackedDurationEvent>().Verify(e => e.AddProperty("Results.AvailableServices", "UPS Ground"));
        }

        [Fact]
        public void GetRates_AddsRateResultProperties_WhenRateResultHasMultipleResults()
        {
            List<UpsServiceRate> result = new List<UpsServiceRate>
            {
                new UpsLocalServiceRate(UpsServiceType.UpsGround, "abcd", 12, "123"),
                new UpsLocalServiceRate(UpsServiceType.UpsNextDayAir, "abcd", 12, "123"),
                new UpsLocalServiceRate(UpsServiceType.Ups2DayAirAM, "abcd", 12, "123")
            };
            localRateClient.Setup(c => c.GetRates(shipment))
                .Returns(GenericResult.FromSuccess(result));

            TelemetricUpsLocalRateClient testObject = mock.Create<TelemetricUpsLocalRateClient>();
            testObject.GetRates(shipment);

            mock.Mock<ITrackedDurationEvent>().Verify(e => e.AddProperty("Results.Quantity", 3.ToString()));
            mock.Mock<ITrackedDurationEvent>().Verify(e => e.AddProperty("Results.AvailableServices", "UPS Ground,UPS Next Day Air®,UPS 2nd Day Air A.M.®"));
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}