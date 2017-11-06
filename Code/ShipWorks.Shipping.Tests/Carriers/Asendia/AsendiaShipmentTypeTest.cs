using Autofac.Extras.Moq;
using Interapptive.Shared.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Asendia;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Tests.Shared;
using System;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Asendia
{
    public class AsendiaShipmentTypeTest : IDisposable
    {
        AutoMock mock;

        public AsendiaShipmentTypeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }
        
        [Fact]
        public void ShipmentTypeCode_IsAsendia()
        {
            var testObject = mock.Create<AsendiaShipmentType>();

            Assert.Equal(ShipmentTypeCode.Asendia, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void ConfigurePrimaryProfile_SetsAsendiaProfileDefaults()
        {
            var repo = mock.Mock<ICarrierAccountRepository<AsendiaAccountEntity, IAsendiaAccountEntity>>();
            repo.SetupGet(r => r.AccountsReadOnly).Returns(new[] { new AsendiaAccountEntity() { AsendiaAccountID = 123456789 } });

            AsendiaShipmentType testObject = mock.Create<AsendiaShipmentType>();

            ShippingProfileEntity profile = new ShippingProfileEntity()
            {
                Asendia = new AsendiaProfileEntity()
            };

            testObject.ConfigurePrimaryProfile(profile);

            Assert.Equal(123456789, profile.Asendia.AsendiaAccountID);
            Assert.Equal((int) AsendiaServiceType.AsendiaPriorityTracked, profile.Asendia.Service);
            Assert.Equal((int) ShipEngineContentsType.Merchandise, profile.Asendia.Contents);
            Assert.Equal((int) ShipEngineNonDeliveryType.ReturnToSender, profile.Asendia.NonDelivery);
            Assert.False(profile.Asendia.NonMachinable);
            Assert.Equal(0, profile.Asendia.Weight);
            Assert.Equal(0, profile.Asendia.DimsProfileID);
            Assert.Equal(0, profile.Asendia.DimsLength);
            Assert.Equal(0, profile.Asendia.DimsWidth);
            Assert.Equal(0, profile.Asendia.DimsHeight);
            Assert.Equal(0, profile.Asendia.DimsWeight);
            Assert.True(profile.Asendia.DimsAddWeight);
        }

        [Fact]
        public void ConfigurePrimaryProfile_SetsAsendiaAccountIDToZero_WhenNoAccounts()
        {
            var repo = mock.Mock<ICarrierAccountRepository<AsendiaAccountEntity, IAsendiaAccountEntity>>();
            repo.SetupGet(r => r.AccountsReadOnly).Returns(new AsendiaAccountEntity[0]);

            AsendiaShipmentType testObject = mock.Create<AsendiaShipmentType>();

            ShippingProfileEntity profile = new ShippingProfileEntity()
            {
                Asendia = new AsendiaProfileEntity()
            };

            testObject.ConfigurePrimaryProfile(profile);

            Assert.Equal(0, profile.Asendia.AsendiaAccountID);
        }

        [Fact]
        public void ConfigurePrimaryProfile_SetsAsendiaAccountIDToFirstAccountID_WhenAccountsExist()
        {
            var repo = mock.Mock<ICarrierAccountRepository<AsendiaAccountEntity, IAsendiaAccountEntity>>();
            repo.SetupGet(r => r.AccountsReadOnly).Returns(new[] { new AsendiaAccountEntity() { AsendiaAccountID = 123456789 }, new AsendiaAccountEntity() { AsendiaAccountID = 987654321 } });

            AsendiaShipmentType testObject = mock.Create<AsendiaShipmentType>();

            ShippingProfileEntity profile = new ShippingProfileEntity()
            {
                Asendia = new AsendiaProfileEntity()
            };

            testObject.ConfigurePrimaryProfile(profile);

            Assert.Equal(123456789, profile.Asendia.AsendiaAccountID);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
