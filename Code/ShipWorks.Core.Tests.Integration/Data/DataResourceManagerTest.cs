using System;
using System.Collections;
using System.Data.HashFunction.CityHash;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Autofac;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using ShipWorks.Users;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Data
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class DataResourceManagerTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ILifetimeScope lifetimeScope;
        private static ExcludeIncludeFieldsList excludeDataFields = 
            new ExcludeIncludeFieldsList((IList) new IEntityFieldCore[] { ResourceFields.Data, ResourceFields.Checksum });
        private static Random random = new Random();

        public DataResourceManagerTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
            lifetimeScope = IoC.BeginLifetimeScope();

            string currentDirectory = Path.Combine(Directory.GetCurrentDirectory(), "results");
            UserSession.InitializeForCurrentDatabase(new TestExecutionMode());
            DataPath.Initialize(
                instancePath: Path.Combine(currentDirectory, "instance"),
                commonSettingsPath: Path.Combine(currentDirectory, "common"),
                tempPath: Path.Combine(currentDirectory, "temp")
            );

            Modify.Order(context.Order).WithShipment().Save();
        }

        [Fact]
        public void CreateFromText_AddsResource_WithNewHash_WhenResourceDoesntAlreadyExist()
        {
            long shipmentID = context.Order.Shipments.First().ShipmentID;
            string textToHash = "It's some text yo!";
            var resourceRef = DataResourceManager.CreateFromText(textToHash, shipmentID);

            ICityHash hasher = CityHashFactory.Instance.Create(new CityHashConfig() { HashSizeInBits = 128 });
            var hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(textToHash)).Hash;

            ResourceEntity resource = new ResourceEntity(resourceRef.ResourceID);
            SqlAdapter.Default.FetchEntity(resource);

            Assert.Equal(resource.Checksum.Take(16), hash);
        }

        [Fact]
        public void CreateFromText_FetchesResource_WithNewHash_WhenResourceDoesAlreadyExist()
        {
            long shipmentID = context.Order.Shipments.First().ShipmentID;
            string textToHash = "It's some text yo!";
            var resourceRef = DataResourceManager.CreateFromText(textToHash, shipmentID);

            ICityHash hasher = CityHashFactory.Instance.Create(new CityHashConfig() { HashSizeInBits = 128 });
            var hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(textToHash)).Hash;

            ResourceEntity resource = new ResourceEntity(resourceRef.ResourceID);
            SqlAdapter.Default.FetchEntity(resource);

            resourceRef = DataResourceManager.CreateFromText(textToHash, shipmentID);
            resource = new ResourceEntity(resourceRef.ResourceID);
            SqlAdapter.Default.FetchEntity(resource);

            Assert.Equal(resource.ResourceID, resourceRef.ResourceID);
            Assert.Equal(resource.Checksum.Take(16), hash);
        }

        [Fact]
        public void CreateFromText_FetchesResource_WhenHashIsSha256()
        {
            long shipmentID = context.Order.Shipments.First().ShipmentID;
            string textToHash = "It's some text yo!";

            var shaHash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(textToHash));
            ResourceEntity resource = CreateSha256Resource(textToHash, shipmentID);

            // Now we call create again.  It should find it and NOT create a new resource entity
            var resourceRef = DataResourceManager.CreateFromText(textToHash, shipmentID);
            resource = new ResourceEntity(resourceRef.ResourceID);
            SqlAdapter.Default.FetchEntity(resource);

            Assert.Equal(resource.ResourceID, resourceRef.ResourceID);
            Assert.Equal(resource.Checksum, shaHash);
        }

        [Fact]
        public void CreateFromBytes_CreatesOnFirstCall_FetchesOnSecond_WithNoError_WhenForcingCreate()
        {
            long consumerId = 1031;
            byte[] bytes = RandomBytes(35000);
            var dataResourceRef1 = DataResourceManager.CreateFromBytes(bytes, consumerId, "fake", true);
            var dataResourceRef2 = DataResourceManager.CreateFromBytes(bytes, consumerId, "fake", true);

            Assert.Equal(dataResourceRef1.ResourceID, dataResourceRef2.ResourceID);
            Assert.Equal(dataResourceRef1.Filename, dataResourceRef2.Filename);
        }

        [Fact]
        public void HashTimings_AllCityHash()
        {
            ISqlAdapter adapter = SqlAdapter.Default;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < 1000; i++)
            {
                long consumerId = (i * 1000) + 31;
                DataResourceManager.CreateFromBytes(RandomBytes(35000), consumerId, $"test{consumerId}", false);
            }

            sw.Stop();
            Debug.WriteLine($"Time to run when all CityHash: {sw.ElapsedMilliseconds}");
        }

        private ResourceEntity CreateSha256Resource(string textToHash, long consumerID)
        {
            var bytes = Encoding.UTF8.GetBytes(textToHash);
            var resourceRef = DataResourceManager.CreateFromText(textToHash, consumerID);
            var shaHash = SHA256.Create().ComputeHash(bytes);

            ResourceEntity resource = new ResourceEntity(resourceRef.ResourceID);
            SqlAdapter.Default.FetchEntity(resource);
            resource.Checksum = shaHash;
            SqlAdapter.Default.SaveEntity(resource);

            return resource;
        }

        public static byte[] RandomBytes(int length)
        {
            byte[] bytes = new byte[length];
            random.NextBytes(bytes);
            return bytes;
        }

        public void Dispose()
        {
            lifetimeScope.Dispose();
            context.Dispose();
        }
    }
}
