using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.BestRate
{
    public class BestRateShippingProfileApplicationStrategyTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ShipmentEntity shipment;
        private readonly ShippingProfileEntity profile;

        private readonly BestRateShippingProfileApplicationStrategy testObject;

        public BestRateShippingProfileApplicationStrategyTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipment = new ShipmentEntity
            {
                BestRate = new BestRateShipmentEntity
                {
                    DimsProfileID = 9,
                    DimsLength = 9,
                    DimsWidth = 9,
                    DimsHeight = 9,
                    DimsWeight = 9,
                    DimsAddWeight = false,
                    ServiceLevel = (int) ServiceLevelType.Anytime
                },
                ContentWeight = 9,
                Insurance = false
            };
            profile = new ShippingProfileEntity
            {
                Packages =
                {
                    new PackageProfileEntity
                    {
                        DimsProfileID = 0,
                        DimsLength = 1,
                        DimsWidth = 2,
                        DimsHeight = 3,
                        DimsWeight = 4,
                        DimsAddWeight = true,
                        Weight = 5
                    }
                },
                BestRate = new BestRateProfileEntity
                {
                    ServiceLevel = (int?) ServiceLevelType.OneDay
                },
                Insurance = true
            };

            testObject = mock.Create<BestRateShippingProfileApplicationStrategy>();
        }
        
        [Fact]
        public void ApplyProfile_AppliesProfilesDimsProfileID()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(0, shipment.BestRate.DimsProfileID);
        }
        
        [Fact]
        public void ApplyProfile_AppliesProfilesDimsWeight()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(4, shipment.BestRate.DimsWeight);
        }
        
        [Fact]
        public void ApplyProfile_AppliesProfilesDimsLength()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(1, shipment.BestRate.DimsLength);
        }
        
        [Fact]
        public void ApplyProfile_AppliesProfilesDimsHeight()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(3, shipment.BestRate.DimsHeight);
        }
        
        [Fact]
        public void ApplyProfile_AppliesProfilesDimsWidth()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(2, shipment.BestRate.DimsWidth);
        }
        
        [Fact]
        public void ApplyProfile_AppliesProfilesDimsAddWeight()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.True(shipment.BestRate.DimsAddWeight);
        }
        
        [Fact]
        public void ApplyProfile_AppliesProfilesServiceLevel()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal((int) ServiceLevelType.OneDay, shipment.BestRate.ServiceLevel);
        }
        
        [Fact]
        public void ApplyProfile_AppliesProfilesInsurance()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(true, shipment.BestRate.Insurance);
        }
        
        [Fact]
        public void ApplyProfile_AppliesProfilesWeight_WhenProfileWeightHasValueGreaterThanZero()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(5, shipment.ContentWeight);
        }
        
        [Fact]
        public void ApplyProfile_DoesNotAppliesProfilesWeight_WhenProfileWeightIsNull()
        {
            profile.Packages[0].Weight = null;
            
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(9, shipment.ContentWeight);
        }
        
        [Fact]
        public void ApplyProfile_DoesNotAppliesProfilesWeight_WhenProfileWeightIsZero()
        {
            profile.Packages[0].Weight = 0;
            
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(9, shipment.ContentWeight);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}