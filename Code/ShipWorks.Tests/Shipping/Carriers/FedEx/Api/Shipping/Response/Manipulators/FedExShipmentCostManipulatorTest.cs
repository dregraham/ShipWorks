using Xunit;
using Moq;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using log4net;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Response.Manipulators
{
    public class FedExShipmentCostManipulatorTest
    {
        private FedExShipResponse fedExShipResponse;

        private Mock<ILog> mockLog;

        private FedExShipmentCostManipulator testObject;
        private ProcessShipmentReply nativeResponse;
        private Mock<CarrierRequest> carrierRequest;

        [TestInitialize]
        public void Initialize()
        {
            mockLog = new Mock<ILog>();
            mockLog.Setup(log => log.WarnFormat(It.IsAny<string>(), 77));

            testObject = new FedExShipmentCostManipulator(mockLog.Object);

            nativeResponse = BuildFedExProcessShipmentReply.BuildValidFedExProcessShipmentReply();
            carrierRequest = new Mock<CarrierRequest>(null, null);

            fedExShipResponse = new FedExShipResponse(nativeResponse, carrierRequest.Object, BuildFedExShipmentEntity.SetupBaseShipmentEntity(), null, null);
        }

        [Fact]
        public void Manipulate_ActualRateCostAddedToShipment_ActualRateTypeMatchesIncludedShipRateDetail()
        {
            testObject.Manipulate(fedExShipResponse);

            Assert.AreEqual(nativeResponse.CompletedShipmentDetail.ShipmentRating.ShipmentRateDetails[1].TotalNetCharge.Amount,
                fedExShipResponse.Shipment.ShipmentCost);
        }


        [Fact]
        public void Manipulate_UsesTotalNetFedExCharge_OriginIsCA()
        {
            fedExShipResponse.Shipment.OriginCountryCode = "CA";

            testObject.Manipulate(fedExShipResponse);

            Assert.AreEqual(nativeResponse.CompletedShipmentDetail.ShipmentRating.ShipmentRateDetails[1].TotalNetFedExCharge.Amount,
                fedExShipResponse.Shipment.ShipmentCost);
        }

        [Fact]
        public void Manipulate_FirstRateCostAddedToShipment_ActualRateTypeDoesNotMatchIncludedShipRateDetail()
        {
            //fedExShipResponse.Reply.CompletedShipmentDetail.ShipmentRating.ActualRateType = ReturnedRateType.RATED_LIST_SHIPMENT;
            nativeResponse.CompletedShipmentDetail.ShipmentRating.ActualRateType = ReturnedRateType.PREFERRED_LIST_SHIPMENT;

            testObject.Manipulate(fedExShipResponse);

            Assert.AreEqual(nativeResponse.CompletedShipmentDetail.ShipmentRating.ShipmentRateDetails[0].TotalNetCharge.Amount,
                fedExShipResponse.Shipment.ShipmentCost);
        }

        [Fact]
        public void Manipulate_ShippingCostLoggedAsZeroAndWarningLogged_NoShipmentRatingInformation()
        {
            //fedExShipResponse.Reply.CompletedShipmentDetail.ShipmentRating = null;
            nativeResponse.CompletedShipmentDetail.ShipmentRating = null;

            testObject.Manipulate(fedExShipResponse);

            Assert.AreEqual(0, fedExShipResponse.Shipment.ShipmentCost);

            mockLog.Verify(log => log.WarnFormat(It.IsAny<string>(), (long) 77), Times.Once());
        }

        [Fact]
        public void Manipulate_ShippingCostLoggedAsTotalNetFedExCharge_OriginIsCanada()
        {
            nativeResponse.CompletedShipmentDetail.ShipmentRating.ActualRateType = ReturnedRateType.PREFERRED_LIST_SHIPMENT;

            fedExShipResponse.Shipment.OriginCountryCode = "CA";

            testObject.Manipulate(fedExShipResponse);

            Assert.AreEqual(
                nativeResponse.CompletedShipmentDetail.ShipmentRating.ShipmentRateDetails[0].TotalNetFedExCharge.Amount,
                fedExShipResponse.Shipment.ShipmentCost);
        }
    }
}
