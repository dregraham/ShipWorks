using System;
using System.Data;
using System.Data.Common;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Data;
using Moq;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Connection;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.Database;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Data
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class SingleUserModeScopeTest : IDisposable
    {
        private readonly DataContext context;
        private readonly AutoMock mock;
        private Mock<ISqlUtility> sqlUtilityWrapper = new Mock<ISqlUtility>();

        public SingleUserModeScopeTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
            mock = context.Mock;
            mock.Provide<ISqlUtility>(new SqlUtilityWrapper());
        }

        [Fact]
        public void DbIsSetToMultiUser_OnDispose()
        {
            string connectionString = string.Empty;
            string dbName = string.Empty;

            bool isSingleUserWhenInScope = false;

            using (IDbConnection connection = SqlSession.Current.OpenConnection())
            {
                connectionString = connection.ConnectionString;
                dbName = connection.Database;

                using (SingleUserModeScope singleUserModeScope = mock.Create<SingleUserModeScope>(
                    new TypedParameter(typeof(DbConnection), connection), new TypedParameter(typeof(TimeSpan), TimeSpan.FromMinutes(1))))
                {
                    isSingleUserWhenInScope = SqlUtility.IsSingleUser(connection.ConnectionString, connection.Database);
                }
            }

            Assert.True(isSingleUserWhenInScope);
            Assert.False(SqlUtility.IsSingleUser(connectionString, dbName));
        }

        [Fact]
        public void DbIsSetToMultiUser_OnRestoreMultiUserMode()
        {
            bool isSingleUserWhenInScopeBeforeRestore = false;
            bool isSingleUserWhenInScopeAfterRestore = false;

            using (IDbConnection connection = SqlSession.Current.OpenConnection())
            {
                using (SingleUserModeScope singleUserModeScope = mock.Create<SingleUserModeScope>(
                    new TypedParameter(typeof(DbConnection), connection), new TypedParameter(typeof(TimeSpan), TimeSpan.FromMinutes(1))))
                {
                    isSingleUserWhenInScopeBeforeRestore = SqlUtility.IsSingleUser(connection.ConnectionString, connection.Database);

                    SingleUserModeScope.RestoreMultiUserMode();
                    isSingleUserWhenInScopeAfterRestore = SqlUtility.IsSingleUser(connection.ConnectionString, connection.Database);
                }
            }

            Assert.True(isSingleUserWhenInScopeBeforeRestore);
            Assert.False(isSingleUserWhenInScopeAfterRestore);
        }

        [Fact]
        public void DbIsSetToMultiUser_WhenExceptionThrownInScope()
        {
            string connectionString = string.Empty;
            string dbName = string.Empty;

            using (IDbConnection connection = SqlSession.Current.OpenConnection())
            {
                connectionString = connection.ConnectionString;
                dbName = connection.Database;

                try
                {
                    using (SingleUserModeScope singleUserModeScope = mock.Create<SingleUserModeScope>(
                        new TypedParameter(typeof(DbConnection), connection), new TypedParameter(typeof(TimeSpan), TimeSpan.FromMinutes(1))))
                    {
                        throw new Exception("testing");
                    }
                }
                catch // Catch just so we can test values and not have the method fail.
                {
                }
            }

            Assert.False(SqlUtility.IsSingleUser(connectionString, dbName));
        }

        [Fact]
        public void DbIsSetToMultiUser_WhenConnectionCloses()
        {
            string connectionString = string.Empty;
            string dbName = string.Empty;

            using (IDbConnection connection = SqlSession.Current.OpenConnection())
            {
                connectionString = connection.ConnectionString;
                dbName = connection.Database;

                using (SingleUserModeScope singleUserModeScope = mock.Create<SingleUserModeScope>(
                    new TypedParameter(typeof(DbConnection), connection), new TypedParameter(typeof(TimeSpan), TimeSpan.FromMinutes(1))))
                {
                    connection.Close();
                }
            }

            Assert.False(SqlUtility.IsSingleUser(connectionString, dbName));
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
