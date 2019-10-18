using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Caching;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Tests.Data.Caching
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class EntityRelationCacheTest
    {
        private readonly DataContext context;
        private readonly Dictionary<EntityType, EntityTypeChangeVersion> entityTypeChangeVersions;
        private readonly EntityRelationCache relationCache;
        private readonly AutoMock mock;

        // The entity types we support caching of and monitor for changes
        private readonly EntityType[] changeMonitoredEntityTypes = {
            EntityType.CustomerEntity,
            EntityType.OrderEntity,
            EntityType.OrderItemEntity,
            EntityType.OrderItemAttributeEntity,
            EntityType.OrderChargeEntity,
            EntityType.OrderPaymentDetailEntity,
            EntityType.ShipmentEntity,
            EntityType.ShipmentCustomsItemEntity,
            EntityType.PrintResultEntity,
            EntityType.NoteEntity,
            EntityType.EmailOutboundEntity,
            EntityType.StoreEntity,
            EntityType.ServiceStatusEntity
        };

        public EntityRelationCacheTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
            entityTypeChangeVersions = changeMonitoredEntityTypes.ToDictionary(e => e, e => new EntityTypeChangeVersion(e));
            relationCache = new EntityRelationCache(new EntityCache(changeMonitoredEntityTypes), new TestExecutionMode());
            mock = context.Mock;

            Modify.Order(context.Order).WithItem().WithItem().WithShipment().Save();
        }

        [Fact]
        public void GetRelatedKeys_ReturnsCorrectValues_ForCustomerToOrderItem()
        {
            int createAdapterCount = 0;
            var ids = new List<long>() { context.Order.CustomerID };
            var keys = relationCache.GetRelatedKeys(ids, EntityType.OrderItemEntity, true, null);

            var orderItems = context.Order.OrderItems;
            Assert.True(orderItems.Select(oi => oi.OrderItemID).Except(keys).None());
            Assert.Equal(1, createAdapterCount);
        }

        [Fact]
        public void GetRelatedKeys_ReturnsCorrectValues_ForCustomerToShipment()
        {
            int createAdapterCount = 0;
            var ids = new List<long>() { context.Order.CustomerID };
            var keys = relationCache.GetRelatedKeys(ids, EntityType.ShipmentEntity, true, null);
            var shipments = context.Order.Shipments;
            Assert.True(shipments.Select(x => x.ShipmentID).Except(keys).None());
            Assert.Equal(1, createAdapterCount);
        }

        [Fact]
        public void GetRelatedKeys_Timing_ForCustomerToShipment()
        {
            int createAdapterCount = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            int times = 1000;
            for (int i = 0; i < 1000; i++)
            {
                var ids = new List<long>() { context.Order.CustomerID };
                var keys = relationCache.GetRelatedKeys(ids, EntityType.ShipmentEntity, true, null);
                var shipments = context.Order.Shipments;
                Assert.True(shipments.Select(x => x.ShipmentID).Except(keys).None());
                Assert.Equal(1, createAdapterCount);
            }

            sw.Stop();
            Debug.WriteLine($"Time to GetRelatedKeys {times}: {sw.ElapsedMilliseconds}");
        }

        [Fact]
        public void GetRelatedKeys_Timing_ForCustomerToOrderItem()
        {
            int createAdapterCount = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            int times = 1000;
            for (int i = 0; i < 1000; i++)
            {
                var ids = new List<long>() { context.Order.CustomerID };
                var keys = relationCache.GetRelatedKeys(ids, EntityType.OrderItemEntity, true, null);
                var orderItems = context.Order.OrderItems;
                Assert.True(orderItems.Select(oi => oi.OrderItemID).Except(keys).None());
                Assert.Equal(1, createAdapterCount);
            }

            sw.Stop();
            Debug.WriteLine($"Time to GetRelatedKeys {times}: {sw.ElapsedMilliseconds}");
        }

        private ISqlAdapter CreateAdapter(ref int counter)
        {
            counter++;
            return new SqlAdapter();
        }
    }
}
