using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
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

        [Fact]
        public void Manipulate_HasNoMasterInformation_SequenceNumberIsZero_Test()
        {
            request.SequenceNumber = 0;

            testObject.Manipulate(request);

            RequestedShipment requestedShipment = FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(request);

            Assert.IsNull(requestedShipment.MasterTrackingId);
        }

        [Fact]
        public void Manipulate_HasMasterInformation_SequenceNumberIsOne_Test()
        {
            request.SequenceNumber = 1;

            testObject.Manipulate(request);

            RequestedShipment requestedShipment = FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(request);

            Assert.IsNotNull(requestedShipment.MasterTrackingId);
        }

        [Fact]
        public void Manipulate_CorrectFormIdSet_FormIdIsXyz_Test()
        {
            request.SequenceNumber = 1;

            testObject.Manipulate(request);

                        RequestedShipment requestedShipment = FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(request);

            Assert.AreEqual(request.ShipmentEntity.FedEx.MasterFormID, requestedShipment.MasterTrackingId.FormId);
        }

        [Fact]
        public void Manipulate_CorrectTrackingIdSet_TrackingNumberIsAbc_Test()
        {
            request.SequenceNumber = 1;

            testObject.Manipulate(request);

            RequestedShipment requestedShipment = FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(request);

            Assert.AreEqual(request.ShipmentEntity.TrackingNumber, requestedShipment.MasterTrackingId.TrackingNumber);
        }
    }
}
