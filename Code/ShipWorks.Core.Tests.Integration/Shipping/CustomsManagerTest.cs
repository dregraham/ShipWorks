using System;
using Autofac;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Other;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Shipping
{
    [Collection("Database collection")]
    public class CustomsManagerTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ShipmentEntity shipment;

        public CustomsManagerTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));

            shipment = Create.Shipment(context.Order)
                .AsOther()
                .Set(x => x.ShipCountryCode, "UK")
                .Save();
        }

        [Fact]
        public void LoadCustomsItems_DelegatesToShipmentType_ToCheckIfCustomsIsRequired()
        {
            var shipmentType = context.Mock.CreateMock<OtherShipmentType>();
            shipmentType.CallBase = true;

            context.Mock.AddRegistration(x =>
                x.RegisterInstance(shipmentType.Object).Keyed<ShipmentType>(ShipmentTypeCode.Other));
            
            CustomsManager.LoadCustomsItems(shipment, false);

            shipmentType.Verify(x => x.IsCustomsRequired(shipment));
        }

        [Fact]
        public void LoadCustomsItems_LoadsCustoms_WhenReloadIsFalseButCustomsAreNotLoaded()
        {
            Modify.Shipment(shipment).Set(x => x.CustomsGenerated, true).Save();
            Create.Entity<ShipmentCustomsItemEntity>().Set(x => x.ShipmentID, shipment.ShipmentID).Save();

            CustomsManager.LoadCustomsItems(shipment, false);

            Assert.NotEmpty(shipment.CustomsItems);
            Assert.True(shipment.CustomsItemsLoaded);
        }

        [Fact]
        public void LoadCustomsItems_LoadsCustoms_WhenCustomsIsCreatedAndReloadIsTrue()
        {
            Modify.Shipment(shipment).Set(x => x.CustomsGenerated, true).Save();
            Create.Entity<ShipmentCustomsItemEntity>().Set(x => x.ShipmentID, shipment.ShipmentID).Save();

            CustomsManager.LoadCustomsItems(shipment, true);

            Assert.NotEmpty(shipment.CustomsItems);
            Assert.True(shipment.CustomsItemsLoaded);
        }

        [Fact]
        public void LoadCustomsItems_ReloadsCustoms_WhenCustomsIsCreatedAndLoadedButReloadIsTrue()
        {
            Modify.Shipment(shipment).Set(x => x.CustomsGenerated, true).Save();
            Create.Entity<ShipmentCustomsItemEntity>().Set(x => x.ShipmentID, shipment.ShipmentID).Save();

            shipment.CustomsItemsLoaded = true;

            CustomsManager.LoadCustomsItems(shipment, true);

            Assert.NotEmpty(shipment.CustomsItems);
            Assert.True(shipment.CustomsItemsLoaded);
        }

        [Fact]
        public void LoadCustomsItems_DoesNotLoadCustoms_WhenCustomsIsLoadedAndReloadIsFalse()
        {
            Modify.Shipment(shipment).Set(x => x.CustomsGenerated, true).Save();
            Create.Entity<ShipmentCustomsItemEntity>().Set(x => x.ShipmentID, shipment.ShipmentID).Save();

            shipment.CustomsItemsLoaded = true;

            CustomsManager.LoadCustomsItems(shipment, false);

            Assert.Empty(shipment.CustomsItems);
            Assert.True(shipment.CustomsItemsLoaded);
        }

        [Fact]
        public void LoadCustomsItems_DoesNotCreateCustoms_WhenShipmentIsProcessed()
        {
            shipment.Processed = true;

            CustomsManager.LoadCustomsItems(shipment, false);

            Assert.False(shipment.CustomsGenerated);
            Assert.Empty(shipment.CustomsItems);
        }

        [Fact]
        public void LoadCustomsItems_MarksCustomsAsGenerated_WhenShipmentIsNotProcessedAndNotAlreadyGenerated()
        {
            CustomsManager.LoadCustomsItems(shipment, false);

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
                })
                .WithItem(i =>
                {
                    i.Set(x => x.Name, "Bar");
                    i.Set(x => x.UnitPrice, 2.2M);
                    i.Set(x => x.Quantity, 2);
                    i.Set(x => x.Weight, 0.6);
                })
                .Set(x => x.ShipCountryCode, "UK").Save();

            CustomsManager.LoadCustomsItems(shipment, false);

            using (SqlAdapter adapter = SqlAdapter.Create(false))
            {
                var predicate = new RelationPredicateBucket(ShipmentCustomsItemFields.ShipmentID == shipment.ShipmentID);
                using (EntityCollection<ShipmentCustomsItemEntity> customs = new EntityCollection<ShipmentCustomsItemEntity>())
                {
                    adapter.FetchEntityCollection(customs, predicate);

                    Assert.Equal(2, customs.Count);

                    Assert.Equal("US", customs[0].CountryOfOrigin);
                    Assert.Equal("Foo", customs[0].Description);
                    Assert.Equal(3, customs[0].Quantity);
                    Assert.Equal(2.5, customs[0].Weight);
                    Assert.Equal(1.8M, customs[0].UnitValue);

                    Assert.Equal("US", customs[1].CountryOfOrigin);
                    Assert.Equal("Bar", customs[1].Description);
                    Assert.Equal(2, customs[1].Quantity);
                    Assert.Equal(0.6, customs[1].Weight);
                    Assert.Equal(2.2M, customs[1].UnitValue);
                }
            }
        }

        public void Dispose() => context.Dispose();
    }
}
