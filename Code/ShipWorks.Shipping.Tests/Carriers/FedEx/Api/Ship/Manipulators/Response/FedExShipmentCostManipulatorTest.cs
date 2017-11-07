using Autofac.Extras.Moq;
using log4net;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Carriers.FedEx;
using ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Response
{
    public class FedExShipmentCostManipulatorTest
    {
        private readonly AutoMock mock;
        private readonly ProcessShipmentReply reply;
        private readonly ShipmentEntity shipment;
        private FedExShipmentCostManipulator testObject;

        public FedExShipmentCostManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            reply = BuildFedExProcessShipmentReply.BuildValidFedExProcessShipmentReply();
            shipment = BuildFedExShipmentEntity.SetupBaseShipmentEntity();
            testObject = mock.Create<FedExShipmentCostManipulator>();
        }

        [Fact]
        public void Manipulate_ActualRateCostAddedToShipment_ActualRateTypeMatchesIncludedShipRateDetail()
        {
            var result = testObject.Manipulate(reply, shipment);

            Assert.Equal(reply.CompletedShipmentDetail.ShipmentRating.ShipmentRateDetails[1].TotalNetCharge.Amount,
                result.Value.ShipmentCost);
        }

        [Fact]
        public void Manipulate_UsesTotalNetFedExCharge_OriginIsCA()
        {
            shipment.OriginCountryCode = "CA";

            var result = testObject.Manipulate(reply, shipment);

            Assert.Equal(reply.CompletedShipmentDetail.ShipmentRating.ShipmentRateDetails[1].TotalNetFedExCharge.Amount,
                result.Value.ShipmentCost);
        }

        [Fact]
        public void Manipulate_FirstRateCostAddedToShipment_ActualRateTypeDoesNotMatchIncludedShipRateDetail()
        {
            //fedExShipResponse.Reply.CompletedShipmentDetail.ShipmentRating.ActualRateType = ReturnedRateType.RATED_LIST_SHIPMENT;
            reply.CompletedShipmentDetail.ShipmentRating.ActualRateType = ReturnedRateType.PREFERRED_LIST_SHIPMENT;

            var result = testObject.Manipulate(reply, shipment);

            Assert.Equal(reply.CompletedShipmentDetail.ShipmentRating.ShipmentRateDetails[0].TotalNetCharge.Amount,
                result.Value.ShipmentCost);
        }

        [Fact]
        public void Manipulate_ShippingCostLoggedAsZeroAndWarningLogged_NoShipmentRatingInformation()
        {
            //fedExShipResponse.Reply.CompletedShipmentDetail.ShipmentRating = null;
            reply.CompletedShipmentDetail.ShipmentRating = null;

            var result = testObject.Manipulate(reply, shipment);

            Assert.Equal(0, result.Value.ShipmentCost);

            mock.Mock<ILog>().Verify(log => log.WarnFormat(It.IsAny<string>(), (long) 77), Times.Once());
        }

        [Fact]
        public void Manipulate_ShippingCostLoggedAsTotalNetFedExCharge_OriginIsCanada()
        {
            reply.CompletedShipmentDetail.ShipmentRating.ActualRateType = ReturnedRateType.PREFERRED_LIST_SHIPMENT;

            shipment.OriginCountryCode = "CA";

            var result = testObject.Manipulate(reply, shipment);

            Assert.Equal(
                reply.CompletedShipmentDetail.ShipmentRating.ShipmentRateDetails[0].TotalNetFedExCharge.Amount,
                result.Value.ShipmentCost);
        }
    }
}
