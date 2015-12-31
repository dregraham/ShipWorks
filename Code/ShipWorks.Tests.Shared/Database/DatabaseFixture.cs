using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
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
using SQL.LocalDB.Test;

namespace ShipWorks.Tests.Shared.Database
{
    /// <summary>
    /// Fixture that will create a database that can be used to test against
    /// </summary>
    public class DatabaseFixture : IDisposable
    {
        private readonly Checkpoint checkpoint;
        private readonly SqlSessionScope sqlSessionScope;
        private readonly ExecutionModeScope executionModeScope;

        /// <summary>
        /// Constructor
        /// </summary>
        [SuppressMessage("SonarQube", "S2930: \"IDisposables\" should be disposed",
            Justification = "We want the database to stick around after the test for debugging purposes")]
        public DatabaseFixture()
        {
            executionModeScope = new ExecutionModeScope(new TestExecutionMode());

            checkpoint = new Checkpoint();
            TempLocalDb db = new TempLocalDb("ShipWorks");

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(db.ConnectionString);

            SqlSessionConfiguration configuration = new SqlSessionConfiguration
            {
                DatabaseName = builder.InitialCatalog,
                ServerInstance = builder.DataSource,
                WindowsAuth = true
            };

            configuration.Freeze();

            sqlSessionScope = new SqlSessionScope(new SqlSession(configuration));

            //using (SqlConnection conn = db.Open())
            //{
            using (new ExistingConnectionScope())
            {
                SqlUtility.EnableClr(ExistingConnectionScope.ScopedConnection);

                using (SqlCommand command = ExistingConnectionScope.ScopedConnection.CreateCommand())
                {
                    command.CommandText = @"ALTER DATABASE ShipWorks
  SET CHANGE_TRACKING = ON
  (CHANGE_RETENTION = 1 DAYS, AUTO_CLEANUP = ON)";
                    command.ExecuteNonQuery();
                }

            }
            //}

            ShipWorksDatabaseUtility.CreateSchemaAndData();

            DataProvider.InitializeForApplication(ExecutionModeScope.Current);
        }

        /// <summary>
        /// Create a data context for use in a test
        /// </summary>
        /// <remarks>When this context is disposed, everything created inside it is rolled back.  Further,
        /// calling this method again will dispose the previous context.  This is because when a test results
        /// in an exception, the context may not be disposed properly in the test itself.</remarks>
        public DataContext CreateDataContext(AutoMock mock)
        {
            DataContext context;

            using (new AuditBehaviorScope(AuditBehaviorUser.SuperUser, AuditReason.Default, AuditState.Disabled))
            {
                checkpoint.Reset(SqlSession.Current.OpenConnection());
                context = SetupFreshData();
            }

            mock.Override<ISecurityContext>()
                .Setup(x => x.DemandPermission(It.IsAny<PermissionType>(), It.IsAny<long>()));

            foreach (IInitializeForCurrentDatabase service in mock.Container.Resolve<IEnumerable<IInitializeForCurrentDatabase>>())
            {
                service.InitializeForCurrentDatabase(ExecutionModeScope.Current);
            }

            // This initializes all the other dependencies
            UserSession.InitializeForCurrentSession();

            return context;
        }

        /// <summary>
        /// Setup fresh data
        /// </summary>
        private DataContext SetupFreshData()
        {
            using (SqlAdapter adapter = new SqlAdapter(SqlSession.Current.OpenConnection()))
            {
                adapter.DeleteEntitiesDirectly(typeof(AuditEntity), new RelationPredicateBucket());
            }

            ShipWorksDatabaseUtility.AddInitialDataAndVersion(SqlSession.Current.OpenConnection());
            ShipWorksDatabaseUtility.AddRequiredData();

            using (SqlAdapter sqlAdapter = new SqlAdapter(SqlSession.Current.OpenConnection()))
            {
                UserEntity user = Create.Entity<UserEntity>().Save(sqlAdapter);
                ComputerEntity computer = Create.Entity<ComputerEntity>().Save(sqlAdapter);

                UserSession.Logon(user, computer, true);

                return new DataContext(user, computer);
            }
        }

        /// <summary>
        /// Drop the database
        /// </summary>
        public void Dispose()
        {
            sqlSessionScope.Dispose();
            executionModeScope.Dispose();
        }
    }
}
