using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.FedEx;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx
{
    public class FedExShippingProfileApplicationStrategyTest
    {
        private readonly AutoMock mock;
        private readonly Mock<ICarrierAccountRetriever<FedExAccountEntity, IFedExAccountEntity>> accountRetriever;

        public FedExShippingProfileApplicationStrategyTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            accountRetriever = mock.Mock<ICarrierAccountRetriever<FedExAccountEntity, IFedExAccountEntity>>();
        }

        [Fact]
        public void ApplyProfile_SetsProfilesRMA()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();

            profile.FedEx.ReturnType = 3;
            profile.FedEx.RmaNumber = "123ddd";
            profile.FedEx.RmaReason = "asdfasdfasdf";
            profile.FedEx.ReturnSaturdayPickup = true;
            profile.FedEx.ThirdPartyConsignee = true;

            mock.Create<FedExShippingProfileApplicationStrategy>().ApplyProfile(profile, shipment);

            Assert.Equal(3, shipment.FedEx.ReturnType);
            Assert.Equal("123ddd", shipment.FedEx.RmaNumber);
            Assert.Equal("asdfasdfasdf", shipment.FedEx.RmaReason);
            Assert.True(shipment.FedEx.ReturnSaturdayPickup);
            Assert.True(shipment.FedEx.ThirdPartyConsignee);
        }

        [Fact]
        public void ApplyProfile_SetsAccountIDToFirstAccountsID_WhenAccountsExistAndProfilesUpsAccountIDIsZero()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.FedEx.FedExAccountID = 0;
            accountRetriever.SetupGet(a => a.AccountsReadOnly).Returns(new[] { new FedExAccountEntity() { FedExAccountID = 12333 } });
            
            mock.Create<FedExShippingProfileApplicationStrategy>().ApplyProfile(profile, shipment);

            Assert.Equal(12333, shipment.FedEx.FedExAccountID);
        }

        [Fact]
        public void ApplyProfile_SetsAccountIDToProfilesAccountsID_WhenProfilesUpsAccountIDIsNotZero()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.FedEx.FedExAccountID = 333333;
            accountRetriever.SetupGet(a => a.AccountsReadOnly).Returns(new[] { new FedExAccountEntity() { FedExAccountID = 12333 } });

            mock.Create<FedExShippingProfileApplicationStrategy>().ApplyProfile(profile, shipment);

            Assert.Equal(333333, shipment.FedEx.FedExAccountID);
        }

        [Fact]
        public void ApplyProfile_SetsProfilesReference()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.FedEx.Signature = 3;
            profile.FedEx.ReferenceFIMS = "FIMS REFERENCE";
            profile.FedEx.ReferenceCustomer = "CUSTOMER REFERENCE";
            profile.FedEx.ReferenceInvoice = "INVOICE REFERENCE";
            profile.FedEx.ReferencePO = "PO REFERENCE";
            profile.FedEx.ReferenceShipmentIntegrity = "what?";

            mock.Create<FedExShippingProfileApplicationStrategy>().ApplyProfile(profile, shipment);

            Assert.Equal(3, shipment.FedEx.Signature);
            Assert.Equal("FIMS REFERENCE", shipment.FedEx.ReferenceFIMS);
            Assert.Equal("CUSTOMER REFERENCE", shipment.FedEx.ReferenceCustomer);
            Assert.Equal("INVOICE REFERENCE", shipment.FedEx.ReferenceInvoice);
            Assert.Equal("PO REFERENCE", shipment.FedEx.ReferencePO);
            Assert.Equal("what?", shipment.FedEx.ReferenceShipmentIntegrity);
        }

        [Fact]
        public void ApplyProfile_SetsProfilesShipmentDetails()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.FedEx.Service = 3;
            profile.FedEx.PackagingType = 2;
            profile.FedEx.DropoffType = 4;
            profile.FedEx.ReturnsClearance = true;
            profile.FedEx.NonStandardContainer = true;
            profile.FedEx.OriginResidentialDetermination = 5;

            mock.Create<FedExShippingProfileApplicationStrategy>().ApplyProfile(profile, shipment);

            Assert.Equal(3, shipment.FedEx.Service);
            Assert.Equal(2, shipment.FedEx.PackagingType);
            Assert.Equal(4, shipment.FedEx.DropoffType);
            Assert.True(shipment.FedEx.ReturnsClearance);
            Assert.True(shipment.FedEx.NonStandardContainer);
            Assert.Equal(5, shipment.FedEx.OriginResidentialDetermination);
        }

        [Fact]
        public void ApplyProfile_SetsProfilesResidentialDetermination()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.FedEx.ResidentialDetermination = 2;
            mock.Create<FedExShippingProfileApplicationStrategy>().ApplyProfile(profile, shipment);

            Assert.Equal(2, shipment.ResidentialDetermination);
        }

        [Fact]
        public void ApplyProfile_SetsProfilesReturnShipment()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.ReturnShipment = true;
            mock.Create<FedExShippingProfileApplicationStrategy>().ApplyProfile(profile, shipment);

            Assert.True(shipment.ReturnShipment);
        }

        [Fact]
        public void ApplyProfile_RemovesExtraPackagesFromShipment()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            shipment.FedEx.Packages.Add(new FedExPackageEntity());
            shipment.FedEx.Packages.Add(new FedExPackageEntity());

            mock.Create<FedExShippingProfileApplicationStrategy>().ApplyProfile(profile, shipment);

            Assert.Single(shipment.FedEx.Packages);
        }

        [Fact]
        public void ApplyProfile_AppliesProfilesPackagesDims()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.Packages.Clear();
            profile.Packages.Add(new FedExProfilePackageEntity()
            {
                DimsProfileID = 0,
                DimsLength = 1,
                DimsWidth = 2,
                DimsHeight = 3
            });
            
            mock.Create<FedExShippingProfileApplicationStrategy>().ApplyProfile(profile, shipment);
            
            Assert.Equal(1, shipment.FedEx.Packages[0].DimsLength);
            Assert.Equal(2, shipment.FedEx.Packages[0].DimsWidth);
            Assert.Equal(3, shipment.FedEx.Packages[0].DimsHeight);
        }

        [Fact]
        public void ApplyProfile_AppliesProfilesMultiPackagesDims()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.Packages.Clear();

            var packageOne = new FedExProfilePackageEntity()
            {
                DimsProfileID = 0,
                DimsLength = 1,
                DimsWidth = 2,
                DimsHeight = 3
            };

            var packageTwo = new FedExProfilePackageEntity()
            {
                DimsProfileID = 0,
                DimsLength = 21,
                DimsWidth = 22,
                DimsHeight = 23
            };

            profile.Packages.Add(packageOne);
            profile.Packages.Add(packageTwo);

            mock.Create<FedExShippingProfileApplicationStrategy>().ApplyProfile(profile, shipment);

            Assert.Equal(1, shipment.FedEx.Packages[0].DimsLength);
            Assert.Equal(2, shipment.FedEx.Packages[0].DimsWidth);
            Assert.Equal(3, shipment.FedEx.Packages[0].DimsHeight);

            Assert.Equal(21, shipment.FedEx.Packages[1].DimsLength);
            Assert.Equal(22, shipment.FedEx.Packages[1].DimsWidth);
            Assert.Equal(23, shipment.FedEx.Packages[1].DimsHeight);
        }

        [Fact]
        public void ApplyProfile_AppliesProfilesPackagesDryIce()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.Packages.Clear();
            profile.Packages.Add(new FedExProfilePackageEntity()
            {
                DryIceWeight = 2
            });

            mock.Create<FedExShippingProfileApplicationStrategy>().ApplyProfile(profile, shipment);

            Assert.Equal(2, shipment.FedEx.Packages[0].DryIceWeight);
        }

        [Fact]
        public void ApplyProfile_AppliesProfilePackagesDangerousGoods()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.Packages.Clear();
            profile.Packages.Add(new FedExProfilePackageEntity()
            {
                DangerousGoodsEnabled = true,
                DangerousGoodsType = 32,
                DangerousGoodsAccessibilityType = 21,
                DangerousGoodsCargoAircraftOnly = true,
                DangerousGoodsEmergencyContactPhone = "123-456-7890",
                DangerousGoodsOfferor = "your mom",
                DangerousGoodsPackagingCount = 4,
                ContainerType = "shoe box",
                NumberOfContainers = 2
            });

            mock.Create<FedExShippingProfileApplicationStrategy>().ApplyProfile(profile, shipment);

            Assert.True(shipment.FedEx.Packages[0].DangerousGoodsEnabled);
            Assert.Equal(32, shipment.FedEx.Packages[0].DangerousGoodsType);
            Assert.Equal(21, shipment.FedEx.Packages[0].DangerousGoodsAccessibilityType);
            Assert.True(shipment.FedEx.Packages[0].DangerousGoodsCargoAircraftOnly);
            Assert.Equal("123-456-7890", shipment.FedEx.Packages[0].DangerousGoodsEmergencyContactPhone);
            Assert.Equal("your mom", shipment.FedEx.Packages[0].DangerousGoodsOfferor);
            Assert.Equal(4, shipment.FedEx.Packages[0].DangerousGoodsPackagingCount);
            Assert.Equal("shoe box", shipment.FedEx.Packages[0].ContainerType);
            Assert.Equal(2, shipment.FedEx.Packages[0].NumberOfContainers);
        }

        [Fact]
        public void ApplyProfile_AppliesProfilePackagesHazardousMaterial()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.Packages.Clear();
            profile.Packages.Add(new FedExProfilePackageEntity()
            {
                HazardousMaterialNumber = "123",
                HazardousMaterialClass = "Stuff",
                HazardousMaterialProperName = "Really Cool Stuff",
                HazardousMaterialPackingGroup = 3,
                HazardousMaterialQuantityValue = 12,
                HazardousMaterialQuanityUnits = 5,
                PackingDetailsCargoAircraftOnly = true,
                PackingDetailsPackingInstructions = "shoe box"
            });

            mock.Create<FedExShippingProfileApplicationStrategy>().ApplyProfile(profile, shipment);

            Assert.Equal("123", shipment.FedEx.Packages[0].HazardousMaterialNumber);
            Assert.Equal("Stuff", shipment.FedEx.Packages[0].HazardousMaterialClass);
            Assert.Equal("Really Cool Stuff", shipment.FedEx.Packages[0].HazardousMaterialProperName);
            Assert.Equal(3, shipment.FedEx.Packages[0].HazardousMaterialPackingGroup);
            Assert.Equal(12, shipment.FedEx.Packages[0].HazardousMaterialQuantityValue);
            Assert.Equal(5, shipment.FedEx.Packages[0].HazardousMaterialQuanityUnits);
            Assert.True(shipment.FedEx.Packages[0].PackingDetailsCargoAircraftOnly);
            Assert.Equal("shoe box", shipment.FedEx.Packages[0].PackingDetailsPackingInstructions);
        }

        [Fact]
        public void ApplyProfile_AppliesProfilePackagesBattery()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.Packages.Clear();
            profile.Packages.Add(new FedExProfilePackageEntity()
            {
                BatteryMaterial = FedExBatteryMaterialType.LithiumMetal,
                BatteryPacking = FedExBatteryPackingType.ContainsInEquipement,
                BatteryRegulatorySubtype = FedExBatteryRegulatorySubType.IATASectionII
            });

            mock.Create<FedExShippingProfileApplicationStrategy>().ApplyProfile(profile, shipment);

            Assert.Equal(FedExBatteryMaterialType.LithiumMetal, shipment.FedEx.Packages[0].BatteryMaterial);
            Assert.Equal(FedExBatteryPackingType.ContainsInEquipement, shipment.FedEx.Packages[0].BatteryPacking);
            Assert.Equal(FedExBatteryRegulatorySubType.IATASectionII, shipment.FedEx.Packages[0].BatteryRegulatorySubtype);
        }

        [Fact]
        public void ApplyProfile_AppliesProfilePackagesSignatory()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.Packages.Clear();
            profile.Packages.Add(new FedExProfilePackageEntity()
            {
                SignatoryContactName = "Joe",
                SignatoryTitle = "Boss",
                SignatoryPlace = "Home"
            });

            mock.Create<FedExShippingProfileApplicationStrategy>().ApplyProfile(profile, shipment);

            Assert.Equal("Joe", shipment.FedEx.Packages[0].SignatoryContactName);
            Assert.Equal("Boss", shipment.FedEx.Packages[0].SignatoryTitle);
            Assert.Equal("Home", shipment.FedEx.Packages[0].SignatoryPlace);
        }

        [Fact]
        public void ApplyProfile_DelegatesToShipmentTypeManagerWithShipment()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            mock.Create<FedExShippingProfileApplicationStrategy>().ApplyProfile(profile, shipment);

            mock.Mock<IShipmentTypeManager>().Verify(s => s.Get(shipment));
        }

        [Fact]
        public void ApplyProfile_SetsOriginIDOnShipment()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.OriginID = 123;

            mock.Create<FedExShippingProfileApplicationStrategy>().ApplyProfile(profile, shipment);

            Assert.Equal(123, shipment.OriginOriginID);
        }

        [Fact]
        public void ApplyProfile_SetsReturnShipmentOnShipment()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.ReturnShipment = true;

            mock.Create<FedExShippingProfileApplicationStrategy>().ApplyProfile(profile, shipment);

            Assert.True(shipment.ReturnShipment);
        }

        [Fact]
        public void ApplyProfile_SetsRequestedLabelFormatOnShipment()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.RequestedLabelFormat = (int) ThermalLanguage.EPL;

            mock.Create<FedExShippingProfileApplicationStrategy>().ApplyProfile(profile, shipment);

            Assert.Equal(ThermalLanguage.EPL, (ThermalLanguage) shipment.OriginOriginID);
        }

        [Fact]
        public void ApplyProfile_DelegatesToShipmentTypeToSaveLabelFormat()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.RequestedLabelFormat = (int) ThermalLanguage.EPL;

            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            var shipmentType = mock.Mock<ShipmentType>();
            shipmentTypeManager.Setup(s => s.Get(shipment)).Returns(shipmentType);

            mock.Create<FedExShippingProfileApplicationStrategy>().ApplyProfile(profile, shipment);

            shipmentType.Verify(s => s.SaveRequestedLabelFormat((ThermalLanguage) shipment.RequestedLabelFormat, shipment));
        }

        [Fact]
        public void ApplyProfile_SetsInsuranceValueOnPackage()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.Insurance = true;

            var insuranceChoice = mock.Mock<IInsuranceChoice>();

            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            var shipmentType = mock.Mock<ShipmentType>();
            shipmentType.Setup(s => s.GetParcelCount(shipment)).Returns(1);
            var shipmentParcel = new ShipmentParcel(shipment, null, insuranceChoice.Object, new Editing.DimensionsAdapter());

            shipmentType.Setup(s => s.GetParcelDetail(shipment, 0)).Returns(shipmentParcel);

            shipmentTypeManager.Setup(s => s.Get(shipment)).Returns(shipmentType);

            var testObject = mock.Create<FedExShippingProfileApplicationStrategy>();

            testObject.ApplyProfile(profile, shipment);

            insuranceChoice.VerifySet(i => i.Insured = true);
        }

        private (ShippingProfileEntity, ShipmentEntity) GetEmptyShipmentAndProfile()
        {
            var profile = new ShippingProfileEntity()
            {
                FedEx = new FedExProfileEntity()
            };

            profile.Packages.Add(new FedExProfilePackageEntity());

            return (profile, new ShipmentEntity()
            {
                FedEx = new FedExShipmentEntity()
            });
        }
    }
}
