using System;
using System.Linq;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Profiles;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Tests.Carriers.Postal
{
    public class PostalShippingProfileApplicationStrategyTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ShippingProfileEntity profile;
        private readonly ShipmentEntity shipment;
        private PackageProfileEntity packageProfile;

        public PostalShippingProfileApplicationStrategyTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            profile = new ShippingProfileEntity()
            {
                Packages = { new PackageProfileEntity() },
                Postal = new PostalProfileEntity()
            };
            shipment = new ShipmentEntity() { Postal = new PostalShipmentEntity() };
            packageProfile = profile.Packages.Single();
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
            var testObject = mock.Create<PostalShippingProfileApplicationStrategy>();
            testObject.ApplyProfile(profile, shipment);
        }


        [Fact]
        public void ApplyProfile_DelegatesToShipmentTypeManagerWithShipment()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            mock.Create<PostalShippingProfileApplicationStrategy>().ApplyProfile(profile, shipment);

            mock.Mock<IShipmentTypeManager>().Verify(s => s.Get(shipment));
        }

        [Fact]
        public void ApplyProfile_SetsOriginIDOnShipment()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.OriginID = 123;

            mock.Create<PostalShippingProfileApplicationStrategy>().ApplyProfile(profile, shipment);

            Assert.Equal(123, shipment.OriginOriginID);
        }

        [Fact]
        public void ApplyProfile_SetsReturnShipmentOnShipment()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.ReturnShipment = true;

            mock.Create<PostalShippingProfileApplicationStrategy>().ApplyProfile(profile, shipment);

            Assert.True(shipment.ReturnShipment);
        }

        [Fact]
        public void ApplyProfile_SetsRequestedLabelFormatOnShipment()
        {
            var (profile, shipment) = GetEmptyShipmentAndProfile();
            profile.RequestedLabelFormat = (int) ThermalLanguage.EPL;

            mock.Create<PostalShippingProfileApplicationStrategy>().ApplyProfile(profile, shipment);

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

            mock.Create<PostalShippingProfileApplicationStrategy>().ApplyProfile(profile, shipment);

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

            var testObject = mock.Create<PostalShippingProfileApplicationStrategy>();

            testObject.ApplyProfile(profile, shipment);

            insuranceChoice.VerifySet(i => i.Insured = true);
        }

        private (ShippingProfileEntity, ShipmentEntity) GetEmptyShipmentAndProfile()
        {
            var profile = new ShippingProfileEntity()
            {
                Postal = new PostalProfileEntity()
            };

            profile.Packages.Add(new PackageProfileEntity());

            return (profile, new ShipmentEntity()
            {
                Postal = new PostalShipmentEntity()
            });
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}