﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ProcessShipmentRequest = ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ProcessShipmentRequest;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Shipping.Request.Manipulators
{
    [TestClass]
    public class FedExReturnsManipulatorTest
    {
        private FedExShipRequest shipRequest;

        private ShipmentEntity shipmentEntity;

        private FedExReturnsManipulator testObject;

        /// <summary>
        /// NativeRequest from shipRequest converted to ProcessShipmentRequest
        /// </summary>
        private ProcessShipmentRequest processShipmentRequest
        {
            get
            {
                return shipRequest.NativeRequest as ProcessShipmentRequest;
            }
        }

        [TestInitialize]
        public void Initialize()
        {
            testObject = new FedExReturnsManipulator();

            shipmentEntity = BuildFedExShipmentEntity.SetupRequestShipmentEntity();

            shipmentEntity.ReturnShipment = true;
            shipmentEntity.FedEx.ReturnType = (int) FedExReturnType.EmailReturnLabel;

            Mock<ICarrierSettingsRepository> settingsRepository = new Mock<ICarrierSettingsRepository>();

            shipRequest = new FedExShipRequest(
                null,
                shipmentEntity,
                null,
                null,
                settingsRepository.Object,
                new ProcessShipmentRequest());
        }

        [TestMethod]
        public void Manipulate_ReturnNotAddedToRequest_ShipmentNotAReturn_Test()
        {
            shipmentEntity.ReturnShipment = false;

            testObject.Manipulate(shipRequest);

            Assert.IsNull(processShipmentRequest.RequestedShipment);
        }

        [TestMethod]
        public void Manipulate_ReturnTypePending_ShipmentEmailReturnLabel_Test()
        {
            testObject.Manipulate(shipRequest);

            Assert.AreEqual(ReturnType.PENDING, processShipmentRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnType);
        }

        [TestMethod]
        public void Manipulate_ReturnTypePrintReturnLabel_ShipmentPrintReturnLabel_Test()
        {
            shipmentEntity.FedEx.ReturnType = (int) FedExReturnType.PrintReturnLabel;

            testObject.Manipulate(shipRequest);

            Assert.AreEqual(ReturnType.PRINT_RETURN_LABEL, processShipmentRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnType);
        }

        [TestMethod]
        public void Manipulate_RmaReasonSet_ShipmentHasRmaReason_Test()
        {
            string rmaReason = "test rma reason";

            shipmentEntity.FedEx.RmaReason = rmaReason;

            testObject.Manipulate(shipRequest);

            Assert.AreEqual(rmaReason, processShipmentRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.Rma.Reason);
        }

        [TestMethod]
        public void Manipulate_RmaReasonNotSet_ShipmentHasNoRmaReason_Test()
        {
            testObject.Manipulate(shipRequest);

            Assert.IsNull(processShipmentRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.Rma);
        }

        [TestMethod]
        public void Manipulate_RmaNumberSet_ShipmentHasRmaNumber_Test()
        {
            string rmaNumber = "test rma number";

            shipmentEntity.FedEx.RmaNumber = rmaNumber;

            testObject.Manipulate(shipRequest);

            Assert.AreEqual(1, processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.Count(x => x.CustomerReferenceType == CustomerReferenceType.RMA_ASSOCIATION && x.Value == rmaNumber));
        }

        [TestMethod]
        public void Manipulate_NoCustomerReferences_ShipmentHasNoRmaNumber_Test()
        {
            testObject.Manipulate(shipRequest);

            Assert.AreEqual(0, processShipmentRequest.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences.Count());
        }

        [TestMethod]
        public void Manipulate_NoEmailDetails_ShipmentPrintReturnLabel_Test()
        {
            shipmentEntity.FedEx.ReturnType = (int) FedExReturnType.PrintReturnLabel;

            testObject.Manipulate(shipRequest);

            Assert.IsNull(processShipmentRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnEMailDetail);
        }

        [TestMethod]
        public void Manipulate_NoPendingShipmentDetail_ShipmentPrintReturnLabel_Test()
        {
            shipmentEntity.FedEx.ReturnType = (int) FedExReturnType.PrintReturnLabel;

            testObject.Manipulate(shipRequest);

            Assert.IsNull(processShipmentRequest.RequestedShipment.SpecialServicesRequested.PendingShipmentDetail);
        }

        [TestMethod]
        public void Manipulate_MerchantPhoneNumberSet_ShipmentEmailReturnLabel_Test()
        {
            testObject.Manipulate(shipRequest);

            Assert.AreEqual(
                shipmentEntity.OriginPhone,
                processShipmentRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnEMailDetail.MerchantPhoneNumber);
        }

        [TestMethod]
        public void Manipulate_PendingShipmentTypeSetToEmail_ShipmentEmailReturnLabel_Test()
        {
            testObject.Manipulate(shipRequest);

            Assert.AreEqual(
                PendingShipmentType.EMAIL,
                processShipmentRequest.RequestedShipment.SpecialServicesRequested.PendingShipmentDetail.Type);
        }

        [TestMethod]
        public void Manipulate_PendingShipmentExpirationSetToThirtyDaysOut_ShipmentEmailReturnLabel_Test()
        {
            testObject.Manipulate(shipRequest);

            Assert.AreEqual(
                DateTime.Today.AddDays(30),
                processShipmentRequest.RequestedShipment.SpecialServicesRequested.PendingShipmentDetail.ExpirationDate);
        }

        [TestMethod]
        public void Manipulate_PendingShipmentEmailSet_ShipmentEmailReturnLabel_Test()
        {
            testObject.Manipulate(shipRequest);

            Assert.AreEqual(
                shipmentEntity.ShipEmail,
                processShipmentRequest.RequestedShipment.SpecialServicesRequested.PendingShipmentDetail.EmailLabelDetail.NotificationEMailAddress);
        }

        [TestMethod]
        public void Manipulate_SautrdayPickupSet_ShipmentEmailReturnLabelWithSaturdayPickup_Test()
        {
            shipmentEntity.FedEx.ReturnSaturdayPickup = true;

            testObject.Manipulate(shipRequest);

            Assert.AreEqual(
                1,
                processShipmentRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnEMailDetail.AllowedSpecialServices.Count(x => x == ReturnEMailAllowedSpecialServiceType.SATURDAY_PICKUP));
        }

        [TestMethod]
        public void Manipulate_SautrdayPickupNotSet_ShipmentEmailReturnLabelWithNoSaturdayPickup_Test()
        {
            shipmentEntity.FedEx.ReturnSaturdayPickup = false;

            testObject.Manipulate(shipRequest);

            Assert.IsTrue(
                processShipmentRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnEMailDetail.AllowedSpecialServices == null ||
                processShipmentRequest.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnEMailDetail.AllowedSpecialServices.Count(x => x == ReturnEMailAllowedSpecialServiceType.SATURDAY_PICKUP) == 0);
        }
    }
}
