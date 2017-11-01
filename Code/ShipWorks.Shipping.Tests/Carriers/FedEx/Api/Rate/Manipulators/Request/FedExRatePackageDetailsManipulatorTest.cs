using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    public class FedExRatePackageDetailsManipulatorTest
    {
        private readonly AutoMock mock;
        private FedExRatePackageDetailsManipulator testObject;

        public FedExRatePackageDetailsManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testObject = mock.Create<FedExRatePackageDetailsManipulator>();
        }

        [Fact]
        public void ShouldApply_ReturnsTrue()
        {
            var result = testObject.ShouldApply(null, FedExRateRequestOptions.None);

            Assert.True(result);
        }

        [Theory]
        [InlineData(1, "1")]
        [InlineData(2, "2")]
        [InlineData(3, "3")]
        public void Manipulate_SetsPackageCount_BasedOnFedExPackageCount(int count, string expected)
        {
            var shipment = Create.Shipment()
                .AsFedEx(f => Enumerable.Range(0, count).Aggregate(f, (fedex, x) => fedex.WithPackage()))
                .Build();

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(expected, result.RequestedShipment.PackageCount);
        }

        [Fact]
        public void Manipulate_DoesNotSetDimensions_WhenPackagingTypeIsNotCustom()
        {
            var shipment = Create.Shipment()
                .AsFedEx(f => f.WithPackage().Set(x => x.PackagingType, (int) FedExPackagingType.Envelope))
                .Build();

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Null(result.RequestedShipment.RequestedPackageLineItems[0].Dimensions);
        }

        [Theory]
        [InlineData(2, "2")]
        [InlineData(2.1, "2")]
        [InlineData(2.4, "2")]
        [InlineData(2.5, "3")]
        [InlineData(2.9, "3")]
        public void Manipulate_RoundsDimensions_Correctly(double value, string expected)
        {
            var shipment = Create.Shipment()
                .AsFedEx(f => f
                    .WithPackage(p => p
                        .Set(x => x.DimsLength, value)
                        .Set(x => x.DimsWidth, value)
                        .Set(x => x.DimsHeight, value))
                    .Set(x => x.PackagingType, (int) FedExPackagingType.Custom))
                .Build();

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(expected, result.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Length);
            Assert.Equal(expected, result.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Width);
            Assert.Equal(expected, result.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Height);
        }

        [Theory]
        [InlineData(FedExLinearUnitOfMeasure.IN, LinearUnits.IN)]
        [InlineData(FedExLinearUnitOfMeasure.CM, LinearUnits.CM)]
        public void Manipulate_DimensionsUnitSetProperly_ForEachPackage(FedExLinearUnitOfMeasure units, LinearUnits expected)
        {
            var shipment = Create.Shipment()
                .AsFedEx(f => f
                    .WithPackage()
                    .WithPackage()
                    .Set(x => x.LinearUnitType, (int) units)
                    .Set(x => x.PackagingType, (int) FedExPackagingType.Custom))
                .Build();

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(expected, result.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Units);
            Assert.Equal(expected, result.RequestedShipment.RequestedPackageLineItems[1].Dimensions.Units);
        }

        [Theory]
        [InlineData(WeightUnitOfMeasure.Pounds, WeightUnits.LB)]
        [InlineData(WeightUnitOfMeasure.Grams, WeightUnits.LB)]
        [InlineData(WeightUnitOfMeasure.Ounces, WeightUnits.LB)]
        [InlineData(WeightUnitOfMeasure.Tonnes, WeightUnits.LB)]
        [InlineData(WeightUnitOfMeasure.Kilograms, WeightUnits.KG)]
        public void Manipulate_WeightUnitSetProperly_ForEachPackage(WeightUnitOfMeasure units, WeightUnits expected)
        {
            var shipment = Create.Shipment()
                .AsFedEx(f => f
                    .WithPackage()
                    .WithPackage()
                    .Set(x => x.WeightUnitType, (int) units))
                .Build();

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(expected, result.RequestedShipment.RequestedPackageLineItems[0].Weight.Units);
            Assert.Equal(expected, result.RequestedShipment.RequestedPackageLineItems[1].Weight.Units);
        }

        [Fact]
        public void Manipulate_SetsWeightValueToZeroPointOne_WhenValueIsZero()
        {
            var shipment = Create.Shipment()
                .AsFedEx(f => f
                    .WithPackage()
                    .WithPackage())
                .Build();

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(0.1m, result.RequestedShipment.RequestedPackageLineItems[0].Weight.Value);
            Assert.Equal(0.1m, result.RequestedShipment.RequestedPackageLineItems[1].Weight.Value);
        }

        [Fact]
        public void Manipulate_WeightSetProperly_TwoPackagesWithWeightInShipment_AndUnitsIsKG()
        {
            var shipment = Create.Shipment()
                .AsFedEx(f => f
                    .WithPackage(p => p.Set(x => x.Weight, 2))
                    .WithPackage(p => p.Set(x => x.Weight, 3))
                    .Set(x => x.WeightUnitType, (int) WeightUnitOfMeasure.Kilograms))
                .Build();

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(2, result.RequestedShipment.RequestedPackageLineItems[0].Weight.Value);
            Assert.Equal(3, result.RequestedShipment.RequestedPackageLineItems[1].Weight.Value);
        }

        [Fact]
        public void Manipulate_InsuredValueSetProperly_TwoPacakgesWithInsuredValue()
        {
            var shipment = Create.Shipment()
                .AsFedEx(f => f
                    .WithPackage(p => p.Set(x => x.DeclaredValue, 10))
                    .WithPackage(p => p.Set(x => x.DeclaredValue, 20)))
                .Build();

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(10, result.RequestedShipment.RequestedPackageLineItems[0].InsuredValue.Amount);
            Assert.Equal(20, result.RequestedShipment.RequestedPackageLineItems[1].InsuredValue.Amount);
            Assert.True(result.RequestedShipment.RequestedPackageLineItems[0].InsuredValue.AmountSpecified);
            Assert.True(result.RequestedShipment.RequestedPackageLineItems[1].InsuredValue.AmountSpecified);
        }

        [Theory]
        [InlineData(CurrencyType.USD, "USD")]
        [InlineData(CurrencyType.CAD, "CAD")]
        [InlineData(CurrencyType.EUR, "EUR")]
        public void Manipulate_SetsInsuranceCurrencyFromShipment_WhenShipmentHasValue(CurrencyType currency, string expected)
        {
            mock.Mock<IFedExSettingsRepository>()
                .Setup(r => r.GetAccountReadOnly(AnyIShipment))
                .Returns(new FedExAccountEntity() { CountryCode = "CN" });
            var shipment = Create.Shipment()
                .AsFedEx(f => f
                    .WithPackage()
                    .WithPackage()
                    .Set(x => x.Currency, (int) currency))
                .Build();

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(expected, result.RequestedShipment.RequestedPackageLineItems[0].InsuredValue.Currency.ToString());
            Assert.Equal(expected, result.RequestedShipment.RequestedPackageLineItems[1].InsuredValue.Currency.ToString());
        }

        [Theory]
        [InlineData("US", "USD")]
        [InlineData("CA", "CAD")]
        public void Manipulate_SetsInsuranceCurrencyFromAccount_WhenShipmentHasNoValue(string country, string expected)
        {
            mock.Mock<IFedExSettingsRepository>()
                .Setup(r => r.GetAccountReadOnly(AnyIShipment))
                .Returns(new FedExAccountEntity() { CountryCode = country });
            var shipment = Create.Shipment().AsFedEx(f => f.WithPackage().WithPackage()).Build();

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(expected, result.RequestedShipment.RequestedPackageLineItems[0].InsuredValue.Currency.ToString());
            Assert.Equal(expected, result.RequestedShipment.RequestedPackageLineItems[1].InsuredValue.Currency.ToString());
        }
    }
}
