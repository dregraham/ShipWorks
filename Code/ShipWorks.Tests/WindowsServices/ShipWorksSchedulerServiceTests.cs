using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Actions.Scheduling;
using System.Threading.Tasks;


namespace ShipWorks.ApplicationCore.WindowsServices.Tests
{
    [TestClass]
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

        [TestMethod]
        public void CanBeStopped()
        {
            Assert.IsTrue(target.CanStop);
        }
    }
}
