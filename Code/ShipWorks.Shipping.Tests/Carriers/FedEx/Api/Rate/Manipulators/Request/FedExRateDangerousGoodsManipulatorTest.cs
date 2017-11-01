using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Shipping.FedEx;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    public class FedExRateDangerousGoodsManipulatorTest : IDisposable
    {
        readonly AutoMock mock;

        public FedExRateDangerousGoodsManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, true)]
        [InlineData(false, true, false)]
        [InlineData(false, false, false)]
        public void ShouldApply_ReturnsAppropriateValue_ForInput(bool firstEnabled, bool secondEnabled, bool expected)
        {
            var shipment = Create.Shipment()
                .AsFedEx(f => f
                    .WithPackage(p => p.Set(x => x.DangerousGoodsEnabled, firstEnabled))
                    .WithPackage(p => p.Set(x => x.DangerousGoodsEnabled, secondEnabled)))
                .Build();
            var testObject = mock.Create<FedExRateDangerousGoodsManipulator>();

            var result = testObject.ShouldApply(shipment, FedExRateRequestOptions.None);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Foo", null, null)]
        [InlineData("Foo", "Bar", null)]
        [InlineData("Foo", null, "Baz")]
        [InlineData(null, "Bar", null)]
        [InlineData(null, "Bar", "Baz")]
        [InlineData(null, null, "Baz")]
        [InlineData("Foo", "Bar", "Baz")]
        public void Manipulator_SetsSignatory_WhenAnySignaturePieceIsSet(string contactName, string place, string title)
        {
            var shipment = Create.Shipment()
                .AsFedEx(f => f.WithPackage(p => p
                    .Set(x => x.SignatoryContactName, contactName)
                    .Set(x => x.SignatoryPlace, place)
                    .Set(x => x.SignatoryTitle, title)))
                .Build(); var testObject = mock.Create<FedExRateDangerousGoodsManipulator>();

            var result = testObject.Manipulate(shipment, new RateRequest());

            var signatory = result.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail.Signatory;
            Assert.NotNull(signatory);
            Assert.Equal(contactName, signatory.ContactName);
            Assert.Equal(place, signatory.Place);
            Assert.Equal(title, signatory.Title);
        }

        [Theory]
        [InlineData("", null, null)]
        [InlineData("", "", null)]
        [InlineData("", null, "")]
        [InlineData(null, "", null)]
        [InlineData(null, "", "")]
        [InlineData(null, null, "")]
        [InlineData("", "", "")]
        public void Manipulator_DoesNotSetSignatory_WhenAllPiecesAreNullOrEmpty(string contactName, string place, string title)
        {
            var shipment = Create.Shipment()
                .AsFedEx(f => f.WithPackage(p => p
                    .Set(x => x.SignatoryContactName, contactName)
                    .Set(x => x.SignatoryPlace, place)
                    .Set(x => x.SignatoryTitle, title)))
                .Build(); var testObject = mock.Create<FedExRateDangerousGoodsManipulator>();

            var result = testObject.Manipulate(shipment, new RateRequest());

            var signatory = result.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail.Signatory;
            Assert.Null(signatory);
        }

        [Theory]
        [InlineData(FedExBatteryMaterialType.LithiumIon, BatteryMaterialType.LITHIUM_ION)]
        [InlineData(FedExBatteryMaterialType.LithiumMetal, BatteryMaterialType.LITHIUM_METAL)]
        public void Manipulate_SetsBatteryMaterial_WithCorrectValue(FedExBatteryMaterialType input, BatteryMaterialType expected)
        {
            var shipment = GetShipment(x => x.BatteryMaterial = input);
            var testObject = mock.Create<FedExRateDangerousGoodsManipulator>();

            var request = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(expected, request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.BatteryDetails[0].Material);
            Assert.True(request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.BatteryDetails[0].MaterialSpecified);
        }

        [Fact]
        public void Manipulate_DoesNotSetBatteryMaterial_WhenMaterialIsNotSpecified()
        {
            var shipment = GetShipment(x => x.BatteryMaterial = FedExBatteryMaterialType.NotSpecified);
            var testObject = mock.Create<FedExRateDangerousGoodsManipulator>();

            var request = testObject.Manipulate(shipment, new RateRequest());

            Assert.False(request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.BatteryDetails[0].MaterialSpecified);
        }

        [Theory]
        [InlineData(FedExBatteryPackingType.ContainsInEquipement, BatteryPackingType.CONTAINED_IN_EQUIPMENT)]
        [InlineData(FedExBatteryPackingType.PackedWithEquipment, BatteryPackingType.PACKED_WITH_EQUIPMENT)]
        public void Manipulate_SetsBatteryPacking_WithCorrectValue(FedExBatteryPackingType input, BatteryPackingType expected)
        {
            var shipment = GetShipment(x => x.BatteryPacking = input);
            var testObject = mock.Create<FedExRateDangerousGoodsManipulator>();

            var request = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(expected, request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.BatteryDetails[0].Packing);
            Assert.True(request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.BatteryDetails[0].PackingSpecified);
        }

        [Fact]
        public void Manipulate_DoesNotSetBatteryPacking_WhenPackingIsNotSpecified()
        {
            var shipment = GetShipment(x => x.BatteryPacking = FedExBatteryPackingType.NotSpecified);
            var testObject = mock.Create<FedExRateDangerousGoodsManipulator>();

            var request = testObject.Manipulate(shipment, new RateRequest());

            Assert.False(request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.BatteryDetails[0].PackingSpecified);
        }

        [Theory]
        [InlineData(FedExBatteryRegulatorySubType.IATASectionII, BatteryRegulatorySubType.IATA_SECTION_II)]
        public void Manipulate_SetsBatteryRegulatorySubtype_WithCorrectValue(FedExBatteryRegulatorySubType input, BatteryRegulatorySubType expected)
        {
            var shipment = GetShipment(x => x.BatteryRegulatorySubtype = input);
            var testObject = mock.Create<FedExRateDangerousGoodsManipulator>();

            var request = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal(expected, request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.BatteryDetails[0].RegulatorySubType);
            Assert.True(request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.BatteryDetails[0].RegulatorySubTypeSpecified);
        }

        [Fact]
        public void Manipulate_DoesNotSetBatteryRegulatorySubtype_WhenRegulatorySubtypeIsNotSpecified()
        {
            var shipment = GetShipment(x => x.BatteryRegulatorySubtype = FedExBatteryRegulatorySubType.NotSpecified);
            var testObject = mock.Create<FedExRateDangerousGoodsManipulator>();

            var request = testObject.Manipulate(shipment, new RateRequest());

            Assert.False(request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.BatteryDetails[0].RegulatorySubTypeSpecified);
        }

        private static ShipWorks.Data.Model.EntityClasses.ShipmentEntity GetShipment(Action<FedExPackageEntity> setBatteryValue)
        {
            return Create.Shipment()
                .AsFedEx(f => f.WithPackage(p => p
                    .Set(x => x.DangerousGoodsEnabled = true)
                    .Set(x => x.DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.Batteries)
                    .Set(setBatteryValue)))
                .Build();
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
