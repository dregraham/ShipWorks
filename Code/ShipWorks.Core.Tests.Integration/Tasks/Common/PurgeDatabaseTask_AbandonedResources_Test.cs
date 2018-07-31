using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.UI;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Tasks.Common
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class PurgeDatabaseTask_AbandonedResources_Test
    {
        private readonly DataContext context;
        private readonly PurgeDatabaseTask testObject;
        private readonly Random random = new Random();

        public PurgeDatabaseTask_AbandonedResources_Test(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                mock.Override<IMessageHelper>();
            });

            testObject = context.Mock.Create<PurgeDatabaseTask>();
            testObject.Purges.Add(PurgeDatabaseType.Downloads); // This will auto-include abandoned resources since we can't select it directly
        }

        [Fact]
        public async Task PurgeDatabaseTask_PurgesTwoResources_WhenHalfOfResourcesAreAbandoned()
        {
            var resource1 = CreateValidResource(1);
            var resource2 = CreateValidResource(2);
            var resource3 = CreateAbandonedResource();
            var resource4 = CreateAbandonedResource();

            testObject.Run(new List<long>(), null);

            Assert.NotNull(FetchResource(resource1));
            Assert.NotNull(FetchResource(resource2));
            Assert.Null(FetchResource(resource3));
            Assert.Null(FetchResource(resource4));
        }

        [Fact]
        public async Task PurgeDatabaseTask_PurgesAllResources_WhenAllResourcesAreAbandoned()
        {
            var resource1 = CreateAbandonedResource();
            var resource2 = CreateAbandonedResource();
            var resource3 = CreateAbandonedResource();
            var resource4 = CreateAbandonedResource();

            testObject.Run(new List<long>(), null);

            Assert.Null(FetchResource(resource1));
            Assert.Null(FetchResource(resource2));
            Assert.Null(FetchResource(resource3));
            Assert.Null(FetchResource(resource4));
        }

        [Fact]
        public async Task PurgeDatabaseTask_PurgesNoAbandonedResources_WhenNoAbandonedResourcesAreOlderThan180Days()
        {
            var resource1 = CreateValidResource(1);
            var resource2 = CreateValidResource(2);
            var resource3 = CreateValidResource(3);
            var resource4 = CreateValidResource(4);

            testObject.Run(new List<long>(), null);

            Assert.NotNull(FetchResource(resource1));
            Assert.NotNull(FetchResource(resource2));
            Assert.NotNull(FetchResource(resource3));
            Assert.NotNull(FetchResource(resource4));
        }

        [Fact]
        public void PurgeDatabaseTask_DoesNotRemoveObjectLabel_WhenIsDeletedIsFalse()
        {
            var label = CreateObjectLabel(1);

            testObject.Run(new List<long>(), null);

            Assert.NotNull(FetchObjectLabel(label));
        }

        [Fact]
        public void PurgeDatabaseTask_DoesNotRemoveObjectLabel_WhenIsDeletedButIsAnAuditChange()
        {
            var audit = Create.Audit(context.User, context.Computer)
                .WithChange(c => c.Set(x => x.EntityID, 1))
                .Save();
            var label = CreateObjectLabel(1, deleted: true);

            testObject.Run(new List<long>(), null);

            Assert.NotNull(FetchObjectLabel(label));
        }

        [Fact]
        public void PurgeDatabaseTask_DoesNotRemoveObjectLabel_WhenIsDeletedButItsParentIsAnAuditChange()
        {
            var audit = Create.Audit(context.User, context.Computer)
                .WithChange(c => c.Set(x => x.EntityID, 1))
                .WithChange(c => c.Set(x => x.EntityID, 6))
                .Save();
            var label = CreateObjectLabel(1, deleted: true, parentID: 6);

            testObject.Run(new List<long>(), null);

            Assert.NotNull(FetchObjectLabel(label));
        }

        [Fact]
        public void PurgeDatabaseTask_RemovesObjectLabel_WhenIsDeletedAndIsNotPartOfAnAuditChange()
        {
            var label = CreateObjectLabel(1, deleted: true);

            testObject.Run(new List<long>(), null);

            Assert.Null(FetchObjectLabel(label));
        }

        private static ResourceEntity FetchResource(IResourceEntity resource)
        {
            using (var sqlAdapter = SqlAdapter.Create(false))
            {
                var factory = new QueryFactory();

                var resourceQuery = factory.Resource.Where(ResourceFields.ResourceID == resource.ResourceID);
                return sqlAdapter.FetchFirst(resourceQuery);
            }
        }

        private static ObjectLabelEntity FetchObjectLabel(IObjectLabelEntity label)
        {
            using (var sqlAdapter = SqlAdapter.Create(false))
            {
                var factory = new QueryFactory();

                var labelQuery = factory.ObjectLabel.Where(ObjectLabelFields.EntityID == label.EntityID);
                return sqlAdapter.FetchFirst(labelQuery);
            }
        }

        private ObjectLabelEntity CreateObjectLabel(long objectID, bool deleted = false, long? parentID = null) =>
            Create.Entity<ObjectLabelEntity>()
                .Set(x => x.EntityID, objectID)
                .Set(x => x.IsDeleted, deleted)
                .Set(x => x.ParentID, parentID)
                .Save();

        private ResourceEntity CreateAbandonedResource() =>
            Create.Entity<ResourceEntity>()
                .Set(x => x.Checksum = CreateChecksum())
                .Set(x => x.Filename, Path.GetRandomFileName())
                .Save();

        private ResourceEntity CreateValidResource(long consumerID)
        {
            var resource = CreateAbandonedResource();

            Create.Entity<ObjectReferenceEntity>()
                .Set(x => x.EntityID, resource.ResourceID)
                .Set(x => x.ConsumerID, consumerID)
                .Save();

            return resource;
        }

        private byte[] CreateChecksum() =>
            Enumerable.Range(1, 30).Select(_ => (byte) random.Next(0, 255)).ToArray();
    }
}
