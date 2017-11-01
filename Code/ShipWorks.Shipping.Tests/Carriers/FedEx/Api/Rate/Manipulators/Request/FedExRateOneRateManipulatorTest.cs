using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    public class FedExRateOneRateManipulatorTest
    {
        private FedExRateOneRateManipulator testObject;
        private ShipmentEntity shipmentEntity;

        public FedExRateOneRateManipulatorTest()
        {
            shipmentEntity = Create.Shipment().AsFedEx().Build();

            testObject = new FedExRateOneRateManipulator();
        }

        [Theory]
        [InlineData(FedExRateRequestOptions.None, false)]
        [InlineData(FedExRateRequestOptions.ExpressFreight, false)]
        [InlineData(FedExRateRequestOptions.SmartPost, false)]
        [InlineData(FedExRateRequestOptions.OneRate, true)]
        [InlineData(FedExRateRequestOptions.OneRate | FedExRateRequestOptions.ExpressFreight, true)]
        public void ShouldApply_ReturnsAppropriateValue_ForInput(FedExRateRequestOptions options, bool expected)
        {
            var result = testObject.ShouldApply(null, options);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Manipulate_AddsOneRateSpecialServiceType_WhenSpecialServiceTypeArrayIsEmpty()
        {
            var result = testObject.Manipulate(shipmentEntity, new RateRequest());

            Assert.Equal(result.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0], ShipmentSpecialServiceType.FEDEX_ONE_RATE);
        }

        [Fact]
        public void Manipulate_AddsOneRateSpecialServiceType_AndRetainsExistingSpecialServices_WhenSpecialServiceTypeArrayIsEmpty()
        {
            var request = new RateRequest();
            var services = request.Ensure(x => x.RequestedShipment).Ensure(x => x.SpecialServicesRequested);
            services.SpecialServiceTypes = new ShipmentSpecialServiceType[1] { ShipmentSpecialServiceType.COD };

            var result = testObject.Manipulate(shipmentEntity, request);

            var serviceTypes = result.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes;
            Assert.Equal(2, serviceTypes.Length);
            Assert.Contains(ShipmentSpecialServiceType.COD, serviceTypes);
            Assert.Contains(ShipmentSpecialServiceType.FEDEX_ONE_RATE, serviceTypes);
        }
    }
}
