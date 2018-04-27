using System;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Other;
using ShipWorks.Shipping.Services;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Services
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class ShippingProfileManagerWrapperTest : IDisposable
    {
        private readonly DataContext context;
        private Mock<ISqlAdapter> sqlAdapter;
        private readonly ShippingProfileManagerWrapper testObject;

        public ShippingProfileManagerWrapperTest(DatabaseFixture db)
        {
            Mock<ISqlAdapterFactory> sqlAdapterFactory = null;

            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock => 
            {
                sqlAdapter = mock.Override<ISqlAdapter>();
                sqlAdapterFactory = mock.Override<ISqlAdapterFactory>();
            });

            sqlAdapterFactory.Setup(s => s.Create()).Returns(sqlAdapter);

            testObject = context.Mock.Create<ShippingProfileManagerWrapper>();
        }

        [Fact]
        public void GetOrCreatePrimaryProfile_ReturnsExistingProfile_WhenProfileExists()
        {
            var profile = Create.Profile()
                .AsPrimary()
                .AsOther()
                .Save();

            var loadedProfile = testObject.GetOrCreatePrimaryProfile(context.Mock.Create<OtherShipmentType>());

            Assert.Equal(profile.ShippingProfileID, loadedProfile.ShippingProfileID);
        }

        [Fact]
        public void GetOrCreatePrimaryProfile_SetsDefaultValues_WhenProfileIsCreated()
        {
            var profile = testObject.GetOrCreatePrimaryProfile(context.Mock.Create<OtherShipmentType>());

            Assert.Equal("Defaults - Other", profile.Name);
            Assert.Equal(ShipmentTypeCode.Other, profile.ShipmentType);
            Assert.True(profile.ShipmentTypePrimary);
        }

        [Fact]
        public void GetOrCreatePrimaryProfile_DelegatesToShipmentTypeForConfiguration_WhenProfileIsCreated()
        {
            Mock<OtherShipmentType> shipmentType = context.Mock.CreateMock<OtherShipmentType>();
            shipmentType.CallBase = true;

            var profile = testObject.GetOrCreatePrimaryProfile(shipmentType.Object);

            shipmentType.Verify(x => x.ConfigurePrimaryProfile(profile));
        }

        [Fact]
        public void LoadProfileData_DoesNotFetchEntityCollection_IfProfileIsNewAndRefreshIfPresentIsTrue()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = ShipmentTypeCode.None };

            profile.IsNew = true;
            bool refreshIfPresent = true;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntityCollection(profile.Packages, It.IsAny<RelationPredicateBucket>()), Times.Never);
        }

        [Fact]
        public void LoadProfileData_DoesFetchEntityCollection_IfProfileIsNotNewAndRefreshIfPresentIsTrue()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = ShipmentTypeCode.None };

            profile.IsNew = false;
            bool refreshIfPresent = true;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntityCollection(profile.Packages, It.IsAny<RelationPredicateBucket>()));
        }

        [Fact]
        public void LoadProfileData_DoesNotFetchEntityCollection_IfProfileIsNotNewAndRefreshIfPresentIsFalse()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = ShipmentTypeCode.None };

            profile.IsNew = false;
            bool refreshIfPresent = false;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntityCollection(profile.Packages, It.IsAny<RelationPredicateBucket>()), Times.Never);
        }

        [Fact]
        public void LoadProfileData_DoesNotFetchEntityCollection_IfProfileIsNewAndRefreshIfPresentIsFalse()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = ShipmentTypeCode.None };

            profile.IsNew = true;
            bool refreshIfPresent = false;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntityCollection(profile.Packages, It.IsAny<RelationPredicateBucket>()), Times.Never);
        }

        [Fact]
        public void LoadProfileData_DoesNotFetchEntity_WhenShipmentTypeCodeIsNone()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = ShipmentTypeCode.None };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<IEntity2>()), Times.Never);
        }

        [Fact]
        public void LoadProfileData_DoesFetchEntity_WhenShipmentTypeCodeIsNotNone()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = ShipmentTypeCode.UpsOnLineTools };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<IEntity2>()));
        }


        [Fact]
        public void LoadProfileData_DoesNotFetchEntity_WhenShipmentTypeCodeIsNull()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = null };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<IEntity2>()), Times.Never);
        }

        [Fact]
        public void LoadProfileData_FetchesUpsProfileEntithy_WhenProfileIsUps()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = ShipmentTypeCode.UpsOnLineTools };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<UpsProfileEntity>()));
        }

        [Fact]
        public void LoadProfileData_FetchesFedExProfileEntity_WhenProfileIsFedEx()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = ShipmentTypeCode.FedEx };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<FedExProfileEntity>()));
        }

        [Fact]
        public void LoadProfileData_FetchesOnTracProfileEntity_WhenProfileIsOnTrac()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = ShipmentTypeCode.OnTrac };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<OnTracProfileEntity>()));
        }

        [Fact]
        public void LoadProfileData_FetchesIParcelProfileEntity_WhenProfileIsIParcel()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = ShipmentTypeCode.iParcel };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<IParcelProfileEntity>()));
        }

        [Fact]
        public void LoadProfileData_FetchesOtherProfileEntity_WhenProfileIsOther()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = ShipmentTypeCode.Other };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<OtherProfileEntity>()));
        }

        [Fact]
        public void LoadProfileData_FetchesBestRateProfileEntity_WhenProfileIsBestRate()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = ShipmentTypeCode.BestRate };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<BestRateProfileEntity>()));
        }

        [Fact]
        public void LoadProfileData_FetchesAmazonProfileEntity_WhenProfileIsAmazon()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = ShipmentTypeCode.Amazon };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<AmazonProfileEntity>()));
        }

        [Fact]
        public void LoadProfileData_FetchesDhlExpressProfileEntity_WhenProfileIsDhlExpress()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = ShipmentTypeCode.DhlExpress };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<DhlExpressProfileEntity>()));
        }

        [Fact]
        public void LoadProfileData_FetchesAsendiaProfileEntity_WhenProfileIsAsendia()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = ShipmentTypeCode.Asendia };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<AsendiaProfileEntity>()));
        }

        [Fact]
        public void LoadProfileData_FetchesChildrenProfileEntity_WhenProfileIsUsps()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = ShipmentTypeCode.Usps };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            sqlAdapter.Setup(a => a.FetchEntity(It.IsAny<IEntity2>())).Callback<IEntity2>((e) => e.Fields.State = EntityState.Fetched);

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<PostalProfileEntity>()));
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<UspsProfileEntity>()));
        }

        [Fact]
        public void LoadProfileData_FetchesChildrenProfileEntity_WhenProfileIsUspsExpress1()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = ShipmentTypeCode.Express1Usps };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            sqlAdapter.Setup(a => a.FetchEntity(It.IsAny<IEntity2>())).Callback<IEntity2>((e) => e.Fields.State = EntityState.Fetched);

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<PostalProfileEntity>()));
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<UspsProfileEntity>()));
        }

        [Fact]
        public void LoadProfileData_FetchesChildrenProfileEntity_WhenProfileIsEndicia()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = ShipmentTypeCode.Endicia };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            sqlAdapter.Setup(a => a.FetchEntity(It.IsAny<IEntity2>())).Callback<IEntity2>((e) => e.Fields.State = EntityState.Fetched);

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<PostalProfileEntity>()));
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<EndiciaProfileEntity>()));
        }

        [Fact]
        public void LoadProfileData_FetchesChildrenProfileEntity_WhenProfileIsEndiciaExpress1()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = ShipmentTypeCode.Express1Endicia };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            sqlAdapter.Setup(a => a.FetchEntity(It.IsAny<IEntity2>())).Callback<IEntity2>((e) => e.Fields.State = EntityState.Fetched);

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<PostalProfileEntity>()));
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<EndiciaProfileEntity>()));
        }

        [Fact]
        public void LoadProfileData_FetchesChildrenProfileEntity_WhenProfileIsPostalWebTools()
        {
            ShippingProfileEntity profile = new ShippingProfileEntity() { ShipmentType = ShipmentTypeCode.PostalWebTools };

            profile.IsNew = false;
            profile.Fields.State = EntityState.Fetched;
            profile.ShippingProfileID = 123;
            bool refreshIfPresent = true;

            sqlAdapter.Setup(a => a.FetchEntity(It.IsAny<IEntity2>())).Callback<IEntity2>((e) => e.Fields.State = EntityState.Fetched);

            testObject.LoadProfileData(profile, refreshIfPresent);
            sqlAdapter.Verify(a => a.FetchEntity(It.IsAny<PostalProfileEntity>()));
        }

        public void Dispose() => context.Dispose();
    }
}
