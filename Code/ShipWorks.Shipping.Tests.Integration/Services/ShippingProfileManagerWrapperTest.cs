using System;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.Other;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Services
{
    [Collection("Database collection")]
    public class ShippingProfileManagerWrapperTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ShippingProfileManagerWrapper testObject;

        public ShippingProfileManagerWrapperTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));

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
        public void GetOrCreatePrimaryProfile_CreatesNewProfile_WhenProfileDoesNotExist()
        {
            var profile = testObject.GetOrCreatePrimaryProfile(context.Mock.Create<OtherShipmentType>());

            using (SqlAdapter adapter = SqlAdapter.Create(false))
            {
                var loadedProfile = new ShippingProfileEntity(profile.ShippingProfileID);

                adapter.FetchEntity(loadedProfile);

                Assert.Equal(EntityState.Fetched, loadedProfile.Fields.State);
            }
        }

        [Fact]
        public void GetOrCreatePrimaryProfile_SetsDefaultValues_WhenProfileIsCreated()
        {
            var profile = testObject.GetOrCreatePrimaryProfile(context.Mock.Create<OtherShipmentType>());

            Assert.Equal("Defaults - Other", profile.Name);
            Assert.Equal(ShipmentTypeCode.Other, profile.ShipmentTypeCode);
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

        #region "Carrier specific tests"
        [Fact]
        public void GetOrCreatePrimaryProfile_SavesObjectTree_WhenTypeIsFedEx()
        {
            var profile = testObject.GetOrCreatePrimaryProfile(context.Mock.Create<FedExShipmentType>());

            using (SqlAdapter adapter = SqlAdapter.Create(false))
            {
                var prefetchPath = new PrefetchPath2(EntityType.ShippingProfileEntity);
                var profilePatch = prefetchPath.Add(ShippingProfileEntity.PrefetchPathFedEx);
                profilePatch.SubPath.Add(FedExProfileEntity.PrefetchPathPackages);

                var loadedProfile = new ShippingProfileEntity(profile.ShippingProfileID);

                adapter.FetchEntity(loadedProfile, prefetchPath);

                Assert.NotNull(loadedProfile.FedEx);
                Assert.Empty(loadedProfile.FedEx.Packages);
            }
        }

        [Fact]
        public void GetOrCreatePrimaryProfile_SavesObjectTree_WhenTypeIsUps()
        {
            var profile = testObject.GetOrCreatePrimaryProfile(context.Mock.Create<UpsOltShipmentType>());

            using (SqlAdapter adapter = SqlAdapter.Create(false))
            {
                var prefetchPath = new PrefetchPath2(EntityType.ShippingProfileEntity);
                var profilePatch = prefetchPath.Add(ShippingProfileEntity.PrefetchPathUps);
                profilePatch.SubPath.Add(UpsProfileEntity.PrefetchPathPackages);

                var loadedProfile = new ShippingProfileEntity(profile.ShippingProfileID);

                adapter.FetchEntity(loadedProfile, prefetchPath);

                Assert.NotNull(loadedProfile.Ups);
                Assert.Empty(loadedProfile.Ups.Packages);
            }
        }

        [Fact]
        public void GetOrCreatePrimaryProfile_SavesObjectTree_WhenTypeIsiParcel()
        {
            var profile = testObject.GetOrCreatePrimaryProfile(context.Mock.Create<iParcelShipmentType>());

            using (SqlAdapter adapter = SqlAdapter.Create(false))
            {
                var prefetchPath = new PrefetchPath2(EntityType.ShippingProfileEntity);
                var profilePatch = prefetchPath.Add(ShippingProfileEntity.PrefetchPathIParcel);
                profilePatch.SubPath.Add(IParcelProfileEntity.PrefetchPathPackages);

                var loadedProfile = new ShippingProfileEntity(profile.ShippingProfileID);

                adapter.FetchEntity(loadedProfile, prefetchPath);

                Assert.NotNull(loadedProfile.IParcel);
                Assert.Empty(loadedProfile.IParcel.Packages);
            }
        }

        [Fact]
        public void GetOrCreatePrimaryProfile_SavesObjectTree_WhenTypeIsUsps()
        {
            var profile = testObject.GetOrCreatePrimaryProfile(context.Mock.Create<UspsShipmentType>());

            using (SqlAdapter adapter = SqlAdapter.Create(false))
            {
                var prefetchPath = new PrefetchPath2(EntityType.ShippingProfileEntity);
                var profilePatch = prefetchPath.Add(ShippingProfileEntity.PrefetchPathPostal);
                profilePatch.SubPath.Add(PostalProfileEntity.PrefetchPathUsps);

                var loadedProfile = new ShippingProfileEntity(profile.ShippingProfileID);

                adapter.FetchEntity(loadedProfile, prefetchPath);

                Assert.NotNull(loadedProfile.Postal);
                Assert.NotNull(loadedProfile.Postal.Usps);
            }
        }

        [Fact]
        public void GetOrCreatePrimaryProfile_SavesObjectTree_WhenTypeIsEndicia()
        {
            var profile = testObject.GetOrCreatePrimaryProfile(context.Mock.Create<EndiciaShipmentType>());

            using (SqlAdapter adapter = SqlAdapter.Create(false))
            {
                var prefetchPath = new PrefetchPath2(EntityType.ShippingProfileEntity);
                var profilePatch = prefetchPath.Add(ShippingProfileEntity.PrefetchPathPostal);
                profilePatch.SubPath.Add(PostalProfileEntity.PrefetchPathEndicia);

                var loadedProfile = new ShippingProfileEntity(profile.ShippingProfileID);

                adapter.FetchEntity(loadedProfile, prefetchPath);

                Assert.NotNull(loadedProfile.Postal);
                Assert.NotNull(loadedProfile.Postal.Endicia);
            }
        }

        [Fact]
        public void GetOrCreatePrimaryProfile_SavesObjectTree_WhenTypeOnTrac()
        {
            var profile = testObject.GetOrCreatePrimaryProfile(context.Mock.Create<OnTracShipmentType>());

            using (SqlAdapter adapter = SqlAdapter.Create(false))
            {
                var prefetchPath = new PrefetchPath2(EntityType.ShippingProfileEntity);
                prefetchPath.Add(ShippingProfileEntity.PrefetchPathOnTrac);

                var loadedProfile = new ShippingProfileEntity(profile.ShippingProfileID);

                adapter.FetchEntity(loadedProfile, prefetchPath);

                Assert.NotNull(loadedProfile.OnTrac);
            }
        }

        [Fact]
        public void GetOrCreatePrimaryProfile_SavesObjectTree_WhenTypeOther()
        {
            var profile = testObject.GetOrCreatePrimaryProfile(context.Mock.Create<OtherShipmentType>());

            using (SqlAdapter adapter = SqlAdapter.Create(false))
            {
                var prefetchPath = new PrefetchPath2(EntityType.ShippingProfileEntity);
                prefetchPath.Add(ShippingProfileEntity.PrefetchPathOther);

                var loadedProfile = new ShippingProfileEntity(profile.ShippingProfileID);

                adapter.FetchEntity(loadedProfile, prefetchPath);

                Assert.NotNull(loadedProfile.Other);
            }
        }
        #endregion

        public void Dispose() => context.Dispose();
    }
}
