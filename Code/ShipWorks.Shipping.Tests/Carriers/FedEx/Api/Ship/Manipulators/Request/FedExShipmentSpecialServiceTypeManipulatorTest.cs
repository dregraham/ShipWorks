using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    public class FedExShipmentSpecialServiceTypeManipulatorTest
    {
        private readonly ProcessShipmentRequest processShipmentRequest;
        private readonly ShipmentEntity shipment;
        private readonly FedExShipmentSpecialServiceTypeManipulator testObject;

        public FedExShipmentSpecialServiceTypeManipulatorTest()
        {
            AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            // Create a ProcessShipmentRequest type and set the properties the manipulator is interested in
            processShipmentRequest = new ProcessShipmentRequest();
            processShipmentRequest.Ensure(r => r.RequestedShipment)
                .Ensure(rs => rs.SpecialServicesRequested)
                .Ensure(ssr => ssr.SpecialServiceTypes);

            // Create our default shipment entity and initialize the properties our test object will be accessing
            shipment = new ShipmentEntity()
            {
                ShipDate = DateTime.Today.AddDays(1),
                FedEx = new FedExShipmentEntity()
                {
                    SaturdayDelivery = false,
                    Service = (int) FedExServiceType.PriorityOvernight
                }
            };

            testObject = mock.Create<FedExShipmentSpecialServiceTypeManipulator>();
        }

        [Fact]
        public void ShouldApply_ReturnsTrue()
        {
            Assert.True(testObject.ShouldApply(shipment, 0));
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null, new ProcessShipmentRequest(), 0));
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenProcessShipmentRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(new ShipmentEntity(), null, 0));
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedShipment()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            processShipmentRequest.RequestedShipment = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // The requested shipment property should be created now
            Assert.NotNull(processShipmentRequest.RequestedShipment);

        }

        [Fact]
        public void Manipulate_AccountsForNullSpecialServicesRequested()
        {
            // Setup the test by configuring the native request to have a null special services requested 
            // property and re-initialize the carrier request with the updated native request
            processShipmentRequest.RequestedShipment.SpecialServicesRequested = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // The special services requested property should be created now
            Assert.NotNull(processShipmentRequest.RequestedShipment.SpecialServicesRequested);

        }

        [Fact]
        public void Manipulate_AccountsForNullSpecialServiceTypesArray()
        {
            // Setup the test by configuring the native request to have a null special service types requested 
            // property and re-initialize the carrier request with the updated native request
            processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = null;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // The special service types requested property should be created now
            Assert.NotNull(processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);

        }

        [Fact]
        public void Manipulate_AddsFutureDayShipment_WhenShipmentIsNotToday()
        {
            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Grab the shipment special service type array and make sure the future shipment type is present
            List<ShipmentSpecialServiceType> serviceTypes = new List<ShipmentSpecialServiceType>();
            serviceTypes.AddRange(processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);

            Assert.True(serviceTypes.Contains(ShipmentSpecialServiceType.FUTURE_DAY_SHIPMENT));
        }

        [Fact]
        public void Manipulate_DoesNotAddFutureDayShipment_WhenShipmentIsToday()
        {
            // Setup the test by adjusting the ship date to be today
            shipment.ShipDate = DateTime.Today;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Grab the shipment special service type array and make sure the future shipment type is present
            List<ShipmentSpecialServiceType> serviceTypes = new List<ShipmentSpecialServiceType>();
            serviceTypes.AddRange(processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);

            Assert.False(serviceTypes.Contains(ShipmentSpecialServiceType.FUTURE_DAY_SHIPMENT));
        }

        [Fact]
        public void Manipulate_AddsSaturdayPickup_WhenShipmentIsSaturday()
        {
            // Setup the test by adjusting the ship date to be a Saturday
            shipment.ShipDate = GetNext(DateTime.Now, DayOfWeek.Saturday);
            shipment.FedEx.DropoffType = (int) FedExDropoffType.RegularPickup;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Grab the shipment special service type array and make sure the future shipment type is present
            List<ShipmentSpecialServiceType> serviceTypes = new List<ShipmentSpecialServiceType>();
            serviceTypes.AddRange(processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);

            Assert.True(serviceTypes.Contains(ShipmentSpecialServiceType.SATURDAY_PICKUP));
        }

        [Fact]
        public void Manipulate_DoesNotAddSaturdayPickup_WhenShipmentIsSaturdayAndDropoffTypeIsNotRegularPickup()
        {
            // Setup the test by adjusting the ship date to be a Saturday
            shipment.ShipDate = GetNext(DateTime.Now, DayOfWeek.Saturday);
            shipment.FedEx.DropoffType = (int) FedExDropoffType.Station;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Grab the shipment special service type array and make sure the future shipment type is present
            List<ShipmentSpecialServiceType> serviceTypes = new List<ShipmentSpecialServiceType>();
            serviceTypes.AddRange(processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);

            Assert.False(serviceTypes.Contains(ShipmentSpecialServiceType.SATURDAY_PICKUP));
        }

        [Fact]
        public void Manipulate_DoesNotAddSaturdayPickup_WhenShipmentIsNotSaturday()
        {
            // Setup the test by making sure the ship date isn't Saturday
            if (shipment.ShipDate.DayOfWeek == DayOfWeek.Saturday)
            {
                shipment.ShipDate = shipment.ShipDate.AddDays(1);
            }

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Grab the shipment special service type array and make sure the future shipment type is present
            List<ShipmentSpecialServiceType> serviceTypes = new List<ShipmentSpecialServiceType>();
            serviceTypes.AddRange(processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);

            Assert.False(serviceTypes.Contains(ShipmentSpecialServiceType.SATURDAY_PICKUP));
        }


        [Fact]
        public void Manipulate_AddsSaturdayDelivery_WhenSaturdayDeliveryIsAvailable()
        {
            // A little tricky because we have to setup the test by making sure that the shipment 
            // is eligible for Saturday delivery; since this determination is in a class ouside of the
            // test object, we won't be testing each of those every combinations

            // A priority overnight must be shipped on friday to be eligible for Saturday delivery
            shipment.FedEx.Service = (int) FedExServiceType.PriorityOvernight;
            shipment.ShipDate = GetNext(DateTime.Now, DayOfWeek.Friday);
            shipment.FedEx.SaturdayDelivery = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Grab the shipment special service type array and make sure the future shipment type is present
            List<ShipmentSpecialServiceType> serviceTypes = new List<ShipmentSpecialServiceType>();
            serviceTypes.AddRange(processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);

            // The Saturday delivery service type should have been added
            Assert.True(serviceTypes.Contains(ShipmentSpecialServiceType.SATURDAY_DELIVERY));
        }

        [Fact]
        public void Manipulate_DoesNotAddSaturdayDelivery_WhenSaturdayDeliveryIsNotAvailable()
        {
            // A little tricky because we have to setup the test by making sure that the shipment 
            // is NOT eligible for Saturday delivery; since this determination is in a class ouside of the
            // test object, we won't be testing each of those every combinations

            // A priority overnight must be shipped on friday to be eligible for Saturday delivery, so 
            // set the ship date to a Monday (i.e. not eligible for Saturday delivery)
            shipment.FedEx.Service = (int) FedExServiceType.PriorityOvernight;
            shipment.ShipDate = GetNext(DateTime.Now, DayOfWeek.Monday);
            shipment.FedEx.SaturdayDelivery = true;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Grab the shipment special service type array and make sure the future shipment type is present
            List<ShipmentSpecialServiceType> serviceTypes = new List<ShipmentSpecialServiceType>();
            serviceTypes.AddRange(processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);

            // The Saturday delivery service type should NOT have been added
            Assert.False(serviceTypes.Contains(ShipmentSpecialServiceType.SATURDAY_DELIVERY));
        }

        [Fact]
        public void Manipulate_AddsFutureShipmentAndSaturdayPickup_WhenShipDateIsSaturdayInFuture()
        {
            // Setup the test by adjusting the ship date to be a Saturday at least a week away
            shipment.ShipDate = GetNext(DateTime.Now.AddDays(7), DayOfWeek.Saturday);
            shipment.FedEx.DropoffType = (int) FedExDropoffType.RegularPickup;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Grab the shipment special service type array and make sure the future shipment type is present
            List<ShipmentSpecialServiceType> serviceTypes = new List<ShipmentSpecialServiceType>();
            serviceTypes.AddRange(processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);

            Assert.True(serviceTypes.Contains(ShipmentSpecialServiceType.FUTURE_DAY_SHIPMENT));
            Assert.True(serviceTypes.Contains(ShipmentSpecialServiceType.SATURDAY_PICKUP));
        }

        [Fact]
        public void Manipulate_DoesNotAddSaturdayPickup_WhenShipDateIsSaturdayAndDropoffTypeIsNotRegularPickup()
        {
            // Setup the test by adjusting the ship date to be a Saturday at least a week away
            shipment.ShipDate = GetNext(DateTime.Now.AddDays(7), DayOfWeek.Saturday);
            shipment.FedEx.DropoffType = (int) FedExDropoffType.Station;

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Grab the shipment special service type array and make sure the future shipment type is present
            List<ShipmentSpecialServiceType> serviceTypes = new List<ShipmentSpecialServiceType>();
            serviceTypes.AddRange(processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);

            Assert.True(serviceTypes.Contains(ShipmentSpecialServiceType.FUTURE_DAY_SHIPMENT));
            Assert.False(serviceTypes.Contains(ShipmentSpecialServiceType.SATURDAY_PICKUP));
        }

        [Fact]
        public void Manipulate_AddsFutureShipmentAndSaturdayDelivery_WhenFutureSaturdayDeliveryRequested()
        {
            // A little tricky because we have to setup the test by making sure that the shipment 
            // is eligible for Saturday delivery; since this determination is in a class ouside of the
            // test object, we won't be testing each of those every combinations

            // A priority overnight must be shipped on Friday to be eligible for Saturday delivery
            shipment.FedEx.Service = (int) FedExServiceType.PriorityOvernight;
            shipment.FedEx.SaturdayDelivery = true;

            // Setup the test by adjusting the ship date to be a Friday that is at least a week away so the
            // test still passes if it's executed on a Friday (i.e. ensuring it would still be a future shipment)
            shipment.ShipDate = GetNext(DateTime.Now.AddDays(7), DayOfWeek.Friday);

            testObject.Manipulate(shipment, processShipmentRequest, 0);

            // Grab the shipment special service type array and make sure the future shipment type is present
            List<ShipmentSpecialServiceType> serviceTypes = new List<ShipmentSpecialServiceType>();
            serviceTypes.AddRange(processShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);

            Assert.True(serviceTypes.Contains(ShipmentSpecialServiceType.FUTURE_DAY_SHIPMENT));
            Assert.True(serviceTypes.Contains(ShipmentSpecialServiceType.SATURDAY_DELIVERY));
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
