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
            var testObject = mock.Create<PostalShippingProfileApplicationStrategy>();
            testObject.ApplyProfile(profile, shipment);

            mock.Mock<IShippingProfileApplicationStrategy>().Verify(s => s.ApplyProfile(profile, shipment), Times.Once);
        }

        [Theory]
        [InlineData(null, 42)]
        [InlineData(0, 42)]
        [InlineData(5, 5)]
        public void ApplyProfile_WeightIsSetProperly(double? profileWeight, double expectedWeight)
        {
            shipment.ContentWeight = 42;
            packageProfile.Weight = profileWeight;

            var testObject = mock.Create<PostalShippingProfileApplicationStrategy>();
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(expectedWeight, shipment.ContentWeight);
        }

        [Fact]
        public void ApplyProfile_DimsProfileIDIsSet()
        {
            packageProfile.DimsProfileID = 44;
            
            var testObject = mock.Create<PostalShippingProfileApplicationStrategy>();
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(42, shipment.Postal.DimsProfileID);
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
            
            var testObject = mock.Create<PostalShippingProfileApplicationStrategy>();
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(42, shipment.Postal.DimsProfileID);
            Assert.Equal(1, shipment.Postal.DimsLength);
            Assert.Equal(1, shipment.Postal.DimsWidth);
            Assert.Equal(1, shipment.Postal.DimsHeight);
            Assert.Equal(1, shipment.Postal.DimsWeight);
            Assert.True(shipment.Postal.DimsAddWeight);
        }

        [Fact]
        public void ApplyProfile_PackagingInformationIsSet()
        {
            profile.Postal.PackagingType = 7;
            profile.Postal.NonRectangular = true;
            profile.Postal.NonMachinable = true;
            
            var testObject = mock.Create<PostalShippingProfileApplicationStrategy>();
            testObject.ApplyProfile(profile, shipment);
            
            Assert.Equal(7, shipment.Postal.PackagingType);
            Assert.True(shipment.Postal.NonRectangular);
            Assert.True(shipment.Postal.NonMachinable);
        }
        
        //TODO: Test all the things!
        
        public void Dispose()
        {
            mock.Dispose();
        }
    }
}