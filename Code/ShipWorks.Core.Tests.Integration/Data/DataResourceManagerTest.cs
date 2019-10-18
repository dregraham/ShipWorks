using System;
using System.Collections;
using System.Data.HashFunction.CityHash;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Security.Cryptography;
using System.Text;
using Autofac;
using Interapptive.Shared.Messaging;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using ShipWorks.Users;
using ShipWorks.Users.Security;
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
        public void CreateFromText_AddsResource_WithNewHash_WhenResourceDoesAlreadyExist()
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
            
            // This will create the resource using CityHash, so we'll need to compute the Sha256 hash
            // and save it to the resource
            var resourceRef = DataResourceManager.CreateFromText(textToHash, shipmentID);
            var shaHash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(textToHash));

            ResourceEntity resource = new ResourceEntity(resourceRef.ResourceID);
            SqlAdapter.Default.FetchEntity(resource);
            resource.Checksum = shaHash;
            SqlAdapter.Default.SaveEntity(resource);

            // Now we create it again.  It should find it and NOT create a new resource entity
            resourceRef = DataResourceManager.CreateFromText(textToHash, shipmentID);
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
        }

        [Fact]
        public void HashTimings_AllCityHash()
        {
            //var textToHash = RandomString(35000); // "It's some text yo!";
            //var bytes = Encoding.UTF8.GetBytes(textToHash);
            ISqlAdapter adapter = SqlAdapter.Default;

            //CreateCityHashResource(textToHash, 1031);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < 1000; i++)
            {
                var textToHash = RandomString(35000); // "It's some text yo!";
                var bytes = Encoding.UTF8.GetBytes(textToHash);
                CreateCityHashResource(textToHash, 1031);
                BuildResourceEntityShell(bytes, adapter, null);
            }

            sw.Stop();
            Debug.WriteLine($"Time to run when all CityHash: {sw.ElapsedMilliseconds}");
        }

        [Fact]
        public void HashTimings_FirstAsSha256()
        {
            ISqlAdapter adapter = SqlAdapter.Default;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < 10; i++)
            {
                var textToHash = RandomString(35000); // "It's some text yo!";
                //var bytes = Encoding.UTF8.GetBytes(textToHash);
                //CreateSha256Resource(textToHash, 1031);
                //BuildResourceEntityShell_Sha256(bytes, adapter, null);
                long consumerId = (i * 1000) + 31;
                byte[] bytes = RandomBytes(35000);
                DataResourceManager.CreateFromBytes(bytes, consumerId, "fake", true);
                DataResourceManager.CreateFromBytes(bytes, consumerId, "fake", true);
                DataResourceManager.CreateFromBytes(bytes, consumerId, "fake");
            }

            sw.Stop();
            Debug.WriteLine($"Time to run when all Sha256: {sw.ElapsedMilliseconds}");
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

        private void CreateCityHashResource(string textToHash, long consumerID)
        {
            var bytes = Encoding.UTF8.GetBytes(textToHash);
            var resourceRef = DataResourceManager.CreateFromText(textToHash, consumerID);
        }

        /// <summary>
        /// Find a resource by data
        /// </summary>
        private static ResourceEntity BuildResourceEntityShell(byte[] data, ISqlAdapter adapter, byte[] hash)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (hash != null)
            {
                return new ResourceEntity
                {
                    Data = data,
                    Checksum = hash
                };
            }

            byte[] cityChecksum = GetHash(data, false);

            // See if we can find an existing resource
            ResourceCollection resources = new ResourceCollection();
            adapter.FetchEntityCollection(resources, new RelationPredicateBucket(ResourceFields.Checksum == (object) cityChecksum), 1, null, null, excludeDataFields);

            if (resources.Any())
            {
                return resources[0];
            }

            byte[] oldChecksum = GetHash(data, true);
            adapter.FetchEntityCollection(resources, new RelationPredicateBucket(ResourceFields.Checksum == (object) oldChecksum), 1, null, null, excludeDataFields);

            if (resources.Any())
            {
                return resources[0];
            }

            hash = cityChecksum;

            // Doesn't exist, create it
            return new ResourceEntity
            {
                Data = data,
                Checksum = hash
            };
        }

        /// <summary>
        /// Find a resource by data
        /// </summary>
        private static ResourceEntity BuildResourceEntityShell_Sha256(byte[] data, ISqlAdapter adapter, byte[] hash)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (hash != null)
            {
                return new ResourceEntity
                {
                    Data = data,
                    Checksum = hash
                };
            }

            byte[] checksum = GetHash(data, true);

            // See if we can find an existing resource
            ResourceCollection resources = new ResourceCollection();
            adapter.FetchEntityCollection(resources, new RelationPredicateBucket(ResourceFields.Checksum == (object) checksum), 1, null, null, excludeDataFields);

            if (resources.Any())
            {
                return resources[0];
            }

            // Doesn't exist, create it
            return new ResourceEntity
            {
                Data = data,
                Checksum = hash
            };
        }

        /// <summary>
        /// Get data hash
        /// </summary>
        private static byte[] GetHash(byte[] data, bool legacyHashMethod)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            byte[] hash;

            if (legacyHashMethod)
            {
                hash = SHA256.Create().ComputeHash(data);
            }
            else
            {
                ICityHash hasher = CityHashFactory.Instance.Create(new CityHashConfig() { HashSizeInBits = 128 });
                hash = hasher.ComputeHash(data).Hash;
            }

            return hash;
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
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
