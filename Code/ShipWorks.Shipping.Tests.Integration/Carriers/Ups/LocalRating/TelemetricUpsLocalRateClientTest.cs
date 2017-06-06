using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Carriers.Ups.LocalRating
{
    public class TelemetricUpsLocalRateClientTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ShipmentEntity shipment;
        private readonly Mock<IUpsLocalRateTable> upsLocalRateTable;

        public TelemetricUpsLocalRateClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            
            shipment = new ShipmentEntity()
            {
                OriginPostalCode = "12345",
                OriginStateProvCode = "MO",
                ShipPostalCode = "67890",
                ShipStateProvCode = "MO",
                Order = new OrderEntity()
                {
                    OrderNumber = 42
                },
                Ups = new UpsShipmentEntity()
                {
                    Packages = {new UpsPackageEntity {DimsLength = 1, DimsHeight = 1, DimsWidth = 1, Weight = 1}}
                }
            };

            upsLocalRateTable = mock.Mock<IUpsLocalRateTable>();
        }

        [Fact]
        public void GetRates_AddsShipmentPropertiesToTelemetryEvent()
        {
            upsLocalRateTable.Setup(r => r.CalculateRates(shipment))
                .Returns(GenericResult.FromSuccess<IEnumerable<UpsLocalServiceRate>>(new List<UpsLocalServiceRate>()));

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
            upsLocalRateTable.Setup(r => r.CalculateRates(shipment))
                .Returns(GenericResult.FromSuccess<IEnumerable<UpsLocalServiceRate>>(new List<UpsLocalServiceRate>()));

            TelemetricUpsLocalRateClient testObject = mock.Create<TelemetricUpsLocalRateClient>();
            testObject.GetRates(shipment);

            mock.Mock<ITrackedDurationEvent>().Verify(e => e.AddProperty("Results.Quantity", 0.ToString()));
            mock.Mock<ITrackedDurationEvent>().Verify(e => e.AddProperty("Results.AvailableServices", string.Empty));
        }

        [Fact]
        public void GetRates_AddsRateResultProperties_WhenRateResultHasOneResult()
        {
            List<UpsLocalServiceRate> result = new List<UpsLocalServiceRate>
            {
                new UpsLocalServiceRate(UpsServiceType.UpsGround, "abcd", 12, "123")
            };

            upsLocalRateTable.Setup(r => r.CalculateRates(shipment))
                .Returns(GenericResult.FromSuccess<IEnumerable<UpsLocalServiceRate>>(result));

            TelemetricUpsLocalRateClient testObject = mock.Create<TelemetricUpsLocalRateClient>();
            testObject.GetRates(shipment);

            mock.Mock<ITrackedDurationEvent>().Verify(e => e.AddProperty("Results.Quantity", 1.ToString()));
            mock.Mock<ITrackedDurationEvent>().Verify(e => e.AddProperty("Results.AvailableServices", "UPS Ground"));
        }

        [Fact]
        public void GetRates_AddsRateResultProperties_WhenRateResultHasMultipleResults()
        {
            List<UpsLocalServiceRate> result = new List<UpsLocalServiceRate>
            {
                new UpsLocalServiceRate(UpsServiceType.UpsGround, "abcd", 12, "123"),
                new UpsLocalServiceRate(UpsServiceType.UpsNextDayAir, "abcd", 12, "123"),
                new UpsLocalServiceRate(UpsServiceType.Ups2DayAirAM, "abcd", 12, "123")
            };
            upsLocalRateTable.Setup(r => r.CalculateRates(shipment))
                .Returns(GenericResult.FromSuccess<IEnumerable<UpsLocalServiceRate>>(result));

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