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

        public FedExMasterTrackingManipulatorTest()
        {
            ShipmentEntity shipmentEntity = BuildFedExShipmentEntity.SetupBaseShipmentEntity();
            shipmentEntity.TrackingNumber = "xyz";
            shipmentEntity.FedEx.MasterFormID = "xyz";

            settingsRepository = new Mock<ICarrierSettingsRepository>();

            request = new FedExShipRequest(null, shipmentEntity, null, null, settingsRepository.Object, new ProcessShipmentRequest());
            
            testObject = new FedExMasterTrackingManipulator();
        }

        [Fact]
        public void Manipulate_HasNoMasterInformation_SequenceNumberIsZero()
        {
            request.SequenceNumber = 0;

            testObject.Manipulate(request);

            RequestedShipment requestedShipment = FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(request);

            Assert.Null(requestedShipment.MasterTrackingId);
        }

        [Fact]
        public void Manipulate_HasMasterInformation_SequenceNumberIsOne()
        {
            request.SequenceNumber = 1;

            testObject.Manipulate(request);

            RequestedShipment requestedShipment = FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(request);

            Assert.NotNull(requestedShipment.MasterTrackingId);
        }

        [Fact]
        public void Manipulate_CorrectFormIdSet_FormIdIsXyz()
        {
            request.SequenceNumber = 1;

            testObject.Manipulate(request);

                        RequestedShipment requestedShipment = FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(request);

            Assert.Equal(request.ShipmentEntity.FedEx.MasterFormID, requestedShipment.MasterTrackingId.FormId);
        }

        [Fact]
        public void Manipulate_CorrectTrackingIdSet_TrackingNumberIsAbc()
        {
            request.SequenceNumber = 1;

            testObject.Manipulate(request);

            RequestedShipment requestedShipment = FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(request);

            Assert.Equal(request.ShipmentEntity.TrackingNumber, requestedShipment.MasterTrackingId.TrackingNumber);
        }
    }
}
