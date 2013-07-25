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
    public class PurgeDatabaseTaskTest
    {
        [TestMethod]
        public void Run_ShouldNotExecuteAnyScripts_WhenNoPurgesAreSelected()
        {
            Mock<ISqlPurgeScriptRunner> scriptRunner = new Mock<ISqlPurgeScriptRunner>();

            PurgeDatabaseTask testObject = new PurgeDatabaseTask(scriptRunner.Object, DefaultDateTimeProvider);

            testObject.Run(null, null);

            scriptRunner.Verify(x => x.RunScript(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>()), Times.Never());
        }

        [TestMethod]
        public void Run_ShouldRunAllSelectedSelectedScripts_WhenTaskHasNoTimeout()
        {
            Mock<ISqlPurgeScriptRunner> scriptRunner = new Mock<ISqlPurgeScriptRunner>();

            PurgeDatabaseTask testObject = new PurgeDatabaseTask(scriptRunner.Object, DefaultDateTimeProvider);
            testObject.Purges.Add(PurgeDatabaseType.Audit);
            testObject.Purges.Add(PurgeDatabaseType.Downloads);
            testObject.Purges.Add(PurgeDatabaseType.Email);
            testObject.Purges.Add(PurgeDatabaseType.Labels);
            testObject.Purges.Add(PurgeDatabaseType.PrintJobs);
            testObject.StopLongPurges = false;

            testObject.Run(null, null);

            scriptRunner.Verify(x => x.RunScript("PurgeAudit", It.IsAny<int>(), It.IsAny<DateTime>()), Times.Once());
            scriptRunner.Verify(x => x.RunScript("PurgeDownload", It.IsAny<int>(), It.IsAny<DateTime>()), Times.Once());
            scriptRunner.Verify(x => x.RunScript("PurgeEmail", It.IsAny<int>(), It.IsAny<DateTime>()), Times.Once());
            scriptRunner.Verify(x => x.RunScript("PurgeLabel", It.IsAny<int>(), It.IsAny<DateTime>()), Times.Once());
            scriptRunner.Verify(x => x.RunScript("PurgePrintResult", It.IsAny<int>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void Run_ShouldStopRunning_WhenTimeoutHasElapsed()
        {
            Mock<ISqlPurgeScriptRunner> scriptRunner = new Mock<ISqlPurgeScriptRunner>();
            Mock<IDateTimeProvider> dateProvider = new Mock<IDateTimeProvider>();
            dateProvider.SetupSequence(x => x.UtcNow).Returns(new DateTime(2013, 7, 24, 12, 15, 21))
                .Returns(new DateTime(2013, 7, 24, 13, 15, 21))
                .Returns(new DateTime(2013, 7, 24, 14, 15, 20))
                .Returns(new DateTime(2013, 7, 24, 14, 15, 22));

            PurgeDatabaseTask testObject = new PurgeDatabaseTask(scriptRunner.Object, dateProvider.Object);
            testObject.Purges.Add(PurgeDatabaseType.Audit);
            testObject.Purges.Add(PurgeDatabaseType.Downloads);
            testObject.Purges.Add(PurgeDatabaseType.Email);
            testObject.Purges.Add(PurgeDatabaseType.Labels);
            testObject.Purges.Add(PurgeDatabaseType.PrintJobs);
            testObject.StopLongPurges = true;
            testObject.TimeoutInHours = 2;

            testObject.Run(null, null);

            scriptRunner.Verify(x => x.RunScript("PurgeAudit", It.IsAny<int>(), It.IsAny<DateTime>()), Times.Once());
            scriptRunner.Verify(x => x.RunScript("PurgeDownload", It.IsAny<int>(), It.IsAny<DateTime>()), Times.Once());
            scriptRunner.Verify(x => x.RunScript("PurgeEmail", It.IsAny<int>(), It.IsAny<DateTime>()), Times.Never());
            scriptRunner.Verify(x => x.RunScript("PurgeLabel", It.IsAny<int>(), It.IsAny<DateTime>()), Times.Never());
            scriptRunner.Verify(x => x.RunScript("PurgePrintResult", It.IsAny<int>(), It.IsAny<DateTime>()), Times.Never());
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
