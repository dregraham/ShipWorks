using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autofac.Extras.Moq;
using Castle.Components.DictionaryAdapter;
using Interapptive.Shared.Collections;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.Data;
using ShipWorks.Data.Caching;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Utility;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Data.Caching
{
    public class EntityRelationCacheTest
    {
        private readonly AutoMock mock;

        public EntityRelationCacheTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void FindRelationChain_ReturnsCorrectValues_WhenAllowOneToMany_IsTrue()
        {

            // The entity types we support caching of and monitor for changes
            EntityType[] changeMonitoredEntityTypes = {
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


            var entityTypeChangeVersions = changeMonitoredEntityTypes.ToDictionary(e => e, e => new EntityTypeChangeVersion(e));

            var entityCache = new EntityCache(changeMonitoredEntityTypes);
            var relationCache = new EntityRelationCache(entityCache, new TestExecutionMode());

            var ids = new List<long>() { 1012 };
            int createAdapterCount = 0;

            relationCache.GetRelatedKeys(ids, EntityType.OrderItemEntity, true, null);

            Assert.Equal(1, createAdapterCount);


        }

        private ISqlAdapter CreateAdapter(ref int counter)
        {
            counter++;
            return mock.Build<ISqlAdapter>();
        }
    }
}
