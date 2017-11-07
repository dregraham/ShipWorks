using Autofac.Extras.Moq;
using Interapptive.Shared.Enums;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Carriers.FedEx;
using ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    public class FedExPackageDetailsManipulatorTest
    {
        private readonly FedExPackageDetailsManipulator testObject;
        private readonly FedExAccountEntity fedExAccount;
        private readonly AutoMock mock;
        private readonly ShipmentEntity shipment;

        public FedExPackageDetailsManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipment = BuildFedExShipmentEntity.SetupRequestShipmentEntity();
            fedExAccount = new FedExAccountEntity();

            mock.Mock<ICarrierSettingsRepository>().Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(fedExAccount);

            testObject = mock.Create<FedExPackageDetailsManipulator>();
        }

        [Theory]
        [InlineData(FedExServiceType.FedExFreightEconomy, false)]
        [InlineData(FedExServiceType.FedExFreightPriority, false)]
        [InlineData(FedExServiceType.FedExGround, true)]
        [InlineData(FedExServiceType.PriorityOvernight, true)]
        public void ShouldApply_ReturnsAppropriateValue_ForGivenInput(FedExServiceType service, bool expected)
        {
            shipment.FedEx.Service = (int) service;
            var result = testObject.ShouldApply(shipment, 0);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Manipulate_PackageCountIsTwo_TwoPackagesInShipment()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(shipment.FedEx.Packages.Count, 2);
            Assert.Equal(result.Value.RequestedShipment.PackageCount, "2");
        }

        [Fact]
        public void Manipulate_DimensionsSetProperly_TwoPackagesWithDimensionsInShipment_AndLinearUnitsIsInchesTest()
        {
            shipment.FedEx.LinearUnitType = (int) FedExLinearUnitOfMeasure.IN;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal((int) FedExPackagingType.Custom, shipment.FedEx.PackagingType);

            CompareDimensions(shipment, shipment.FedEx.Packages[0], result.Value.RequestedShipment.RequestedPackageLineItems[0]);
        }

        [Fact]
        public void Manipulate_DimensionsRoundedProperly_WhenDimensionsAreDecimals()
        {
            shipment.FedEx.LinearUnitType = (int) FedExLinearUnitOfMeasure.IN;
            shipment.FedEx.Packages[0].DimsHeight = 3.2;
            shipment.FedEx.Packages[0].DimsLength = 3.9;
            shipment.FedEx.Packages[0].DimsWidth = 3.5;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            RequestedPackageLineItem requestedPackage = result.Value.RequestedShipment.RequestedPackageLineItems[0];

            Assert.Equal("3", requestedPackage.Dimensions.Height);
            Assert.Equal("4", requestedPackage.Dimensions.Length);
            Assert.Equal("4", requestedPackage.Dimensions.Width);
        }

        [Fact]
        public void Manipulate_DimensionsSetProperly_TwoPackagesWithDimensionsInShipment_AndLinearUnitsIsCentimetersTest()
        {
            shipment.FedEx.LinearUnitType = (int) FedExLinearUnitOfMeasure.CM;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal((int) FedExPackagingType.Custom, shipment.FedEx.PackagingType);

            CompareDimensions(shipment, shipment.FedEx.Packages[0], result.Value.RequestedShipment.RequestedPackageLineItems[0]);
        }

        [Fact]
        public void Manipulate_WeightSetProperly_TwoPackagesWithWeightInShipment_AndWieghtUnitsIsPounds()
        {
            shipment.FedEx.WeightUnitType = (int) WeightUnitOfMeasure.Pounds;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            ValidateWeight(shipment, shipment.FedEx.Packages[0], result.Value.RequestedShipment.RequestedPackageLineItems[0]);
        }

        [Fact]
        public void Manipulate_WeightSetProperly_TwoPackagesWithWeightInShipment_AndWieghtUnitsIsKilograms()
        {
            shipment.FedEx.WeightUnitType = (int) WeightUnitOfMeasure.Kilograms;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            ValidateWeight(shipment, shipment.FedEx.Packages[0], result.Value.RequestedShipment.RequestedPackageLineItems[0]);
        }

        [Fact]
        public void Manipulate_InsuredValueSetProperly_TwoPacakgesWithInsuredValue()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            ValidateValue(shipment.FedEx.Packages[0], result.Value.RequestedShipment.RequestedPackageLineItems[0]);

        }

        [Theory]
        [InlineData(0, "1")]
        [InlineData(1, "2")]
        public void Manipulate_AssignsSequenceNumber(int sequence, string expected)
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), sequence);

            Assert.Equal(expected, result.Value.RequestedShipment.RequestedPackageLineItems[0].SequenceNumber);
        }

        private void ValidateValue(FedExPackageEntity fedExPackageEntity, RequestedPackageLineItem requestedPackageLineItem)
        {
            Assert.Equal("USD", requestedPackageLineItem.InsuredValue.Currency.ToString());

            Assert.Equal(fedExPackageEntity.DeclaredValue, requestedPackageLineItem.InsuredValue.Amount);
        }

        /// <summary>
        /// Compare weight of request and entity
        /// </summary>
        /// <param name="fedExPackageEntity"></param>
        /// <param name="requestedPackageLineItem"></param>
        private void ValidateWeight(ShipmentEntity shipment, FedExPackageEntity fedExPackageEntity, RequestedPackageLineItem requestedPackageLineItem)
        {
            decimal packageEntityWeight = FedExUtility.GetPackageTotalWeight(fedExPackageEntity);
            Assert.Equal(packageEntityWeight, requestedPackageLineItem.Weight.Value);

            if (shipment.FedEx.WeightUnitType == (int) WeightUnitOfMeasure.Pounds)
            {
                Assert.Equal(WeightUnits.LB, requestedPackageLineItem.Weight.Units);
            }
            else
            {
                Assert.Equal(WeightUnits.KG, requestedPackageLineItem.Weight.Units);
            }
        }

        /// <summary>
        /// Compare dimensions of request and entity
        /// </summary>
        private static void CompareDimensions(ShipmentEntity shipment, FedExPackageEntity fedExPackageEntity, RequestedPackageLineItem requestedPackage)
        {
            if (shipment.FedEx.LinearUnitType == (int) FedExLinearUnitOfMeasure.CM)
            {
                Assert.Equal(LinearUnits.CM, requestedPackage.Dimensions.Units);
            }
            else
            {
                Assert.Equal(LinearUnits.IN, requestedPackage.Dimensions.Units);
            }
            Assert.Equal(fedExPackageEntity.DimsLength.ToString(), requestedPackage.Dimensions.Length);
            Assert.Equal(fedExPackageEntity.DimsWidth.ToString(), requestedPackage.Dimensions.Width);
            Assert.Equal(fedExPackageEntity.DimsHeight.ToString(), requestedPackage.Dimensions.Height);
        }
    }
}
