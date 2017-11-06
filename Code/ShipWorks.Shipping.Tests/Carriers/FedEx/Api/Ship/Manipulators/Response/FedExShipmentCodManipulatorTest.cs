using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Response
{
    public class FedExShipmentCodManipulatorTest
    {
        private FedExShipmentCodManipulator testObject;
        private readonly ShipmentEntity shipmentEntity;
        private readonly ProcessShipmentReply processShipmentReply;

        public FedExShipmentCodManipulatorTest()
        {
            shipmentEntity = BuildFedExShipmentEntity.SetupBaseShipmentEntity();
            processShipmentReply = BuildFedExProcessShipmentReply.BuildValidFedExProcessShipmentReply();

            testObject = new FedExShipmentCodManipulator();
        }

        [Fact]
        public void Manipulate_CodTrackingNumberAndFormIDAddedToShipment_ResponseIncludesCodTrackingInfo()
        {
            testObject.Manipulate(processShipmentReply, shipmentEntity);

            Assert.Equal(shipmentEntity.FedEx.CodTrackingNumber, processShipmentReply.CompletedShipmentDetail.MasterTrackingId.TrackingNumber);
            Assert.Equal(shipmentEntity.FedEx.CodTrackingFormID, processShipmentReply.CompletedShipmentDetail.MasterTrackingId.FormId);
        }
    }
}
