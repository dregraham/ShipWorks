using System;
using System.Data.Common;
using System.Reflection;
using Autofac.Extras.Moq;
using Interapptive.Shared.Data;
using ShipWorks.ApplicationCore.CommandLineOptions;
using ShipWorks.Data.Connection;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.Database;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Data
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class UpgradeDatabaseSchemaCommandLineOptionTest : IDisposable
    {
        private readonly Assembly thisAssembly = Assembly.GetExecutingAssembly();
        private readonly DataContext context;
        private readonly AutoMock mock;
        private readonly DatabaseFixture db;

        public UpgradeDatabaseSchemaCommandLineOptionTest(DatabaseFixture db)
        {
            this.db = db;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
            mock = context.Mock;
        }

        [Fact]
        public void DbIsSetToMultiUser_WithBadBackup_AfterCreatingBackupAndRestoring()
        {
            string connectionString = string.Empty;
            string dbName = string.Empty;
            string backupPathAndfilename = string.Empty;

            using (DbConnection connection = SqlSession.Current.OpenConnection())
            {
                connectionString = connection.ConnectionString;
                dbName = connection.Database;

                backupPathAndfilename = DbUtils.GetRestoreBackupFilename(thisAssembly, "ShipWorks.Core.Tests.Integration.DbBackups.ShipWorks_Bad.bk");

                db.ResetDatabase(connection, dbName, backupPathAndfilename);
            }

            UpgradeDatabaseSchemaCommandLineOption upgradeDatabaseSchemaCommand = new UpgradeDatabaseSchemaCommandLineOption();
            upgradeDatabaseSchemaCommand.Execute(null);

            Assert.False(SqlUtility.IsSingleUser(connectionString, dbName));
            DbUtils.DeleteBackupFile(backupPathAndfilename);

            // Reset so the original checkout is valid 
            db.ResetDatabase();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
