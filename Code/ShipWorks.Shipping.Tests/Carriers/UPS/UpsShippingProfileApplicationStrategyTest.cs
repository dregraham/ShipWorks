using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Ups;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS
{
    public class UpsShippingProfileApplicationStrategyTest
    {
        private readonly AutoMock mock;
        private readonly Mock<ICarrierAccountRetriever<UpsAccountEntity, IUpsAccountEntity>> accountRetriever;
        private readonly Mock<ShipmentType> shipmentType;
        private readonly Mock<IShipmentTypeManager> shipmentTypeManager;
        private readonly Mock<IShippingProfileApplicationStrategy> baseShippingProfileApplicationStrategy;
        private readonly UpsShippingProfileApplicationStrategy testObject;

        public UpsShippingProfileApplicationStrategyTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            accountRetriever = mock.Mock<ICarrierAccountRetriever<UpsAccountEntity, IUpsAccountEntity>>();
            shipmentType = mock.Mock<ShipmentType>();
            shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            shipmentTypeManager.Setup(s => s.Get(It.IsAny<ShipmentEntity>())).Returns(shipmentType);
            baseShippingProfileApplicationStrategy = mock.Mock<IShippingProfileApplicationStrategy>();

            testObject = mock.Create<UpsShippingProfileApplicationStrategy>();
        }

        [Fact]
        public void ApplyProfile_DelegatesToShipmentTypeManagerForShipmentType()
        {
            var shipment = new ShipmentEntity() { Ups = new UpsShipmentEntity() };
            var profile = new ShippingProfileEntity() { Ups = new UpsProfileEntity() };

            testObject.ApplyProfile(profile, shipment);

            shipmentTypeManager.Verify(s => s.Get(shipment));
        }

        [Fact]
        public void ApplyProfile_AppliesProfilesPackagesDims()
        {
            var shipment = new ShipmentEntity() { Ups = new UpsShipmentEntity() };
            var profile = new ShippingProfileEntity() { Ups = new UpsProfileEntity() };

            var package = new UpsProfilePackageEntity()
            {
                DimsProfileID = 0,
                DimsLength = 1,
                DimsWidth = 2,
                DimsHeight = 3
            };

            profile.Packages.Add(package);

            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(1, shipment.Ups.Packages[0].DimsLength);
            Assert.Equal(2, shipment.Ups.Packages[0].DimsWidth);
            Assert.Equal(3, shipment.Ups.Packages[0].DimsHeight);
        }

        [Fact]
        public void ApplyProfile_AppliesProfilesMultiPackagesDims()
        {
            var shipment = new ShipmentEntity() { Ups = new UpsShipmentEntity() };
            var profile = new ShippingProfileEntity() { Ups = new UpsProfileEntity() };

            var packageOne = new UpsProfilePackageEntity()
            {
                DimsProfileID = 0,
                DimsLength = 1,
                DimsWidth = 2,
                DimsHeight = 3
            };

            var packageTwo = new UpsProfilePackageEntity()
            {
                DimsProfileID = 0,
                DimsLength = 21,
                DimsWidth = 22,
                DimsHeight = 23
            };

            profile.Packages.Add(packageOne);
            profile.Packages.Add(packageTwo);

            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(1, shipment.Ups.Packages[0].DimsLength);
            Assert.Equal(2, shipment.Ups.Packages[0].DimsWidth);
            Assert.Equal(3, shipment.Ups.Packages[0].DimsHeight);

            Assert.Equal(21, shipment.Ups.Packages[1].DimsLength);
            Assert.Equal(22, shipment.Ups.Packages[1].DimsWidth);
            Assert.Equal(23, shipment.Ups.Packages[1].DimsHeight);
        }

        [Fact]
        public void ApplyProfile_AppliesProfilesPackagesDryIce()
        {
            var shipment = new ShipmentEntity() { Ups = new UpsShipmentEntity() };
            var profile = new ShippingProfileEntity() { Ups = new UpsProfileEntity() };

            var package = new UpsProfilePackageEntity()
            {
                DryIceEnabled = true,
                DryIceIsForMedicalUse = true,
                DryIceRegulationSet = 1,
                DryIceWeight = 2
            };

            profile.Packages.Add(package);

            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(true, shipment.Ups.Packages[0].DryIceEnabled);
            Assert.Equal(true, shipment.Ups.Packages[0].DryIceIsForMedicalUse);
            Assert.Equal(1, shipment.Ups.Packages[0].DryIceRegulationSet);
            Assert.Equal(2, shipment.Ups.Packages[0].DryIceWeight);
        }

        [Fact]
        public void ApplyProfile_AppliesProfilesPackagesVerbalConfirmation()
        {
            var shipment = new ShipmentEntity() { Ups = new UpsShipmentEntity() };
            var profile = new ShippingProfileEntity() { Ups = new UpsProfileEntity() };

            var package = new UpsProfilePackageEntity()
            {
                VerbalConfirmationEnabled = true,
                VerbalConfirmationName = "Satan",
                VerbalConfirmationPhone = "666-666-6666",
                VerbalConfirmationPhoneExtension = "666"
            };

            profile.Packages.Add(package);

            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(true, shipment.Ups.Packages[0].VerbalConfirmationEnabled);
            Assert.Equal("Satan", shipment.Ups.Packages[0].VerbalConfirmationName);
            Assert.Equal("666-666-6666", shipment.Ups.Packages[0].VerbalConfirmationPhone);
            Assert.Equal("666", shipment.Ups.Packages[0].VerbalConfirmationPhoneExtension);
        }

        [Fact]
        public void ApplyProfile_RemovesExtraPackagesFromShipment()
        {
            var shipment = new ShipmentEntity() { Ups = new UpsShipmentEntity() };
            shipment.Ups.Packages.Add(new UpsPackageEntity());
            shipment.Ups.Packages.Add(new UpsPackageEntity());

            var profile = new ShippingProfileEntity() { Ups = new UpsProfileEntity() };
            var package = new UpsProfilePackageEntity();
            profile.Packages.Add(package);

            testObject.ApplyProfile(profile, shipment);

            Assert.Single(shipment.Ups.Packages);
        }
        
        [Fact]
        public void ApplyProfile_SetsResidentialDetermination()
        {
            var shipment = new ShipmentEntity() { Ups = new UpsShipmentEntity() };
            shipment.Ups.Packages.Add(new UpsPackageEntity());
            shipment.Ups.Packages.Add(new UpsPackageEntity());

            var profile = new ShippingProfileEntity() { Ups = new UpsProfileEntity() { ResidentialDetermination = 2 } };
            var package = new UpsProfilePackageEntity();
            profile.Packages.Add(package);

            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(2, shipment.ResidentialDetermination);
        }

        [Fact]
        public void ApplyProfile_DelegatesToShipmentTypeToUpdateDynamicShipmentData()
        {
            var shipment = new ShipmentEntity() { Ups = new UpsShipmentEntity() };
            shipment.Ups.Packages.Add(new UpsPackageEntity());
            shipment.Ups.Packages.Add(new UpsPackageEntity());

            var profile = new ShippingProfileEntity() { Ups = new UpsProfileEntity() { ResidentialDetermination = 2 } };
            var package = new UpsProfilePackageEntity();
            profile.Packages.Add(package);

            testObject.ApplyProfile(profile, shipment);

            shipmentType.Verify(s => s.UpdateDynamicShipmentData(shipment));
        }

        [Fact]
        public void ApplyProfile_SetsAccountIDToFirstAccountsID_WhenAccountsExistAndProfilesUpsAccountIDIsZero()
        {
            var shipment = new ShipmentEntity() { Ups = new UpsShipmentEntity() };
            shipment.Ups.Packages.Add(new UpsPackageEntity());

            accountRetriever.SetupGet(a => a.AccountsReadOnly).Returns(new[] { new UpsAccountEntity() {UpsAccountID = 12333 } });

            var profile = new ShippingProfileEntity() { Ups = new UpsProfileEntity() { UpsAccountID = 0} };
            var package = new UpsProfilePackageEntity();
            profile.Packages.Add(package);

            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(12333, shipment.Ups.UpsAccountID);
        }

        [Fact]
        public void ApplyProfile_SetsAccountIDToProfilesAccountsID_WhenProfilesUpsAccountIDIsNotZero()
        {
            var shipment = new ShipmentEntity() { Ups = new UpsShipmentEntity() };
            shipment.Ups.Packages.Add(new UpsPackageEntity());

            accountRetriever.SetupGet(a => a.AccountsReadOnly).Returns(new[] { new UpsAccountEntity() { UpsAccountID = 12333 } });

            var profile = new ShippingProfileEntity() { Ups = new UpsProfileEntity() { UpsAccountID = 333333 } };
            var package = new UpsProfilePackageEntity();
            profile.Packages.Add(package);

            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(333333, shipment.Ups.UpsAccountID);
        }

        [Fact]
        public void ApplyProfile_SetsShipmentChargeProperties()
        {
            var shipment = new ShipmentEntity() { Ups = new UpsShipmentEntity() };
            shipment.Ups.Packages.Add(new UpsPackageEntity());

            var profile = new ShippingProfileEntity()
            {
                Ups = new UpsProfileEntity()
                {
                    ShipmentChargeType = 2,
                    ShipmentChargePostalCode = "12345",
                    ShipmentChargeCountryCode = "CA",
                    ShipmentChargeAccount = "123zzz"
                }
            };

            var package = new UpsProfilePackageEntity();
            profile.Packages.Add(package);

            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(2, shipment.Ups.ShipmentChargeType);
            Assert.Equal("12345", shipment.Ups.ShipmentChargePostalCode);
            Assert.Equal("CA", shipment.Ups.ShipmentChargeCountryCode);
            Assert.Equal("123zzz", shipment.Ups.ShipmentChargeAccount);
        }

        [Fact]
        public void ApplyProfile_SetsShipmentMailInnovationsProperties()
        {
            var shipment = new ShipmentEntity() { Ups = new UpsShipmentEntity() };
            shipment.Ups.Packages.Add(new UpsPackageEntity());

            var profile = new ShippingProfileEntity()
            {
                Ups = new UpsProfileEntity()
                {
                    UspsPackageID = "14",
                    CostCenter = "13",
                    IrregularIndicator = 5,
                    Cn22Number = "12"
                }
            };

            var package = new UpsProfilePackageEntity();
            profile.Packages.Add(package);

            testObject.ApplyProfile(profile, shipment);

            Assert.Equal("14", shipment.Ups.UspsPackageID);
            Assert.Equal("13", shipment.Ups.CostCenter);
            Assert.Equal(5, shipment.Ups.IrregularIndicator);
            Assert.Equal("12", shipment.Ups.Cn22Number);
        }

        [Fact]
        public void ApplyProfile_SetsShipmentInternationalProperties()
        {
            var shipment = new ShipmentEntity() { Ups = new UpsShipmentEntity() };
            shipment.Ups.Packages.Add(new UpsPackageEntity());

            var profile = new ShippingProfileEntity()
            {
                Ups = new UpsProfileEntity()
                {
                    Subclassification = 12,
                    Endorsement = 1,
                    PaperlessAdditionalDocumentation = true,
                    CommercialPaperlessInvoice = true,
                    ShipperRelease = true,
                    CarbonNeutral = true
                }
            };

            var package = new UpsProfilePackageEntity();
            profile.Packages.Add(package);

            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(12, shipment.Ups.Subclassification);
            Assert.Equal(1, shipment.Ups.Endorsement);
            Assert.True(shipment.Ups.PaperlessAdditionalDocumentation);
            Assert.True(shipment.Ups.CommercialPaperlessInvoice);
            Assert.True(shipment.Ups.ShipperRelease);
            Assert.True(shipment.Ups.CarbonNeutral);
        }

        [Fact]
        public void ApplyProfile_SetsShipmentReturnProperties()
        {
            var shipment = new ShipmentEntity() { Ups = new UpsShipmentEntity() };
            shipment.Ups.Packages.Add(new UpsPackageEntity());

            var profile = new ShippingProfileEntity()
            {
                Ups = new UpsProfileEntity()
                {
                    ReturnService = 2,
                    ReturnContents = "banana hammocks",
                    ReturnUndeliverableEmail = "John@foo.com"
                }
            };

            var package = new UpsProfilePackageEntity();
            profile.Packages.Add(package);

            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(2, shipment.Ups.ReturnService);
            Assert.Equal("banana hammocks", shipment.Ups.ReturnContents);
            Assert.Equal("John@foo.com", shipment.Ups.ReturnUndeliverableEmail);
        }

        [Fact]
        public void ApplyProfile_SetsShipmentEmailNotifyProperties()
        {
            var shipment = new ShipmentEntity() { Ups = new UpsShipmentEntity() };
            shipment.Ups.Packages.Add(new UpsPackageEntity());

            var profile = new ShippingProfileEntity()
            {
                Ups = new UpsProfileEntity()
                {
                    EmailNotifySender = 2,
                    EmailNotifyRecipient = 3,
                    EmailNotifyOther = 1,
                    EmailNotifyOtherAddress = "support@shipworks.com", 
                    EmailNotifyFrom = "m.mulaosmanovic@shipworks.com",
                    EmailNotifySubject = 2,
                    EmailNotifyMessage = "Blah Blah Blah"
                }
            };

            var package = new UpsProfilePackageEntity();
            profile.Packages.Add(package);

            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(2, shipment.Ups.EmailNotifySender);
            Assert.Equal(3, shipment.Ups.EmailNotifyRecipient);
            Assert.Equal(1, shipment.Ups.EmailNotifyOther);
            Assert.Equal("support@shipworks.com", shipment.Ups.EmailNotifyOtherAddress);
            Assert.Equal("m.mulaosmanovic@shipworks.com", shipment.Ups.EmailNotifyFrom);
            Assert.Equal(2, shipment.Ups.EmailNotifySubject);
            Assert.Equal("Blah Blah Blah", shipment.Ups.EmailNotifyMessage);
        }

        [Fact]
        public void ApplyProfile_SetsShipmentPayorProperties()
        {
            var shipment = new ShipmentEntity() { Ups = new UpsShipmentEntity() };
            shipment.Ups.Packages.Add(new UpsPackageEntity());

            var profile = new ShippingProfileEntity()
            {
                Ups = new UpsProfileEntity()
                {
                    PayorType = 2,
                    PayorAccount = "zzz123",
                    PayorPostalCode = "63040",
                    PayorCountryCode = "CA"
                }
            };

            var package = new UpsProfilePackageEntity();
            profile.Packages.Add(package);

            testObject.ApplyProfile(profile, shipment);
            
            Assert.Equal(2, shipment.Ups.PayorType);
            Assert.Equal("zzz123", shipment.Ups.PayorAccount);
            Assert.Equal("63040", shipment.Ups.PayorPostalCode);
            Assert.Equal("CA", shipment.Ups.PayorCountryCode);
        }

        [Fact]
        public void ApplyProfile_SetsShipmentServiceProperties()
        {
            var shipment = new ShipmentEntity() { Ups = new UpsShipmentEntity() };
            shipment.Ups.Packages.Add(new UpsPackageEntity());

            var profile = new ShippingProfileEntity()
            {
                Ups = new UpsProfileEntity()
                {
                    DeliveryConfirmation = 2,
                    ReferenceNumber = "abcdefg",
                    ReferenceNumber2 = "1234567",
                    Service = 3,
                    SaturdayDelivery = true
                }
            };

            var package = new UpsProfilePackageEntity();
            profile.Packages.Add(package);

            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(2, shipment.Ups.DeliveryConfirmation);
            Assert.Equal("abcdefg", shipment.Ups.ReferenceNumber);
            Assert.Equal("1234567", shipment.Ups.ReferenceNumber2);
            Assert.Equal(3, shipment.Ups.Service);
            Assert.Equal(true, shipment.Ups.SaturdayDelivery);
        }

        [Fact]
        public void ApplyProfile_DelegatesToShipmentTypeManagerWithShipment()
        {
            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();

            var testObject = mock.Create<BaseShippingProfileApplicationStrategy>();
            var shipment = new ShipmentEntity();
            var profile = new ShippingProfileEntity();

            testObject.ApplyProfile(profile, shipment);

            shipmentTypeManager.Verify(s => s.Get(shipment));
        }

        [Fact]
        public void ApplyProfile_SetsOriginIDOnShipment()
        {
            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();

            var testObject = mock.Create<BaseShippingProfileApplicationStrategy>();
            var shipment = new ShipmentEntity();
            var profile = new ShippingProfileEntity() { OriginID = 123 };

            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(123, shipment.OriginOriginID);
        }

        [Fact]
        public void ApplyProfile_SetsReturnShipmentOnShipment()
        {
            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();

            var testObject = mock.Create<BaseShippingProfileApplicationStrategy>();
            var shipment = new ShipmentEntity();
            var profile = new ShippingProfileEntity() { ReturnShipment = true };

            testObject.ApplyProfile(profile, shipment);

            Assert.True(shipment.ReturnShipment);
        }

        [Fact]
        public void ApplyProfile_SetsRequestedLabelFormatOnShipment()
        {
            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();

            var testObject = mock.Create<BaseShippingProfileApplicationStrategy>();
            var shipment = new ShipmentEntity();
            var profile = new ShippingProfileEntity() { RequestedLabelFormat = (int) ThermalLanguage.EPL };

            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(ThermalLanguage.EPL, (ThermalLanguage) shipment.OriginOriginID);
        }

        [Fact]
        public void ApplyProfile_DelegatesToShipmentTypeToSaveLabelFormat()
        {
            var shipment = new ShipmentEntity();
            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            var shipmentType = mock.Mock<ShipmentType>();
            shipmentTypeManager.Setup(s => s.Get(shipment)).Returns(shipmentType);

            var testObject = mock.Create<BaseShippingProfileApplicationStrategy>();

            var profile = new ShippingProfileEntity() { RequestedLabelFormat = (int) ThermalLanguage.EPL };

            testObject.ApplyProfile(profile, shipment);

            shipmentType.Verify(s => s.SaveRequestedLabelFormat((ThermalLanguage) shipment.RequestedLabelFormat, shipment));
        }

        [Fact]
        public void ApplyProfile_SetsInsuranceValueOnPackage()
        {
            var insuranceChoice = mock.Mock<IInsuranceChoice>();

            var shipment = new ShipmentEntity();
            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            var shipmentType = mock.Mock<ShipmentType>();
            shipmentType.Setup(s => s.GetParcelCount(shipment)).Returns(1);
            var shipmentParcel = new ShipmentParcel(shipment, null, insuranceChoice.Object, new Editing.DimensionsAdapter());

            shipmentType.Setup(s => s.GetParcelDetail(shipment, 0)).Returns(shipmentParcel);

            shipmentTypeManager.Setup(s => s.Get(shipment)).Returns(shipmentType);

            var testObject = mock.Create<BaseShippingProfileApplicationStrategy>();
            var profile = new ShippingProfileEntity() { Insurance = true };

            testObject.ApplyProfile(profile, shipment);

            insuranceChoice.VerifySet(i => i.Insured = true);
        }

    }
}
