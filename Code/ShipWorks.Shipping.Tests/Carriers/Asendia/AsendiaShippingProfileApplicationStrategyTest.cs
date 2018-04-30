using System;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Interapptive.Shared.Enums;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Asendia;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Tests.Carriers.Asendia
{
    public class AsendiaShippingProfileApplicationStrategyTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ShippingProfileEntity profile;
        private readonly ShipmentEntity shipment;

        public AsendiaShippingProfileApplicationStrategyTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            profile = new ShippingProfileEntity
            {
                Packages = { new PackageProfileEntity() },
                Asendia = new AsendiaProfileEntity()
            };
            shipment = new ShipmentEntity()
            {
                Asendia = new AsendiaShipmentEntity()
            };
        }

        [Fact]
        public void ApplyProfile_AccountIdSetFromProfile_WhenProfileAccountIdNotZero()
        {
            profile.Asendia.AsendiaAccountID = 45;
            Apply();
            Assert.Equal(45, shipment.Asendia.AsendiaAccountID);
        }

        [Fact]
        public void ApplyProfile_AccountIdSetFromAccountRepository_WhenProfileAccountIdZero()
        {
            profile.Asendia.AsendiaAccountID = 0;
            mock.Mock<ICarrierAccountRepository<AsendiaAccountEntity, IAsendiaAccountEntity>>()
                .SetupGet(r => r.AccountsReadOnly)
                .Returns(new[] { new AsendiaAccountEntity(47) });
            
            Apply();
            
            Assert.Equal(47, shipment.Asendia.AsendiaAccountID);
        }

        [Fact]
        public void ApplyProfile_AsendiaShipmentFieldsSetCorrectly()
        {
            profile.Asendia.Service = AsendiaServiceType.AsendiaISAL;
            profile.Asendia.ShippingProfile.Insurance = true;
            profile.Asendia.NonMachinable = true;
            profile.Asendia.NonDelivery = 5;
            profile.Asendia.Contents = 42;
            
            Apply();
            
            Assert.Equal(AsendiaServiceType.AsendiaISAL, shipment.Asendia.Service);
            Assert.True(shipment.Asendia.Insurance);
            Assert.True(shipment.Asendia.NonMachinable);
            Assert.Equal(5, shipment.Asendia.NonDelivery);
            Assert.Equal(42, shipment.Asendia.Contents);
        }

        [Theory]
        [InlineData(null, 42)]
        [InlineData(0, 42)]
        [InlineData(2, 2)]
        public void ApplyProfile_WeightIsSetCorrectly(double? weight, double expectedValue)
        {
            profile.Packages[0].Weight = weight;
            shipment.ContentWeight = 42;

            Apply();
            
            Assert.Equal(expectedValue, shipment.ContentWeight);
        }

        [Fact]
        public void ApplyProfile_DimensionsAreSetCorrectly()
        {
            var packageProfile = profile.Packages[0];
            packageProfile.DimsProfileID = 1;
            packageProfile.DimsWeight = 2;
            packageProfile.DimsLength = 3;
            packageProfile.DimsHeight = 4;
            packageProfile.DimsWidth = 5;
            packageProfile.DimsAddWeight = true;
            
            Apply();
            
            Assert.Equal(1, shipment.Asendia.DimsProfileID);
            Assert.Equal(2, shipment.Asendia.DimsWeight);
            Assert.Equal(3, shipment.Asendia.DimsLength);
            Assert.Equal(4, shipment.Asendia.DimsHeight);
            Assert.Equal(5, shipment.Asendia.DimsWidth);
            Assert.True(shipment.Asendia.DimsAddWeight);
        }

        [Fact]
        public void ApplyProfile_UpdateTotalWeightCalled()
        {
            var shipmentType = mock.FromFactory<IShipmentTypeManager>()
                .Mock(m => m.Get(shipment));

            Apply();
            
            shipmentType.Verify(s=>s.UpdateTotalWeight(shipment), Times.Once);
        }
        
        [Fact]
        public void ApplyProfile_UpdateDynamicShipmentData()
        {
            var shipmentType = mock.FromFactory<IShipmentTypeManager>()
                .Mock(m => m.Get(shipment));

            Apply();
            
            shipmentType.Verify(s=>s.UpdateDynamicShipmentData(shipment), Times.Once);
        }

        private void Apply()
        {
            var testObject = mock.Create<AsendiaShippingProfileApplicationStrategy>();
            testObject.ApplyProfile(profile, shipment);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}