using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Actions.Scheduling;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Actions.Triggers;
using System;
using System.Threading;

namespace ShipWorks.Tests.Actions.Scheduling
{
    [TestClass]
    public class SchedulerTest
    {
        private Scheduler testObject;

        private Mock<ISchedulingEngine> schedulingEngine;

        [TestInitialize]
        public void Initialize()
        {
            schedulingEngine = new Mock<ISchedulingEngine>();
            schedulingEngine.Setup(e => e.Schedule(It.IsAny<ActionEntity>(), It.IsAny<ScheduledTrigger>()));
            schedulingEngine.Setup(e => e.IsExistingJob(It.IsAny<ActionEntity>(), It.IsAny<ScheduledTrigger>())).Returns(false);

            testObject = new Scheduler(schedulingEngine.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(SchedulingException))]
        public void ScheduleAction_ThrowsSchedulingException_WhenJobDoesNotExist_AndStartDateOccursInPast_Test()
        {
            ActionEntity action = new ActionEntity();

            // Set the trigger to be five seconds in the past
            ScheduledTrigger trigger = new ScheduledTrigger { StartDateTimeInUtc = DateTime.UtcNow.AddSeconds(-5)};

            // Simulate engine to see this as a new job
            schedulingEngine.Setup(e => e.IsExistingJob(It.IsAny<ActionEntity>(), It.IsAny<ScheduledTrigger>())).Returns(false);

            // Throw scheduling exception
            testObject.ScheduleAction(action, trigger);
        }

        [TestMethod]
        [ExpectedException(typeof(SchedulingException))]
        public void ScheduleAction_ThrowsSchedulingException_WhenJobDoesNotExist_AndStartDateOccursNow_Test()
        {
            ActionEntity action = new ActionEntity();

            // Set the trigger to be now
            ScheduledTrigger trigger = new ScheduledTrigger { StartDateTimeInUtc = DateTime.UtcNow };

            // Simulate engine to see this as a new job
            schedulingEngine.Setup(e => e.IsExistingJob(It.IsAny<ActionEntity>(), It.IsAny<ScheduledTrigger>())).Returns(false);

            // Throw scheduling exception
            testObject.ScheduleAction(action, trigger);
        }

        [TestMethod]
        public void ScheduleAction_DelegatesToSchedulingEngine_WhenJobDoesNotExist_AndStartDateIsInFuture_Test()
        {
            ActionEntity action = new ActionEntity();

            // Set the trigger to be in the future
            ScheduledTrigger trigger = new ScheduledTrigger { StartDateTimeInUtc = DateTime.UtcNow.AddSeconds(5) };

            // Simulate engine to see this as a new job
            schedulingEngine.Setup(e => e.IsExistingJob(It.IsAny<ActionEntity>(), It.IsAny<ScheduledTrigger>())).Returns(false);

            testObject.ScheduleAction(action, trigger);

            schedulingEngine.Verify(e => e.Schedule(action, trigger), Times.Once());
        }

        [TestMethod]
        public void ScheduleAction_DelegatesToScheduler_WhenJobExists_AndStartDateOccursInPast_Test()
        {
            ActionEntity action = new ActionEntity();

            // Set the trigger to be five seconds in the past
            ScheduledTrigger trigger = new ScheduledTrigger { StartDateTimeInUtc = DateTime.UtcNow.AddSeconds(-5) };

            // Simulate engine to see this as an existing job
            schedulingEngine.Setup(e => e.IsExistingJob(It.IsAny<ActionEntity>(), It.IsAny<ScheduledTrigger>())).Returns(true);

            testObject.ScheduleAction(action, trigger);
        }

        [TestMethod]
        public void ScheduleAction_DelegatesToScheduler_WhenJobExists_AndStartDateOccursNow_Test()
        {
            ActionEntity action = new ActionEntity();

            // Set the trigger to be now
            ScheduledTrigger trigger = new ScheduledTrigger { StartDateTimeInUtc = DateTime.UtcNow };

            // Simulate engine to see this as an existing job
            schedulingEngine.Setup(e => e.IsExistingJob(It.IsAny<ActionEntity>(), It.IsAny<ScheduledTrigger>())).Returns(true);

            testObject.ScheduleAction(action, trigger);
        }

        [TestMethod]
        public void ScheduleAction_DelegatesToSchedulingEngine_WhenJobExists_AndStartDateIsInFuture_Test()
        {
            ActionEntity action = new ActionEntity();

            // Set the trigger to be in the future
            ScheduledTrigger trigger = new ScheduledTrigger { StartDateTimeInUtc = DateTime.UtcNow.AddSeconds(5) };

            // Simulate engine to see this as an existing job
            schedulingEngine.Setup(e => e.IsExistingJob(It.IsAny<ActionEntity>(), It.IsAny<ScheduledTrigger>())).Returns(true);

            testObject.ScheduleAction(action, trigger);

            schedulingEngine.Verify(e => e.Schedule(action, trigger), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(SchedulingException))]
        public void ScheduleAction_ThrowsSchedulingException_WhenExceptionIsThrownBySchedulingEngine_Test()
        {
            ActionEntity action = new ActionEntity();
            ScheduledTrigger trigger = new ScheduledTrigger();

            // Simulate engine to see this as an existing job (so trigger start date does not matter)
            // and setup the schedule method throws an out of memory exception
            schedulingEngine.Setup(e => e.IsExistingJob(It.IsAny<ActionEntity>(), It.IsAny<ScheduledTrigger>())).Returns(true);
            schedulingEngine.Setup(e => e.Schedule(It.IsAny<ActionEntity>(), It.IsAny<ScheduledTrigger>())).Throws(new OutOfMemoryException());

            // Throws scheduling exception
            testObject.ScheduleAction(action, trigger);
        }
        
        [TestMethod]
        public void UnscheduleAction_DelegatesToSchedulingEngine_WhenJobExists_Test()
        {
            ActionEntity action = new ActionEntity();
            ScheduledTrigger trigger = new ScheduledTrigger();

            // Simulate that the job exists
            schedulingEngine.Setup(e => e.IsExistingJob(It.IsAny<ActionEntity>(), It.IsAny<ScheduledTrigger>())).Returns(true);

            testObject.UnscheduleAction(action, trigger);

            schedulingEngine.Verify(s => s.Unschedule(action), Times.Once());
        }

        [TestMethod]
        public void UnscheduleAction_DoesNotDelegateToSchedulingEngine_WhenJobDoesNotExist_Test()
        {
            ActionEntity action = new ActionEntity();
            ScheduledTrigger trigger = new ScheduledTrigger();

            // Simulate that the job does not exist
            schedulingEngine.Setup(e => e.IsExistingJob(It.IsAny<ActionEntity>(), It.IsAny<ScheduledTrigger>())).Returns(false);

            testObject.UnscheduleAction(action, trigger);

            schedulingEngine.Verify(s => s.Unschedule(action), Times.Never());
        }

        [TestMethod]
        [ExpectedException(typeof(SchedulingException))]
        public void UnscheduleAction_ThrowsSchedulingException_WhenExceptionIsThrownBySchedulingEngine_Test()
        {
            ActionEntity action = new ActionEntity();
            ScheduledTrigger trigger = new ScheduledTrigger();

            // Simulate that the job does not exist
            schedulingEngine.Setup(e => e.IsExistingJob(It.IsAny<ActionEntity>(), It.IsAny<ScheduledTrigger>())).Throws(new InvalidOperationException());

            // Throws an exception
            testObject.UnscheduleAction(action, trigger);
        }

        [TestMethod]
        public void RunAsync_DelegatesToSchedulingEngine()
        {
            var token = new CancellationToken(false);

            testObject.RunAsync(token);

            schedulingEngine.Verify(x => x.RunAsync(token), Times.Once());
        }
    }
}
