using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Ship;
using ShipWorks.Shipping.Carriers.FedEx.Enums;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Shipping.Request.Manipulators
{
    [TestClass]
    public class FedExHomeDeliveryManipulatorTest
    {
        private FedExHomeDeliveryManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;


        [TestInitialize]
        public void Initialize()
        {
            shipmentEntity = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity
                {
                   Service = (int) FedExServiceType.GroundHomeDelivery,
                   HomeDeliveryType = (int)FedExHomeDeliveryType.Appointment,
                   HomeDeliveryInstructions = "Some instructions",
                   HomeDeliveryDate = DateTime.Parse("1/1/2013")
                }
            };

            nativeRequest = new ProcessShipmentRequest()
            {
                RequestedShipment = new RequestedShipment()
                {
                    SpecialServicesRequested = new ShipmentSpecialServicesRequested() {SpecialServiceTypes = new ShipmentSpecialServiceType[0]}
                }
            };

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject = new FedExHomeDeliveryManipulator();

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
            // setup the test by setting the requested shipment to null
            nativeRequest.RequestedShipment = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment);
        }

        [TestMethod]
        public void Manipulate_SpecialServiceIsNotChanged_WhenServiceIsNotHomeDelivery_Test()
        {
            shipmentEntity.FedEx.Service = (int) FedExServiceType.FirstOvernight;
            nativeRequest.RequestedShipment.SpecialServicesRequested = null;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNull(nativeRequest.RequestedShipment.SpecialServicesRequested);
        }

        [TestMethod]
        public void Manipulate_SpecialServiceIsNotChanged_WhenHomeDeliveryTypeIsNone_Test()
        {
            shipmentEntity.FedEx.HomeDeliveryType = (int)FedExHomeDeliveryType.None;
            nativeRequest.RequestedShipment.SpecialServicesRequested = null;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNull(nativeRequest.RequestedShipment.SpecialServicesRequested);
        }

        [TestMethod]
        public void Manipulate_ServiceTypeArraySizeIsOne_Test()
        {
            nativeRequest.RequestedShipment.SpecialServicesRequested = null;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(1, nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Length);
        }

        [TestMethod]
        public void Manipulate_AddsHomeDeliveryServiceType_WhenServiceTypeArrayIsNull_Test()
        {
            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = null;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(ShipmentSpecialServiceType.HOME_DELIVERY_PREMIUM, nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0]);
        }

        [TestMethod]
        public void Manipulate_AddsHomeDeliveryServiceType_WhenSpecialServicesRequestIsNull_Test()
        {
            nativeRequest.RequestedShipment.SpecialServicesRequested = null;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(ShipmentSpecialServiceType.HOME_DELIVERY_PREMIUM, nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0]);
        }

        [TestMethod]
        public void Manipulate_HomeDeliveryDetailsIsNotNull_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment.SpecialServicesRequested.HomeDeliveryPremiumDetail.HomeDeliveryPremiumType);
        }

        [TestMethod]
        public void Manipulate_PremiumTypeIsDateCertain_Test()
        {
            shipmentEntity.FedEx.HomeDeliveryType = (int) FedExHomeDeliveryType.DateCertain;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(HomeDeliveryPremiumType.DATE_CERTAIN, nativeRequest.RequestedShipment.SpecialServicesRequested.HomeDeliveryPremiumDetail.HomeDeliveryPremiumType);
        }

        [TestMethod]
        public void Manipulate_PremiumTypeIsEvening_Test()
        {
            shipmentEntity.FedEx.HomeDeliveryType = (int)FedExHomeDeliveryType.Evening;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(HomeDeliveryPremiumType.EVENING, nativeRequest.RequestedShipment.SpecialServicesRequested.HomeDeliveryPremiumDetail.HomeDeliveryPremiumType);
        }

        [TestMethod]
        public void Manipulate_PremiumTypeIsAppointment_Test()
        {
            shipmentEntity.FedEx.HomeDeliveryType = (int)FedExHomeDeliveryType.Appointment;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(HomeDeliveryPremiumType.APPOINTMENT, nativeRequest.RequestedShipment.SpecialServicesRequested.HomeDeliveryPremiumDetail.HomeDeliveryPremiumType);
        }

        [TestMethod]
        public void Manipulate_HomeDeliveryPhoneNumber_Test()
        {
            shipmentEntity.FedEx.HomeDeliveryType = (int)FedExHomeDeliveryType.Appointment;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(shipmentEntity.FedEx.HomeDeliveryPhone, nativeRequest.RequestedShipment.SpecialServicesRequested.HomeDeliveryPremiumDetail.PhoneNumber);
        }

        [TestMethod]
        public void Manipulate_HomeDeliveryDateIsAssigned_WhenDeliveryTypeIsDateCertain_Test()
        {
            shipmentEntity.FedEx.HomeDeliveryType = (int)FedExHomeDeliveryType.DateCertain;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(shipmentEntity.FedEx.HomeDeliveryDate, nativeRequest.RequestedShipment.SpecialServicesRequested.HomeDeliveryPremiumDetail.Date);
        }

        [TestMethod]
        public void Manipulate_HomeDeliveryDateSpecifiedIsTrue_WhenDeliveryTypeIsDateCertain_Test()
        {
            shipmentEntity.FedEx.HomeDeliveryType = (int)FedExHomeDeliveryType.DateCertain;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.SpecialServicesRequested.HomeDeliveryPremiumDetail.DateSpecified);
        }

    }
}
