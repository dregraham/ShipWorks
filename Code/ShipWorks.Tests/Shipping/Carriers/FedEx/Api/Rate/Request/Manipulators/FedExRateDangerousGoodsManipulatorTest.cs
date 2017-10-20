using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Shipping.FedEx;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators
{
    public class FedExRateDangerousGoodsManipulatorTest : IDisposable
    {
        readonly AutoMock mock;

        public FedExRateDangerousGoodsManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Theory]
        [InlineData(FedExBatteryMaterialType.LithiumIon, BatteryMaterialType.LITHIUM_ION)]
        [InlineData(FedExBatteryMaterialType.LithiumMetal, BatteryMaterialType.LITHIUM_METAL)]
        public void Manipulate_SetsBatteryMaterial_WithCorrectValue(FedExBatteryMaterialType input, BatteryMaterialType expected)
        {
            var shipment = GetShipment(x => x.BatteryMaterial = input);
            var testObject = mock.Create<FedExRateDangerousGoodsManipulator>();

            var request = testObject.Manipulate(shipment, new RateRequest(), 0);

            Assert.Equal(expected, request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.BatteryDetails[0].Material);
            Assert.True(request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.BatteryDetails[0].MaterialSpecified);
        }

        [Fact]
        public void Manipulate_DoesNotSetBatteryMaterial_WhenMaterialIsNotSpecified()
        {
            var shipment = GetShipment(x => x.BatteryMaterial = FedExBatteryMaterialType.NotSpecified);
            var testObject = mock.Create<FedExRateDangerousGoodsManipulator>();

            var request = testObject.Manipulate(shipment, new RateRequest(), 0);

            Assert.False(request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.BatteryDetails[0].MaterialSpecified);
        }

        [Theory]
        [InlineData(FedExBatteryPackingType.ContainsInEquipement, BatteryPackingType.CONTAINED_IN_EQUIPMENT)]
        [InlineData(FedExBatteryPackingType.PackedWithEquipment, BatteryPackingType.PACKED_WITH_EQUIPMENT)]
        public void Manipulate_SetsBatteryPacking_WithCorrectValue(FedExBatteryPackingType input, BatteryPackingType expected)
        {
            var shipment = GetShipment(x => x.BatteryPacking = input);
            var testObject = mock.Create<FedExRateDangerousGoodsManipulator>();

            var request = testObject.Manipulate(shipment, new RateRequest(), 0);

            Assert.Equal(expected, request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.BatteryDetails[0].Packing);
            Assert.True(request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.BatteryDetails[0].PackingSpecified);
        }

        [Fact]
        public void Manipulate_DoesNotSetBatteryPacking_WhenPackingIsNotSpecified()
        {
            var shipment = GetShipment(x => x.BatteryPacking = FedExBatteryPackingType.NotSpecified);
            var testObject = mock.Create<FedExRateDangerousGoodsManipulator>();

            var request = testObject.Manipulate(shipment, new RateRequest(), 0);

            Assert.False(request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.BatteryDetails[0].PackingSpecified);
        }

        [Theory]
        [InlineData(FedExBatteryRegulatorySubType.IATASectionII, BatteryRegulatorySubType.IATA_SECTION_II)]
        public void Manipulate_SetsBatteryRegulatorySubtype_WithCorrectValue(FedExBatteryRegulatorySubType input, BatteryRegulatorySubType expected)
        {
            var shipment = GetShipment(x => x.BatteryRegulatorySubtype = input);
            var testObject = mock.Create<FedExRateDangerousGoodsManipulator>();

            var request = testObject.Manipulate(shipment, new RateRequest(), 0);

            Assert.Equal(expected, request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.BatteryDetails[0].RegulatorySubType);
            Assert.True(request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.BatteryDetails[0].RegulatorySubTypeSpecified);
        }

        [Fact]
        public void Manipulate_DoesNotSetBatteryRegulatorySubtype_WhenRegulatorySubtypeIsNotSpecified()
        {
            var shipment = GetShipment(x => x.BatteryRegulatorySubtype = FedExBatteryRegulatorySubType.NotSpecified);
            var testObject = mock.Create<FedExRateDangerousGoodsManipulator>();

            var request = testObject.Manipulate(shipment, new RateRequest(), 0);

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
