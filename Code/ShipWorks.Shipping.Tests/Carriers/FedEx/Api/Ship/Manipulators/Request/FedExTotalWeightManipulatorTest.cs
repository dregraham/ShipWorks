using Autofac.Extras.Moq;
using Interapptive.Shared.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    public class FedExTotalWeightManipulatorTest
    {
        private readonly FedExTotalWeightManipulator testObject;
        private readonly AutoMock mock;
        private readonly ShipmentEntity shipment;

        public FedExTotalWeightManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            // Create our default shipment entity and initialize the properties our test object will be accessing
            shipment = new ShipmentEntity()
            {
                TotalWeight = 3.245f,
                FedEx = new FedExShipmentEntity { WeightUnitType = (int) WeightUnitOfMeasure.Pounds }
            };

            testObject = new FedExTotalWeightManipulator();
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedShipment()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.NotNull(result.Value.RequestedShipment);
        }

        [Fact]
        public void Manipulate_SetsUnitsToPounds()
        {
            shipment.FedEx.WeightUnitType = (int) WeightUnitOfMeasure.Pounds;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Weight weight = result.Value.RequestedShipment.TotalWeight;
            Assert.Equal(WeightUnits.LB, weight.Units);
        }

        [Fact]
        public void Manipulate_SetsUnitsToKilograms()
        {
            shipment.FedEx.WeightUnitType = (int) WeightUnitOfMeasure.Kilograms;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Weight weight = result.Value.RequestedShipment.TotalWeight;
            Assert.Equal(WeightUnits.KG, weight.Units);
        }

        [Fact]
        public void Manipulate_SetsWeightValue()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Weight weight = result.Value.RequestedShipment.TotalWeight;
            Assert.Equal((decimal) shipment.TotalWeight, weight.Value);
        }
    }
}
