using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.ServiceFilters;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating.ServiceFilters
{
    public class ShipmentServiceFilterTest
    {
        private readonly ShipmentServiceFilter testObject;

        public ShipmentServiceFilterTest()
        {
            testObject = new ShipmentServiceFilter();
        }

        [Fact]
        public void GetEligibleServices_ReturnsAllButUpsGroundAnd3DaySelect_WhenShipDateIsSaturday()
        {
            UpsShipmentEntity upsShipment =
                new UpsShipmentEntity {Shipment = new ShipmentEntity {ShipDate = new DateTime(2017, 5, 13)}};

            Assert.DoesNotContain(UpsServiceType.UpsGround,
                testObject.GetEligibleServices(upsShipment,
                    new[] {UpsServiceType.UpsGround, UpsServiceType.Ups2DayAir, UpsServiceType.Ups3DaySelect}));

            Assert.DoesNotContain(UpsServiceType.Ups3DaySelect,
                testObject.GetEligibleServices(upsShipment,
                    new[] { UpsServiceType.UpsGround, UpsServiceType.Ups2DayAir, UpsServiceType.Ups3DaySelect }));
        }

        [Fact]
        public void GetEligibleServices_ReturnsAllServices_WhenShipDateIsNotSaturday()
        {
            UpsShipmentEntity upsShipment =
                new UpsShipmentEntity { Shipment = new ShipmentEntity { ShipDate = new DateTime(2017, 5, 14) } };

            UpsServiceType[] services = {UpsServiceType.UpsGround, UpsServiceType.Ups2DayAir, UpsServiceType.Ups3DaySelect};

            Assert.Equal(services, testObject.GetEligibleServices(upsShipment, services));
        }
    }
}