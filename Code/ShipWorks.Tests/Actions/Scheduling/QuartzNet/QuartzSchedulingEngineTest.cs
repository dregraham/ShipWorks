using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Quartz.Impl.Triggers;
using ShipWorks.Actions.Scheduling.QuartzNet;
using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Threading;
using Quartz.Impl;
using Quartz;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using Quartz.Spi;


namespace ShipWorks.Tests.Actions.Scheduling.QuartzNet
{
    [TestClass]
    public class QuartzSchedulingEngineTest
    {
        Mock<Quartz.IScheduler> scheduler;
        Mock<IActionScheduleAdapter> scheduleAdapter;

        QuartzSchedulingEngine testObject;

        [TestInitialize]
        public void Initialize()
        {
            scheduler = new Mock<Quartz.IScheduler>();

            var schedulerFactory = new Mock<Quartz.ISchedulerFactory>();
            schedulerFactory.Setup(x => x.GetScheduler()).Returns(scheduler.Object);

            scheduleAdapter = new Mock<IActionScheduleAdapter>();
            scheduleAdapter.Setup(x => x.Adapt(It.IsAny<ActionSchedule>()))
                .Returns(new QuartzActionSchedule());

            testObject = new QuartzSchedulingEngine(schedulerFactory.Object, scheduleAdapter.Object);
        }


        [TestMethod]
        public void Schedule_DelegatesToScheduler_Test()
        {
            ActionEntity action = new ActionEntity { ActionID = 1 };
            ActionSchedule schedule = new Mock<ActionSchedule>().Object;

            testObject.Schedule(action, schedule);

            scheduler.Verify(s => s.ScheduleJob(It.IsAny<IJobDetail>(), It.IsAny<ISimpleTrigger>()), Times.Once());
        }

        [TestMethod]
        public void Schedule_RemovesExistingJob_Test()
        {
            scheduler.Setup(s => s.GetJobDetail(It.IsAny<JobKey>())).Returns(new JobDetailImpl());

            // Setup methods used when removing
            scheduler.Setup(s => s.UnscheduleJob(It.IsAny<TriggerKey>()));
            scheduler.Setup(s => s.GetTriggersOfJob(It.IsAny<JobKey>())).Returns(new List<ITrigger>());

            ActionEntity action = new ActionEntity { ActionID = 1 };
            ActionSchedule schedule = new Mock<ActionSchedule>().Object;

            testObject.Schedule(action, schedule);

            scheduler.Verify(s => s.DeleteJob(It.IsAny<JobKey>()), Times.Once());
        }

        [TestMethod]
        public void Schedule_DoesNotRemoveJob_WhenJobIsNew_Test()
        {
            // Setup to return null to simulate the job does not exist
            scheduler.Setup(s => s.GetJobDetail(It.IsAny<JobKey>())).Returns<IJobDetail>(null);

            ActionEntity action = new ActionEntity { ActionID = 1 };
            ActionSchedule schedule = new Mock<ActionSchedule>().Object;

            testObject.Schedule(action, schedule);

            scheduler.Verify(s => s.DeleteJob(It.IsAny<JobKey>()), Times.Never());
        }

        [TestMethod]
        public void Schedule_ShutsdownScheduler_Test()
        {
            ActionEntity action = new ActionEntity { ActionID = 1 };
            ActionSchedule schedule = new Mock<ActionSchedule>().Object;

            testObject.Schedule(action, schedule);

            scheduler.Verify(s => s.Shutdown(true), Times.Once());
        }

        [TestMethod]
        public void IsExistingJob_DelegatesToScheduler_Test()
        {
            ActionEntity action = new ActionEntity { ActionID = 1 };

            testObject.HasExistingSchedule(action);

            scheduler.Verify(s => s.GetJobDetail(It.IsAny<JobKey>()), Times.Once());
        }

        [TestMethod]
        public void IsExistingJob_ReturnsFalse_WhenSchedulerReturnsNull_Test()
        {
            scheduler.Setup(s => s.GetJobDetail(It.IsAny<JobKey>())).Returns<IJobDetail>(null);

            Assert.IsFalse(testObject.HasExistingSchedule(new ActionEntity()));
        }

        [TestMethod]
        public void IsExistingJob_ReturnsTrue_WhenReturnsNonNull_Test()
        {
            scheduler.Setup(s => s.GetJobDetail(It.IsAny<JobKey>())).Returns(new JobDetailImpl());

            ActionEntity action = new ActionEntity { ActionID = 1 };

            Assert.IsTrue(testObject.HasExistingSchedule(action));
        }

        [TestMethod]
        public void IsExistingJob_ShutsdownScheduler_Test()
        {
            ActionEntity action = new ActionEntity { ActionID = 1 };

            testObject.HasExistingSchedule(action);

            scheduler.Verify(s => s.Shutdown(true), Times.Once());
        }

        [TestMethod]
        public void Unschedule_DelegatesToSchedulerForTriggers_Test()
        {
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
            scheduler.Setup(s => s.GetTriggersOfJob(It.IsAny<JobKey>())).Returns(new List<ITrigger>());

            testObject.Unschedule(new ActionEntity { ActionID = 1 });

            scheduler.Verify(s => s.UnscheduleJob(It.IsAny<TriggerKey>()), Times.Never());
        }

        [TestMethod]
        public void Unschedule_DoesNotDelegateToSchedulerToUnscheduleJob_WhenActionIsNull_Test()
        {
            scheduler.Setup(s => s.GetTriggersOfJob(It.IsAny<JobKey>())).Returns(new List<ITrigger>());

            testObject.Unschedule(null);

            scheduler.Verify(s => s.UnscheduleJob(It.IsAny<TriggerKey>()), Times.Never());
        }

        [TestMethod]
        public void Unschedule_DelegatesToSchedulerToDeleteJob_Test()
        {
            scheduler.Setup(s => s.GetTriggersOfJob(It.IsAny<JobKey>())).Returns(new List<ITrigger>());

            testObject.Unschedule(new ActionEntity { ActionID = 1 });

            scheduler.Verify(s => s.DeleteJob(It.IsAny<JobKey>()), Times.Once());
        }

        [TestMethod]
        public void Unschedule_DoesNotDelegateToSchedulerToDeleteJob_WhenActionIsNull_Test()
        {
            scheduler.Setup(s => s.GetTriggersOfJob(It.IsAny<JobKey>())).Returns(new List<ITrigger>());

            testObject.Unschedule(null);

            scheduler.Verify(s => s.DeleteJob(It.IsAny<JobKey>()), Times.Never());
        }

        [TestMethod]
        public void Unschedule_ShutsdownScheduler_Test()
        {
            scheduler.Setup(s => s.GetTriggersOfJob(It.IsAny<JobKey>())).Returns(new List<ITrigger>());

            testObject.Unschedule(new ActionEntity { ActionID = 1 });

            scheduler.Verify(s => s.Shutdown(true), Times.Once());
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

        [TestMethod]
        public void AdaptedScheduleBuilderIsAppliedToTrigger()
        {
            var actionSchedule = new Mock<ActionSchedule>().Object;

            var trigger = new Mock<IMutableTrigger>().Object;

            var quartzSchedule = new Mock<IScheduleBuilder>();
            quartzSchedule.Setup(x => x.Build()).Returns(trigger);

            scheduleAdapter.Setup(x => x.Adapt(actionSchedule))
                .Returns(new QuartzActionSchedule { ScheduleBuilder = quartzSchedule.Object });

            testObject.Schedule(new ActionEntity { ActionID = 2 }, actionSchedule);

            scheduler.Verify(x => x.ScheduleJob(It.IsAny<IJobDetail>(), trigger), Times.Once());
        }

        [TestMethod]
        public void AdaptedCalendarIsAddedToSchedulerWithJobName()
        {
            var actionSchedule = new Mock<ActionSchedule>().Object;

            var quartzCalendar = new Mock<ICalendar>().Object;

            scheduleAdapter.Setup(x => x.Adapt(actionSchedule))
                .Returns(new QuartzActionSchedule { Calendar = quartzCalendar });

            string jobName = null;
            scheduler.Setup(x => x.ScheduleJob(It.IsAny<IJobDetail>(), It.IsAny<ITrigger>()))
                .Callback<IJobDetail, ITrigger>((j, t) => { jobName = j.Key.Name; });

            testObject.Schedule(new ActionEntity { ActionID = 2 }, actionSchedule);

            scheduler.Verify(x => x.AddCalendar(jobName, quartzCalendar, true, true), Times.Once());
        }

        [TestMethod]
        public void AdaptedCalendarIsAppliedToTrigger()
        {
            var actionSchedule = new Mock<ActionSchedule>().Object;

            scheduleAdapter.Setup(x => x.Adapt(actionSchedule))
                .Returns(new QuartzActionSchedule { Calendar = new Mock<ICalendar>().Object });

            string calendarName = null;
            scheduler.Setup(x => x.AddCalendar(It.IsAny<string>(), It.IsAny<ICalendar>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Callback<string, ICalendar, bool, bool>((n, c, b1, b2) => { calendarName = n; });

            testObject.Schedule(new ActionEntity { ActionID = 2 }, actionSchedule);

            scheduler.Verify(
                x => x.ScheduleJob(
                    It.IsAny<IJobDetail>(),
                    It.Is<ITrigger>(t => t.CalendarName == calendarName)
                ),
                Times.Once()
            );
        }
    }
}
