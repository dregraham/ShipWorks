using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    public class FedExOneRateManipulatorTest
    {
        private FedExOneRateManipulator testObject;
        private ShipmentEntity shipment;

        public FedExOneRateManipulatorTest()
        {
            shipment = Create.Shipment().AsFedEx().Build();
            testObject = new FedExOneRateManipulator();
        }

        [Theory]
        [InlineData(FedExServiceType.OneRate2Day, true)]
        [InlineData(FedExServiceType.OneRate2DayAM, true)]
        [InlineData(FedExServiceType.OneRateExpressSaver, true)]
        [InlineData(FedExServiceType.OneRateFirstOvernight, true)]
        [InlineData(FedExServiceType.OneRatePriorityOvernight, true)]
        [InlineData(FedExServiceType.OneRateStandardOvernight, true)]
        [InlineData(FedExServiceType.PriorityOvernight, false)]
        [InlineData(FedExServiceType.FedExFreightPriority, false)]
        public void ShouldApply_ReturnsAppropriateValue_ForGivenInput(FedExServiceType service, bool expected)
        {
            shipment.FedEx.Service = (int) service;

            var result = testObject.ShouldApply(shipment, 0);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Manipulate_AddsOneRateSpecialServiceType_AndSpecialServiceTypeArrayIsEmpty()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Contains(ShipmentSpecialServiceType.FEDEX_ONE_RATE, result.Value.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
        }

        [Fact]
        public void Manipulate_AddsOneRateSpecialServiceType_AndSpecialServiceTypeArrayIsNotEmpty()
        {
            var request = new ProcessShipmentRequest();
            request.Ensure(x => x.RequestedShipment).Ensure(x => x.SpecialServicesRequested).SpecialServiceTypes = new[] { ShipmentSpecialServiceType.COD };

            var result = testObject.Manipulate(shipment, request, 0);

            Assert.Contains(ShipmentSpecialServiceType.FEDEX_ONE_RATE, result.Value.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
            Assert.Contains(ShipmentSpecialServiceType.COD, result.Value.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
        }
    }
}
