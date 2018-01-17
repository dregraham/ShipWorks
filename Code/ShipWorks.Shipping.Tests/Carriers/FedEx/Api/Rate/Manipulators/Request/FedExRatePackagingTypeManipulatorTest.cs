using System;
using Autofac.Extras.Moq;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    public class FedExRatePackagingTypeManipulatorTest
    {
        private FedExRatePackagingTypeManipulator testObject;
        private readonly AutoMock mock;

        public FedExRatePackagingTypeManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testObject = mock.Create<FedExRatePackagingTypeManipulator>();
        }

        [Theory]
        [InlineData(FedExPackagingType.Box, PackagingType.FEDEX_BOX)]
        [InlineData(FedExPackagingType.Box10Kg, PackagingType.FEDEX_10KG_BOX)]
        [InlineData(FedExPackagingType.Box25Kg, PackagingType.FEDEX_25KG_BOX)]
        [InlineData(FedExPackagingType.Custom, PackagingType.YOUR_PACKAGING)]
        [InlineData(FedExPackagingType.Envelope, PackagingType.FEDEX_ENVELOPE)]
        [InlineData(FedExPackagingType.Pak, PackagingType.FEDEX_PAK)]
        [InlineData(FedExPackagingType.Tube, PackagingType.FEDEX_TUBE)]
        [InlineData(FedExPackagingType.SmallBox, PackagingType.FEDEX_SMALL_BOX)]
        [InlineData(FedExPackagingType.MediumBox, PackagingType.FEDEX_MEDIUM_BOX)]
        [InlineData(FedExPackagingType.LargeBox, PackagingType.FEDEX_LARGE_BOX)]
        [InlineData(FedExPackagingType.ExtraLargeBox, PackagingType.FEDEX_EXTRA_LARGE_BOX)]
        public void Manipulate_SetFedExPackagingType_FromFedExPackage(FedExPackagingType packagingType, PackagingType expected)
        {
            var shipment = Create.Shipment().AsFedEx(f => f.Set(x => x.PackagingType, (int) packagingType)).Build();

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(expected, result.RequestedShipment.PackagingType);
        }

        [Fact]
        public void Manipulate_PackagingTypeSpecifiedIsTrue()
        {
            var shipment = Create.Shipment().AsFedEx().Build();

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.True(result.RequestedShipment.PackagingTypeSpecified);
        }

        [Fact]
        public void Manipulate_ThrowsException_WhenPackageTypeIsUnknown()
        {
            var shipment = Create.Shipment().AsFedEx(f => f.Set(x => x.PackagingType, 999)).Build();

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate(shipment, new RateRequest()));
        }
    }
}
