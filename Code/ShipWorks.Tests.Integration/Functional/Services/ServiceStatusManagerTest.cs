using System;
using System.Linq;
using ShipWorks.ApplicationCore.Services;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.Linq;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.Database;
using Xunit;

namespace ShipWorks.Tests.Integration.MSTest.Functional.Services
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class ServiceStatusManagerTest : IDisposable
    {
        private readonly DataContext context;

        public ServiceStatusManagerTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
        }

        [Fact]
        [Trait("Category", "ApplicationCore")]
        [Trait("Category", "ContinuousIntegration")]
        public void CheckIn_DoesNotThrowORMEntityOutOfSyncException_WhenDatabaseValuesHaveChanged()
        {
            ServiceStatusEntity firstServiceStatus = null;

            using (SqlAdapter adapter = SqlAdapter.Default)
            {
                firstServiceStatus = new LinqMetaData(adapter).ServiceStatus.FirstOrDefault();
            }

            if (firstServiceStatus == null)
            {
                Assert.False(true, "Could not retrieve a service status");
            }

            // Imitate something else modifying the service status without refetching it
            firstServiceStatus.LastStartDateTime = DateTime.Now.Subtract(new TimeSpan(0, 0, 0, 15));
            SqlAdapter.Default.SaveEntity(firstServiceStatus);

            ServiceStatusManager.CheckIn(firstServiceStatus);
        }

        public void Dispose() => context.Dispose();
    }
}
