using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Data;
using Moq;
using Respawn;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Tests.Shared.EntityBuilders;
using ShipWorks.Users;
using ShipWorks.Users.Audit;
using ShipWorks.Users.Security;

namespace ShipWorks.Tests.Shared.Database
{
    /// <summary>
    /// Fixture that will create a database that can be used to test against
    /// </summary>
    public class DatabaseFixture : IDisposable
    {
        private DataContext currentContext;
        readonly ShipWorksLocalDb db;
        readonly Checkpoint checkpoint;
        private readonly SqlSessionScope sqlSessionScope;

        /// <summary>
        /// Constructor
        /// </summary>
        public DatabaseFixture()
        {
            checkpoint = new Checkpoint();
            db = new ShipWorksLocalDb("ShipWorks");

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(db.ConnectionString);

            SqlSessionConfiguration configuration = new SqlSessionConfiguration
            {
                DatabaseName = builder.InitialCatalog,
                ServerInstance = builder.DataSource,
                WindowsAuth = true
            };

            configuration.Freeze();

            sqlSessionScope = new SqlSessionScope(new SqlSession(configuration));

            using (SqlConnection conn = db.Open())
            {
                using (new ExistingConnectionScope(conn))
                {
                    SqlUtility.EnableClr(conn);

                    using (SqlCommand command = conn.CreateCommand())
                    {
                        command.CommandText = @"ALTER DATABASE ShipWorks
  SET CHANGE_TRACKING = ON
  (CHANGE_RETENTION = 1 DAYS, AUTO_CLEANUP = ON)";
                        command.ExecuteNonQuery();
                    }

                    ShipWorksDatabaseUtility.CreateSchemaAndData(() => db.Open(), inTransaction => new SqlAdapter(db.Open()));
                }
            }

            DataProvider.InitializeForApplication(new TestExecutionMode());
        }

        /// <summary>
        /// Create a data context for use in a test
        /// </summary>
        /// <remarks>When this context is disposed, everything created inside it is rolled back.  Further,
        /// calling this method again will dispose the previous context.  This is because when a test results
        /// in an exception, the context may not be disposed properly in the test itself.</remarks>
        public DataContext CreateDataContext(AutoMock mock)
        {
            currentContext?.Dispose();

            using (new AuditBehaviorScope(AuditBehaviorUser.SuperUser, AuditReason.Default, AuditState.Disabled))
            {
                checkpoint.Reset(SqlSession.Current.OpenConnection());
                SetupFreshData();
            }

            mock.Override<ISecurityContext>()
                    .Setup(x => x.DemandPermission(It.IsAny<PermissionType>(), It.IsAny<long>()));

            TestExecutionMode executionMode = new TestExecutionMode();

            foreach (IInitializeForCurrentDatabase service in mock.Container.Resolve<IEnumerable<IInitializeForCurrentDatabase>>())
            {
                service.InitializeForCurrentDatabase(executionMode);
            }

            foreach (IInitializeForCurrentSession service in mock.Container.Resolve<IEnumerable<IInitializeForCurrentSession>>())
            {
                service.InitializeForCurrentSession();
            }

            currentContext = new DataContext(() => SqlSession.Current.OpenConnection(), mock);
            return currentContext;
        }

        /// <summary>
        /// Setup fresh data
        /// </summary>
        private void SetupFreshData()
        {
            using (new ExistingConnectionScope(SqlSession.Current.OpenConnection()))
            {
                using (SqlAdapter adapter = new SqlAdapter(SqlSession.Current.OpenConnection()))
                {
                    adapter.DeleteEntitiesDirectly(typeof(AuditEntity), new RelationPredicateBucket());
                }

                ShipWorksDatabaseUtility.AddInitialDataAndVersion(SqlSession.Current.OpenConnection());
                ShipWorksDatabaseUtility.AddRequiredData(SqlSession.Current.OpenConnection, inTransaction => new SqlAdapter(SqlSession.Current.OpenConnection()));

                using (SqlAdapter sqlAdapter = new SqlAdapter(SqlSession.Current.OpenConnection()))
                {
                    UserEntity user = Create.Entity<UserEntity>().Save(sqlAdapter);
                    ComputerEntity computer = Create.Entity<ComputerEntity>().Save(sqlAdapter);

                    UserSession.Logon(user, computer, true);
                }
            }
        }

        /// <summary>
        /// Drop the database
        /// </summary>
        public void Dispose()
        {
            sqlSessionScope.Dispose();
            currentContext?.Dispose();
            //db.Dispose();
        }
    }
}
