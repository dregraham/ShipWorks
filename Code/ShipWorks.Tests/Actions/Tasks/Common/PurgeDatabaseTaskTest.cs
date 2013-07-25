using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Interapptive.Shared.Utility;
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

            scriptRunner.Verify(x => x.RunScript(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never());
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

            scriptRunner.Verify(x => x.RunScript("PurgeAudit", It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
            scriptRunner.Verify(x => x.RunScript("PurgeDownload", It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
            scriptRunner.Verify(x => x.RunScript("PurgeEmail", It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
            scriptRunner.Verify(x => x.RunScript("PurgeLabel", It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
            scriptRunner.Verify(x => x.RunScript("PurgePrintResult", It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
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

            scriptRunner.Verify(x => x.RunScript("PurgeLabel", It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
            scriptRunner.Verify(x => x.RunScript("PurgePrintResult", It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
            scriptRunner.Verify(x => x.RunScript("PurgeEmail", It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never());
            scriptRunner.Verify(x => x.RunScript("PurgeAudit", It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never());
            scriptRunner.Verify(x => x.RunScript("PurgeDownload", It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never());
        }

        [TestMethod]
        public void Run_ShouldRunScriptsInCorrectOrder_WhenAllAreSelected()
        {
            List<string> calledPurges = new List<string>();

            Mock<ISqlPurgeScriptRunner> scriptRunner = new Mock<ISqlPurgeScriptRunner>();
            scriptRunner.Setup(x => x.RunScript(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Callback<string, DateTime, DateTime>((x, y, z) => calledPurges.Add(x));

            PurgeDatabaseTask testObject = new PurgeDatabaseTask(scriptRunner.Object, DefaultDateTimeProvider);
            testObject.Purges.Add(PurgeDatabaseType.Audit);
            testObject.Purges.Add(PurgeDatabaseType.Downloads);
            testObject.Purges.Add(PurgeDatabaseType.Email);
            testObject.Purges.Add(PurgeDatabaseType.Labels);
            testObject.Purges.Add(PurgeDatabaseType.PrintJobs);

            testObject.Run(null, null);

            Assert.AreEqual("PurgeLabel", calledPurges[0]);
            Assert.AreEqual("PurgePrintResult", calledPurges[1]);
            Assert.AreEqual("PurgeEmail", calledPurges[2]);
            Assert.AreEqual("PurgeAudit", calledPurges[3]);
            Assert.AreEqual("PurgeDownload", calledPurges[4]);
        }

        [TestMethod]
        public void Run_ShouldRunScriptsInCorrectOrder_WhenOnlySomeAreSelected()
        {
            List<string> calledPurges = new List<string>();

            Mock<ISqlPurgeScriptRunner> scriptRunner = new Mock<ISqlPurgeScriptRunner>();
            scriptRunner.Setup(x => x.RunScript(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Callback<string, DateTime, DateTime>((x, y, z) => calledPurges.Add(x));

            PurgeDatabaseTask testObject = new PurgeDatabaseTask(scriptRunner.Object, DefaultDateTimeProvider);
            testObject.Purges.Add(PurgeDatabaseType.Audit);
            testObject.Purges.Add(PurgeDatabaseType.Downloads);
            testObject.Purges.Add(PurgeDatabaseType.PrintJobs);

            testObject.Run(null, null);

            Assert.AreEqual("PurgePrintResult", calledPurges[0]);
            Assert.AreEqual("PurgeAudit", calledPurges[1]);
            Assert.AreEqual("PurgeDownload", calledPurges[2]);
        }

        [TestMethod]
        public void Run_ShouldPassRetentionDateToRunScript()
        {
            Mock<ISqlPurgeScriptRunner> scriptRunner = new Mock<ISqlPurgeScriptRunner>();
            scriptRunner.Setup(x => x.SqlUtcDateTime).Returns(new DateTime(2013, 7, 25, 3, 45, 16));

            PurgeDatabaseTask testObject = new PurgeDatabaseTask(scriptRunner.Object, DefaultDateTimeProvider);
            testObject.Purges.Add(PurgeDatabaseType.Audit);
            testObject.RetentionPeriodInDays = 63;
            testObject.StopLongPurges = false;

            testObject.Run(null, null);

            DateTime expectedDate = new DateTime(2013, 9, 26, 3, 45, 16);
            scriptRunner.Verify(x => x.RunScript(It.IsAny<string>(), expectedDate, It.IsAny<DateTime>()), Times.Once()); 
        }

        [TestMethod]
        public void Run_ShouldPassSqlTimeoutToRunScript_WhenStopLongPurgesIsTrue()
        {
            Mock<ISqlPurgeScriptRunner> scriptRunner = new Mock<ISqlPurgeScriptRunner>();
            scriptRunner.Setup(x => x.SqlUtcDateTime).Returns(new DateTime(2013, 7, 25, 3, 45, 16));

            PurgeDatabaseTask testObject = new PurgeDatabaseTask(scriptRunner.Object, DefaultDateTimeProvider);
            testObject.Purges.Add(PurgeDatabaseType.Audit);
            testObject.StopLongPurges = true;
            testObject.TimeoutInHours = 4;

            testObject.Run(null, null);

            DateTime expectedDate = new DateTime(2013, 7, 25, 7, 45, 16);
            scriptRunner.Verify(x => x.RunScript(It.IsAny<string>(), It.IsAny<DateTime>(), expectedDate), Times.Once());
        }

        [TestMethod]
        public void Run_ShouldPassMaxDateAsTimeToRunScript_WhenStopLongPurgesIsFalse()
        {
            Mock<ISqlPurgeScriptRunner> scriptRunner = new Mock<ISqlPurgeScriptRunner>();

            PurgeDatabaseTask testObject = new PurgeDatabaseTask(scriptRunner.Object, DefaultDateTimeProvider);
            testObject.Purges.Add(PurgeDatabaseType.Audit);
            testObject.StopLongPurges = false;
            testObject.TimeoutInHours = 4;

            testObject.Run(null, null);

            scriptRunner.Verify(x => x.RunScript(It.IsAny<string>(), It.IsAny<DateTime>(), SqlDateTime.MaxValue.Value), Times.Once());
        }

        #region "Support methods"
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
        #endregion
    }
}
