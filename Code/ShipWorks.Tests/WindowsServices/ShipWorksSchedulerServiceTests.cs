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
        public void OnStart_RunsTheScheduler()
        {
            target.OnStart(null);

            scheduler.Verify(x => x.RunAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public void CanBeStopped()
        {
            Assert.IsTrue(target.CanStop);
        }

        [TestMethod]
        public void OnStop_CancelsTheSchedulerTask()
        {
            var taskSource = new TaskCompletionSource<object>();

            scheduler.Setup(x => x.RunAsync(It.IsAny<CancellationToken>()))
                .Callback<CancellationToken>(token => {
                    token.Register(() => { taskSource.SetCanceled(); });
                })
                .Returns(taskSource.Task);

            target.OnStart(null);
            target.OnStop();

            Assert.IsTrue(taskSource.Task.IsCanceled);
        }
    }
}
