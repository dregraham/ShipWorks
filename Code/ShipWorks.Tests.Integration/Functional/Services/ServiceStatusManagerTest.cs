using System;
using System.Linq;
using Moq;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.ApplicationCore.Services;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.Linq;
using ShipWorks.Startup;
using Xunit;

namespace ShipWorks.Tests.Integration.MSTest.Functional.Services
{
    public class ServiceStatusManagerTest
    {
        private readonly Mock<ExecutionMode> executionMode;
        private Mock<IProgressReporter> progressReporter;

        public ServiceStatusManagerTest()
        {
            ContainerInitializer.Initialize();

            executionMode = new Mock<ExecutionMode>();
            executionMode.Setup(m => m.IsUISupported).Returns(true);

            progressReporter = new Mock<IProgressReporter>();

            var initializer = new ShipWorksInitializer(executionMode.Object, null);
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
    }
}
