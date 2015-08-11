using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Response.Manipulators
{
    public class FedExTrackingManipulatorTest
    {
        private FedExShipmentTrackingManipulator testObject;

        FedExShipResponse fedExShipResponse;
        private ShipmentEntity shipmentEntity;
        ProcessShipmentReply processShipmentReply;
        private Mock<CarrierRequest> carrierRequest;

        [TestInitialize]
        public void Initialize()
        {
            

            shipmentEntity= BuildFedExShipmentEntity.SetupBaseShipmentEntity();
            processShipmentReply = BuildFedExProcessShipmentReply.BuildValidFedExProcessShipmentReply();

            carrierRequest = new Mock<CarrierRequest>(null, null);
            fedExShipResponse = new FedExShipResponse(processShipmentReply, carrierRequest.Object, shipmentEntity, null, null);

            testObject = new FedExShipmentTrackingManipulator();
        }


        [Fact]
        public void Manipulate_MasterTrackingNumberAddedToShipment_WhenMasterTrackingIdPresent_Test()
        {
            testObject.Manipulate(fedExShipResponse);
            Assert.AreEqual(shipmentEntity.TrackingNumber, "MasterTrackingNumber");
        }

        [Fact]
        public void Manipulate_PackageTrackingNumberAddedToShipment_WhenNoMasterTrackingIdPresent_Test()
        {
            //remove master tracking
            processShipmentReply.CompletedShipmentDetail.MasterTrackingId = null;
            
            testObject.Manipulate(fedExShipResponse);
            Assert.AreEqual(shipmentEntity.TrackingNumber, "Package1Tracking");
        }

        [Fact]
        public void Manipulate_PackageTrackingNumberAddedToSecondPackage_WhenSequenceNumberIsTwo_Test()
        {
            processShipmentReply.CompletedShipmentDetail.CompletedPackageDetails[0].SequenceNumber = "2";

            testObject.Manipulate(fedExShipResponse);

            Assert.IsNull(fedExShipResponse.Shipment.FedEx.Packages[0].TrackingNumber);           
            Assert.AreEqual("Package1Tracking", fedExShipResponse.Shipment.FedEx.Packages[1].TrackingNumber);
        }
    }
}
