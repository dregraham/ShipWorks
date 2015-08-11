using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz.Impl;
using log4net;
using Xunit;
using Moq;
using Quartz;
using ShipWorks.Actions.Scheduling.QuartzNet;

namespace ShipWorks.Tests.Actions.Scheduling.QuartzNet
{
    public class ActionJobTest
    {
        // NOTE: we can only test the path that results in the action not being sent
        // to the dispatcher since the DispatchScheduledAction method is static
        private ActionJob testObject;

        private Mock<IJobExecutionContext> executionContext;
        private Mock<ILog> log;
        private Mock<IScheduler> scheduler;
        private Mock<IJobDetail> jobDetail;

        private JobKey jobKey;
        private JobDataMap jobDataMap;
        
        public ActionJobTest()
        {
            log = new Mock<ILog>();

            scheduler = new Mock<IScheduler>();
            
            jobKey = new JobKey("someJobKey");

            jobDataMap = new JobDataMap();

            jobDetail = new Mock<IJobDetail>();
            jobDetail.Setup(j => j.JobDataMap).Returns(jobDataMap);
            jobDetail.Setup(j => j.Key).Returns(jobKey);
            
            executionContext = new Mock<IJobExecutionContext>();
            executionContext.Setup(c => c.Scheduler).Returns(scheduler.Object);
            executionContext.Setup(c => c.JobDetail).Returns(jobDetail.Object);

            testObject = new ActionJob(log.Object);
        }

        [Fact]
        public void Execute_DeletesJob_WhenJobDataMapDoesNotContainActionJobKey_Test()
        {
            jobDataMap.Clear();

            testObject.Execute(executionContext.Object);

            scheduler.Verify(s => s.DeleteJob(jobKey), Times.Once());
        }

        [Fact]
        public void Execute_LogsError_WhenJobDataMapDoesNotContainActionJobKey_Test()
        {
            jobDataMap.Clear();

            testObject.Execute(executionContext.Object);

            log.Verify(l => l.ErrorFormat(It.IsAny<string>(), jobDetail.Object.Key), Times.Once());
        }

        [Fact]
        public void Execute_DeletesJob_WhenActionIdInJobDataMapIsNull_Test()
        {
            // Set action ID to a null value
            jobDataMap.Add("ActionID", null);
            
            testObject.Execute(executionContext.Object);

            scheduler.Verify(s => s.DeleteJob(jobKey), Times.Once());
        }

        [Fact]
        public void Execute_LogsError_WhenActionIdInJobDataMapIsNull_Test()
        {
            // Set action ID to a null value
            jobDataMap.Add("ActionID", null);

            testObject.Execute(executionContext.Object);

            log.Verify(l => l.ErrorFormat(It.IsAny<string>(), jobDetail.Object.Key), Times.Once());
        }

        [Fact]
        public void Execute_DeletesJob_WhenActionIdInJobDataMapIsAlphaNumeric_Test()
        {
            // Set action ID to an alpha value
            jobDataMap.Add("ActionID", "abc123");
            
            testObject.Execute(executionContext.Object);

            scheduler.Verify(s => s.DeleteJob(jobKey), Times.Once());
        }
        
        [Fact]
        public void Execute_LogsError_WhenActionIdInJobDataMapIsAlphaNumeric_Test()
        {
            // Set action ID to an alpha value
            jobDataMap.Add("ActionID", "abc123");

            testObject.Execute(executionContext.Object);

            log.Verify(l => l.ErrorFormat(It.IsAny<string>(), jobDetail.Object.Key), Times.Once());
        }
    }
}
