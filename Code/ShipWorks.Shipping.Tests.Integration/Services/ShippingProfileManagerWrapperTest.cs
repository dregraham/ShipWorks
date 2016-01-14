using System;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.Other;
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

                Assert.Equal(EntityState.Fetched, loadedProfile.Fields.State);
                Assert.NotNull(loadedProfile.FedEx);
                Assert.Empty(loadedProfile.FedEx.Packages);
            }
        }
        #endregion

        public void Dispose() => context.Dispose();
    }
}
