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

        public FedExTrackingManipulatorTest()
        {
            

            shipmentEntity= BuildFedExShipmentEntity.SetupBaseShipmentEntity();
            processShipmentReply = BuildFedExProcessShipmentReply.BuildValidFedExProcessShipmentReply();

            carrierRequest = new Mock<CarrierRequest>(null, null);
            fedExShipResponse = new FedExShipResponse(processShipmentReply, carrierRequest.Object, shipmentEntity, null, null);

            testObject = new FedExShipmentTrackingManipulator();
        }


        [Fact]
        public void Manipulate_MasterTrackingNumberAddedToShipment_WhenMasterTrackingIdPresent()
        {
            testObject.Manipulate(fedExShipResponse);
            Assert.Equal(shipmentEntity.TrackingNumber, "MasterTrackingNumber");
        }

        [Fact]
        public void Manipulate_PackageTrackingNumberAddedToShipment_WhenNoMasterTrackingIdPresent()
        {
            //remove master tracking
            processShipmentReply.CompletedShipmentDetail.MasterTrackingId = null;
            
            testObject.Manipulate(fedExShipResponse);
            Assert.Equal(shipmentEntity.TrackingNumber, "Package1Tracking");
        }

        [Fact]
        public void Manipulate_PackageTrackingNumberAddedToSecondPackage_WhenSequenceNumberIsTwo()
        {
            processShipmentReply.CompletedShipmentDetail.CompletedPackageDetails[0].SequenceNumber = "2";

            testObject.Manipulate(fedExShipResponse);

            Assert.Null(fedExShipResponse.Shipment.FedEx.Packages[0].TrackingNumber);           
            Assert.Equal("Package1Tracking", fedExShipResponse.Shipment.FedEx.Packages[1].TrackingNumber);
        }
    }
}
