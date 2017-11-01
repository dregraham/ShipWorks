using System;
using Autofac.Extras.Moq;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    public class FedExRateDryIceManipulatorTest : IDisposable
    {
        readonly AutoMock mock;

        public FedExRateDryIceManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Theory]
        [InlineData(0, 0, false)]
        [InlineData(1, 0, true)]
        [InlineData(0, 1, false)]
        [InlineData(1, 1, true)]
        public void ShouldApply_ReturnsValue_ForGivenInputs(double first, double second, bool expected)
        {
            var shipment = Create.Shipment().AsFedEx(f => f
                    .WithPackage(p => p.Set(x => x.DryIceWeight, first))
                    .WithPackage(p => p.Set(x => x.DryIceWeight, second)))
                .Build();
            var testObject = mock.Create<FedExRateDryIceManipulator>();

            var result = testObject.ShouldApply(shipment, FedExRateRequestOptions.None);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenPackageHasDryIceButNonCustomPackaging()
        {
            var shipment = Create.Shipment().AsFedEx(f => f
                    .WithPackage(p => p.Set(x => x.DryIceWeight, 1))
                    .Set(x => x.PackagingType, (int) FedExPackagingType.Envelope))
                .Build();
            var testObject = mock.Create<FedExRateDryIceManipulator>();

            Assert.Throws<FedExException>(() => testObject.Manipulate(shipment, new RateRequest()));
        }

        [Fact]
        public void Manipulate_AddsDryIceToSpecialServices_WhenSpecialServicesIsEmpty()
        {
            var shipment = Create.Shipment()
                .AsFedEx(f => f
                    .WithPackage(p => p.Set(x => x.DryIceWeight, 1.2))
                    .Set(x => x.PackagingType, (int) FedExPackagingType.Custom))
                .Build();
            var testObject = mock.Create<FedExRateDryIceManipulator>();

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Contains(PackageSpecialServiceType.DRY_ICE,
                result.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes);
        }

        [Fact]
        public void Manipulate_AddsDryIceToSpecialServices_WhenSpecialServicesContainsOtherItems()
        {
            var rateRequest = new RateRequest();
            var services = rateRequest.Ensure(x => x.RequestedShipment)
                .EnsureAtLeastOne(x => x.RequestedPackageLineItems)
                .Ensure(x => x.SpecialServicesRequested);
            services.SpecialServiceTypes = new[] { PackageSpecialServiceType.COD };

            var shipment = Create.Shipment()
                .AsFedEx(f => f
                    .WithPackage(p => p.Set(x => x.DryIceWeight, 1.2))
                    .Set(x => x.PackagingType, (int) FedExPackagingType.Custom))
                .Build();
            var testObject = mock.Create<FedExRateDryIceManipulator>();

            var result = testObject.Manipulate(shipment, rateRequest);

            Assert.Contains(PackageSpecialServiceType.DRY_ICE,
                result.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes);
            Assert.Contains(PackageSpecialServiceType.COD,
                result.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes);
        }

        [Fact]
        public void Manipulate_SetsWeight_WhenSpecialServicesIsEmpty()
        {
            var shipment = Create.Shipment()
                .AsFedEx(f => f
                    .WithPackage(p => p.Set(x => x.DryIceWeight, 2.2046))
                    .Set(x => x.PackagingType, (int) FedExPackagingType.Custom))
                .Build();
            var testObject = mock.Create<FedExRateDryIceManipulator>();

            var result = testObject.Manipulate(shipment, new RateRequest());

            var weight = result.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DryIceWeight;
            Assert.Equal(WeightUnits.KG, weight.Units);
            Assert.True(weight.UnitsSpecified);
            Assert.Equal(1M, weight.Value);
            Assert.True(weight.ValueSpecified);
        }

        [Fact]
        public void Manipulate_SetsWeight_WhenSpecialServicesIsAlreadySet()
        {
            var rateRequest = new RateRequest();
            rateRequest.Ensure(x => x.RequestedShipment)
                .EnsureAtLeastOne(x => x.RequestedPackageLineItems)
                .Ensure(x => x.SpecialServicesRequested)
                .Ensure(x => x.CodDetail);

            var shipment = Create.Shipment()
                .AsFedEx(f => f
                    .WithPackage(p => p.Set(x => x.DryIceWeight, 2.2046))
                    .Set(x => x.PackagingType, (int) FedExPackagingType.Custom))
                .Build();
            var testObject = mock.Create<FedExRateDryIceManipulator>();

            var result = testObject.Manipulate(shipment, rateRequest);

            Assert.NotNull(result.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.CodDetail);

            var weight = result.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DryIceWeight;
            Assert.Equal(WeightUnits.KG, weight.Units);
            Assert.True(weight.UnitsSpecified);
            Assert.Equal(1M, weight.Value);
            Assert.True(weight.ValueSpecified);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
