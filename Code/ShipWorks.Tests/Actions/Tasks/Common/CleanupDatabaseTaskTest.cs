using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using log4net.Appender;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Actions.Tasks.Common;

namespace ShipWorks.Tests.Actions.Tasks.Common
{
    [TestClass]
    public class CleanupDatabaseTaskTest
    {
        [TestMethod]
        public void Run_ShouldNotExecuteAnyScripts_WhenNoPurgesAreSelected()
        {
            Mock<ISqlPurgeScriptRunner> scriptRunner = new Mock<ISqlPurgeScriptRunner>();

            CleanupDatabaseTask testObject = new CleanupDatabaseTask(scriptRunner.Object, DefaultDateTimeProvider);

            testObject.Run(null, null);

            scriptRunner.Verify(x => x.RunScript(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>()), Times.Never());
        }

        [TestMethod]
        public void Run_ShouldRunAllSelectedSelectedScripts_WhenTaskHasNoTimeout()
        {
            Mock<ISqlPurgeScriptRunner> scriptRunner = new Mock<ISqlPurgeScriptRunner>();

            CleanupDatabaseTask testObject = new CleanupDatabaseTask(scriptRunner.Object, DefaultDateTimeProvider);
            testObject.Purges.Add(CleanupDatabaseType.Audit);
            testObject.Purges.Add(CleanupDatabaseType.Downloads);
            testObject.Purges.Add(CleanupDatabaseType.Email);
            testObject.Purges.Add(CleanupDatabaseType.Labels);
            testObject.Purges.Add(CleanupDatabaseType.PrintJobs);
            testObject.StopLongCleanups = false;

            testObject.Run(null, null);

            scriptRunner.Verify(x => x.RunScript("AuditCleanup", It.IsAny<int>(), It.IsAny<DateTime>()), Times.Once());
            scriptRunner.Verify(x => x.RunScript("DownloadCleanup", It.IsAny<int>(), It.IsAny<DateTime>()), Times.Once());
            scriptRunner.Verify(x => x.RunScript("EmailCleanup", It.IsAny<int>(), It.IsAny<DateTime>()), Times.Once());
            scriptRunner.Verify(x => x.RunScript("LabelCleanup", It.IsAny<int>(), It.IsAny<DateTime>()), Times.Once());
            scriptRunner.Verify(x => x.RunScript("PrintResultCleanup", It.IsAny<int>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void Run_ShouldStopRunning_WhenTimeoutHasElapsed()
        {
            Mock<ISqlPurgeScriptRunner> scriptRunner = new Mock<ISqlPurgeScriptRunner>();
            Mock<IDateTimeProvider> dateProvider = new Mock<IDateTimeProvider>();
            dateProvider.SetupMany(x => x.UtcNow, new DateTime(2013, 7, 24, 12, 15, 21), 
                new DateTime(2013, 7, 24, 13, 15, 21),
                new DateTime(2013, 7, 24, 14, 15, 20), 
                new DateTime(2013, 7, 24, 14, 15, 22));

            CleanupDatabaseTask testObject = new CleanupDatabaseTask(scriptRunner.Object, dateProvider.Object);
            testObject.Purges.Add(CleanupDatabaseType.Audit);
            testObject.Purges.Add(CleanupDatabaseType.Downloads);
            testObject.Purges.Add(CleanupDatabaseType.Email);
            testObject.Purges.Add(CleanupDatabaseType.Labels);
            testObject.Purges.Add(CleanupDatabaseType.PrintJobs);
            testObject.StopLongCleanups = true;
            testObject.StopAfterHours = 2;

            testObject.Run(null, null);

            scriptRunner.Verify(x => x.RunScript("AuditCleanup", It.IsAny<int>(), It.IsAny<DateTime>()), Times.Once());
            scriptRunner.Verify(x => x.RunScript("DownloadCleanup", It.IsAny<int>(), It.IsAny<DateTime>()), Times.Once());
            scriptRunner.Verify(x => x.RunScript("EmailCleanup", It.IsAny<int>(), It.IsAny<DateTime>()), Times.Never());
            scriptRunner.Verify(x => x.RunScript("LabelCleanup", It.IsAny<int>(), It.IsAny<DateTime>()), Times.Never());
            scriptRunner.Verify(x => x.RunScript("PrintResultCleanup", It.IsAny<int>(), It.IsAny<DateTime>()), Times.Never());
        }




        /// <summary>
        /// Gets a script runner that does nothing
        /// </summary>
        private static ISqlPurgeScriptRunner DefaultScriptRunner
        {
            get
            {
                return new Mock<ISqlPurgeScriptRunner>().Object;
            }
        }

        /// <summary>
        /// Gets a date time provider that does nothing
        /// </summary>
        private static IDateTimeProvider DefaultDateTimeProvider
        {
            get
            {
                return new Mock<IDateTimeProvider>().Object;
            }
        }
    }
}
