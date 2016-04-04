using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Data;
using Moq;
using Respawn;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Tests.Shared.EntityBuilders;
using ShipWorks.Users;
using ShipWorks.Users.Audit;
using ShipWorks.Users.Security;
using SQL.LocalDB.Test;

namespace ShipWorks.Tests.Shared.Database
{
    /// <summary>
    /// Fixture that will create a database that can be used to test against
    /// </summary>
    [SuppressMessage("SonarQube", "S2930: \"IDisposables\" should be disposed",
        Justification = "We want the database to stick around after the test for debugging purposes")]
    [SuppressMessage("SonarQube", "S2931: Classes with \"IDisposable\" members should implement \"IDisposable\"",
        Justification = "We're not disposing this class because some ShipWorks functions run a little after we" +
        "dispose of the sql session. This would cause the test runner to crash even though all tests" +
        "ran successfully")]
    [SuppressMessage("Code Analysis", "CA2213: Disposable fields should be disposed",
        Justification = "We don't want to dispose these fields since they need to live during the test")]
    public class DatabaseFixture : IDisposable
    {
        private readonly Checkpoint checkpoint;
        private readonly SqlSessionScope sqlSessionScope;
        private readonly ExecutionModeScope executionModeScope;

        /// <summary>
        /// Constructor
        /// </summary>
        public DatabaseFixture()
        {
            string databaseName = AppDomain.CurrentDomain
                .GetAssemblies()
                .Select(x => x.GetName().Name)
                .FirstOrDefault(x => x.Contains("Integration") && x.Contains("ShipWorks"))
                .Replace(".", "_");

            executionModeScope = new ExecutionModeScope(new TestExecutionMode());

            checkpoint = new Checkpoint();
            TempLocalDb db = new TempLocalDb(databaseName);

            sqlSessionScope = CreateSqlSessionScope(db.ConnectionString);

            SqlUtility.EnableClr(db.Open());

            using (SqlCommand command = db.Open().CreateCommand())
            {
                command.CommandText = string.Format(enableChangeTrackingScript, databaseName);
                command.ExecuteNonQuery();
            }

            ShipWorksDatabaseUtility.CreateSchemaAndData();

            DataProvider.InitializeForApplication(ExecutionModeScope.Current);
        }

        /// <summary>
        /// Create the Sql Session scope that will be used for the rest of the test run
        /// </summary>
        private static SqlSessionScope CreateSqlSessionScope(string connectionString)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);

            SqlSessionConfiguration configuration = new SqlSessionConfiguration
            {
                DatabaseName = builder.InitialCatalog,
                ServerInstance = builder.DataSource,
                WindowsAuth = true
            };

            configuration.Freeze();

            return new SqlSessionScope(new SqlSession(configuration));
        }

        /// <summary>
        /// Create a data context for use in a test
        /// </summary>
        /// <remarks>When this context is disposed, everything created inside it is rolled back.  Further,
        /// calling this method again will dispose the previous context.  This is because when a test results
        /// in an exception, the context may not be disposed properly in the test itself.</remarks>
        public DataContext CreateDataContext(Action<IContainer> initializeContainer) =>
            CreateDataContext(initializeContainer, x => { });

        /// <summary>
        /// Create a data context for use in a test
        /// </summary>
        /// <remarks>When this context is disposed, everything created inside it is rolled back.  Further,
        /// calling this method again will dispose the previous context.  This is because when a test results
        /// in an exception, the context may not be disposed properly in the test itself.</remarks>
        public DataContext CreateDataContext(Action<IContainer> initializeContainer, Action<AutoMock> configureMock)
        {
            AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            initializeContainer(mock.Container);
            configureMock?.Invoke(mock);
            OverrideMainFormInAutofacContainer(mock);

            using (new AuditBehaviorScope(AuditBehaviorUser.SuperUser, AuditReason.Default, AuditState.Disabled))
            {
                using (var connection = SqlSession.Current.OpenConnection())
                {
                    checkpoint.Reset(connection);
                    var command = connection.CreateCommand();
                    command.CommandText =
@"IF OBJECTPROPERTY(object_id('dbo.GetDatabaseGuid'), N'IsProcedure') = 1
DROP PROCEDURE [dbo].[GetDatabaseGuid]";
                    command.ExecuteNonQuery();
                }
            }

            var context = SetupFreshData();

            // Unless the test calls for something different, we're going to ignore security checks
            mock.Override<ISecurityContext>()
                .Setup(x => x.DemandPermission(It.IsAny<PermissionType>(), It.IsAny<long>()));

            foreach (IInitializeForCurrentDatabase service in mock.Container.Resolve<IEnumerable<IInitializeForCurrentDatabase>>())
            {
                service.InitializeForCurrentDatabase(ExecutionModeScope.Current);
            }

            // This initializes all the other dependencies
            UserSession.InitializeForCurrentSession(ExecutionModeScope.Current);

            return new DataContext(mock, context.Item1, context.Item2);
        }

        /// <summary>
        /// Override registration for Control, which is usually MainForm
        /// </summary>
        /// <remarks>MainForm doesn't exist in tests, so this needs to be done</remarks>
        private static void OverrideMainFormInAutofacContainer(AutoMock mock)
        {
            var builder = new ContainerBuilder();
            builder.Register(c => new Control())
                .As<Control>()
                .As<IWin32Window>()
                .ExternallyOwned();
            builder.Update(mock.Container);
        }

        /// <summary>
        /// Setup fresh data
        /// </summary>
        private Tuple<UserEntity, ComputerEntity> SetupFreshData()
        {
            ShipWorksDatabaseUtility.AddInitialDataAndVersion(SqlSession.Current.OpenConnection());
            ShipWorksDatabaseUtility.AddRequiredData();

            using (var lifetimeScope = IoC.BeginLifetimeScope())
            {
                var writer = lifetimeScope.Resolve<ICustomerLicenseWriter>();
                writer.Write(new DummyLegacyLicense());
            }

            using (SqlAdapter sqlAdapter = new SqlAdapter(SqlSession.Current.OpenConnection()))
            {
                UserEntity user = Create.Entity<UserEntity>().Save(sqlAdapter);
                ComputerEntity computer = Create.Entity<ComputerEntity>().Save(sqlAdapter);

                UserSession.Logon(user, computer, true);

                return Tuple.Create(user, computer);
            }
        }

        private const string enableChangeTrackingScript = @"ALTER DATABASE {0}
  SET CHANGE_TRACKING = ON
  (CHANGE_RETENTION = 1 DAYS, AUTO_CLEANUP = ON)";

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            sqlSessionScope.Dispose();
            executionModeScope.Dispose();
        }
    }
}
