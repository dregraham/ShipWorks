using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    public class FedExRatePickupManipulatorTest
    {
        private FedExRatePickupManipulator testObject;
        private ShipmentEntity shipment;

        public FedExRatePickupManipulatorTest()
        {
            shipment = Create.Shipment().AsFedEx().Build();

            testObject = new FedExRatePickupManipulator();
        }

        [Fact]
        public void ShouldApply_ReturnsTrue()
        {
            var result = testObject.ShouldApply(null, FedExRateRequestOptions.None);
            Assert.True(result);
        }

        [Theory]
        [InlineData(FedExDropoffType.BusinessServiceCenter, DropoffType.BUSINESS_SERVICE_CENTER)]
        [InlineData(FedExDropoffType.DropBox, DropoffType.DROP_BOX)]
        [InlineData(FedExDropoffType.RegularPickup, DropoffType.REGULAR_PICKUP)]
        [InlineData(FedExDropoffType.RequestCourier, DropoffType.REQUEST_COURIER)]
        [InlineData(FedExDropoffType.Station, DropoffType.STATION)]
        public void Manipulate_ReturnsCorrectDropoffType_BasedOnFedExShipment(FedExDropoffType dropoffType, DropoffType expected)
        {
            shipment.FedEx.DropoffType = (int) dropoffType;

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(expected, result.RequestedShipment.DropoffType);
        }

        [Fact]
        public void Manipulate_DropoffTypeSpecifiedIsTrue()
        {
            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.True(result.RequestedShipment.DropoffTypeSpecified);
        }
    }
}
