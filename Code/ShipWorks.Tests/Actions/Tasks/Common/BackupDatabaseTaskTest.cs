using Xunit;
using ShipWorks.Actions.Tasks.Common;

namespace ShipWorks.Tests.Actions.Tasks.Common
{
    public class BackupDatabaseTaskTest
    {
        [Fact]
        public void Initialize_DeserializesXmlCorrectly_Test()
        {
            // Create a new purge database task to serialize
            BackupDatabaseTask initialObject = new BackupDatabaseTask();
            initialObject.BackupDirectory = @"c:\shipworks\backup";
            initialObject.LimitNumberOfBackupsRetained = true;
            initialObject.FilePrefix = "MyBackup";
            initialObject.KeepNumberOfBackups = 9;

            string serializedObject = initialObject.SerializeSettings();

            // Create a test purge database task and deserialize its settings
            BackupDatabaseTask testObject = new BackupDatabaseTask();
            testObject.Initialize(serializedObject);

            Assert.AreEqual(true, testObject.LimitNumberOfBackupsRetained);
            Assert.AreEqual(9, testObject.KeepNumberOfBackups);
            Assert.AreEqual(@"c:\shipworks\backup", testObject.BackupDirectory);
            Assert.AreEqual("MyBackup", testObject.FilePrefix);
        }
    }
}
