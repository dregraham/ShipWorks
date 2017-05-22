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

        public TelemetricUpsLocalRateClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            
            Mock<IUpsRateClient> rateClient = mock.Mock<IUpsRateClient>();
            Mock<IIndex<UpsRatingMethod, IUpsRateClient>> repo = mock.MockRepository.Create<IIndex<UpsRatingMethod, IUpsRateClient>>();
            repo.Setup(x => x[It.IsAny<UpsRatingMethod>()])
                .Returns(rateClient.Object);
            mock.Provide(repo.Object);
        }

        [Fact]
        public void GetRates_AddsShipmentPropertiesToTelemetryEvent()
        {
            UpsShipmentEntity shipment = new UpsShipmentEntity
            {
                Shipment = new ShipmentEntity
                {
                    OriginPostalCode = "12345",
                    OriginStateProvCode = "MO",
                    ShipPostalCode = "67890",
                    ShipStateProvCode = "MO"
                },
                Packages = {new UpsPackageEntity {DimsLength = 1, DimsHeight = 1, DimsWidth = 1, Weight = 1}}
            };

            TelemetricUpsLocalRateClient testObject = mock.Create<TelemetricUpsLocalRateClient>();
            testObject.GetRates(shipment.Shipment);

            mock.Mock<ITrackedDurationEvent>().Verify(e => e.AddProperty("Shipment.Origin.StateProvince", shipment.Shipment.OriginStateProvCode));
            mock.Mock<ITrackedDurationEvent>().Verify(e => e.AddProperty("Shipment.Origin.PostalCode", shipment.Shipment.OriginPostalCode));
            mock.Mock<ITrackedDurationEvent>().Verify(e => e.AddProperty("Shipment.Destination.StateProvince", shipment.Shipment.ShipStateProvCode));
            mock.Mock<ITrackedDurationEvent>().Verify(e => e.AddProperty("Shipment.Destination.PostalCode", shipment.Shipment.ShipPostalCode));
        }

        [Fact]
        public void GetRates_AddsRateResultProperties_WhenRateResultIsEmpty()
        {
            UpsShipmentEntity shipment = new UpsShipmentEntity
            {
                Shipment = new ShipmentEntity
                {
                    OriginPostalCode = "12345",
                    OriginStateProvCode = "MO",
                    ShipPostalCode = "67890",
                    ShipStateProvCode = "MO"
                },
                Packages = { new UpsPackageEntity { DimsLength = 1, DimsHeight = 1, DimsWidth = 1, Weight = 1 } }
            };

            TelemetricUpsLocalRateClient testObject = mock.Create<TelemetricUpsLocalRateClient>();
            testObject.GetRates(shipment.Shipment);

            mock.Mock<ITrackedDurationEvent>().Verify(e => e.AddProperty("Results.Quantity", 0.ToString()));
            mock.Mock<ITrackedDurationEvent>().Verify(e => e.AddProperty("Results.AvailableServices", string.Empty));
        }

        [Fact]
        public void GetRates_AddsRateResultProperties_WhenRateResultHasOneResult()
        {
            UpsShipmentEntity shipment = new UpsShipmentEntity
            {
                Shipment = new ShipmentEntity
                {
                    OriginPostalCode = "12345",
                    OriginStateProvCode = "MO",
                    ShipPostalCode = "67890",
                    ShipStateProvCode = "MO",
                    Order = new OrderEntity(42)
                },
                Packages = { new UpsPackageEntity { DimsLength = 1, DimsHeight = 1, DimsWidth = 1, Weight = 1 } }
            };

            IEnumerable<UpsLocalServiceRate> result = new List<UpsLocalServiceRate>() {new UpsLocalServiceRate(UpsServiceType.UpsGround, "abcd", 12, "123")};

            mock.Mock<IUpsLocalRateTable>().Setup(t => t.CalculateRates(shipment.Shipment)).Returns(GenericResult.FromSuccess(result));

            TelemetricUpsLocalRateClient testObject = mock.Create<TelemetricUpsLocalRateClient>();
            testObject.GetRates(shipment.Shipment);

            mock.Mock<ITrackedDurationEvent>().Verify(e => e.AddProperty("Results.Quantity", 1.ToString()));
            mock.Mock<ITrackedDurationEvent>().Verify(e => e.AddProperty("Results.AvailableServices", "UPS Ground"));
        }

        [Fact]
        public void GetRates_AddsRateResultProperties_WhenRateResultHasMultipleResults()
        {
            UpsShipmentEntity shipment = new UpsShipmentEntity
            {
                Shipment = new ShipmentEntity
                {
                    OriginPostalCode = "12345",
                    OriginStateProvCode = "MO",
                    ShipPostalCode = "67890",
                    ShipStateProvCode = "MO",
                    Order = new OrderEntity(42)
                },
                Packages = { new UpsPackageEntity { DimsLength = 1, DimsHeight = 1, DimsWidth = 1, Weight = 1 } }
            };

            IEnumerable<UpsLocalServiceRate> result = new List<UpsLocalServiceRate>
            {
                new UpsLocalServiceRate(UpsServiceType.UpsGround, "abcd", 12, "123"),
                new UpsLocalServiceRate(UpsServiceType.UpsNextDayAir, "abcd", 12, "123"),
                new UpsLocalServiceRate(UpsServiceType.Ups2DayAirAM, "abcd", 12, "123")
            };

            mock.Mock<IUpsLocalRateTable>().Setup(t => t.CalculateRates(shipment.Shipment)).Returns(GenericResult.FromSuccess(result));

            TelemetricUpsLocalRateClient testObject = mock.Create<TelemetricUpsLocalRateClient>();
            testObject.GetRates(shipment.Shipment);

            mock.Mock<ITrackedDurationEvent>().Verify(e => e.AddProperty("Results.Quantity", 3.ToString()));
            mock.Mock<ITrackedDurationEvent>().Verify(e => e.AddProperty("Results.AvailableServices", "UPS Ground,UPS Next Day Air®,UPS 2nd Day Air A.M.®"));
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}