using System;
using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Postal.Usps
{
    public class UspsShippingProfileApplicationStrategyTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly UspsShippingProfileApplicationStrategy testObject;
        private readonly ShippingProfileEntity profile;
        private readonly ShipmentEntity shipment;
        private readonly PackageProfileEntity packageProfile;

        public UspsShippingProfileApplicationStrategyTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testObject = mock.Create<UspsShippingProfileApplicationStrategy>();
            profile = new ShippingProfileEntity()
            {
                Packages = { new PackageProfileEntity() },
                Postal = new PostalProfileEntity
                {
                    Usps = new UspsProfileEntity()
                }
            };
            
            shipment = new ShipmentEntity()
            {
                Postal = new PostalShipmentEntity()
                {
                    Usps = new UspsShipmentEntity()
                }
            };
            packageProfile = profile.Packages.Single();
        }

        [Fact]
        public void ApplyProfile_AppliesUspsAccountID()
        {
            shipment.Postal.Usps.UspsAccountID = 2;
            profile.Postal.Usps.UspsAccountID = 1;

            Apply();

            Assert.Equal(1, shipment.Postal.Usps.UspsAccountID);
        }

        [Fact]
        public void ApplyProfile_AppliesRequireFullAddressValidation()
        {
            shipment.Postal.Usps.RequireFullAddressValidation = false;
            profile.Postal.Usps.RequireFullAddressValidation = true;
            
            Apply();

            Assert.True(shipment.Postal.Usps.RequireFullAddressValidation);
        }

        [Fact]
        public void ApplyProfile_AppliesHidePostage()
        {
            shipment.Postal.Usps.HidePostage = false;
            profile.Postal.Usps.HidePostage = true;

            Apply();

            Assert.True(shipment.Postal.Usps.HidePostage);
        }

        [Fact]
        public void ApplyProfile_AppliesRateShop()
        {
            shipment.Postal.Usps.RateShop = false;
            profile.Postal.Usps.RateShop = true;
            
            Apply();

            Assert.True(shipment.Postal.Usps.RateShop);
        }

        [Fact]
        public void ApplyProfile_AppliesPostalProfile()
        {
            shipment.Postal.Usps.Insurance = false;
            profile.Insurance = true;

            Apply();

            Assert.True(shipment.Postal.Usps.Insurance);
        }
        
         [Theory]
        [InlineData(null, 42)]
        [InlineData(0, 42)]
        [InlineData(5, 5)]
        public void ApplyProfile_WeightIsSetProperly(double? profileWeight, double expectedWeight)
        {
            // Here just to make sure content weight changes or stays the same after apply 
            shipment.ContentWeight = 42;
            packageProfile.Weight = profileWeight;

            Apply();

            Assert.Equal(expectedWeight, shipment.ContentWeight);
        }

        [Fact]
        public void ApplyProfile_DimsProfileIDIsSet()
        {
            packageProfile.DimsProfileID = 44;

            Apply();

            Assert.Equal(44, shipment.Postal.DimsProfileID);
        }

        [Fact]
        public void ApplyProfile_DimsensionsAreSet()
        {
            packageProfile.DimsProfileID = 44;
            packageProfile.DimsLength = 1;
            packageProfile.DimsWidth = 2;
            packageProfile.DimsHeight = 3;
            packageProfile.DimsWeight = 4;
            packageProfile.DimsAddWeight = true;

            Apply();

            Assert.Equal(44, shipment.Postal.DimsProfileID);
            Assert.Equal(1, shipment.Postal.DimsLength);
            Assert.Equal(2, shipment.Postal.DimsWidth);
            Assert.Equal(3, shipment.Postal.DimsHeight);
            Assert.Equal(4, shipment.Postal.DimsWeight);
            Assert.True(shipment.Postal.DimsAddWeight);
        }

        [Fact]
        public void ApplyProfile_PackagingInformationIsSet()
        {
            profile.Postal.PackagingType = 7;
            profile.Postal.NonRectangular = true;
            profile.Postal.NonMachinable = true;

            Apply();

            Assert.Equal(7, shipment.Postal.PackagingType);
            Assert.True(shipment.Postal.NonRectangular);
            Assert.True(shipment.Postal.NonMachinable);
        }

        [Fact]
        public void ApplyProfile_CustomsInformationIsSet()
        {
            profile.Postal.CustomsContentType = 42;
            profile.Postal.CustomsContentDescription = "Not Drugs";

            Apply();

            Assert.Equal(42, shipment.Postal.CustomsContentType);
            Assert.Equal("Not Drugs", shipment.Postal.CustomsContentDescription);
        }

        [Fact]
        public void ApplyProfile_ExpressSignatureIsSet()
        {
            profile.Postal.ExpressSignatureWaiver = true;

            Apply();

            Assert.True(shipment.Postal.ExpressSignatureWaiver);
        }

        [Fact]
        public void ApplyProfile_FacilityInfoIsSet()
        {
            profile.Postal.SortType = 42;
            profile.Postal.EntryFacility = 43;

            Apply();

            Assert.Equal(42, shipment.Postal.SortType);
            Assert.Equal(43, shipment.Postal.EntryFacility);
        }

        [Fact]
        public void ApplyProfile_MemoIsSet()
        {
            profile.Postal.Memo1 = "m1";
            profile.Postal.Memo2 = "m2";
            profile.Postal.Memo3 = "m3";

            Apply();

            Assert.Equal("m1", shipment.Postal.Memo1);
            Assert.Equal("m2", shipment.Postal.Memo2);
            Assert.Equal("m3", shipment.Postal.Memo3);
        }

        [Fact]
        public void ApplyProfile_NoPostageSet()
        {
            profile.Postal.NoPostage = true;
            Apply();
            Assert.True(shipment.Postal.NoPostage);
        }

        [Fact]
        public void ApplyProfile_InsuranceSet()
        {
            profile.Insurance = true;
            Apply();
            Assert.True(shipment.Postal.Insurance);
        }

        [Fact]
        public void ApplyProfile_UpdateDymanicShipmentDataIsCalled()
        {
            var shipmentType = mock.Mock<ShipmentType>();
            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            shipmentTypeManager.Setup(s => s.Get(It.IsAny<ShipmentEntity>())).Returns(shipmentType);

            Apply();

            shipmentType.Verify(s => s.UpdateDynamicShipmentData(shipment), Times.Once);
        }

        [Fact]
        public void ApplyProfile_UpdateTotalWeightIsCalled()
        {
            var shipmentType = mock.Mock<ShipmentType>();
            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            shipmentTypeManager.Setup(s => s.Get(It.IsAny<ShipmentEntity>())).Returns(shipmentType);

            Apply();
            shipmentType.Verify(s => s.UpdateTotalWeight(shipment), Times.Once);
        }

        private void Apply()
        {
            testObject.ApplyProfile(profile, shipment);
        }

        [Fact]
        public void ApplyProfile_DelegatesToShipmentTypeManagerWithShipment()
        {
            Apply();

            mock.Mock<IShipmentTypeManager>().Verify(s => s.Get(shipment));
        }

        [Fact]
        public void ApplyProfile_SetsOriginIDOnShipment()
        {
            profile.OriginID = 123;

            Apply();

            Assert.Equal(123, shipment.OriginOriginID);
        }

        [Fact]
        public void ApplyProfile_SetsReturnShipmentOnShipment()
        {
            profile.ReturnShipment = true;

            Apply();

            Assert.True(shipment.ReturnShipment);
        }

        [Fact]
        public void ApplyProfile_SetsRequestedLabelFormatOnShipment()
        {
            profile.RequestedLabelFormat = (int) ThermalLanguage.EPL;

            Apply();

            Assert.Equal(ThermalLanguage.EPL, (ThermalLanguage) shipment.OriginOriginID);
        }

        [Fact]
        public void ApplyProfile_DelegatesToShipmentTypeToSaveLabelFormat()
        {
            profile.RequestedLabelFormat = (int) ThermalLanguage.EPL;
            
            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            var shipmentType = mock.Mock<ShipmentType>();
            shipmentTypeManager.Setup(s => s.Get(shipment)).Returns(shipmentType);

            Apply();

            shipmentType.Verify(s => s.SaveRequestedLabelFormat((ThermalLanguage) shipment.RequestedLabelFormat, shipment));
        }

        [Fact]
        public void ApplyProfile_SetsInsuranceValueOnPackage()
        {
            profile.Insurance = true;

            var insuranceChoice = mock.Mock<IInsuranceChoice>();
            
            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            var shipmentType = mock.Mock<ShipmentType>();
            shipmentType.Setup(s => s.GetParcelCount(shipment)).Returns(1);
            var shipmentParcel = new ShipmentParcel(shipment, null, insuranceChoice.Object, new Editing.DimensionsAdapter());

            shipmentType.Setup(s => s.GetParcelDetail(shipment, 0)).Returns(shipmentParcel);

            shipmentTypeManager.Setup(s => s.Get(shipment)).Returns(shipmentType);

            Apply();

            insuranceChoice.VerifySet(i => i.Insured = true);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}