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

        [Fact]
        public void ApplyProfile_DelegatesToBaseStrategy()
        {
            Apply();

            mock.Mock<IShippingProfileApplicationStrategy>().Verify(s => s.ApplyProfile(profile, shipment), Times.Once);
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
            Apply();
            mock.Mock<ShipmentType>().Verify(s => s.UpdateDynamicShipmentData(shipment), Times.Once);
        }

        [Fact]
        public void ApplyProfile_UpdateTotalWeightIsCalled()
        {
            Apply();
            mock.Mock<ShipmentType>().Verify(s => s.UpdateTotalWeight(shipment), Times.Once);
        }

        private void Apply()
        {
            var testObject = mock.Create<PostalShippingProfileApplicationStrategy>();
            testObject.ApplyProfile(profile, shipment);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}