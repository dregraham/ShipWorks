using Xunit;
using ShipWorks.Actions.Scheduling;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Actions.Scheduling.ActionSchedules;
using System;
using System.Threading;

namespace ShipWorks.Tests.Actions.Scheduling
{
    public class SchedulerTest
    {
        private Scheduler testObject;

        private Mock<ISchedulingEngine> schedulingEngine;

        [TestInitialize]
        public void Initialize()
        {
            schedulingEngine = new Mock<ISchedulingEngine>();

            // Simulate engine to see jobs as new by default
            schedulingEngine.Setup(e => e.HasExistingSchedule(It.IsAny<ActionEntity>())).Returns(false);

            testObject = new Scheduler(schedulingEngine.Object);
        }

        [Fact]
        public void ScheduleAction_DelegatesToSchedulingEngine_WhenJobDoesNotExist_AndStartDateIsInFuture_Test()
        {
            ActionEntity action = new ActionEntity();

            // Set the trigger to be in the future
            ActionSchedule schedule = new Mock<ActionSchedule>().Object;
            schedule.StartDateTimeInUtc = DateTime.UtcNow.AddSeconds(5);

            testObject.ScheduleAction(action, schedule);

            schedulingEngine.Verify(e => e.Schedule(action, schedule), Times.Once());
        }

        [Fact]
        public void ScheduleAction_DelegatesToScheduler_WhenJobExists_AndStartDateOccursInPast_Test()
        {
            ActionEntity action = new ActionEntity();

            // Set the trigger to be five seconds in the past
            ActionSchedule schedule = new Mock<ActionSchedule>().Object;
            schedule.StartDateTimeInUtc = DateTime.UtcNow.AddSeconds(-5);

            // Simulate engine to see this as an existing job
            schedulingEngine.Setup(e => e.HasExistingSchedule(action)).Returns(true);

            testObject.ScheduleAction(action, schedule);
        }

        [Fact]
        public void ScheduleAction_DelegatesToScheduler_WhenJobExists_AndStartDateOccursNow_Test()
        {
            ActionEntity action = new ActionEntity();

            // Set the trigger to be now
            ActionSchedule schedule = new Mock<ActionSchedule>().Object;
            schedule.StartDateTimeInUtc = DateTime.UtcNow;

            // Simulate engine to see this as an existing job
            schedulingEngine.Setup(e => e.HasExistingSchedule(action)).Returns(true);

            testObject.ScheduleAction(action, schedule);
        }

        [Fact]
        public void ScheduleAction_DelegatesToSchedulingEngine_WhenJobExists_AndStartDateIsInFuture_Test()
        {
            ActionEntity action = new ActionEntity();

            // Set the trigger to be in the future
            ActionSchedule schedule = new Mock<ActionSchedule>().Object;
            schedule.StartDateTimeInUtc = DateTime.UtcNow.AddSeconds(5);

            // Simulate engine to see this as an existing job
            schedulingEngine.Setup(e => e.HasExistingSchedule(action)).Returns(true);

            testObject.ScheduleAction(action, schedule);

            schedulingEngine.Verify(e => e.Schedule(action, schedule), Times.Once());
        }

        [Fact]
        [ExpectedException(typeof(SchedulingException))]
        public void ScheduleAction_ThrowsSchedulingException_WhenExceptionIsThrownBySchedulingEngine_Test()
        {
            ActionEntity action = new ActionEntity();
            ActionSchedule schedule = new Mock<ActionSchedule>().Object;

            // Simulate engine to see this as an existing job (so trigger start date does not matter)
            // and setup the schedule method throws an out of memory exception
            schedulingEngine.Setup(e => e.HasExistingSchedule(action)).Returns(true);
            schedulingEngine.Setup(e => e.Schedule(It.IsAny<ActionEntity>(), It.IsAny<ActionSchedule>())).Throws(new OutOfMemoryException());

            // Throws scheduling exception
            testObject.ScheduleAction(action, schedule);
        }

        [Fact]
        public void UnscheduleAction_DelegatesToSchedulingEngine_WhenJobExists_Test()
        {
            ActionEntity action = new ActionEntity();

            // Simulate that the job exists
            schedulingEngine.Setup(e => e.HasExistingSchedule(action)).Returns(true);

            testObject.UnscheduleAction(action);

            schedulingEngine.Verify(s => s.Unschedule(action), Times.Once());
        }

        [Fact]
        public void UnscheduleAction_DoesNotDelegateToSchedulingEngine_WhenJobDoesNotExist_Test()
        {
            ActionEntity action = new ActionEntity();

            testObject.UnscheduleAction(action);

            schedulingEngine.Verify(s => s.Unschedule(action), Times.Never());
        }

        [Fact]
        [ExpectedException(typeof(SchedulingException))]
        public void UnscheduleAction_ThrowsSchedulingException_WhenExceptionIsThrownBySchedulingEngine_Test()
        {
            ActionEntity action = new ActionEntity();

            // Simulate that the job does not exist
            schedulingEngine.Setup(e => e.HasExistingSchedule(It.IsAny<ActionEntity>())).Throws(new InvalidOperationException());

            // Throws an exception
            testObject.UnscheduleAction(action);
        }

        [Fact]
        public void RunAsync_DelegatesToSchedulingEngine()
        {
            var token = new CancellationToken(false);

            testObject.RunAsync(token);

            schedulingEngine.Verify(x => x.RunAsync(token), Times.Once());
        }

        [Fact, ExpectedException(typeof(SchedulingException))]
        public void ScheduleAction_ValidatesSchedule()
        {
            var schedule = new Mock<ActionSchedule>();
            schedule.Setup(x => x.Validate()).Throws<SchedulingException>();

            try
            {
                testObject.ScheduleAction(new ActionEntity(), schedule.Object);
            }
            catch
            {
                schedulingEngine.Verify(x => x.Schedule(It.IsAny<ActionEntity>(), It.IsAny<ActionSchedule>()), Times.Never());
                throw;
            }
        }
    }
}
