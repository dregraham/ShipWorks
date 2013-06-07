using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Quartz.Impl.Triggers;
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

            testObject.Schedule(action, trigger);

            scheduler.Verify(s => s.ScheduleJob(It.IsAny<IJobDetail>(), It.IsAny<ISimpleTrigger>()), Times.Once());
        }

        [TestMethod]
        public void Schedule_RemovesExistingJob_Test()
        {
            scheduler.Setup(s => s.ScheduleJob(It.IsAny<IJobDetail>(), It.IsAny<ITrigger>()));
            scheduler.Setup(s => s.GetJobDetail(It.IsAny<JobKey>())).Returns(new JobDetailImpl());
            
            // Setup methods used when removing
            scheduler.Setup(s => s.UnscheduleJob(It.IsAny<TriggerKey>()));
            scheduler.Setup(s => s.DeleteJob(It.IsAny<JobKey>()));
            scheduler.Setup(s => s.GetTriggersOfJob(It.IsAny<JobKey>())).Returns(new List<ITrigger>());

            ActionEntity action = new ActionEntity { ActionID = 1 };
            CronTrigger trigger = new CronTrigger { StartDateTimeInUtc = DateTime.UtcNow };

            testObject.Schedule(action, trigger);

            scheduler.Verify(s => s.DeleteJob(It.IsAny<JobKey>()), Times.Once());
        }

        [TestMethod]
        public void Schedule_DoesNotRemoveJob_WhenJobIsNew_Test()
        {
            // Setup to return null to simulate the job does not exist
            scheduler.Setup(s => s.ScheduleJob(It.IsAny<IJobDetail>(), It.IsAny<ITrigger>()));
            scheduler.Setup(s => s.GetJobDetail(It.IsAny<JobKey>())).Returns<IJobDetail>(null);
            
            ActionEntity action = new ActionEntity { ActionID = 1 };
            CronTrigger trigger = new CronTrigger { StartDateTimeInUtc = DateTime.UtcNow };
            
            testObject.Schedule(action, trigger);

            scheduler.Verify(s => s.DeleteJob(It.IsAny<JobKey>()), Times.Never());
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

            Assert.IsTrue(testObject.IsExistingJob(action, trigger));
        }

        [TestMethod]
        public void Unschedule_DelegatesToSchedulerForTriggers_Test()
        {
            scheduler.Setup(s => s.UnscheduleJob(It.IsAny<TriggerKey>()));
            scheduler.Setup(s => s.DeleteJob(It.IsAny<JobKey>()));
            scheduler.Setup(s => s.GetTriggersOfJob(It.IsAny<JobKey>()))
                     .Returns
                     (
                        new List<ITrigger>
                        {
                            new SimpleTriggerImpl { Key = new TriggerKey("someKey") }
                        }
                     );

            testObject.Unschedule(new ActionEntity { ActionID = 1 });
            
            scheduler.Verify(s => s.GetTriggersOfJob(It.IsAny<JobKey>()), Times.Once());
        }

        [TestMethod]
        public void Unschedule_DoesNotDelegateToSchedulerForTriggers_WhenActionIsNull_Test()
        {
            scheduler.Setup(s => s.UnscheduleJob(It.IsAny<TriggerKey>()));
            scheduler.Setup(s => s.DeleteJob(It.IsAny<JobKey>()));
            scheduler.Setup(s => s.GetTriggersOfJob(It.IsAny<JobKey>()))
                     .Returns
                     (
                        new List<ITrigger>
                        {
                            new SimpleTriggerImpl { Key = new TriggerKey("someKey") }
                        }
                     );

            testObject.Unschedule(null);

            scheduler.Verify(s => s.GetTriggersOfJob(It.IsAny<JobKey>()), Times.Never());
        }

        [TestMethod]
        public void Unschedule_DelegatesToSchedulerToUnscheduleJob_WithOneTrigger_Test()
        {
            scheduler.Setup(s => s.UnscheduleJob(It.IsAny<TriggerKey>()));
            scheduler.Setup(s => s.DeleteJob(It.IsAny<JobKey>()));
            scheduler.Setup(s => s.GetTriggersOfJob(It.IsAny<JobKey>()))
                     .Returns
                     (
                        new List<ITrigger>
                        {
                            new SimpleTriggerImpl { Key = new TriggerKey("someKey") }
                        }
                     );

            testObject.Unschedule(new ActionEntity { ActionID = 1 });
            
            scheduler.Verify(s => s.UnscheduleJob(It.IsAny<TriggerKey>()), Times.Once());
        }

        [TestMethod]
        public void Unschedule_DelegatesToSchedulerToUnscheduleJob_WithMultipleTriggers_Test()
        {
            scheduler.Setup(s => s.UnscheduleJob(It.IsAny<TriggerKey>()));
            scheduler.Setup(s => s.DeleteJob(It.IsAny<JobKey>()));
            scheduler.Setup(s => s.GetTriggersOfJob(It.IsAny<JobKey>()))
                     .Returns
                     (
                        new List<ITrigger>
                        {
                            new SimpleTriggerImpl { Key = new TriggerKey("someKey") },
                            new SimpleTriggerImpl { Key = new TriggerKey("anotherKey") },
                            new SimpleTriggerImpl { Key = new TriggerKey("oneMoreKey") }
                        }
                     );

            testObject.Unschedule(new ActionEntity { ActionID = 1 });
            
            scheduler.Verify(s => s.UnscheduleJob(It.IsAny<TriggerKey>()), Times.Exactly(3));
        }

        [TestMethod]
        public void Unschedule_DelegatesToSchedulerToUnscheduleJob_WithZeroTriggers_Test()
        {
            scheduler.Setup(s => s.UnscheduleJob(It.IsAny<TriggerKey>()));
            scheduler.Setup(s => s.DeleteJob(It.IsAny<JobKey>()));
            scheduler.Setup(s => s.GetTriggersOfJob(It.IsAny<JobKey>())).Returns(new List<ITrigger>());

            testObject.Unschedule(new ActionEntity { ActionID = 1 });
            
            scheduler.Verify(s => s.UnscheduleJob(It.IsAny<TriggerKey>()), Times.Never());
        }

        [TestMethod]
        public void Unschedule_DoesNotDelegateToSchedulerToUnscheduleJob_WhenActionIsNull_Test()
        {
            scheduler.Setup(s => s.UnscheduleJob(It.IsAny<TriggerKey>()));
            scheduler.Setup(s => s.DeleteJob(It.IsAny<JobKey>()));
            scheduler.Setup(s => s.GetTriggersOfJob(It.IsAny<JobKey>())).Returns(new List<ITrigger>());

            testObject.Unschedule(null);

            scheduler.Verify(s => s.UnscheduleJob(It.IsAny<TriggerKey>()), Times.Never());
        }

        [TestMethod]
        public void Unschedule_DelegatesToSchedulerToDeleteJob_Test()
        {
            scheduler.Setup(s => s.UnscheduleJob(It.IsAny<TriggerKey>()));
            scheduler.Setup(s => s.DeleteJob(It.IsAny<JobKey>()));
            scheduler.Setup(s => s.GetTriggersOfJob(It.IsAny<JobKey>())).Returns(new List<ITrigger>());

            testObject.Unschedule(new ActionEntity { ActionID = 1 });

            scheduler.Verify(s => s.DeleteJob(It.IsAny<JobKey>()), Times.Once());
        }

        [TestMethod]
        public void Unschedule_DoesNotDelegateToSchedulerToDeleteJob_WhenActionIsNull_Test()
        {
            scheduler.Setup(s => s.UnscheduleJob(It.IsAny<TriggerKey>()));
            scheduler.Setup(s => s.DeleteJob(It.IsAny<JobKey>()));
            scheduler.Setup(s => s.GetTriggersOfJob(It.IsAny<JobKey>())).Returns(new List<ITrigger>());

            testObject.Unschedule(null);

            scheduler.Verify(s => s.DeleteJob(It.IsAny<JobKey>()), Times.Never());
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
