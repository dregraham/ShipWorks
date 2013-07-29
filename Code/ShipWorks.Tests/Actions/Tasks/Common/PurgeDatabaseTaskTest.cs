using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Interapptive.Shared.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Stores.Platforms.PayPal.WebServices;

namespace ShipWorks.Tests.Actions.Tasks.Common
{
    [TestClass]
    public class PurgeDatabaseTaskTest
    {
        [TestMethod]
        public void Run_ShouldNotExecuteAnyScripts_WhenNoPurgesAreSelected()
        {
            Mock<ISqlPurgeScriptRunner> scriptRunner = MockedScriptRunner;

            PurgeDatabaseTask testObject = new PurgeDatabaseTask(scriptRunner.Object, DefaultDateTimeProvider);

            testObject.Run(null, null);

            scriptRunner.Verify(x => x.RunScript(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never());
        }

        [TestMethod]
        public void Run_ShouldRunAllSelectedScripts_WhenTaskHasNoTimeout()
        {
            Mock<ISqlPurgeScriptRunner> scriptRunner = MockedScriptRunner;

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
        public void Run_ShouldRunSelectedScripts_WhenTaskHasNoTimeout()
        {
            Mock<ISqlPurgeScriptRunner> scriptRunner = MockedScriptRunner;

            PurgeDatabaseTask testObject = new PurgeDatabaseTask(scriptRunner.Object, DefaultDateTimeProvider);
            testObject.Purges.Add(PurgeDatabaseType.Email);
            testObject.StopLongPurges = false;

            testObject.Run(null, null);

            scriptRunner.Verify(x => x.RunScript("PurgeAudit", It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never());
            scriptRunner.Verify(x => x.RunScript("PurgeDownload", It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never());
            scriptRunner.Verify(x => x.RunScript("PurgeEmail", It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
            scriptRunner.Verify(x => x.RunScript("PurgeLabel", It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never());
            scriptRunner.Verify(x => x.RunScript("PurgePrintResult", It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never());
        }

        [TestMethod]
        public void Run_ShouldStopRunning_WhenTimeoutHasElapsed()
        {
            Mock<ISqlPurgeScriptRunner> scriptRunner = MockedScriptRunner;
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

            Mock<ISqlPurgeScriptRunner> scriptRunner = MockedScriptRunner;
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

            Mock<ISqlPurgeScriptRunner> scriptRunner = MockedScriptRunner;
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
            testObject.RetentionPeriodInDays = 8;
            testObject.StopLongPurges = false;

            testObject.Run(null, null);

            DateTime expectedDate = new DateTime(2013, 7, 17, 3, 45, 16);
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
            Mock<ISqlPurgeScriptRunner> scriptRunner = MockedScriptRunner;

            PurgeDatabaseTask testObject = new PurgeDatabaseTask(scriptRunner.Object, DefaultDateTimeProvider);
            testObject.Purges.Add(PurgeDatabaseType.Audit);
            testObject.StopLongPurges = false;
            testObject.TimeoutInHours = 4;

            testObject.Run(null, null);

            scriptRunner.Verify(x => x.RunScript(It.IsAny<string>(), It.IsAny<DateTime>(), SqlDateTime.MaxValue.Value), Times.Once());
        }

        [TestMethod]
        public void Run_ShouldThrowActionTaskRunException_WhenSqlExceptionIsCaught()
        {
            Mock<ISqlPurgeScriptRunner> scriptRunner = MockedScriptRunner;
            scriptRunner.Setup(x => x.RunScript(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Throws(new Mock<DbException>().Object);

            PurgeDatabaseTask testObject = new PurgeDatabaseTask(scriptRunner.Object, DefaultDateTimeProvider);
            testObject.Purges.Add(PurgeDatabaseType.Audit);

            try
            {
                testObject.Run(null, null);
                Assert.Fail("ActionTaskRunException should have been thrown.");
            }
            catch (ActionTaskRunException ex)
            {
                // Ensure a successful test since the correct exception was thrown.
                Assert.IsInstanceOfType(ex.InnerException, typeof(ExceptionCollection));
                Assert.IsInstanceOfType(((ExceptionCollection)ex.InnerException).Exceptions[0], typeof(DbException));
            }
        }

        [TestMethod]
        public void Run_ShouldRunSecondPurge_WhenFirstPurgeThrowsException()
        {
            Mock<ISqlPurgeScriptRunner> scriptRunner = MockedScriptRunner;

            scriptRunner.Setup(x => x.RunScript("PurgeLabel", It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Throws(new Mock<DbException>().Object);

            PurgeDatabaseTask testObject = new PurgeDatabaseTask(scriptRunner.Object, DefaultDateTimeProvider);
            testObject.Purges.Add(PurgeDatabaseType.Labels);
            testObject.Purges.Add(PurgeDatabaseType.PrintJobs);

            try
            {
                testObject.Run(null, null);
            }
            catch (ActionTaskRunException)
            {
                // We don't care about exceptions here...
            }

            scriptRunner.Verify(x => x.RunScript("PurgeLabel", It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
            scriptRunner.Verify(x => x.RunScript("PurgePrintResult", It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once());
        }

        [TestMethod]
        public void Run_ShouldThrowAggregatedException_WhenPurgesThrowsExceptions()
        {
            Mock<ISqlPurgeScriptRunner> scriptRunner = MockedScriptRunner;

            scriptRunner.Setup(x => x.RunScript("PurgeLabel", It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Throws(new Mock<DbException>().Object);
            scriptRunner.Setup(x => x.RunScript("PurgeDownload", It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Throws(new Mock<DbException>().Object);

            PurgeDatabaseTask testObject = new PurgeDatabaseTask(scriptRunner.Object, DefaultDateTimeProvider);
            testObject.Purges.Add(PurgeDatabaseType.Labels);
            testObject.Purges.Add(PurgeDatabaseType.PrintJobs);
            testObject.Purges.Add(PurgeDatabaseType.Downloads);

            try
            {
                testObject.Run(null, null);
            }
            catch (ActionTaskRunException ex)
            {
                Assert.IsTrue(ex.Message.ToLower().Contains("label"));
                Assert.IsTrue(ex.Message.ToLower().Contains("download"));
                Assert.IsInstanceOfType(ex.InnerException, typeof (ExceptionCollection));
                ExceptionCollection exceptions = (ExceptionCollection) ex.InnerException;
                Assert.AreEqual(2, exceptions.Exceptions.OfType<DbException>().Count());
            }
        }

        [TestMethod]
        public void DeserializeXml_ShouldDeserializeCorrectly()
        {
            // Create a new purge database task to serialize
            PurgeDatabaseTask initialObject = new PurgeDatabaseTask();
            initialObject.Purges.Add(PurgeDatabaseType.Audit);
            initialObject.Purges.Add(PurgeDatabaseType.Email);
            initialObject.StopLongPurges = true;
            initialObject.TimeoutInHours = 8;
            initialObject.RetentionPeriodInDays = 19;

            string serializedObject = initialObject.SerializeSettings();

            // Create a test purge database task and deserialize its settings
            PurgeDatabaseTask testObject = new PurgeDatabaseTask();
            testObject.Initialize(serializedObject);
            
            Assert.AreEqual(true, testObject.StopLongPurges);
            Assert.AreEqual(8, testObject.TimeoutInHours);
            Assert.AreEqual(19, testObject.RetentionPeriodInDays);
            Assert.AreEqual(2, testObject.Purges.Count);
            Assert.IsTrue(testObject.Purges.Contains(PurgeDatabaseType.Audit));
            Assert.IsTrue(testObject.Purges.Contains(PurgeDatabaseType.Email));
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

        private static Mock<ISqlPurgeScriptRunner> MockedScriptRunner
        {
            get
            {
                Mock<ISqlPurgeScriptRunner> mock = new Mock<ISqlPurgeScriptRunner>();
                mock.Setup(x => x.SqlUtcDateTime).Returns(new DateTime(2013, 7, 7, 1, 1, 1));
                return mock;
            }
        }
        #endregion
    }
}
