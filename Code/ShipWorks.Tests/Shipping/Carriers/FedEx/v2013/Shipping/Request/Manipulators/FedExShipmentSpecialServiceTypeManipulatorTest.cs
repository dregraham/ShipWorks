﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013;
using Moq;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Shipping.Request.Manipulators
{
    [TestClass]
    public class FedExShipmentSpecialServiceTypeManipulatorTest
    {
        private FedExShipmentSpecialServiceTypeManipulator testObject;
        private Mock<CarrierRequest> carrierRequest;
        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        [TestInitialize]
        public void Initialize()
        {
            // Create a ProcessShipmentRequest type and set the properties the manipulator is interested in
            nativeRequest = new ProcessShipmentRequest()
            {
                RequestedShipment = new RequestedShipment()
                {
                    SpecialServicesRequested = new ShipmentSpecialServicesRequested()
                    {
                        SpecialServiceTypes = new ShipmentSpecialServiceType[0]
                    }
                }
            };

            // Create our default shipment entity and initialize the properties our test object will be accessing
            shipmentEntity = new ShipmentEntity()
            {
                ShipDate = DateTime.Today.AddDays(1),
                FedEx = new FedExShipmentEntity()
                {
                    SaturdayDelivery = false,
                    Service = (int)FedExServiceType.PriorityOvernight
                }
            };


            // Setup the carrier request's NativeRequest property to return the ProcessShipmentRequest object
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject = new FedExShipmentSpecialServiceTypeManipulator();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            testObject.Manipulate(null);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull_Test()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, null);

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new CancelPendingShipmentRequest());

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        public void Manipulate_AccountsForNullRequestedShipment_Test()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            nativeRequest.RequestedShipment = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested shipment property should be created now
            Assert.IsNotNull(nativeRequest.RequestedShipment);

        }

        [TestMethod]
        public void Manipulate_AccountsForNullSpecialServicesRequested_Test()
        {
            // Setup the test by configuring the native request to have a null special services requested 
            // property and re-initialize the carrier request with the updated native request
            nativeRequest.RequestedShipment.SpecialServicesRequested = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The special services requested property should be created now
            Assert.IsNotNull(nativeRequest.RequestedShipment.SpecialServicesRequested);

        }

        [TestMethod]
        public void Manipulate_AccountsForNullSpecialServiceTypesArray_Test()
        {
            // Setup the test by configuring the native request to have a null special service types requested 
            // property and re-initialize the carrier request with the updated native request
            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The special service types requested property should be created now
            Assert.IsNotNull(nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);

        }

        [TestMethod]
        public void Manipulate_AddsFutureDayShipment_WhenShipmentIsNotToday_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Grab the shipment special service type array and make sure the future shipment type is present
            List<ShipmentSpecialServiceType> serviceTypes = new List<ShipmentSpecialServiceType>();
            serviceTypes.AddRange(((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);

            Assert.IsTrue(serviceTypes.Contains(ShipmentSpecialServiceType.FUTURE_DAY_SHIPMENT));
        }

        [TestMethod]
        public void Manipulate_DoesNotAddFutureDayShipment_WhenShipmentIsToday_Test()
        {
            // Setup the test by adjusting the ship date to be today
            shipmentEntity.ShipDate = DateTime.Today;

            testObject.Manipulate(carrierRequest.Object);

            // Grab the shipment special service type array and make sure the future shipment type is present
            List<ShipmentSpecialServiceType> serviceTypes = new List<ShipmentSpecialServiceType>();
            serviceTypes.AddRange(((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);

            Assert.IsFalse(serviceTypes.Contains(ShipmentSpecialServiceType.FUTURE_DAY_SHIPMENT));
        }

        [TestMethod]
        public void Manipulate_AddsSaturdayPickup_WhenShipmentIsSaturday_Test()
        {
            // Setup the test by adjusting the ship date to be a Saturday
            shipmentEntity.ShipDate = GetNext(DateTime.Now, DayOfWeek.Saturday);

            testObject.Manipulate(carrierRequest.Object);

            // Grab the shipment special service type array and make sure the future shipment type is present
            List<ShipmentSpecialServiceType> serviceTypes = new List<ShipmentSpecialServiceType>();
            serviceTypes.AddRange(((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);

            Assert.IsTrue(serviceTypes.Contains(ShipmentSpecialServiceType.SATURDAY_PICKUP));
        }

        [TestMethod]
        public void Manipulate_DoesNotAddSaturdayPickup_WhenShipmentIsNotSaturday_Test()
        {
            // Setup the test by making sure the ship date isn't Saturday
            if (shipmentEntity.ShipDate.DayOfWeek == DayOfWeek.Saturday)
            {
                shipmentEntity.ShipDate = shipmentEntity.ShipDate.AddDays(1);
            }

            testObject.Manipulate(carrierRequest.Object);

            // Grab the shipment special service type array and make sure the future shipment type is present
            List<ShipmentSpecialServiceType> serviceTypes = new List<ShipmentSpecialServiceType>();
            serviceTypes.AddRange(((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);

            Assert.IsFalse(serviceTypes.Contains(ShipmentSpecialServiceType.SATURDAY_PICKUP));
        }


        [TestMethod]
        public void Manipulate_AddsSaturdayDelivery_WhenSaturdayDeliveryIsAvailable_Test()
        {
            // A little tricky because we have to setup the test by making sure that the shipment 
            // is eligible for Saturday delivery; since this determination is in a class ouside of the
            // test object, we won't be testing each of those every combinations

            // A priority overnight must be shipped on friday to be eligible for Saturday delivery
            shipmentEntity.FedEx.Service = (int)FedExServiceType.PriorityOvernight;
            shipmentEntity.ShipDate = GetNext(DateTime.Now, DayOfWeek.Friday);
            shipmentEntity.FedEx.SaturdayDelivery = true;

            testObject.Manipulate(carrierRequest.Object);

            // Grab the shipment special service type array and make sure the future shipment type is present
            List<ShipmentSpecialServiceType> serviceTypes = new List<ShipmentSpecialServiceType>();
            serviceTypes.AddRange(((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);

            // The Saturday delivery service type should have been added
            Assert.IsTrue(serviceTypes.Contains(ShipmentSpecialServiceType.SATURDAY_DELIVERY));
        }

        [TestMethod]
        public void Manipulate_DoesNotAddSaturdayDelivery_WhenSaturdayDeliveryIsNotAvailable_Test()
        {
            // A little tricky because we have to setup the test by making sure that the shipment 
            // is NOT eligible for Saturday delivery; since this determination is in a class ouside of the
            // test object, we won't be testing each of those every combinations

            // A priority overnight must be shipped on friday to be eligible for Saturday delivery, so 
            // set the ship date to a Monday (i.e. not eligible for Saturday delivery)
            shipmentEntity.FedEx.Service = (int)FedExServiceType.PriorityOvernight;
            shipmentEntity.ShipDate = GetNext(DateTime.Now, DayOfWeek.Monday);
            shipmentEntity.FedEx.SaturdayDelivery = true;

            testObject.Manipulate(carrierRequest.Object);

            // Grab the shipment special service type array and make sure the future shipment type is present
            List<ShipmentSpecialServiceType> serviceTypes = new List<ShipmentSpecialServiceType>();
            serviceTypes.AddRange(((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);

            // The Saturday delivery service type should NOT have been added
            Assert.IsFalse(serviceTypes.Contains(ShipmentSpecialServiceType.SATURDAY_DELIVERY));
        }

        [TestMethod]
        public void Manipulate_AddsFutureShipmentAndSaturdayPickup_WhenShipDateIsSaturdayInFuture_Test()
        {
            // Setup the test by adjusting the ship date to be a Saturday at least a week away
            shipmentEntity.ShipDate = GetNext(DateTime.Now.AddDays(7), DayOfWeek.Saturday);

            testObject.Manipulate(carrierRequest.Object);

            // Grab the shipment special service type array and make sure the future shipment type is present
            List<ShipmentSpecialServiceType> serviceTypes = new List<ShipmentSpecialServiceType>();
            serviceTypes.AddRange(((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);

            Assert.IsTrue(serviceTypes.Contains(ShipmentSpecialServiceType.FUTURE_DAY_SHIPMENT));
            Assert.IsTrue(serviceTypes.Contains(ShipmentSpecialServiceType.SATURDAY_PICKUP));
        }

        [TestMethod]
        public void Manipulate_AddsFutureShipmentAndSaturdayDelivery_WhenFutureSaturdayDeliveryRequested_Test()
        {
            // A little tricky because we have to setup the test by making sure that the shipment 
            // is eligible for Saturday delivery; since this determination is in a class ouside of the
            // test object, we won't be testing each of those every combinations

            // A priority overnight must be shipped on Friday to be eligible for Saturday delivery
            shipmentEntity.FedEx.Service = (int)FedExServiceType.PriorityOvernight;
            shipmentEntity.FedEx.SaturdayDelivery = true;

            // Setup the test by adjusting the ship date to be a Friday that is at least a week away so the
            // test still passes if it's executed on a Friday (i.e. ensuring it would still be a future shipment)
            shipmentEntity.ShipDate = GetNext(DateTime.Now.AddDays(7), DayOfWeek.Friday);

            testObject.Manipulate(carrierRequest.Object);

            // Grab the shipment special service type array and make sure the future shipment type is present
            List<ShipmentSpecialServiceType> serviceTypes = new List<ShipmentSpecialServiceType>();
            serviceTypes.AddRange(((ProcessShipmentRequest)carrierRequest.Object.NativeRequest).RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);

            Assert.IsTrue(serviceTypes.Contains(ShipmentSpecialServiceType.FUTURE_DAY_SHIPMENT));
            Assert.IsTrue(serviceTypes.Contains(ShipmentSpecialServiceType.SATURDAY_DELIVERY));
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
