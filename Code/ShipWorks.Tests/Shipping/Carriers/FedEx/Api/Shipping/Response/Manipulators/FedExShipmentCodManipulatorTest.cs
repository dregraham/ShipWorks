using Xunit;
using Moq;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Response.Manipulators
{
    public class FedExShipmentCodManipulatorTest
    {
        private FedExShipmentCodManipulator testObject;

        private FedExShipResponse fedExShipResponse;
        private Mock<CarrierRequest> carrierRequest;

        public FedExShipmentCodManipulatorTest()
        {
            carrierRequest = new Mock<CarrierRequest>(null, null);

            fedExShipResponse = new FedExShipResponse(BuildFedExProcessShipmentReply.BuildValidFedExProcessShipmentReply(), carrierRequest.Object, 
                BuildFedExShipmentEntity.SetupBaseShipmentEntity(), null, null);

            testObject = new FedExShipmentCodManipulator();
        }

        [Fact]
        public void Manipulate_CodTrackingNumberAndFormIDAddedToShipment_ResponseIncludesCodTrackingInfo()
        {
            testObject.Manipulate(fedExShipResponse);

            ProcessShipmentReply nativeResponse = fedExShipResponse.NativeResponse as ProcessShipmentReply;
            Assert.Equal(fedExShipResponse.Shipment.FedEx.CodTrackingNumber, nativeResponse.CompletedShipmentDetail.MasterTrackingId.TrackingNumber);
            Assert.Equal(fedExShipResponse.Shipment.FedEx.CodTrackingFormID, nativeResponse.CompletedShipmentDetail.MasterTrackingId.FormId);
        }
    }
}
