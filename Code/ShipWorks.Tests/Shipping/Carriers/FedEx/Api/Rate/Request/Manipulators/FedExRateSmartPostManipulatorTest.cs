using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators
{
    [TestClass]
    public class FedExRateSmartPostManipulatorTest
    {
        private FedExRateSmartPostManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private RateRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        [TestInitialize]
        public void Initialize()
        {
            shipmentEntity = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity
                {
                    SmartPostHubID = "5015",
                    SmartPostIndicia = (int) FedExSmartPostIndicia.MediaMail,
                    SmartPostEndorsement = (int) FedExSmartPostEndorsement.ReturnService
                }
            };

            nativeRequest = new RateRequest
            {
                RequestedShipment = new RequestedShipment
                {
                    RequestedPackageLineItems = new RequestedPackageLineItem[0],
                    TotalInsuredValue = new Money()
                }
            };

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject = new FedExRateSmartPostManipulator();
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
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotRateRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new RateReply());

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        public void Manipulate_AccountsForNullRequestedShipment_Test()
        {
            // Setup the native request to have a null requested shipment
            nativeRequest.RequestedShipment = null;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment);
        }

        [TestMethod]
        public void Manipulate_AccountsForNullPackageLineItems_Test()
        {
            // Setup the native request to have a null array of line items
            nativeRequest.RequestedShipment.RequestedPackageLineItems = null;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment.RequestedPackageLineItems);
            Assert.AreEqual(0, nativeRequest.RequestedShipment.RequestedPackageLineItems.Length);
        }

        [TestMethod]
        public void Manipulate_AccountsForNullTotalInsuredValue_Test()
        {
            // Setup the native request to have a null total insured value
            nativeRequest.RequestedShipment.TotalInsuredValue = null;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment.TotalInsuredValue);
        }

        [TestMethod]
        public void Manipulate_AssignsSmartPostDetail_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment.SmartPostDetail);
        }

        [TestMethod]
        public void Manipulate_SmartPostDetailIsNull_WhenHubIdIsNull_Test()
        {
            // This should set the smart post hub ID to a null value
            shipmentEntity.FedEx = new FedExShipmentEntity();

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNull(nativeRequest.RequestedShipment.SmartPostDetail);
        }

        [TestMethod]
        public void Manipulate_SmartPostDetailIsNull_WhenFedExShipmentIsNull_Test()
        {
            shipmentEntity.FedEx = null;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNull(nativeRequest.RequestedShipment.SmartPostDetail);
        }

        [TestMethod]
        public void Manipulate_SmartPostDetailIsNull_WhenShipmentIsNull_Test()
        {
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), null, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNull(nativeRequest.RequestedShipment.SmartPostDetail);
        }

        [TestMethod]
        public void Manipulate_SetsSmartHubId_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Since this is delegated to another object that is a static utility class, we
            // can check that hub ID has a non-empty string
            Assert.IsFalse(string.IsNullOrEmpty(nativeRequest.RequestedShipment.SmartPostDetail.HubId));
        }

        [TestMethod]
        public void Manipulate_IndiciaTypeIsParcelSelect_Test()
        {
            shipmentEntity.FedEx.SmartPostIndicia = (int) FedExSmartPostIndicia.ParcelSelect;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(SmartPostIndiciaType.PARCEL_SELECT, nativeRequest.RequestedShipment.SmartPostDetail.Indicia);
        }

        [TestMethod]
        public void Manipulate_IndiciaTypeIsMediaMail_Test()
        {
            shipmentEntity.FedEx.SmartPostIndicia = (int)FedExSmartPostIndicia.MediaMail;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(SmartPostIndiciaType.MEDIA_MAIL, nativeRequest.RequestedShipment.SmartPostDetail.Indicia);
        }

        [TestMethod]
        public void Manipulate_IndiciaTypeIsPresortedBoundPrintedMatter_Test()
        {
            shipmentEntity.FedEx.SmartPostIndicia = (int)FedExSmartPostIndicia.BoundPrintedMatter;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(SmartPostIndiciaType.PRESORTED_BOUND_PRINTED_MATTER, nativeRequest.RequestedShipment.SmartPostDetail.Indicia);
        }

        [TestMethod]
        public void Manipulate_IndiciaTypeIsPresortedStandard_Test()
        {
            shipmentEntity.FedEx.SmartPostIndicia = (int)FedExSmartPostIndicia.PresortedStandard;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(SmartPostIndiciaType.PRESORTED_STANDARD, nativeRequest.RequestedShipment.SmartPostDetail.Indicia);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Manipulate_ThrowsInvalidOperationException_WhenUnrecognizedIndiciaTypeIsProvided_Test()
        {
            shipmentEntity.FedEx.SmartPostIndicia = 97;

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        public void Manipulate_IndiciaSpecifiedIsTrue_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.SmartPostDetail.IndiciaSpecified);
        }

        [TestMethod]
        public void Manipulate_AncillaryEndorsementIsAddressCorrection_Test()
        {
            shipmentEntity.FedEx.SmartPostEndorsement = (int) FedExSmartPostEndorsement.AddressCorrection;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(SmartPostAncillaryEndorsementType.ADDRESS_CORRECTION, nativeRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsement);
        }

        [TestMethod]
        public void Manipulate_AncillaryEndorsementSpecifiedIsTrue_WhenEndorsementTypeIsAddressCorrection_Test()
        {
            shipmentEntity.FedEx.SmartPostEndorsement = (int)FedExSmartPostEndorsement.AddressCorrection;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsementSpecified);
        }

        [TestMethod]
        public void Manipulate_AncillaryEndorsementIsChangeService_Test()
        {
            shipmentEntity.FedEx.SmartPostEndorsement = (int)FedExSmartPostEndorsement.ChangeService;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(SmartPostAncillaryEndorsementType.CHANGE_SERVICE, nativeRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsement);
        }

        [TestMethod]
        public void Manipulate_AncillaryEndorsementSpecifiedIsTrue_WhenEndorsementTypeIsChangeService_Test()
        {
            shipmentEntity.FedEx.SmartPostEndorsement = (int)FedExSmartPostEndorsement.ChangeService;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsementSpecified);
        }

        [TestMethod]
        public void Manipulate_AncillaryEndorsementIsForwardingService_Test()
        {
            shipmentEntity.FedEx.SmartPostEndorsement = (int)FedExSmartPostEndorsement.ForwardingService;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(SmartPostAncillaryEndorsementType.FORWARDING_SERVICE, nativeRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsement);
        }

        [TestMethod]
        public void Manipulate_AncillaryEndorsementSpecifiedIsTrue_WhenEndorsementTypeIsForwardingService_Test()
        {
            shipmentEntity.FedEx.SmartPostEndorsement = (int)FedExSmartPostEndorsement.ForwardingService;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsementSpecified);
        }

        [TestMethod]
        public void Manipulate_AncillaryEndorsementIsLeaveIfNoResponse_Test()
        {
            shipmentEntity.FedEx.SmartPostEndorsement = (int)FedExSmartPostEndorsement.LeaveIfNoResponse;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(SmartPostAncillaryEndorsementType.CARRIER_LEAVE_IF_NO_RESPONSE, nativeRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsement);
        }

        [TestMethod]
        public void Manipulate_AncillaryEndorsementSpecifiedIsTrue_WhenEndorsementTypeIsLeaveIfNoResponse_Test()
        {
            shipmentEntity.FedEx.SmartPostEndorsement = (int)FedExSmartPostEndorsement.LeaveIfNoResponse;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsementSpecified);
        }

        [TestMethod]
        public void Manipulate_AncillaryEndorsementIsReturnService_Test()
        {
            shipmentEntity.FedEx.SmartPostEndorsement = (int)FedExSmartPostEndorsement.ReturnService;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(SmartPostAncillaryEndorsementType.RETURN_SERVICE, nativeRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsement);
        }

        [TestMethod]
        public void Manipulate_AncillaryEndorsementSpecifiedIsTrue_WhenEndorsementTypeIsReturnService_Test()
        {
            shipmentEntity.FedEx.SmartPostEndorsement = (int)FedExSmartPostEndorsement.ReturnService;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsementSpecified);
        }
        
        [TestMethod]
        public void Manipulate_AncillaryEndorsementSpecifiedIsFalse_WhenEndorsementTypeIsNone_Test()
        {
            shipmentEntity.FedEx.SmartPostEndorsement = (int)FedExSmartPostEndorsement.None;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsFalse(nativeRequest.RequestedShipment.SmartPostDetail.AncillaryEndorsementSpecified);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Manipulate_ThrowsInvalidOperationException_WhenUnrecognizedEndorsementIsProvided_Test()
        {
            shipmentEntity.FedEx.SmartPostEndorsement= 97;

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        public void Manipulate_ShipmentTotalInsuredValueIsZero_Test()
        {
            // Set the amount to 43 to prove that it gets set back to 0
            nativeRequest.RequestedShipment.TotalInsuredValue.Amount = 43;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(0, nativeRequest.RequestedShipment.TotalInsuredValue.Amount);
        }

        [TestMethod]
        public void Manipulate_EachPackageTotalInsuredAmountIsZero_Test()
        {
            // Create a few packages in the FedEx shipment to work with
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity() { DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity() { DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity() { DimsHeight = 2, DimsWidth = 2, DimsLength = 2 });

            testObject.Manipulate(carrierRequest.Object);

            for (int i = 0; i < shipmentEntity.FedEx.Packages.Count; i++)
            {
                Assert.AreEqual(0, nativeRequest.RequestedShipment.RequestedPackageLineItems[i].InsuredValue.Amount);
            }
        }

        [TestMethod]
        public void Manipulate_ServiceTypeIsSmartPost_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(ServiceType.SMART_POST, nativeRequest.RequestedShipment.ServiceType);
        }

        [TestMethod]
        public void Manipulate_ServiceTypeSpecifiedIsTrue_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.ServiceTypeSpecified);
        }
    }
}
