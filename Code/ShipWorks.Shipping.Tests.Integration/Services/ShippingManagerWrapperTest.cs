﻿using System;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Services;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Services
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class ShippingManagerWrapperTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly DataContext context;
        private readonly ShipmentEntity shipment;

        public ShippingManagerWrapperTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
            mock = context.Mock;

            var customerLicense = mock.Create<CustomerLicense>(new TypedParameter(typeof(string), "someKey"));

            // Mock up the CustomerLicense constructor parameter Func<string, ICustomerLicense>
            var repo = mock.MockRepository.Create<Func<string, ICustomerLicense>>();
            repo.Setup(x => x(It.IsAny<string>()))
                .Returns(customerLicense);
            mock.Provide(repo.Object);

            var writer = mock.MockRepository.Create<ICustomerLicenseWriter>();
            writer.Setup(w => w.Write(It.IsAny<ICustomerLicense>())).Callback(() => { });
            mock.Provide(writer.Object);

            var licenseService = mock.MockRepository.Create<ILicenseService>();
            licenseService.Setup(ls => ls.CheckRestriction(It.IsAny<EditionFeature>(), It.IsAny<object>()))
                .Returns(EditionRestrictionLevel.None);
            mock.Provide(licenseService.Object);

            shipment = Create.Shipment(context.Order).Save();
        }

        [Fact]
        public void ChangeShipmentType_CreatesPostalAndUspsEntities_WhenChangedToUsps()
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
        public void ChangeShipmentType_DoesNotDeleteOtherEntity_WhenChangedToUsps()
        {
            Modify.Shipment(shipment).AsOther().Save();

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
        public void ChangeShipmentType_AppliesDefaultProfile_WhenChangedToNewShipmentType()
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
        public void ChangeShipmentType_DoesNotApplyDefaultProfile_WhenChangedToExistingShipmentType()
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
            context.Dispose();
        }
    }
}
