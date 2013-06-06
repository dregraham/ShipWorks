using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Actions.Scheduling.QuartzNet;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Threading;
using Quartz.Impl;
using Quartz;


namespace ShipWorks.Tests.Actions.Scheduling.QuartzNet
{
    [TestClass]
    public class QuartzSchedulingEngineTest
    {
        Mock<Quartz.IScheduler> scheduler;
        QuartzSchedulingEngine testObject;

        [TestInitialize]
        public void Initialize()
        {
            scheduler = new Mock<Quartz.IScheduler>();

            var schedulerFactory = new Mock<Quartz.ISchedulerFactory>();
            schedulerFactory.Setup(x => x.GetScheduler()).Returns(scheduler.Object);

            testObject = new QuartzSchedulingEngine(schedulerFactory.Object);
        }


        [TestMethod]
        public void Schedule_DelegatesToScheduler_Test()
        {
            scheduler.Setup(s => s.ScheduleJob(It.IsAny<IJobDetail>(), It.IsAny<ITrigger>()));
            
            ActionEntity action = new ActionEntity { ActionID = 1 };
            CronTrigger trigger = new CronTrigger { StartDateTimeInUtc = DateTime.UtcNow };

            // This will obviously need to change as the Quartz scheduling engine gets implemented
            testObject.Schedule(action, trigger);

            scheduler.Verify(s => s.ScheduleJob(It.IsAny<IJobDetail>(), It.IsAny<ISimpleTrigger>()), Times.Once());
        }
        
        [TestMethod]
        public void IsExistingJob_DelegatesToScheduler_Test()
        {
            ActionEntity action = new ActionEntity { ActionID = 1 };
            CronTrigger trigger = new CronTrigger { StartDateTimeInUtc = DateTime.UtcNow };

            scheduler.Setup(s => s.GetJobDetail(It.IsAny<JobKey>())).Returns<IJobDetail>(null);

            testObject.IsExistingJob(action, trigger);

            scheduler.Verify(s => s.GetJobDetail(It.IsAny<JobKey>()), Times.Once());
        }

        [TestMethod]
        public void IsExistingJob_ReturnsFalse_WhenSchedulerReturnsNull_Test()
        {
            scheduler.Setup(s => s.GetJobDetail(It.IsAny<JobKey>())).Returns<IJobDetail>(null);

            Assert.IsFalse(testObject.IsExistingJob(new ActionEntity(), new CronTrigger()));
        }

        [TestMethod]
        public void IsExistingJob_ReturnsTrue_WhenReturnsNonNull_Test()
        {
            scheduler.Setup(s => s.GetJobDetail(It.IsAny<JobKey>())).Returns(new JobDetailImpl());

            ActionEntity action = new ActionEntity { ActionID = 1 };
            CronTrigger trigger = new CronTrigger { StartDateTimeInUtc = DateTime.UtcNow };

            testObject.Schedule(action, trigger);
            Assert.IsTrue(testObject.IsExistingJob(action, trigger));
        }

        [TestMethod]
        public void CanStartScheduler()
        {
            testObject.RunAsync(CancellationToken.None);

            scheduler.Verify(x => x.Start(), Times.Once());
        }

        [TestMethod]
        public void CanStopScheduler()
        {
            var canceller = new CancellationTokenSource();

            var task = testObject.RunAsync(canceller.Token);

            canceller.Cancel();

            scheduler.Verify(x => x.Shutdown(true), Times.Once());

            Assert.IsTrue(task.IsCanceled);
        }
    }
}
