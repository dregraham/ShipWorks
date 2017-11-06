using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators.International;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators.International
{
    public class FedExAdmissibilityManipulatorTest
    {
        private FedExAdmissibilityManipulator testObject;
        private ShipmentEntity shipment;

        public FedExAdmissibilityManipulatorTest()
        {
            shipment = Create.Shipment().AsFedEx().Build();
            testObject = new FedExAdmissibilityManipulator();
        }

        [Theory]
        [InlineData("CA", true)]
        [InlineData("US", false)]
        [InlineData("UK", false)]
        [InlineData("FR", false)]
        [InlineData("DE", false)]
        [InlineData("ES", false)]
        public void ShouldApply_ReturnsAppropriateValue_ForGivenInput(string countryCode, bool expected)
        {
            shipment.ShipCountryCode = countryCode;

            var result = testObject.ShouldApply(shipment, 0);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Manipulate_PhysicalPackagingTypeSpecifiedIsTrue_WhenShipCountryCodeIsCA()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.True(result.Value.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackagingSpecified);
        }

        [Theory]
        [InlineData(FedExPhysicalPackagingType.Bag, PhysicalPackagingType.BAG)]
        [InlineData(FedExPhysicalPackagingType.Barrel, PhysicalPackagingType.BARREL)]
        [InlineData(FedExPhysicalPackagingType.BasketOrHamper, PhysicalPackagingType.BASKET)]
        [InlineData(FedExPhysicalPackagingType.Box, PhysicalPackagingType.BOX)]
        [InlineData(FedExPhysicalPackagingType.Bucket, PhysicalPackagingType.BUCKET)]
        [InlineData(FedExPhysicalPackagingType.Bundle, PhysicalPackagingType.BUNDLE)]
        [InlineData(FedExPhysicalPackagingType.Carton, PhysicalPackagingType.CARTON)]
        [InlineData(FedExPhysicalPackagingType.Case, PhysicalPackagingType.CASE)]
        [InlineData(FedExPhysicalPackagingType.Container, PhysicalPackagingType.CONTAINER)]
        [InlineData(FedExPhysicalPackagingType.Crate, PhysicalPackagingType.CRATE)]
        [InlineData(FedExPhysicalPackagingType.Cylinder, PhysicalPackagingType.CYLINDER)]
        [InlineData(FedExPhysicalPackagingType.Drum, PhysicalPackagingType.DRUM)]
        [InlineData(FedExPhysicalPackagingType.Envelope, PhysicalPackagingType.ENVELOPE)]
        [InlineData(FedExPhysicalPackagingType.Pail, PhysicalPackagingType.PAIL)]
        [InlineData(FedExPhysicalPackagingType.Pallet, PhysicalPackagingType.PALLET)]
        [InlineData(FedExPhysicalPackagingType.Pieces, PhysicalPackagingType.PIECE)]
        [InlineData(FedExPhysicalPackagingType.Reel, PhysicalPackagingType.REEL)]
        [InlineData(FedExPhysicalPackagingType.Roll, PhysicalPackagingType.ROLL)]
        [InlineData(FedExPhysicalPackagingType.Skid, PhysicalPackagingType.SKID)]
        [InlineData(FedExPhysicalPackagingType.Tank, PhysicalPackagingType.TANK)]
        [InlineData(FedExPhysicalPackagingType.Tube, PhysicalPackagingType.TUBE)]
        [InlineData(FedExPhysicalPackagingType.Hamper, PhysicalPackagingType.HAMPER)]
        [InlineData(FedExPhysicalPackagingType.Other, PhysicalPackagingType.OTHER)]
        public void Manipulate_SetsPackagingType_FromShipment(FedExPhysicalPackagingType packaging, PhysicalPackagingType expected)
        {
            shipment.FedEx.CustomsAdmissibilityPackaging = (int) packaging;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(expected, result.Value.RequestedShipment.RequestedPackageLineItems[0].PhysicalPackaging);
        }

        [Fact]
        public void Manipulate_ReturnsFailure_WhenFedExTypeIsUnknown()
        {
            shipment.FedEx.CustomsAdmissibilityPackaging = int.MaxValue;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.True(result.Failure);
            Assert.IsAssignableFrom<InvalidOperationException>(result.Exception);
        }
    }
}
