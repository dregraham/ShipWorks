using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    [TestClass]
    public class FedExSmartPostManipulatorTest
    {
        private FedExSmartPostManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;
        private FedExAccountEntity account;

        [TestInitialize]
        public void Initialize()
        {
            shipmentEntity = BuildFedExShipmentEntity.SetupRequestShipmentEntity();
            shipmentEntity.FedEx.Service = (int)FedExServiceType.SmartPost;
            shipmentEntity.FedEx.SmartPostHubID = "5531";
            
            account = new FedExAccountEntity { AccountNumber = "1234", MeterNumber = "45453", SmartPostHubList = "<Root><HubID>5531</HubID></Root>" };

            nativeRequest = new ProcessShipmentRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            carrierRequest.Setup(r => r.CarrierAccountEntity).Returns(account);

            testObject = new FedExSmartPostManipulator();
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
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenSmartPostHubIDIsNull_Test()
        {
            shipmentEntity.FedEx.SmartPostHubID = null;

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenSmartPostHubIDIsBlank_Test()
        {
            shipmentEntity.FedEx.SmartPostHubID = string.Empty;

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenSmartPostIndiciaTypeIsInvalid_Test()
        {
            carrierRequest.Object.ShipmentEntity.FedEx.SmartPostIndicia = 239955;
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
        public void Manipulate_TotalInsuredValueIsNull_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // The requested shipment property should be created now
            Assert.IsNull(nativeRequest.RequestedShipment.TotalInsuredValue);
        }

        [TestMethod]
        public void Manipulate_SmartPostShipmentDetailIsNull_WhenShipmentTypeIsNotSmartPost_Test()
        {
            shipmentEntity.FedEx.Service = (int) FedExServiceType.FedEx1DayFreight;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNull(nativeRequest.RequestedShipment);
        }

        [TestMethod]
        public void Manipulate_SmartPostHubIDsMatch_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(shipmentEntity.FedEx.SmartPostHubID, nativeRequest.RequestedShipment.SmartPostDetail.HubId);
        }

        [TestMethod]
        public void Manipulate_SmartPostIndiciaTypesCorrect_WhenConverted_Test()
        {
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            shipmentEntity.FedEx.SmartPostIndicia = (int)FedExSmartPostIndicia.BoundPrintedMatter;
            testObject.Manipulate(carrierRequest.Object);
            Assert.AreEqual(SmartPostIndiciaType.PRESORTED_BOUND_PRINTED_MATTER, nativeRequest.RequestedShipment.SmartPostDetail.Indicia);
            Assert.AreEqual(true, nativeRequest.RequestedShipment.SmartPostDetail.IndiciaSpecified);

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            shipmentEntity.FedEx.SmartPostIndicia = (int)FedExSmartPostIndicia.MediaMail;
            testObject.Manipulate(carrierRequest.Object);
            Assert.AreEqual(SmartPostIndiciaType.MEDIA_MAIL, nativeRequest.RequestedShipment.SmartPostDetail.Indicia);
            Assert.AreEqual(true, nativeRequest.RequestedShipment.SmartPostDetail.IndiciaSpecified);

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            shipmentEntity.FedEx.SmartPostIndicia = (int)FedExSmartPostIndicia.ParcelReturn;
            testObject.Manipulate(carrierRequest.Object);
            Assert.AreEqual(SmartPostIndiciaType.PARCEL_RETURN, nativeRequest.RequestedShipment.SmartPostDetail.Indicia);
            Assert.AreEqual(true, nativeRequest.RequestedShipment.SmartPostDetail.IndiciaSpecified);

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            shipmentEntity.FedEx.SmartPostIndicia = (int)FedExSmartPostIndicia.ParcelSelect;
            testObject.Manipulate(carrierRequest.Object);
            Assert.AreEqual(SmartPostIndiciaType.PARCEL_SELECT, nativeRequest.RequestedShipment.SmartPostDetail.Indicia);
            Assert.AreEqual(true, nativeRequest.RequestedShipment.SmartPostDetail.IndiciaSpecified);

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            shipmentEntity.FedEx.SmartPostIndicia = (int)FedExSmartPostIndicia.PresortedStandard;
            testObject.Manipulate(carrierRequest.Object);
            Assert.AreEqual(SmartPostIndiciaType.PRESORTED_STANDARD, nativeRequest.RequestedShipment.SmartPostDetail.Indicia);
            Assert.AreEqual(true, nativeRequest.RequestedShipment.SmartPostDetail.IndiciaSpecified);
        }

        [TestMethod]
        public void Manipulate_SmartPostEndorsementsCorrect_WhenConverted_Test()
        {
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            shipmentEntity.FedEx.SmartPostEndorsement = (int)FedExSmartPostEndorsement.AddressCorrection;
            testObject.Manipulate(carrierRequest.Object);
            Assert.AreEqual(SmartPostAncillaryEndorsementType.ADDRESS_CORRECTION, nativeRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsement);
            Assert.AreEqual(true, nativeRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsementSpecified);

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            shipmentEntity.FedEx.SmartPostEndorsement = (int)FedExSmartPostEndorsement.ChangeService;
            testObject.Manipulate(carrierRequest.Object);
            Assert.AreEqual(SmartPostAncillaryEndorsementType.CHANGE_SERVICE, nativeRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsement);
            Assert.AreEqual(true, nativeRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsementSpecified);

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            shipmentEntity.FedEx.SmartPostEndorsement = (int)FedExSmartPostEndorsement.ForwardingService;
            testObject.Manipulate(carrierRequest.Object);
            Assert.AreEqual(SmartPostAncillaryEndorsementType.FORWARDING_SERVICE, nativeRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsement);
            Assert.AreEqual(true, nativeRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsementSpecified);

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            shipmentEntity.FedEx.SmartPostEndorsement = (int)FedExSmartPostEndorsement.LeaveIfNoResponse;
            testObject.Manipulate(carrierRequest.Object);
            Assert.AreEqual(SmartPostAncillaryEndorsementType.CARRIER_LEAVE_IF_NO_RESPONSE, nativeRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsement);
            Assert.AreEqual(true, nativeRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsementSpecified);

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            shipmentEntity.FedEx.SmartPostEndorsement = (int)FedExSmartPostEndorsement.ReturnService;
            testObject.Manipulate(carrierRequest.Object);
            Assert.AreEqual(SmartPostAncillaryEndorsementType.RETURN_SERVICE, nativeRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsement);
            Assert.AreEqual(true, nativeRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsementSpecified);

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            shipmentEntity.FedEx.SmartPostEndorsement = (int)FedExSmartPostEndorsement.None;
            testObject.Manipulate(carrierRequest.Object);
            Assert.AreEqual(false, nativeRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsementSpecified);
        }

        [TestMethod]
        public void Manipulate_SmartPostConfirmationIsCorrectForIndiciaType_Test()
        {
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            shipmentEntity.FedEx.SmartPostIndicia = (int) FedExSmartPostIndicia.ParcelSelect;
            testObject.Manipulate(carrierRequest.Object);
            Assert.AreEqual(true, shipmentEntity.FedEx.SmartPostConfirmation);

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            shipmentEntity.FedEx.SmartPostConfirmation = false;
            shipmentEntity.FedEx.SmartPostIndicia = (int)FedExSmartPostIndicia.BoundPrintedMatter;
            testObject.Manipulate(carrierRequest.Object);
            Assert.AreEqual(false, shipmentEntity.FedEx.SmartPostConfirmation);

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            shipmentEntity.FedEx.SmartPostConfirmation = false;
            shipmentEntity.FedEx.SmartPostIndicia = (int)FedExSmartPostIndicia.MediaMail;
            testObject.Manipulate(carrierRequest.Object);
            Assert.AreEqual(false, shipmentEntity.FedEx.SmartPostConfirmation);

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            shipmentEntity.FedEx.SmartPostConfirmation = false;
            shipmentEntity.FedEx.SmartPostIndicia = (int)FedExSmartPostIndicia.ParcelReturn;
            testObject.Manipulate(carrierRequest.Object);
            Assert.AreEqual(false, shipmentEntity.FedEx.SmartPostConfirmation);

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            shipmentEntity.FedEx.SmartPostConfirmation = false;
            shipmentEntity.FedEx.SmartPostIndicia = (int)FedExSmartPostIndicia.PresortedStandard;
            testObject.Manipulate(carrierRequest.Object);
            Assert.AreEqual(false, shipmentEntity.FedEx.SmartPostConfirmation);
        }

        [TestMethod]
        public void Manipulate_SmartPostCustomerManifestIdIsNull_WhenSmartPostCustomerManifestIsNull_Test()
        {
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            shipmentEntity.FedEx.SmartPostCustomerManifest = null;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNull(nativeRequest.RequestedShipment.SmartPostDetail.CustomerManifestId);
        }

        [TestMethod]
        public void Manipulate_SmartPostCustomerManifestIdMatchesValue_WhenSmartPostCustomerManifestPresent_Test()
        {
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            shipmentEntity.FedEx.SmartPostCustomerManifest = "asdf1234";

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual("asdf1234", nativeRequest.RequestedShipment.SmartPostDetail.CustomerManifestId);
        }



    }
}
