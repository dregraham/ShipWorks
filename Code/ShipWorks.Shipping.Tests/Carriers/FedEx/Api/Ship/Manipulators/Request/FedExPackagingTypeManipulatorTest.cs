using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExPackagingTypeManipulatorTest
    {
        private FedExPackagingTypeManipulator testObject;
        private ShipmentEntity shipment;

        public FedExPackagingTypeManipulatorTest()
        {
            shipment = BuildFedExShipmentEntity.SetupRequestShipmentEntity();
            shipment.FedEx.PackagingType = (int) FedExPackagingType.Box;

            testObject = new FedExPackagingTypeManipulator();
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
        public void Manipulate_FedExPackagingTypeManipulator_ReturnsPackagingType(FedExPackagingType packaging, PackagingType expected)
        {
            shipment.FedEx.PackagingType = (int) packaging;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(expected, result.Value.RequestedShipment.PackagingType);
        }

        [Fact]
        public void Manipulate_ReturnsFailure_WhenPackagingIsUnknown()
        {
            shipment.FedEx.PackagingType = int.MaxValue;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.True(result.Failure);
            Assert.IsAssignableFrom<InvalidOperationException>(result.Exception);
        }
    }
}
