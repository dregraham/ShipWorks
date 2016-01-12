using System;
using Autofac.Extras.Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Startup;
using ShipWorks.Stores;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Services
{
    [Collection("Database collection")]
    public class ChangeShipmentTypeTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ShipmentEntity shipment;

        public ChangeShipmentTypeTest(DatabaseFixture db)
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            ContainerInitializer.Initialize(mock.Container);

            db.CreateDataContext(mock);

            var store = Create.Entity<GenericModuleStoreEntity>().Save();
            var customer = Create.Entity<CustomerEntity>().Save();
            var order = Create.Order(store, customer).Save();

            shipment = Create.Shipment(order).AsOther().Save();

            // Reset the static fields before each test
            StoreManager.CheckForChanges();
        }

        [Fact]
        public void CreatesPostalAndUspsEntities_WhenChangedToUsps()
        {
            ShippingManagerWrapper wrapper = mock.Create<ShippingManagerWrapper>();

            wrapper.ChangeShipmentType(ShipmentTypeCode.Usps, shipment);

            using (SqlAdapter sqlAdapter = SqlAdapter.Create(false))
            {
                IPrefetchPath2 shipmentPrefetchPath = new PrefetchPath2(EntityType.ShipmentEntity);
                shipmentPrefetchPath.Add(ShipmentEntity.PrefetchPathPostal)
                    .SubPath.Add(PostalShipmentEntity.PrefetchPathUsps);

                ShipmentEntity loadedShipment = new ShipmentEntity(shipment.ShipmentID);
                sqlAdapter.FetchEntity(loadedShipment, shipmentPrefetchPath);

                Assert.NotNull(loadedShipment.Postal);
                Assert.NotNull(loadedShipment.Postal.Usps);
            }
        }

        [Fact]
        public void DoesNotDeleteOtherEntity_WhenChangedToUsps()
        {
            ShippingManagerWrapper wrapper = mock.Create<ShippingManagerWrapper>();

            wrapper.ChangeShipmentType(ShipmentTypeCode.Usps, shipment);

            using (SqlAdapter sqlAdapter = SqlAdapter.Create(false))
            {
                IPrefetchPath2 shipmentPrefetchPath = new PrefetchPath2(EntityType.ShipmentEntity);
                shipmentPrefetchPath.Add(ShipmentEntity.PrefetchPathOther);

                ShipmentEntity loadedShipment = new ShipmentEntity(shipment.ShipmentID);
                sqlAdapter.FetchEntity(loadedShipment, shipmentPrefetchPath);

                Assert.NotNull(loadedShipment.Other);
            }
        }

        [Fact]
        public void AppliesDefaultProfile_WhenChangedToNewShipmentType()
        {
            OnTracProfileEntity onTracProfile = Create.Entity<OnTracProfileEntity>()
                .SetDefaultsOnNullableFields()
                .Set(x => x.Reference2, "FOO")
                .Build();

            Create.Entity<ShippingProfileEntity>()
                .SetDefaultsOnNullableFields()
                .Set(x => x.ShipmentTypePrimary, true)
                .Set(x => x.ShipmentTypeCode, ShipmentTypeCode.OnTrac)
                .Set(x => x.OnTrac, onTracProfile)
                .Save();

            ShippingProfileManager.CheckForChangesNeeded();

            ShippingManagerWrapper wrapper = mock.Create<ShippingManagerWrapper>();

            wrapper.ChangeShipmentType(ShipmentTypeCode.OnTrac, shipment);

            using (SqlAdapter sqlAdapter = SqlAdapter.Create(false))
            {
                IPrefetchPath2 shipmentPrefetchPath = new PrefetchPath2(EntityType.ShipmentEntity);
                shipmentPrefetchPath.Add(ShipmentEntity.PrefetchPathOnTrac);

                ShipmentEntity loadedShipment = new ShipmentEntity(shipment.ShipmentID);
                sqlAdapter.FetchEntity(loadedShipment, shipmentPrefetchPath);

                Assert.Equal("FOO", loadedShipment.OnTrac.Reference2);
            }
        }

        [Fact]
        public void DoesNotApplyDefaultProfile_WhenChangedToExistingShipmentType()
        {
            Modify.Shipment(shipment).AsOnTrac().Set(x => x.ShipmentTypeCode, ShipmentTypeCode.Other).Save();

            OnTracProfileEntity onTracProfile = Create.Entity<OnTracProfileEntity>()
                .SetDefaultsOnNullableFields()
                .Set(x => x.Reference2, "FOO")
                .Build();

            Create.Entity<ShippingProfileEntity>()
                .SetDefaultsOnNullableFields()
                .Set(x => x.ShipmentTypePrimary, true)
                .Set(x => x.ShipmentTypeCode, ShipmentTypeCode.OnTrac)
                .Set(x => x.OnTrac, onTracProfile)
                .Save();

            ShippingProfileManager.CheckForChangesNeeded();

            ShippingManagerWrapper wrapper = mock.Create<ShippingManagerWrapper>();

            wrapper.ChangeShipmentType(ShipmentTypeCode.OnTrac, shipment);

            using (SqlAdapter sqlAdapter = SqlAdapter.Create(false))
            {
                IPrefetchPath2 shipmentPrefetchPath = new PrefetchPath2(EntityType.ShipmentEntity);
                shipmentPrefetchPath.Add(ShipmentEntity.PrefetchPathOnTrac);

                ShipmentEntity loadedShipment = new ShipmentEntity(shipment.ShipmentID);
                sqlAdapter.FetchEntity(loadedShipment, shipmentPrefetchPath);

                Assert.NotEqual("FOO", loadedShipment.OnTrac.Reference2);
            }
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
