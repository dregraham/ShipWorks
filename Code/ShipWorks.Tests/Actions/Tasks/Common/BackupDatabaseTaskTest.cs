using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Actions.Tasks.Common;

namespace ShipWorks.Tests.Actions.Tasks.Common
{
    [TestClass]
    public class BackupDatabaseTaskTest
    {
        [TestMethod]
        public void DeserializeXml_ShouldDeserializeCorrectly()
        {
            // Create a new purge database task to serialize
            BackupDatabaseTask initialObject = new BackupDatabaseTask();
            initialObject.BackupDirectory = @"c:\shipworks\backup";
            initialObject.CleanOldBackups = true;
            initialObject.FilePrefix = "MyBackup";
            initialObject.KeepNumberOfBackups = 9;

            string serializedObject = initialObject.SerializeSettings();

            // Create a test purge database task and deserialize its settings
            BackupDatabaseTask testObject = new BackupDatabaseTask();
            testObject.Initialize(serializedObject);

            Assert.AreEqual(true, testObject.CleanOldBackups);
            Assert.AreEqual(9, testObject.KeepNumberOfBackups);
            Assert.AreEqual(@"c:\shipworks\backup", testObject.BackupDirectory);
            Assert.AreEqual("MyBackup", testObject.FilePrefix);
        }
    }
}
