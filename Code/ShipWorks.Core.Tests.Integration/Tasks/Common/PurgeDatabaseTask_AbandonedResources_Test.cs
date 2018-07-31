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
        private long nextConsumerID = 0;

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
            var resource1 = CreateValidResource();
            var resource2 = CreateValidResource();
            var resource3 = CreateAbandonedResource();
            var resource4 = CreateAbandonedResource();

            testObject.Run(new List<long>(), null);

            AssertResourceExists(resource1);
            AssertResourceExists(resource2);
            AssertResourceDoesNotExists(resource3);
            AssertResourceDoesNotExists(resource4);
        }

        [Fact]
        public async Task PurgeDatabaseTask_PurgesAllResources_WhenAllResourcesAreAbandoned()
        {
            var resource1 = CreateAbandonedResource();
            var resource2 = CreateAbandonedResource();
            var resource3 = CreateAbandonedResource();
            var resource4 = CreateAbandonedResource();

            testObject.Run(new List<long>(), null);

            AssertResourceDoesNotExists(resource1);
            AssertResourceDoesNotExists(resource2);
            AssertResourceDoesNotExists(resource3);
            AssertResourceDoesNotExists(resource4);
        }

        [Fact]
        public async Task PurgeDatabaseTask_PurgesNoAbandonedResources_WhenNoAbandonedResourcesAreOlderThan180Days()
        {
            var resource1 = CreateValidResource();
            var resource2 = CreateValidResource();
            var resource3 = CreateValidResource();
            var resource4 = CreateValidResource();

            testObject.Run(new List<long>(), null);

            AssertResourceExists(resource1);
            AssertResourceExists(resource2);
            AssertResourceExists(resource3);
            AssertResourceExists(resource4);
        }

        private static void AssertResourceExists(ResourceEntity resource)
        {
            using (var sqlAdapter = SqlAdapter.Create(false))
            {
                var factory = new QueryFactory();

                var resourceQuery = factory.Resource.Where(ResourceFields.ResourceID == resource.ResourceID);
                var entity = sqlAdapter.FetchFirst(resourceQuery);
                Assert.NotNull(entity);
            }
        }

        private static void AssertResourceDoesNotExists(ResourceEntity resource)
        {
            using (var sqlAdapter = SqlAdapter.Create(false))
            {
                var factory = new QueryFactory();

                var resourceQuery = factory.Resource.Where(ResourceFields.ResourceID == resource.ResourceID);
                var entity = sqlAdapter.FetchFirst(resourceQuery);
                Assert.Null(entity);
            }
        }

        private ResourceEntity CreateAbandonedResource() =>
            Create.Entity<ResourceEntity>()
                .Set(x => x.Checksum = CreateChecksum())
                .Set(x => x.Filename, Path.GetRandomFileName())
                .Save();

        private ResourceEntity CreateValidResource()
        {
            nextConsumerID += 1;
            var resource = CreateAbandonedResource();

            Create.Entity<ObjectReferenceEntity>()
                .Set(x => x.EntityID, resource.ResourceID)
                .Set(x => x.ConsumerID, nextConsumerID)
                .Save();

            return resource;
        }

        private byte[] CreateChecksum() =>
            Enumerable.Range(1, 30).Select(_ => (byte) random.Next(0, 255)).ToArray();
    }
}
