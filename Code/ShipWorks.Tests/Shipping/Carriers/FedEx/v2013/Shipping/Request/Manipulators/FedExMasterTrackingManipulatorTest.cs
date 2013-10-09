using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Shipping.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using Moq;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Environment;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Shipping.Request.Manipulators
{
    [TestClass]
    public class FedExMasterTrackingManipulatorTest
    {
        private FedExMasterTrackingManipulator testObject;

        private FedExShipRequest request;
        private Mock<ICarrierSettingsRepository> settingsRepository;

        [TestInitialize]
        public void Initialize()
        {
            ShipmentEntity shipmentEntity = BuildFedExShipmentEntity.SetupBaseShipmentEntity();
            shipmentEntity.TrackingNumber = "xyz";
            shipmentEntity.FedEx.MasterFormID = "xyz";

            settingsRepository = new Mock<ICarrierSettingsRepository>();

            request = new FedExShipRequest(null, shipmentEntity, null, null, settingsRepository.Object, new ProcessShipmentRequest());
            
            testObject = new FedExMasterTrackingManipulator();
        }

        [TestMethod]
        public void Manipulate_HasNoMasterInformation_SequenceNumberIsZero_Test()
        {
            request.SequenceNumber = 0;

            testObject.Manipulate(request);

            RequestedShipment requestedShipment = FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(request);

            Assert.IsNull(requestedShipment.MasterTrackingId);
        }

        [TestMethod]
        public void Manipulate_HasMasterInformation_SequenceNumberIsOne_Test()
        {
            request.SequenceNumber = 1;

            testObject.Manipulate(request);

            RequestedShipment requestedShipment = FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(request);

            Assert.IsNotNull(requestedShipment.MasterTrackingId);
        }

        [TestMethod]
        public void Manipulate_CorrectFormIdSet_FormIdIsXyz_Test()
        {
            request.SequenceNumber = 1;

            testObject.Manipulate(request);

                        RequestedShipment requestedShipment = FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(request);

            Assert.AreEqual(request.ShipmentEntity.FedEx.MasterFormID, requestedShipment.MasterTrackingId.FormId);
        }

        [TestMethod]
        public void Manipulate_CorrectTrackingIdSet_TrackingNumberIsAbc_Test()
        {
            request.SequenceNumber = 1;

            testObject.Manipulate(request);

            RequestedShipment requestedShipment = FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(request);

            Assert.AreEqual(request.ShipmentEntity.TrackingNumber, requestedShipment.MasterTrackingId.TrackingNumber);
        }
    }
}
