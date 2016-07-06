using System;
using Autofac;
using Autofac.Core.Lifetime;
using Autofac.Core.Registration;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Other;
using ShipWorks.Shipping.Services;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Shipping
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class CustomsManagerWrapperTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ShipmentEntity shipment;
        private readonly SqlAdapter adapter;
        private readonly CustomsManagerWrapper testObject;
        private readonly ILifetimeScope lifetimeScope;

        public CustomsManagerWrapperTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));

            shipment = Create.Shipment(context.Order)
                .AsOther()
                .Set(x => x.ShipCountryCode, "UK")
                .Save();

            adapter = new SqlAdapter(false);

            lifetimeScope = IoC.BeginLifetimeScope();

            testObject = new CustomsManagerWrapper(lifetimeScope.Resolve<IShippingManager>());
        }

        [Fact]
        public void LoadCustomsItems_ThrowsArgumentNull_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.LoadCustomsItems(null, false, adapter));
        }


        [Fact]
        public void LoadCustomsItems_ThrowsArgumentNull_WhenSqlAdapterIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.LoadCustomsItems(shipment, false, null));
        }

        [Fact]
        public void LoadCustomsItems_DelegatesToShipmentType_ToCheckIfCustomsIsRequired()
        {
            var shipmentType = context.Mock.CreateMock<OtherShipmentType>();
            shipmentType.CallBase = true;

            context.Mock.AddRegistration(x =>
                x.RegisterInstance(shipmentType.Object).Keyed<ShipmentType>(ShipmentTypeCode.Other));

            testObject.LoadCustomsItems(shipment, false, adapter);

            shipmentType.Verify(x => x.IsCustomsRequired(shipment));
        }

        [Fact]
        public void LoadCustomsItems_LoadsCustoms_WhenReloadIsFalseButCustomsAreNotLoaded()
        {
            Modify.Shipment(shipment).Set(x => x.CustomsGenerated, true).Save();
            Create.Entity<ShipmentCustomsItemEntity>().Set(x => x.ShipmentID, shipment.ShipmentID).Save();

            testObject.LoadCustomsItems(shipment, false, adapter);

            Assert.NotEmpty(shipment.CustomsItems);
            Assert.True(shipment.CustomsItemsLoaded);
        }

        [Fact]
        public void LoadCustomsItems_LoadsCustoms_WhenCustomsIsCreatedAndReloadIsTrue()
        {
            Modify.Shipment(shipment).Set(x => x.CustomsGenerated, true).Save();
            Create.Entity<ShipmentCustomsItemEntity>().Set(x => x.ShipmentID, shipment.ShipmentID).Save();

            testObject.LoadCustomsItems(shipment, true, adapter);

            Assert.NotEmpty(shipment.CustomsItems);
            Assert.True(shipment.CustomsItemsLoaded);
        }

        [Fact]
        public void LoadCustomsItems_ReloadsCustoms_WhenCustomsIsCreatedAndLoadedButReloadIsTrue()
        {
            Modify.Shipment(shipment).Set(x => x.CustomsGenerated, true).Save();
            Create.Entity<ShipmentCustomsItemEntity>().Set(x => x.ShipmentID, shipment.ShipmentID).Save();

            shipment.CustomsItemsLoaded = true;

            testObject.LoadCustomsItems(shipment, true, adapter);

            Assert.NotEmpty(shipment.CustomsItems);
            Assert.True(shipment.CustomsItemsLoaded);
        }

        [Fact]
        public void LoadCustomsItems_DoesNotLoadCustoms_WhenCustomsIsLoadedAndReloadIsFalse()
        {
            Modify.Shipment(shipment).Set(x => x.CustomsGenerated, true).Save();
            Create.Entity<ShipmentCustomsItemEntity>().Set(x => x.ShipmentID, shipment.ShipmentID).Save();

            shipment.CustomsItemsLoaded = true;

            testObject.LoadCustomsItems(shipment, false, adapter);

            Assert.Empty(shipment.CustomsItems);
            Assert.True(shipment.CustomsItemsLoaded);
        }

        [Fact]
        public void LoadCustomsItems_DoesNotCreateCustoms_WhenShipmentIsProcessed()
        {
            shipment.Processed = true;

            testObject.LoadCustomsItems(shipment, false, adapter);

            Assert.False(shipment.CustomsGenerated);
            Assert.Empty(shipment.CustomsItems);
        }

        [Fact]
        public void LoadCustomsItems_MarksCustomsAsGenerated_WhenShipmentIsNotProcessedAndNotAlreadyGenerated()
        {
            testObject.LoadCustomsItems(shipment, false, adapter);

            Assert.True(shipment.CustomsGenerated);
            Assert.Empty(shipment.CustomsItems);
        }

        [Fact]
        public void LoadCustomsItems_SavesCustomsItems_FromOrderItems()
        {
            Modify.Order(context.Order)
                .WithItem(i =>
                {
                    i.Set(x => x.Name, "Foo");
                    i.Set(x => x.UnitPrice, 1.8M);
                    i.Set(x => x.Quantity, 3);
                    i.Set(x => x.Weight, 2.5);
                    i.WithItemAttribute(ia =>
                    {
                        ia.Set(x => x.Name, "ItemAttr1.1");
                        ia.Set(x => x.UnitPrice, 1.23M);
                    });
                    i.WithItemAttribute(ia =>
                    {
                        ia.Set(x => x.Name, "ItemAttr1.2");
                        ia.Set(x => x.UnitPrice, 2.59M);
                    });
                })
                .WithItem(i =>
                {
                    i.Set(x => x.Name, "Bar");
                    i.Set(x => x.UnitPrice, 2.2M);
                    i.Set(x => x.Quantity, 2);
                    i.Set(x => x.Weight, 0.6);
                    i.WithItemAttribute(ia =>
                    {
                        ia.Set(x => x.Name, "ItemAttr2.1");
                        ia.Set(x => x.UnitPrice, 3.96M);
                    });
                    i.WithItemAttribute(ia =>
                    {
                        ia.Set(x => x.Name, "ItemAttr2.2");
                        ia.Set(x => x.UnitPrice, 6.45M);
                    });
                })
                .Set(x => x.ShipCountryCode, "UK").Save();

            using (SqlAdapter transactedAdapter = new SqlAdapter(true))
            {
                testObject.LoadCustomsItems(shipment, false, transactedAdapter);
                transactedAdapter.Commit();
            }

            var predicate = new RelationPredicateBucket(ShipmentCustomsItemFields.ShipmentID == shipment.ShipmentID);
            using (EntityCollection<ShipmentCustomsItemEntity> customs = new EntityCollection<ShipmentCustomsItemEntity>())
            {
                adapter.FetchEntityCollection(customs, predicate);

                Assert.Equal(2, customs.Count);

                Assert.Equal("US", customs[0].CountryOfOrigin);
                Assert.Equal("Foo", customs[0].Description);
                Assert.Equal(3, customs[0].Quantity);
                Assert.Equal(2.5, customs[0].Weight);
                Assert.Equal(5.62M, customs[0].UnitValue);

                Assert.Equal("US", customs[1].CountryOfOrigin);
                Assert.Equal("Bar", customs[1].Description);
                Assert.Equal(2, customs[1].Quantity);
                Assert.Equal(0.6, customs[1].Weight);
                Assert.Equal(12.61M, customs[1].UnitValue);
            }
        }

        public void Dispose()
        {
            adapter.Dispose();
            lifetimeScope.Dispose();
            context.Dispose();
        }
    }
}
