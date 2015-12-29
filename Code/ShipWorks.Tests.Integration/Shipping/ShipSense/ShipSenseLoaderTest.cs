using System;
using System.Diagnostics;
using Moq;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.Common.Threading;
using ShipWorks.Shipping.ShipSense;
using ShipWorks.Shipping.ShipSense.Population;
using Xunit;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.ShipSense
{
    public class ShipSenseLoaderTest
    {
        private ShipSenseLoader testObject;

        private readonly Mock<ExecutionMode> executionMode;
        private readonly Mock<IProgressReporter> progressReporter;

        public ShipSenseLoaderTest()
        {
            executionMode = new Mock<ExecutionMode>();
            executionMode.Setup(m => m.IsUISupported).Returns(true);

            progressReporter = new Mock<IProgressReporter>();

            var initializer = new ShipWorksInitializer(executionMode.Object, null);
        }

        [Fact]
        [Trait("Category", "ShipSense")]
        [Trait("Category", "ContinuousIntegration")]
        public void LoadData_WithSeededDatabase_CompletesInFiveSecondsOrLess_Test()
        {
            // This assumes it is being run against the "seeded" database (see SeedDatabase.sql script
            // in solution directory)
            Stopwatch stopWatch = new Stopwatch();
            using (ShipSenseLoaderGateway gateway = new ShipSenseLoaderGateway(new Knowledgebase()))
            {
                testObject = new ShipSenseLoader(progressReporter.Object, gateway);

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
