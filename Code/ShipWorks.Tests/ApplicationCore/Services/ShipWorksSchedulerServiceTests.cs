using Xunit;
using Moq;
using ShipWorks.Actions.Scheduling;
using ShipWorks.ApplicationCore.Services.Fakes;


namespace ShipWorks.ApplicationCore.Services.Tests
{
    public class ShipWorksSchedulerServiceTests
    {
        Mock<IScheduler> scheduler;
        FakeShipWorksSchedulerService target;

        [TestInitialize]
        public void Initialize()
        {
            scheduler = new Mock<IScheduler>();
            target = new FakeShipWorksSchedulerService(scheduler.Object);
        }

        [Fact]
        public void CanBeStopped()
        {
            // This test is no longer working due to not able to get a SW InstanceID...
            // So ignoring for now.
            Assert.IsTrue(target.CanStop);
        }
    }
}
