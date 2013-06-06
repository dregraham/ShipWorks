using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Actions.Scheduling.QuartzNet;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Threading;


namespace ShipWorks.Tests.Actions.Scheduling.QuartzNet
{
    [TestClass]
    public class QuartzSchedulingEngineTest
    {
        Mock<Quartz.IScheduler> scheduler;
        QuartzSchedulingEngine target;

        [TestInitialize]
        public void Initialize()
        {
            scheduler = new Mock<Quartz.IScheduler>();
            target = new QuartzSchedulingEngine(scheduler.Object);
        }


        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Schedule_ThrowsNotImplementedException_Test()
        {
            // This will obviously need to change as the Quartz scheduling engine gets implemented
            target.Schedule(new ActionEntity(), new CronTrigger());
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void IsExistingJob_ThrowsNotImplementedException_Test()
        {
            // This will obviously need to change as the Quartz scheduling engine gets implemented
            target.IsExistingJob(new ActionEntity(), new CronTrigger());
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void GetTrigger_ThrowsNotImplementedException_Test()
        {
            // This will obviously need to change as the Quartz scheduling engine gets implemented
            target.GetTrigger(new ActionEntity());
        }


        [TestMethod]
        public void CanStartScheduler()
        {
            target.RunAsync(CancellationToken.None);

            scheduler.Verify(x => x.Start(), Times.Once());
        }

        [TestMethod]
        public void CanStopScheduler()
        {
            var canceller = new CancellationTokenSource();

            var task = target.RunAsync(canceller.Token);

            canceller.Cancel();

            scheduler.Verify(x => x.Shutdown(true), Times.Once());

            Assert.IsTrue(task.IsCanceled);
        }
    }
}
