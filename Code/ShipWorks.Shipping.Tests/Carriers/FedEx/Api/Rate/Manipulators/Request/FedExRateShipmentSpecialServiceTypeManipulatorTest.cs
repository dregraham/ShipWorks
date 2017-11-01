using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    public class FedExRateShipmentSpecialServiceTypeManipulatorTest
    {
        private FedExRateShipmentSpecialServiceTypeManipulator testObject;
        private readonly AutoMock mock;
        private ShipmentEntity shipment;
        private readonly DateTime today = new DateTime(2017, 11, 1);

        public FedExRateShipmentSpecialServiceTypeManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            mock.Mock<IDateTimeProvider>()
                .SetupGet(x => x.Today)
                .Returns(today);

            shipment = new ShipmentEntity()
            {
                ShipDate = today.AddDays(1),
                FedEx = new FedExShipmentEntity()
                {
                    SaturdayDelivery = false,
                    Service = (int) FedExServiceType.PriorityOvernight
                }
            };

            testObject = mock.Create<FedExRateShipmentSpecialServiceTypeManipulator>();
        }

        [Fact]
        public void Manipulate_AddsFutureDayShipment_WhenShipmentIsNotToday()
        {
            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Contains(ShipmentSpecialServiceType.FUTURE_DAY_SHIPMENT,
                result.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
        }

        [Fact]
        public void Manipulate_DoesNotAddFutureDayShipment_WhenShipmentIsToday()
        {
            shipment.ShipDate = today;

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.DoesNotContain(ShipmentSpecialServiceType.FUTURE_DAY_SHIPMENT,
                result.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
        }

        [Fact]
        public void Manipulate_AddsSaturdayPickup_WhenShipmentIsSaturday()
        {
            shipment.ShipDate = GetNext(today, DayOfWeek.Saturday);
            shipment.FedEx.DropoffType = (int) FedExDropoffType.RegularPickup;

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Contains(ShipmentSpecialServiceType.SATURDAY_PICKUP,
                result.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
        }

        [Fact]
        public void Manipulate_DoesNotAddSaturdayPickup_WhenShipmentIsSaturdayAndDropoffTypeIsNotRegularPickup()
        {
            shipment.ShipDate = GetNext(today, DayOfWeek.Saturday);
            shipment.FedEx.DropoffType = (int) FedExDropoffType.Station;

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.DoesNotContain(ShipmentSpecialServiceType.SATURDAY_PICKUP,
                result.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
        }

        [Fact]
        public void Manipulate_DoesNotAddSaturdayPickup_WhenShipmentIsNotSaturday()
        {
            // Setup the test by making sure the ship date isn't Saturday
            if (shipment.ShipDate.DayOfWeek == DayOfWeek.Saturday)
            {
                shipment.ShipDate = shipment.ShipDate.AddDays(1);
            }

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.DoesNotContain(ShipmentSpecialServiceType.SATURDAY_PICKUP,
                result.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
        }

        [Fact]
        public void Manipulate_AddsSaturdayDelivery_WhenSaturdayDeliveryIsAvailable()
        {
            // A little tricky because we have to setup the test by making sure that the shipment 
            // is eligible for Saturday delivery; since this determination is in a class outside of the
            // test object, we won't be testing each of those every combinations

            // A priority overnight must be shipped on Friday to be eligible for Saturday delivery
            shipment.FedEx.Service = (int) FedExServiceType.PriorityOvernight;
            shipment.ShipDate = GetNext(today, DayOfWeek.Friday);
            shipment.FedEx.SaturdayDelivery = true;

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Contains(ShipmentSpecialServiceType.SATURDAY_DELIVERY,
                result.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
        }

        [Fact]
        public void Manipulate_DoesNotAddSaturdayDelivery_WhenSaturdayDeliveryIsNotAvailable()
        {
            // A little tricky because we have to setup the test by making sure that the shipment 
            // is NOT eligible for Saturday delivery; since this determination is in a class outside of the
            // test object, we won't be testing each of those every combinations

            // A priority overnight must be shipped on Friday to be eligible for Saturday delivery, so 
            // set the ship date to a Monday (i.e. not eligible for Saturday delivery)
            shipment.FedEx.Service = (int) FedExServiceType.PriorityOvernight;
            shipment.ShipDate = GetNext(today, DayOfWeek.Monday);
            shipment.FedEx.SaturdayDelivery = true;

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.DoesNotContain(ShipmentSpecialServiceType.SATURDAY_PICKUP,
                result.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
        }

        [Fact]
        public void Manipulate_AddsFutureShipmentAndSaturdayPickup_WhenShipDateIsSaturdayInFuture()
        {
            // Setup the test by adjusting the ship date to be a Saturday at least a week away
            shipment.ShipDate = GetNext(today.AddDays(7), DayOfWeek.Saturday);
            shipment.FedEx.DropoffType = (int) FedExDropoffType.RegularPickup;

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Contains(ShipmentSpecialServiceType.FUTURE_DAY_SHIPMENT,
                result.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
            Assert.Contains(ShipmentSpecialServiceType.SATURDAY_PICKUP,
                result.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
        }

        [Fact]
        public void Manipulate_DoesNotAddSaturdayPickup_WhenShipDateIsSaturdayAndDropoffTypeIsNotRegularPickup()
        {
            // Setup the test by adjusting the ship date to be a Saturday at least a week away
            shipment.ShipDate = GetNext(today.AddDays(7), DayOfWeek.Saturday);
            shipment.FedEx.DropoffType = (int) FedExDropoffType.Station;

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Contains(ShipmentSpecialServiceType.FUTURE_DAY_SHIPMENT,
                result.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
            Assert.DoesNotContain(ShipmentSpecialServiceType.SATURDAY_PICKUP,
                result.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
        }

        [Fact]
        public void Manipulate_AddsFutureShipmentAndSaturdayDelivery_WhenFutureSaturdayDeliveryRequested()
        {
            // A little tricky because we have to setup the test by making sure that the shipment 
            // is eligible for Saturday delivery; since this determination is in a class outside of the
            // test object, we won't be testing each of those every combinations

            // A priority overnight must be shipped on Friday to be eligible for Saturday delivery
            shipment.FedEx.Service = (int) FedExServiceType.PriorityOvernight;
            shipment.FedEx.SaturdayDelivery = true;

            // Setup the test by adjusting the ship date to be a Friday that is at least a week away so the
            // test still passes if it's executed on a Friday (i.e. ensuring it would still be a future shipment)
            shipment.ShipDate = GetNext(today.AddDays(7), DayOfWeek.Friday);

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Contains(ShipmentSpecialServiceType.FUTURE_DAY_SHIPMENT,
                result.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
            Assert.Contains(ShipmentSpecialServiceType.SATURDAY_DELIVERY,
                result.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
        }

        /// <summary>
        /// Gets the date of the next occurrence of the given day of the week.
        /// </summary>
        /// <param name="from">The date to start at.</param>
        /// <param name="dayOfWeek">The day of week.</param>
        /// <returns>The DateTime of the next occurrence of the given DayOfWeek.</returns>
        private DateTime GetNext(DateTime from, DayOfWeek dayOfWeek)
        {
            DateTime date = new DateTime(from.Ticks);

            while (date.DayOfWeek != dayOfWeek)
            {
                date = date.AddDays(1);
            }

            return date;
        }
    }
}
