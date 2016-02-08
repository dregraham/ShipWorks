using System;
using System.Diagnostics;
using Moq;
using ShipWorks.Common.Threading;
using ShipWorks.Shipping.ShipSense;
using ShipWorks.Shipping.ShipSense.Population;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.Database;
using Xunit;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.ShipSense
{
    [Collection("Database collection")]
    public class ShipSenseLoaderTest
    {
        private ShipSenseLoader testObject;
        private readonly DataContext context;

        public ShipSenseLoaderTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
        }

        [Fact]
        [Trait("Category", "ShipSense")]
        [Trait("Category", "ContinuousIntegration")]
        public void LoadData_WithSeededDatabase_CompletesInFiveSecondsOrLess()
        {
            // This assumes it is being run against the "seeded" database (see SeedDatabase.sql script
            // in solution directory)
            Stopwatch stopWatch = new Stopwatch();
            using (ShipSenseLoaderGateway gateway = new ShipSenseLoaderGateway(new Knowledgebase()))
            {
                testObject = new ShipSenseLoader(new Mock<IProgressReporter>().Object, gateway);

                stopWatch.Start();
                testObject.LoadData();
                stopWatch.Stop();
            }

            Assert.True(stopWatch.ElapsedMilliseconds < 5000);

            Console.WriteLine(@"===================================================================================================");
            Console.WriteLine(@"====                           ShipSense Loader                                                ====");
            Console.WriteLine(@"===================================================================================================");
            Console.WriteLine(@"Elapsed time: {0} seconds", stopWatch.ElapsedMilliseconds / 1000.0M);
            Console.WriteLine(@"===================================================================================================");
        }
    }
}
