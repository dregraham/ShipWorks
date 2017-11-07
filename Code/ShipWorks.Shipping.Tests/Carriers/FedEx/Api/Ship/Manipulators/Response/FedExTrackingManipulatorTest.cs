using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Response
{
    public class FedExTrackingManipulatorTest
    {
        private FedExShipmentTrackingManipulator testObject;
        private readonly ShipmentEntity shipmentEntity;
        private readonly ProcessShipmentReply processShipmentReply;
        private Mock<CarrierRequest> carrierRequest;

        public FedExTrackingManipulatorTest()
        {
            shipmentEntity = BuildFedExShipmentEntity.SetupBaseShipmentEntity();
            carrierRequest = new Mock<CarrierRequest>(null, null);
            processShipmentReply = BuildFedExProcessShipmentReply.BuildValidFedExProcessShipmentReply();

            testObject = new FedExShipmentTrackingManipulator();
        }

        [Fact]
        public void Manipulate_MasterTrackingNumberAddedToShipment_WhenMasterTrackingIdPresent()
        {
            testObject.Manipulate(processShipmentReply, shipmentEntity);
            Assert.Equal(shipmentEntity.TrackingNumber, "MasterTrackingNumber");
        }

        [Fact]
        public void Manipulate_PackageTrackingNumberAddedToShipment_WhenNoMasterTrackingIdPresent()
        {
            //remove master tracking
            processShipmentReply.CompletedShipmentDetail.MasterTrackingId = null;

            testObject.Manipulate(processShipmentReply, shipmentEntity);
            Assert.Equal(shipmentEntity.TrackingNumber, "Package1Tracking");
        }

        [Fact]
        public void Manipulate_PackageTrackingNumberAddedToSecondPackage_WhenSequenceNumberIsTwo()
        {
            processShipmentReply.CompletedShipmentDetail.CompletedPackageDetails[0].SequenceNumber = "2";

            testObject.Manipulate(processShipmentReply, shipmentEntity);

            Assert.Null(shipmentEntity.FedEx.Packages[0].TrackingNumber);
            Assert.Equal("Package1Tracking", shipmentEntity.FedEx.Packages[1].TrackingNumber);
        }
    }
}
