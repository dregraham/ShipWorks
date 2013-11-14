using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Response.Manipulators
{
    [TestClass]
    public class FedExShipmentCodManipulatorTest
    {
        private FedExShipmentCodManipulator testObject;

        private FedExShipResponse fedExShipResponse;
        private Mock<CarrierRequest> carrierRequest;

        [TestInitialize]
        public void Initialize()
        {
            carrierRequest = new Mock<CarrierRequest>(null, null);

            fedExShipResponse = new FedExShipResponse(BuildFedExProcessShipmentReply.BuildValidFedExProcessShipmentReply(), carrierRequest.Object, 
                BuildFedExShipmentEntity.SetupBaseShipmentEntity(), null, null);

            testObject = new FedExShipmentCodManipulator();
        }

        [TestMethod]
        public void Manipulate_CodTrackingNumberAndFormIDAddedToShipment_ResponseIncludesCodTrackingInfo()
        {
            testObject.Manipulate(fedExShipResponse);

            ProcessShipmentReply nativeResponse = fedExShipResponse.NativeResponse as ProcessShipmentReply;
            Assert.AreEqual(fedExShipResponse.Shipment.FedEx.CodTrackingNumber, nativeResponse.CompletedShipmentDetail.MasterTrackingId.TrackingNumber);
            Assert.AreEqual(fedExShipResponse.Shipment.FedEx.CodTrackingFormID, nativeResponse.CompletedShipmentDetail.MasterTrackingId.FormId);
        }
    }
}
