using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Linq;
using Interapptive.Shared.Utility;
using Xunit;
using Moq;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Data.Connection;

namespace ShipWorks.Tests.Actions.Tasks.Common
{
    public class PurgeDatabaseTaskTest
    {
        private readonly string auditScript = EnumHelper.GetApiValue(PurgeDatabaseType.Audit);
        private readonly string emailScript = EnumHelper.GetApiValue(PurgeDatabaseType.Email);
        private readonly string labelScript = EnumHelper.GetApiValue(PurgeDatabaseType.Labels);
        private readonly string printResultScript = EnumHelper.GetApiValue(PurgeDatabaseType.PrintJobs);
        private readonly string abandonedResourcesScript = EnumHelper.GetApiValue(PurgeDatabaseType.AbandonedResources);

        [Fact]
        public void Run_ShouldNotExecuteAnyScripts_WhenNoPurgesAreSelected()
        {
            Mock<ISqlPurgeScriptRunner> scriptRunner = MockedScriptRunner;

            PurgeDatabaseTask testObject = new PurgeDatabaseTask(scriptRunner.Object, DefaultDateTimeProvider);

            testObject.Run(null, null);

            scriptRunner.Verify(x => x.RunScript(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime?>(), It.IsAny<int>()), Times.Never());
        }

        [Fact]
        public void Run_ShouldRunAllSelectedScripts_WhenTaskHasNoTimeout()
        {
            Mock<ISqlPurgeScriptRunner> scriptRunner = MockedScriptRunner;

            PurgeDatabaseTask testObject = new PurgeDatabaseTask(scriptRunner.Object, DefaultDateTimeProvider);
            testObject.Purges.Add(PurgeDatabaseType.Audit);
            testObject.Purges.Add(PurgeDatabaseType.Email);
            testObject.Purges.Add(PurgeDatabaseType.Labels);
            testObject.Purges.Add(PurgeDatabaseType.PrintJobs);
            testObject.CanTimeout = false;

            testObject.Run(null, null);

            scriptRunner.Verify(x => x.RunScript(auditScript, It.IsAny<DateTime>(), It.IsAny<DateTime?>(), It.IsAny<int>()), Times.Once());
            scriptRunner.Verify(x => x.RunScript(emailScript, It.IsAny<DateTime>(), It.IsAny<DateTime?>(), It.IsAny<int>()), Times.Once());
            scriptRunner.Verify(x => x.RunScript(labelScript, It.IsAny<DateTime>(), It.IsAny<DateTime?>(), It.IsAny<int>()), Times.Once());
            scriptRunner.Verify(x => x.RunScript(printResultScript, It.IsAny<DateTime>(), It.IsAny<DateTime?>(), It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public void Run_ShouldRunSelectedScripts_WhenTaskHasNoTimeout()
        {
            Mock<ISqlPurgeScriptRunner> scriptRunner = MockedScriptRunner;

            PurgeDatabaseTask testObject = new PurgeDatabaseTask(scriptRunner.Object, DefaultDateTimeProvider);
            testObject.Purges.Add(PurgeDatabaseType.Email);
            testObject.CanTimeout = false;

            testObject.Run(null, null);

            scriptRunner.Verify(x => x.RunScript(auditScript, It.IsAny<DateTime>(), It.IsAny<DateTime?>(), It.IsAny<int>()), Times.Never());
            scriptRunner.Verify(x => x.RunScript(emailScript, It.IsAny<DateTime>(), It.IsAny<DateTime?>(), It.IsAny<int>()), Times.Once());
            scriptRunner.Verify(x => x.RunScript(labelScript, It.IsAny<DateTime>(), It.IsAny<DateTime?>(), It.IsAny<int>()), Times.Never());
            scriptRunner.Verify(x => x.RunScript(printResultScript, It.IsAny<DateTime>(), It.IsAny<DateTime?>(), It.IsAny<int>()), Times.Never());
        }

        [Fact]
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
            testObject.Purges.Add(PurgeDatabaseType.Email);
            testObject.Purges.Add(PurgeDatabaseType.Labels);
            testObject.Purges.Add(PurgeDatabaseType.PrintJobs);
            testObject.CanTimeout = true;
            testObject.TimeoutInHours = 2;

            testObject.Run(null, null);

            scriptRunner.Verify(x => x.RunScript(labelScript, It.IsAny<DateTime>(), It.IsAny<DateTime?>(), It.IsAny<int>()), Times.Once());
            scriptRunner.Verify(x => x.RunScript(printResultScript, It.IsAny<DateTime>(), It.IsAny<DateTime?>(), It.IsAny<int>()), Times.Once());
            scriptRunner.Verify(x => x.RunScript(emailScript, It.IsAny<DateTime>(), It.IsAny<DateTime?>(), It.IsAny<int>()), Times.Never());
            scriptRunner.Verify(x => x.RunScript(auditScript, It.IsAny<DateTime>(), It.IsAny<DateTime?>(), It.IsAny<int>()), Times.Never());
        }

        [Fact]
        public void Run_ShouldRunScriptsInCorrectOrder_WhenAllAreSelected()
        {
            List<string> calledPurges = new List<string>();

            Mock<ISqlPurgeScriptRunner> scriptRunner = MockedScriptRunner;
            scriptRunner.Setup(x => x.RunScript(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime?>(), It.IsAny<int>()))
                .Callback<string, DateTime, DateTime?, int>((x, y, z, zz) => calledPurges.Add(x));

            PurgeDatabaseTask testObject = new PurgeDatabaseTask(scriptRunner.Object, DefaultDateTimeProvider);
            testObject.Purges.Add(PurgeDatabaseType.Audit);
            testObject.Purges.Add(PurgeDatabaseType.Email);
            testObject.Purges.Add(PurgeDatabaseType.Labels);
            testObject.Purges.Add(PurgeDatabaseType.PrintJobs);
            testObject.Purges.Add(PurgeDatabaseType.AbandonedResources);

            testObject.Run(null, null);

            Assert.AreEqual(labelScript, calledPurges[0]);
            Assert.AreEqual(printResultScript, calledPurges[1]);
            Assert.AreEqual(emailScript, calledPurges[2]);
            Assert.AreEqual(auditScript, calledPurges[3]);
            Assert.AreEqual(abandonedResourcesScript, calledPurges[4]);
        }

        [Fact]
        public void Run_ShouldRunScriptsInCorrectOrder_WhenOnlySomeAreSelected()
        {
            List<string> calledPurges = new List<string>();

            Mock<ISqlPurgeScriptRunner> scriptRunner = MockedScriptRunner;
            scriptRunner.Setup(x => x.RunScript(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime?>(), It.IsAny<int>()))
                .Callback<string, DateTime, DateTime?, int>((x, y, z, zz) => calledPurges.Add(x));

            PurgeDatabaseTask testObject = new PurgeDatabaseTask(scriptRunner.Object, DefaultDateTimeProvider);
            testObject.Purges.Add(PurgeDatabaseType.Audit);
            testObject.Purges.Add(PurgeDatabaseType.PrintJobs);
            testObject.Purges.Add(PurgeDatabaseType.AbandonedResources);

            testObject.Run(null, null);

            Assert.AreEqual(printResultScript, calledPurges[0]);
            Assert.AreEqual(auditScript, calledPurges[1]);
            Assert.AreEqual(abandonedResourcesScript, calledPurges[2]);
        }

        [Fact]
        public void Run_ShouldPassRetentionDateToRunScript()
        {
            Mock<ISqlPurgeScriptRunner> scriptRunner = new Mock<ISqlPurgeScriptRunner>();
            scriptRunner.Setup(x => x.SqlUtcDateTime).Returns(new DateTime(2013, 7, 25, 3, 45, 16));

            PurgeDatabaseTask testObject = new PurgeDatabaseTask(scriptRunner.Object, DefaultDateTimeProvider);
            testObject.Purges.Add(PurgeDatabaseType.Audit);
            testObject.RetentionPeriodInDays = 8;
            testObject.CanTimeout = false;

            testObject.Run(null, null);

            DateTime expectedDate = new DateTime(2013, 7, 17, 3, 45, 16);
            scriptRunner.Verify(x => x.RunScript(It.IsAny<string>(), expectedDate, It.IsAny<DateTime?>(), It.IsAny<int>()), Times.Exactly(2)); 
        }

        [Fact]
        public void Run_ShouldPassSqlTimeoutToRunScript_WhenStopLongPurgesIsTrue()
        {
            Mock<ISqlPurgeScriptRunner> scriptRunner = new Mock<ISqlPurgeScriptRunner>();
            scriptRunner.Setup(x => x.SqlUtcDateTime).Returns(new DateTime(2013, 7, 25, 3, 45, 16));

            PurgeDatabaseTask testObject = new PurgeDatabaseTask(scriptRunner.Object, DefaultDateTimeProvider);
            testObject.Purges.Add(PurgeDatabaseType.Audit);
            testObject.CanTimeout = true;
            testObject.TimeoutInHours = 4;

            testObject.Run(null, null);

            DateTime expectedDate = new DateTime(2013, 7, 25, 7, 45, 16);
            scriptRunner.Verify(x => x.RunScript(It.IsAny<string>(), It.IsAny<DateTime>(), expectedDate, It.IsAny<int>()), Times.Exactly(2));
        }

        [Fact]
        public void Run_ShouldPassMaxDateAsTimeToRunScript_WhenStopLongPurgesIsFalse()
        {
            Mock<ISqlPurgeScriptRunner> scriptRunner = MockedScriptRunner;

            PurgeDatabaseTask testObject = new PurgeDatabaseTask(scriptRunner.Object, DefaultDateTimeProvider);
            testObject.Purges.Add(PurgeDatabaseType.Audit);
            testObject.CanTimeout = false;
            testObject.TimeoutInHours = 4;

            testObject.Run(null, null);

            scriptRunner.Verify(x => x.RunScript(It.IsAny<string>(), It.IsAny<DateTime>(), null, It.IsAny<int>()), Times.Exactly(2));
        }

        [Fact]
        public void Run_ShouldThrowActionTaskRunException_WhenSqlExceptionIsCaught()
        {
            Mock<ISqlPurgeScriptRunner> scriptRunner = MockedScriptRunner;
            scriptRunner.Setup(x => x.RunScript(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime?>(), It.IsAny<int>()))
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

        [Fact]
        public void Run_ShouldRunSecondPurge_WhenFirstPurgeThrowsException()
        {
            Mock<ISqlPurgeScriptRunner> scriptRunner = MockedScriptRunner;

            scriptRunner.Setup(x => x.RunScript(labelScript, It.IsAny<DateTime>(), It.IsAny<DateTime?>(), It.IsAny<int>()))
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

            scriptRunner.Verify(x => x.RunScript(labelScript, It.IsAny<DateTime>(), It.IsAny<DateTime?>(), It.IsAny<int>()), Times.Once());
            scriptRunner.Verify(x => x.RunScript(printResultScript, It.IsAny<DateTime>(), It.IsAny<DateTime?>(), It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public void Run_ShouldThrowAggregatedException_WhenPurgesThrowsExceptions()
        {
            Mock<ISqlPurgeScriptRunner> scriptRunner = MockedScriptRunner;

            scriptRunner.Setup(x => x.RunScript(labelScript, It.IsAny<DateTime>(), It.IsAny<DateTime?>(), It.IsAny<int>()))
                .Throws(new Mock<DbException>().Object);
            scriptRunner.Setup(x => x.RunScript(emailScript, It.IsAny<DateTime>(), It.IsAny<DateTime?>(), It.IsAny<int>()))
                .Throws(new Mock<DbException>().Object);

            PurgeDatabaseTask testObject = new PurgeDatabaseTask(scriptRunner.Object, DefaultDateTimeProvider);
            testObject.Purges.Add(PurgeDatabaseType.Labels);
            testObject.Purges.Add(PurgeDatabaseType.PrintJobs);
            testObject.Purges.Add(PurgeDatabaseType.Email);

            try
            {
                testObject.Run(null, null);
            }
            catch (ActionTaskRunException ex)
            {
                Assert.IsTrue(ex.Message.ToLower().Contains("label"));
                Assert.IsTrue(ex.Message.ToLower().Contains("email"));
                Assert.IsInstanceOfType(ex.InnerException, typeof (ExceptionCollection));
                ExceptionCollection exceptions = (ExceptionCollection) ex.InnerException;
                Assert.AreEqual(2, exceptions.Exceptions.OfType<DbException>().Count());
            }
        }

        [Fact]
        public void DeserializeXml_ShouldDeserializeCorrectly()
        {
            // Create a new purge database task to serialize
            PurgeDatabaseTask initialObject = new PurgeDatabaseTask();
            initialObject.Purges.Add(PurgeDatabaseType.Audit);
            initialObject.Purges.Add(PurgeDatabaseType.Email);
            initialObject.CanTimeout = true;
            initialObject.TimeoutInHours = 8;
            initialObject.RetentionPeriodInDays = 19;

            string serializedObject = initialObject.SerializeSettings();

            // Create a test purge database task and deserialize its settings
            PurgeDatabaseTask testObject = new PurgeDatabaseTask();
            testObject.Initialize(serializedObject);
            
            Assert.AreEqual(true, testObject.CanTimeout);
            Assert.AreEqual(8, testObject.TimeoutInHours);
            Assert.AreEqual(19, testObject.RetentionPeriodInDays);
            Assert.AreEqual(2, testObject.Purges.Count);
            Assert.IsTrue(testObject.Purges.Contains(PurgeDatabaseType.Audit));
            Assert.IsTrue(testObject.Purges.Contains(PurgeDatabaseType.Email));
        }

        [Fact]
        public void Run_ShouldShrinkDatabase_WhenRequested()
        {
            Mock<ISqlPurgeScriptRunner> scriptRunner = MockedScriptRunner;
            PurgeDatabaseTask testObject = new PurgeDatabaseTask(scriptRunner.Object, DefaultDateTimeProvider);
            testObject.ReclaimDiskSpace = true;
            testObject.CanTimeout = false;

            testObject.Run(null, null);
            
            scriptRunner.Verify(x => x.ShrinkDatabase(), Times.Once());
        }

        [Fact]
        public void Run_ShouldNotShrinkDatabase_WhenNotRequested()
        {
            Mock<ISqlPurgeScriptRunner> scriptRunner = MockedScriptRunner;
            PurgeDatabaseTask testObject = new PurgeDatabaseTask(scriptRunner.Object, DefaultDateTimeProvider);
            testObject.ReclaimDiskSpace = false;
            testObject.CanTimeout = false;

            testObject.Run(null, null);

            scriptRunner.Verify(x => x.ShrinkDatabase(), Times.Never());
        }

        [Fact]
        public void Run_ShouldNotShrinkDatabase_WhenRequestedButTimeoutHasElapsed()
        {
            Mock<ISqlPurgeScriptRunner> scriptRunner = MockedScriptRunner;

            Mock<IDateTimeProvider> dateProvider = new Mock<IDateTimeProvider>();
            dateProvider.SetupSequence(x => x.UtcNow).Returns(new DateTime(2013, 7, 24, 12, 15, 21))
                .Returns(new DateTime(2013, 7, 24, 14, 15, 22));
            PurgeDatabaseTask testObject = new PurgeDatabaseTask(scriptRunner.Object, DefaultDateTimeProvider);
            testObject.ReclaimDiskSpace = true;
            testObject.CanTimeout = true;
            testObject.TimeoutInHours = 1;

            testObject.Run(null, null);

            scriptRunner.Verify(x => x.ShrinkDatabase(), Times.Never());
        }

        [Fact]
        public void Run_ShouldShrinkDatabase_WhenTimeoutHasElapsedButCanTimeoutIsFalse()
        {
            Mock<ISqlPurgeScriptRunner> scriptRunner = MockedScriptRunner;

            Mock<IDateTimeProvider> dateProvider = new Mock<IDateTimeProvider>();
            dateProvider.SetupSequence(x => x.UtcNow).Returns(new DateTime(2013, 7, 24, 12, 15, 21))
                .Returns(new DateTime(2013, 7, 24, 14, 15, 22));
            PurgeDatabaseTask testObject = new PurgeDatabaseTask(scriptRunner.Object, DefaultDateTimeProvider);
            testObject.ReclaimDiskSpace = true;
            testObject.CanTimeout = false;
            testObject.TimeoutInHours = 1;

            testObject.Run(null, null);

            scriptRunner.Verify(x => x.ShrinkDatabase(), Times.Once());
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
