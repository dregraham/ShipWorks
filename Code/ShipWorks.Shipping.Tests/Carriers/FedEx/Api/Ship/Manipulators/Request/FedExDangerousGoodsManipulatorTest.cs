using System;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Shipping.FedEx;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    public class FedExDangerousGoodsManipulatorTest
    {
        private ShipmentEntity shipment;
        private FedExDangerousGoodsManipulator testObject;

        readonly AutoMock mock;

        public FedExDangerousGoodsManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipment = Create.Shipment().AsFedEx().Build();
            shipment.FedEx.RequestedLabelFormat = (int) ThermalLanguage.None;

            FedExPackageEntity package = new FedExPackageEntity
            {
                DangerousGoodsEnabled = true,
                DangerousGoodsAccessibilityType = (int) FedExDangerousGoodsAccessibilityType.Accessible,
                DangerousGoodsEmergencyContactPhone = "555-555-5555",
                DangerousGoodsOfferor = "some offeror",
                DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.Batteries,
                DangerousGoodsCargoAircraftOnly = false,
            };

            // Add the package containing dangerous goods to the fedex shipment
            shipment.FedEx.Packages.Add(package);

            testObject = mock.Create<FedExDangerousGoodsManipulator>();
        }

        [Fact]
        public void Manipulate_ReturnsFailureWithFedExException_WhenDangerousGoodsEnabled_AndThermalLabelRequested()
        {
            shipment.FedEx.RequestedLabelFormat = (int) ThermalLanguage.ZPL;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.True(result.Failure);
            Assert.IsAssignableFrom<FedExException>(result.Exception);
        }

        [Theory]
        [InlineData(ThermalLanguage.ZPL)]
        [InlineData(ThermalLanguage.EPL)]
        public void Manipulate_NoException_WhenDangerousGoodsNotEnabled_AndThermalLabelRequested(ThermalLanguage language)
        {
            shipment.FedEx.RequestedLabelFormat = (int) language;

            shipment.FedEx.Packages[0].DangerousGoodsEnabled = false;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.True(result.Success);
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedShipment()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            // The requested shipment property should be created now
            Assert.NotNull(result.Value.RequestedShipment);
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedPackageLineItems()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            // The requested package line items property should be created now
            Assert.NotNull(result.Value.RequestedShipment.RequestedPackageLineItems);
        }

        [Fact]
        public void Manipulate_DangerousGoodsDetailPropertyIsNotNull_WhenDangerousGoodsEnabledIsTrue()
        {
            shipment.FedEx.Packages[0].DangerousGoodsEnabled = true;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.NotNull(result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail);
        }

        [Theory]
        [InlineData(true, false, 0, true)]
        [InlineData(true, true, 0, true)]
        [InlineData(false, false, 0, false)]
        [InlineData(false, true, 0, false)]
        [InlineData(true, false, 1, false)]
        [InlineData(true, true, 1, true)]
        [InlineData(false, false, 1, false)]
        [InlineData(false, true, 1, true)]
        public void ShouldApply_ReturnsAppropriateValue_ForGivenInput(bool firstEnabled, bool secondEnabled, int sequenceNumber, bool expected)
        {
            var testShipment = Create.Shipment().AsFedEx(f => f
                    .WithPackage(p => p.Set(x => x.DangerousGoodsEnabled, firstEnabled))
                    .WithPackage(p => p.Set(x => x.DangerousGoodsEnabled, secondEnabled)))
                .Build();

            var result = testObject.ShouldApply(testShipment, sequenceNumber);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Manipulate_AddsDangerousGoodsOption()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            PackageSpecialServicesRequested servicesRequested = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested;
            Assert.Equal(PackageSpecialServiceType.DANGEROUS_GOODS, servicesRequested.SpecialServiceTypes[0]);
        }

        [Fact]
        public void Manipulate_AddsDangerousGoodsOption_WhenSpecialServicesRequestedIsNull()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            PackageSpecialServicesRequested servicesRequested = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested;
            Assert.Equal(PackageSpecialServiceType.DANGEROUS_GOODS, servicesRequested.SpecialServiceTypes[0]);
        }


        [Fact]
        public void Manipulate_AddsDangerousGoodsOption_WhenServiceTypesIsNull()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            PackageSpecialServicesRequested servicesRequested = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested;
            Assert.Equal(PackageSpecialServiceType.DANGEROUS_GOODS, servicesRequested.SpecialServiceTypes[0]);
        }

        [Fact]
        public void Manipulate_AddsDangerousGoodsOption_AndRetainsExistingServiceTypes_WhenServiceTypesIsNotEmpty()
        {
            var request = new ProcessShipmentRequest();
            request.Ensure(x => x.RequestedShipment)
                .EnsureAtLeastOne(x => x.RequestedPackageLineItems)
                .Ensure(x => x.SpecialServicesRequested)
                .SpecialServiceTypes =
                new[]
                {
                    PackageSpecialServiceType.COD,
                    PackageSpecialServiceType.DRY_ICE
                };

            var result = testObject.Manipulate(shipment, request, 0);

            // Check that the previous service types are retained
            PackageSpecialServicesRequested servicesRequested = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested;
            Assert.Contains(PackageSpecialServiceType.COD, servicesRequested.SpecialServiceTypes);
            Assert.Contains(PackageSpecialServiceType.DRY_ICE, servicesRequested.SpecialServiceTypes);
            Assert.Contains(PackageSpecialServiceType.DANGEROUS_GOODS, servicesRequested.SpecialServiceTypes);
        }

        [Fact]
        public void Manipulate_AccessibilityTypeIsAccessible_WhenDangerousGoodsTypeIsLithiumBatteries()
        {
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.Batteries;
            shipment.FedEx.Packages[0].DangerousGoodsAccessibilityType = (int) FedExDangerousGoodsAccessibilityType.Accessible;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(DangerousGoodsAccessibilityType.ACCESSIBLE, dangerousGoods.Accessibility);
        }

        [Fact]
        public void Manipulate_AccessibilityTypeIsInAccessible_WhenDangerousGoodsTypeIsLithiumBatteries()
        {
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.Batteries;
            shipment.FedEx.Packages[0].DangerousGoodsAccessibilityType = (int) FedExDangerousGoodsAccessibilityType.Inaccessible;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(DangerousGoodsAccessibilityType.INACCESSIBLE, dangerousGoods.Accessibility);
        }

        [Fact]
        public void Manipulate_AccessibilitySpecifiedIsTrue_WhenDangerousGoodsTypeIsLithiumBatteries()
        {
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.Batteries;
            shipment.FedEx.Packages[0].DangerousGoodsAccessibilityType = (int) FedExDangerousGoodsAccessibilityType.Inaccessible;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.True(dangerousGoods.AccessibilitySpecified);
        }

        [Fact]
        public void Manipulate_AddsBatterySpecialServiceType_WhenShipmentHasBatteries()
        {
            var shipment = GetShipment(x => x.BatteryMaterial = FedExBatteryMaterialType.LithiumIon);
            var testObject = mock.Create<FedExDangerousGoodsManipulator>();

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.True(result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes.Contains(PackageSpecialServiceType.BATTERY));
        }
        [Fact]
        public void Manipulate_DoesNotAddBatterySpecialServiceType_WhenShipmentHasNoBatteries()
        {
            //var shipment = GetShipment(x => x.BatteryMaterial = FedExBatteryMaterialType.NotSpecified);
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.NotApplicable;

            var testObject = mock.Create<FedExDangerousGoodsManipulator>();

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.False(result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes.Contains(PackageSpecialServiceType.BATTERY));
        }

        [Theory]
        [InlineData(FedExBatteryMaterialType.LithiumIon, BatteryMaterialType.LITHIUM_ION)]
        [InlineData(FedExBatteryMaterialType.LithiumMetal, BatteryMaterialType.LITHIUM_METAL)]
        public void Manipulate_SetsBatteryMaterial_WithCorrectValue(FedExBatteryMaterialType input, BatteryMaterialType expected)
        {
            shipment = GetShipment(x => x.BatteryMaterial = input);
            testObject = mock.Create<FedExDangerousGoodsManipulator>();

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);


            Assert.Equal(expected, result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested
                    .BatteryDetails[0].Material);

            Assert.True(result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.BatteryDetails[0].MaterialSpecified);
        }

        [Fact]
        public void Manipulate_DoesNotSetBatteryMaterial_WhenMaterialIsNotSpecified()
        {
            shipment = GetShipment(x => x.BatteryMaterial = FedExBatteryMaterialType.NotSpecified);
            testObject = mock.Create<FedExDangerousGoodsManipulator>();

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.False(result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.BatteryDetails[0].MaterialSpecified);
        }

        [Theory]
        [InlineData(FedExBatteryPackingType.ContainsInEquipement, BatteryPackingType.CONTAINED_IN_EQUIPMENT)]
        [InlineData(FedExBatteryPackingType.PackedWithEquipment, BatteryPackingType.PACKED_WITH_EQUIPMENT)]
        public void Manipulate_SetsBatteryPacking_WithCorrectValue(FedExBatteryPackingType input, BatteryPackingType expected)
        {
            shipment = GetShipment(x => x.BatteryPacking = input);
            testObject = mock.Create<FedExDangerousGoodsManipulator>();

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(expected, result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.BatteryDetails[0].Packing);
            Assert.True(result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.BatteryDetails[0].PackingSpecified);
        }

        [Fact]
        public void Manipulate_DoesNotSetBatteryPacking_WhenPackingIsNotSpecified()
        {
            shipment = GetShipment(x => x.BatteryPacking = FedExBatteryPackingType.NotSpecified);
            testObject = mock.Create<FedExDangerousGoodsManipulator>();

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.False(result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.BatteryDetails[0].PackingSpecified);
        }

        [Theory]
        [InlineData(FedExBatteryRegulatorySubType.IATASectionII, BatteryRegulatorySubType.IATA_SECTION_II)]
        public void Manipulate_SetsBatteryRegulatorySubtype_WithCorrectValue(FedExBatteryRegulatorySubType input, BatteryRegulatorySubType expected)
        {
            shipment = GetShipment(x => x.BatteryRegulatorySubtype = input);
            testObject = mock.Create<FedExDangerousGoodsManipulator>();

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(expected, result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.BatteryDetails[0].RegulatorySubType);
            Assert.True(result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.BatteryDetails[0].RegulatorySubTypeSpecified);
        }

        [Fact]
        public void Manipulate_DoesNotSetBatteryRegulatorySubtype_WhenRegulatorySubtypeIsNotSpecified()
        {
            shipment = GetShipment(x => x.BatteryRegulatorySubtype = FedExBatteryRegulatorySubType.NotSpecified);
            testObject = mock.Create<FedExDangerousGoodsManipulator>();

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.False(result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.BatteryDetails[0].RegulatorySubTypeSpecified);
        }

        [Fact]
        public void Manipulate_AccessibilityTypeIsAccessible_WhenDangerousGoodsTypeIsORMD()
        {
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.OrmD;
            shipment.FedEx.Packages[0].DangerousGoodsAccessibilityType = (int) FedExDangerousGoodsAccessibilityType.Accessible;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(DangerousGoodsAccessibilityType.ACCESSIBLE, dangerousGoods.Accessibility);
        }

        [Fact]
        public void Manipulate_AccessibilityTypeIsInAccessible_WhenDangerousGoodsTypeIsORMD()
        {
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.OrmD;
            shipment.FedEx.Packages[0].DangerousGoodsAccessibilityType = (int) FedExDangerousGoodsAccessibilityType.Inaccessible;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(DangerousGoodsAccessibilityType.INACCESSIBLE, dangerousGoods.Accessibility);
        }

        [Fact]
        public void Manipulate_AccessibilitySpecifiedIsFalse_WhenDangerousGoodsTypeIsHazardousMaterials()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.HazardousMaterials;
            shipment.FedEx.Packages[0].DangerousGoodsAccessibilityType = (int) FedExDangerousGoodsAccessibilityType.Inaccessible;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.False(dangerousGoods.AccessibilitySpecified);
        }

        [Fact]
        public void Manipulate_AccessibilitySpecifiedIsTrue_WhenDangerousGoodsTypeIsORMD()
        {
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.OrmD;
            shipment.FedEx.Packages[0].DangerousGoodsAccessibilityType = (int) FedExDangerousGoodsAccessibilityType.Inaccessible;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.True(dangerousGoods.AccessibilitySpecified);
        }

        [Fact]
        public void Manipulate_CargoAircraftIsTrue()
        {
            shipment.FedEx.Packages[0].DangerousGoodsCargoAircraftOnly = true;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.True(dangerousGoods.CargoAircraftOnly);
        }

        [Fact]
        public void Manipulate_CargoAircraftIsFalse()
        {
            shipment.FedEx.Packages[0].DangerousGoodsCargoAircraftOnly = false;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.False(dangerousGoods.CargoAircraftOnly);
        }

        [Fact]
        public void Manipulate_CargoAircraftSpecifiedIsTrue()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.True(dangerousGoods.CargoAircraftOnlySpecified);
        }

        [Fact]
        public void Manipulate_EmergencyContactNumber()
        {
            shipment.FedEx.Packages[0].DangerousGoodsEmergencyContactPhone = "123-4565-7890";

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal("123-4565-7890", dangerousGoods.EmergencyContactNumber);
        }

        [Fact]
        public void Manipulate_Offeror()
        {
            shipment.FedEx.Packages[0].DangerousGoodsOfferor = "the offeror";

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal("the offeror", dangerousGoods.Offeror);
        }

        [Fact]
        public void Manipulate_OptionArrayHasOneItem()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(1, dangerousGoods.Options.Length);
        }

        [Fact]
        public void Manipulate_OptionIsNotApplicable()
        {
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.NotApplicable;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Null(dangerousGoods.Options);
        }

        [Fact]
        public void Manipulate_OptionIsLithiumBattery()
        {
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.Batteries;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(HazardousCommodityOptionType.BATTERY, dangerousGoods.Options[0]);
        }

        [Fact]
        public void Manipulate_OptionIsOrmD()
        {
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.OrmD;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(HazardousCommodityOptionType.ORM_D, dangerousGoods.Options[0]);
        }

        [Fact]
        public void Manipulate_OptionIsHazardousMaterials()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.HazardousMaterials;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(HazardousCommodityOptionType.HAZARDOUS_MATERIALS, dangerousGoods.Options[0]);
        }

        [Fact]
        public void Manipulate_ReturnsFailureWithInvalidOperationException_WhenUnrecognizedOptionTypeIsProvided()
        {
            shipment.FedEx.Packages[0].DangerousGoodsType = int.MaxValue;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.True(result.Failure);
            Assert.IsAssignableFrom<InvalidOperationException>(result.Exception);
        }

        [Fact]
        public void Manipulate_ContainerIsNull_WhenOptionIsNotHazardousMaterials()
        {
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.Batteries;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Null(dangerousGoods.Containers);
        }

        [Fact]
        public void Manipulate_OP900Set_WhenDangerousGoodsSet()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.True(result.Value.RequestedShipment.ShippingDocumentSpecification.ShippingDocumentTypes.Contains(RequestedShippingDocumentType.OP_900));
            Assert.Equal(ShippingDocumentImageType.PDF, result.Value.RequestedShipment.ShippingDocumentSpecification.Op900Detail.Format.ImageType);
            Assert.Equal(ShippingDocumentStockType.OP_900_LL_B, result.Value.RequestedShipment.ShippingDocumentSpecification.Op900Detail.Format.StockType);
            Assert.True(result.Value.RequestedShipment.ShippingDocumentSpecification.Op900Detail.Format.ImageTypeSpecified);
            Assert.True(result.Value.RequestedShipment.ShippingDocumentSpecification.Op900Detail.Format.StockTypeSpecified);
        }

        [Fact]
        public void Manipulate_ContainerContainsOneItem_WhenOptionIsHazardousMaterials()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.HazardousMaterials;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(1, dangerousGoods.Containers.Length);
        }

        [Fact]
        public void Manipulate_HazardousCommoditiesContainsOneItem_WhenOptionIsHazardousMaterials()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.HazardousMaterials;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(1, dangerousGoods.Containers[0].HazardousCommodities.Length);
        }

        [Fact]
        public void Manipulate_HazardousCommoditiesContentDescriptionIsNotNull_WhenOptionIsHazardousMaterials()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.HazardousMaterials;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.NotNull(dangerousGoods.Containers[0].HazardousCommodities[0]);
        }

        [Fact]
        public void Manipulate_HazardousCommoditiesContentDescriptionId_WhenOptionIsHazardousMaterials()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;
            shipment.FedEx.Packages[0].HazardousMaterialNumber = "UN2533";
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.HazardousMaterials;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal("UN2533", dangerousGoods.Containers[0].HazardousCommodities[0].Description.Id);
        }

        [Fact]
        public void Manipulate_HazardousCommoditiesContentDescriptionHazardClass_WhenOptionIsHazardousMaterials()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;
            shipment.FedEx.Packages[0].HazardousMaterialClass = "6.1";
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.HazardousMaterials;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal("6.1", dangerousGoods.Containers[0].HazardousCommodities[0].Description.HazardClass);
        }

        [Fact]
        public void Manipulate_HazardousCommoditiesProperShippingName_WhenOptionIsHazardousMaterials()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;
            shipment.FedEx.Packages[0].HazardousMaterialProperName = "Methyl trichloroacetate";
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.HazardousMaterials;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal("Methyl trichloroacetate", dangerousGoods.Containers[0].HazardousCommodities[0].Description.ProperShippingName);
        }

        [Fact]
        public void Manipulate_HazardousCommoditiesPackingGroupIsDefault_WhenOptionIsHazardousMaterials()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;
            shipment.FedEx.Packages[0].HazardousMaterialPackingGroup = (int) FedExHazardousMaterialsPackingGroup.Default;
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.HazardousMaterials;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(HazardousCommodityPackingGroupType.DEFAULT, dangerousGoods.Containers[0].HazardousCommodities[0].Description.PackingGroup);
        }

        [Fact]
        public void Manipulate_HazardousCommoditiesPackingGroupIsI_WhenOptionIsHazardousMaterials()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;
            shipment.FedEx.Packages[0].HazardousMaterialPackingGroup = (int) FedExHazardousMaterialsPackingGroup.I;
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.HazardousMaterials;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(HazardousCommodityPackingGroupType.I, dangerousGoods.Containers[0].HazardousCommodities[0].Description.PackingGroup);
        }

        [Fact]
        public void Manipulate_HazardousCommoditiesPackingGroupIsII_WhenOptionIsHazardousMaterials()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;
            shipment.FedEx.Packages[0].HazardousMaterialPackingGroup = (int) FedExHazardousMaterialsPackingGroup.II;
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.HazardousMaterials;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(HazardousCommodityPackingGroupType.II, dangerousGoods.Containers[0].HazardousCommodities[0].Description.PackingGroup);
        }

        [Fact]
        public void Manipulate_HazardousCommoditiesPackingGroupIsIII_WhenOptionIsHazardousMaterials()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;
            shipment.FedEx.Packages[0].HazardousMaterialPackingGroup = (int) FedExHazardousMaterialsPackingGroup.III;
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.HazardousMaterials;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(HazardousCommodityPackingGroupType.III, dangerousGoods.Containers[0].HazardousCommodities[0].Description.PackingGroup);
        }

        [Fact]
        public void Manipulate_HazardousCommoditiesPackingGroupIsNotSpecified_WhenOptionIsHazardousMaterials_AndPackingGroupIsNotApplicable()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;
            shipment.FedEx.Packages[0].HazardousMaterialPackingGroup = (int) FedExHazardousMaterialsPackingGroup.NotApplicable;
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.HazardousMaterials;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.False(dangerousGoods.Containers[0].HazardousCommodities[0].Description.PackingGroupSpecified);
        }

        [Fact]
        public void Manipulate_ReturnsFailureWithInvalidOperationException_WhenHazardousCommoditiesPackingGroupIsNotRecognized_AndOptionIsHazardousMaterials()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;
            shipment.FedEx.Packages[0].HazardousMaterialPackingGroup = 45;
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.HazardousMaterials;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.True(result.Failure);
            Assert.IsAssignableFrom<InvalidOperationException>(result.Exception);
        }

        [Fact]
        public void Manipulate_HazardousCommoditiesPackingGroupSpecifiedIsTrue_WhenOptionIsHazardousMaterials()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;
            shipment.FedEx.Packages[0].HazardousMaterialPackingGroup = (int) FedExHazardousMaterialsPackingGroup.I;
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.HazardousMaterials;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.True(dangerousGoods.Containers[0].HazardousCommodities[0].Description.PackingGroupSpecified);
        }

        [Fact]
        public void Manipulate_QuantityAmount_WhenOptionIsHazardousMaterials()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.HazardousMaterials;
            shipment.FedEx.Packages[0].HazardousMaterialQuantityValue = 2.4;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(2.4M, dangerousGoods.Containers[0].HazardousCommodities[0].Quantity.Amount);
        }

        [Fact]
        public void Manipulate_QuantityAmountSpecifiedIsTrue_WhenOptionIsHazardousMaterials()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.HazardousMaterials;
            shipment.FedEx.Packages[0].HazardousMaterialQuantityValue = 2.4;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.True(dangerousGoods.Containers[0].HazardousCommodities[0].Quantity.AmountSpecified);
        }

        [Fact]
        public void Manipulate_QuantityUnits_WhenOptionIsHazardousMaterials()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.HazardousMaterials;
            shipment.FedEx.Packages[0].HazardousMaterialQuantityValue = 2.4;
            shipment.FedEx.Packages[0].HazardousMaterialQuanityUnits = (int) FedExHazardousMaterialsQuantityUnits.Kilogram;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal("KG", dangerousGoods.Containers[0].HazardousCommodities[0].Quantity.Units);
        }

        [Fact]
        public void Manipulate_PackagingCount_WhenOptionIsHazardousMaterials()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.HazardousMaterials;
            shipment.FedEx.Packages[0].DangerousGoodsPackagingCount = 1;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal("1", dangerousGoods.Packaging.Count);
        }

        [Fact]
        public void Manipulate_PackagingUnits_WhenOptionIsHazardousMaterials()
        {
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.HazardousMaterials;
            shipment.FedEx.Packages[0].HazardousMaterialQuanityUnits = (int) FedExHazardousMaterialsQuantityUnits.Liters;
            shipment.FedEx.Packages[0].DangerousGoodsPackagingCount = 1;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(EnumHelper.GetDescription(FedExHazardousMaterialsQuantityUnits.Liters), dangerousGoods.Packaging.Units);
        }

        [Fact]
        public void Manipulate_PackagingIsNull_WhenOptionIsNotHazardousMaterials()
        {
            shipment.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.Batteries;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Null(dangerousGoods.Packaging);
        }

        [Fact]
        public void Manipulate_UsesSequenceNumberOnRequest_WhenDangerousGoodsEnabled()
        {
            // Setup the FedEx shipment to contain multiple packages to test that the 
            // manipulator process the correct package in the shipment when the 
            // sequence number is not zero
            shipment.FedEx.Packages.Clear();
            shipment.FedEx.Packages.Add(new FedExPackageEntity { DangerousGoodsEnabled = false });
            shipment.FedEx.Packages.Add(new FedExPackageEntity { DangerousGoodsEnabled = false });
            shipment.FedEx.Packages.Add(new FedExPackageEntity { DangerousGoodsEnabled = true });
            shipment.FedEx.Packages.Add(new FedExPackageEntity { DangerousGoodsEnabled = false });
            shipment.FedEx.Packages.Add(new FedExPackageEntity { DangerousGoodsEnabled = false });

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 2);

            // Since the sequence number is two, we should have a non-null value for the dangerous goods
            // property since the dangerous goods enabled flag is set to true for the third item in the package list
            DangerousGoodsDetail dangerousGoods = result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.NotNull(dangerousGoods);
        }

        private static ShipmentEntity GetShipment(Action<FedExPackageEntity> setBatteryValue)
        {
            return Create.Shipment()
                .AsFedEx(f => f.WithPackage(p => p
                        .Set(x => x.DangerousGoodsEnabled = true)
                        .Set(x => x.DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.Batteries)
                        .Set(setBatteryValue))
                    .Set(x => x.RequestedLabelFormat = (int) ThermalLanguage.None))
                .Build();
        }
    }
}
