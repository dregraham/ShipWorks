using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExHomeDeliveryManipulatorTest
    {
        private FedExHomeDeliveryManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;


        public FedExHomeDeliveryManipulatorTest()
        {
            shipmentEntity = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity
                {
                    Service = (int)FedExServiceType.GroundHomeDelivery,
                    HomeDeliveryType = (int)FedExHomeDeliveryType.Appointment,
                    HomeDeliveryInstructions = "Some instructions",
                    HomeDeliveryDate = DateTime.Parse("1/1/2013")
                }
            };

            nativeRequest = new ProcessShipmentRequest()
            {
                RequestedShipment = new RequestedShipment()
                {
                    SpecialServicesRequested = new ShipmentSpecialServicesRequested() { SpecialServiceTypes = new ShipmentSpecialServiceType[0] }
                }
            };

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject = new FedExHomeDeliveryManipulator();

        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, null);

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new object());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedShipment()
        {
            // setup the test by setting the requested shipment to null
            nativeRequest.RequestedShipment = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.NotNull(nativeRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_SpecialServiceIsNotChanged_WhenServiceIsNotHomeDelivery()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FirstOvernight;
            nativeRequest.RequestedShipment.SpecialServicesRequested = null;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Null(nativeRequest.RequestedShipment.SpecialServicesRequested);
        }

        [Fact]
        public void Manipulate_SpecialServiceIsNotChanged_WhenHomeDeliveryTypeIsNone()
        {
            shipmentEntity.FedEx.HomeDeliveryType = (int)FedExHomeDeliveryType.None;
            nativeRequest.RequestedShipment.SpecialServicesRequested = null;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Null(nativeRequest.RequestedShipment.SpecialServicesRequested);
        }

        [Fact]
        public void Manipulate_ServiceTypeArraySizeIsOne()
        {
            nativeRequest.RequestedShipment.SpecialServicesRequested = null;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(1, nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
        }

        [Fact]
        public void Manipulate_AddsHomeDeliveryServiceType_WhenServiceTypeArrayIsNull()
        {
            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = null;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(ShipmentSpecialServiceType.HOME_DELIVERY_PREMIUM, nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0]);
        }

        [Fact]
        public void Manipulate_AddsHomeDeliveryServiceType_WhenSpecialServicesRequestIsNull()
        {
            nativeRequest.RequestedShipment.SpecialServicesRequested = null;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(ShipmentSpecialServiceType.HOME_DELIVERY_PREMIUM, nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0]);
        }

        [Fact]
        public void Manipulate_HomeDeliveryDetailsIsNotNull()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.NotNull(nativeRequest.RequestedShipment.SpecialServicesRequested.HomeDeliveryPremiumDetail.HomeDeliveryPremiumType);
        }

        [Fact]
        public void Manipulate_PremiumTypeIsDateCertain()
        {
            shipmentEntity.FedEx.HomeDeliveryType = (int)FedExHomeDeliveryType.DateCertain;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(HomeDeliveryPremiumType.DATE_CERTAIN, nativeRequest.RequestedShipment.SpecialServicesRequested.HomeDeliveryPremiumDetail.HomeDeliveryPremiumType);
        }

        [Fact]
        public void Manipulate_PremiumTypeIsEvening()
        {
            shipmentEntity.FedEx.HomeDeliveryType = (int)FedExHomeDeliveryType.Evening;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(HomeDeliveryPremiumType.EVENING, nativeRequest.RequestedShipment.SpecialServicesRequested.HomeDeliveryPremiumDetail.HomeDeliveryPremiumType);
        }

        [Fact]
        public void Manipulate_PremiumTypeIsAppointment()
        {
            shipmentEntity.FedEx.HomeDeliveryType = (int)FedExHomeDeliveryType.Appointment;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(HomeDeliveryPremiumType.APPOINTMENT, nativeRequest.RequestedShipment.SpecialServicesRequested.HomeDeliveryPremiumDetail.HomeDeliveryPremiumType);
        }

        [Fact]
        public void Manipulate_HomeDeliveryPhoneNumber()
        {
            shipmentEntity.FedEx.HomeDeliveryType = (int)FedExHomeDeliveryType.Appointment;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(shipmentEntity.FedEx.HomeDeliveryPhone, nativeRequest.RequestedShipment.SpecialServicesRequested.HomeDeliveryPremiumDetail.PhoneNumber);
        }

        [Fact]
        public void Manipulate_HomeDeliveryDateIsAssigned_WhenDeliveryTypeIsDateCertain()
        {
            shipmentEntity.FedEx.HomeDeliveryType = (int)FedExHomeDeliveryType.DateCertain;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(shipmentEntity.FedEx.HomeDeliveryDate, nativeRequest.RequestedShipment.SpecialServicesRequested.HomeDeliveryPremiumDetail.Date);
        }

        [Fact]
        public void Manipulate_HomeDeliveryDateSpecifiedIsTrue_WhenDeliveryTypeIsDateCertain()
        {
            shipmentEntity.FedEx.HomeDeliveryType = (int)FedExHomeDeliveryType.DateCertain;

            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.RequestedShipment.SpecialServicesRequested.HomeDeliveryPremiumDetail.DateSpecified);
        }

    }
}
