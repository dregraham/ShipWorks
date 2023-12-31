﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Data;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using Moq;
using Respawn;
using ShipWorks.Api.Configuration;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Administration.Recovery;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Shipping;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.Database.IntegrationDB;
using ShipWorks.Tests.Shared.EntityBuilders;
using ShipWorks.Users;
using ShipWorks.Users.Audit;
using ShipWorks.Users.Security;

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
        protected bool clearTestData = true;
        private Checkpoint checkpoint;
        private SqlSessionScope sqlSessionScope;
        private ExecutionModeScope executionModeScope;
        private string databaseName;
        private readonly string databaseSource = @"localhost";

        /// <summary>
        /// Constructor
        /// </summary>
        public DatabaseFixture()
        {
            ResetDatabase();
        }

        /// <summary>
        /// Delete and create the database
        /// </summary>
        public void ResetDatabase()
        {
            string databasePrefix = string.Empty;

            try
            {
                string gitPath = Path.GetPathRoot(AppDomain.CurrentDomain.BaseDirectory) +
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory.Split('\\').Skip(1).TakeWhile(x => x != "Code").ToArray());

                string branchName = new DirectoryInfo(gitPath).Name;
                databasePrefix = branchName.Split('-').LastOrDefault();

                if (!databasePrefix.IsNumeric())
                {
                    databasePrefix = branchName.Replace("feature", string.Empty);
                }

                databasePrefix = databasePrefix
                    .Replace("-", "_")
                    .Replace("(", string.Empty)
                    .Replace(")", string.Empty).Truncate(50);
            }
            catch (Exception ex)
            {
                // Don't use a prefix if we can't find the git path
                Console.WriteLine(ex.Message);
            }

            databaseName = AppDomain.CurrentDomain
                .GetAssemblies()
                .Select(x => x.GetName().Name)
                .FirstOrDefault(x => (x.Contains("Integration") || x.Contains("Specs")) && x.Contains("ShipWorks"))
                .Replace("ShipWorks", "SW_" + databasePrefix)
                .Replace(".", "_");

            executionModeScope = new ExecutionModeScope(new TestExecutionMode());
            sqlSessionScope?.Dispose();

            if (clearTestData)
            {
                checkpoint = new Checkpoint();
                TempIntegrationDB db = new TempIntegrationDB(databaseName, databaseSource);

                sqlSessionScope = CreateSqlSessionScope(db.ConnectionString);

                SqlUtility.EnableClr(db.Open());

                using (SqlCommand command = db.Open().CreateCommand())
                {
                    command.CommandText = string.Format(enableChangeTrackingScript, databaseName);
                    command.ExecuteNonQuery();
                }

                ShipWorksDatabaseUtility.CreateSchemaAndData();
            }
            else
            {
                string connectionString = $"Data Source = {databaseSource}; Initial Catalog = {databaseName}";

                sqlSessionScope = CreateSqlSessionScope(connectionString);
            }

            DataProvider.InitializeForApplication(ExecutionModeScope.Current);
        }

        /// <summary>
        /// Delete and restore the database with given backup file
        /// </summary>
        public void ResetDatabase(DbConnection connection, string databaseName, string backupFileName)
        {
            DbUtils.RestoreDb(connection, backupFileName);

            executionModeScope = new ExecutionModeScope(new TestExecutionMode());
            sqlSessionScope?.Dispose();

            if (clearTestData)
            {
                checkpoint = new Checkpoint();
                TempIntegrationDB db = new TempIntegrationDB(databaseName, databaseSource, false);

                sqlSessionScope = CreateSqlSessionScope(db.ConnectionString);

                SqlUtility.EnableClr(db.Open());
            }
            else
            {
                string connectionString = $"Data Source = {databaseSource}; Initial Catalog = {databaseName}";

                sqlSessionScope = CreateSqlSessionScope(connectionString);
            }

            DataProvider.InitializeForApplication(ExecutionModeScope.Current);
            DbUtils.DropAssemblies();
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
            try
            {
                DataContext dataContext = null;
                SqlAdapterRetry<Exception> retry = new SqlAdapterRetry<Exception>(5, -6, "");

                retry.ExecuteWithRetry(() => dataContext = CreateDataContextInternal(initializeContainer, configureMock));

                return dataContext;
            }
            catch (Exception ex)
            {
                StringBuilder msg = new StringBuilder();
                msg.AppendLine(ex.Message);
                msg.AppendLine(ex.StackTrace);

                if (ex.InnerException != null)
                {
                    var baseException = ex.GetBaseException();
                    msg.AppendLine(baseException.Message);
                    msg.AppendLine(baseException.StackTrace);
                }

                Console.WriteLine(msg.ToString());

                throw;
            }
        }

        /// <summary>
        /// Create a data context for use in a test
        /// </summary>
        /// <remarks>
        /// When this context is disposed, everything created inside it is rolled back.  Further,
        /// calling this method again will dispose the previous context.  This is because when a test results
        /// in an exception, the context may not be disposed properly in the test itself.
        /// </remarks>
        public DataContext CreateDataContext(Action<AutoMock, ContainerBuilder> addExtraRegistrations)
        {
            AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            var container = ContainerInitializer.Initialize(builder =>
            {
                addExtraRegistrations(mock, builder);

                var securityContext = mock.Override<ISecurityContext>();
                securityContext.Setup(x => x.DemandPermission(It.IsAny<PermissionType>(), null));
                securityContext.Setup(x => x.DemandPermission(It.IsAny<PermissionType>(), It.IsAny<long>()));
                securityContext.Setup(x => x.HasPermission(It.IsAny<PermissionType>())).Returns(true);
                securityContext.Setup(x => x.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long>())).Returns(true);

                OverrideMainFormInAutofacContainer(mock, builder);
            });

            if (clearTestData)
            {
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
            }

            var context = SetupFreshData(container);

            ShippingManager.InitializeForCurrentDatabase();

            foreach (IInitializeForCurrentDatabase service in container.Resolve<IEnumerable<IInitializeForCurrentDatabase>>())
            {
                service.InitializeForCurrentDatabase(ExecutionModeScope.Current);
            }

            // This initializes all the other dependencies
            UserSession.InitializeForCurrentSession(ExecutionModeScope.Current);

            ShipWorksSession.Initialize(
                Guid.Parse("{00000000-0000-0000-0000-000000000001}"),
                Guid.Parse("{00000000-0000-0000-0000-000000000002}"),
                Guid.Parse("{00000000-0000-0000-0000-000000000003}"),
                null);

            try
            {
                DataPath.Initialize();
                SqlSessionScope.ScopedSqlSession.Configuration.RefetchSettingsFile();
                SqlSessionScope.ScopedSqlSession.Configuration.Save();
            }
            catch (IOException)
            {
                // Just ignore if we get an IOException as we don't care about the running lock.
            }

            return new DataContext(mock, context.Item1, context.Item2, DataPath.InstanceRoot, container);
        }

        /// <summary>
        /// Create a data context for use in a test
        /// </summary>
        /// <remarks>When this context is disposed, everything created inside it is rolled back.  Further,
        /// calling this method again will dispose the previous context.  This is because when a test results
        /// in an exception, the context may not be disposed properly in the test itself.</remarks>
        private DataContext CreateDataContextInternal(Action<IContainer> initializeContainer, Action<AutoMock> configureMock)
        {
            AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            initializeContainer(mock.Container);
            configureMock?.Invoke(mock);
            OverrideMainFormInAutofacContainer(mock);

            if (clearTestData)
            {
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
            }

            var context = SetupFreshData(mock.Container);

            var securityContext = mock.Override<ISecurityContext>();
            securityContext.Setup(x => x.DemandPermission(It.IsAny<PermissionType>(), null));
            securityContext.Setup(x => x.DemandPermission(It.IsAny<PermissionType>(), It.IsAny<long>()));
            securityContext.Setup(x => x.HasPermission(It.IsAny<PermissionType>())).Returns(true);
            securityContext.Setup(x => x.HasPermission(It.IsAny<PermissionType>(), It.IsAny<long>())).Returns(true);
            securityContext.Setup(x => x.RequestPermission(It.IsAny<PermissionType>(), It.IsAny<long>())).Returns(Result.FromSuccess());

            var apiSettingsRepository = mock.Override<IApiSettingsRepository>();
            apiSettingsRepository.Setup(r => r.Load()).Returns(new ApiSettings());

            ShippingManager.InitializeForCurrentDatabase();

            foreach (IInitializeForCurrentDatabase service in mock.Container.Resolve<IEnumerable<IInitializeForCurrentDatabase>>())
            {
                service.InitializeForCurrentDatabase(ExecutionModeScope.Current);
            }

            // This initializes all the other dependencies
            UserSession.InitializeForCurrentSession(ExecutionModeScope.Current);

            ShipWorksSession.Initialize(Guid.NewGuid());

            DataPath.Initialize();
            SqlSessionScope.ScopedSqlSession.Configuration.RefetchSettingsFile();
            SqlSessionScope.ScopedSqlSession.Configuration.Save();

            return new DataContext(mock, context.Item1, context.Item2, DataPath.InstanceRoot);
        }

        /// <summary>
        /// Override registration for Control, which is usually MainForm
        /// </summary>
        /// <remarks>MainForm doesn't exist in tests, so this needs to be done</remarks>
        private static void OverrideMainFormInAutofacContainer(AutoMock mock)
        {
            var builder = new ContainerBuilder();

            OverrideMainFormInAutofacContainer(mock, builder);

#pragma warning disable CS0618 // Type or member is obsolete
            builder.Update(mock.Container);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        /// <summary>
        /// Override registration for Control, which is usually MainForm
        /// </summary>
        /// <remarks>MainForm doesn't exist in tests, so this needs to be done</remarks>
        private static void OverrideMainFormInAutofacContainer(AutoMock mock, ContainerBuilder builder) =>
            builder.Register(c => new Control())
                .As<Control>()
                .As<IWin32Window>()
                .ExternallyOwned();

        /// <summary>
        /// Setup fresh data
        /// </summary>
        private Tuple<UserEntity, ComputerEntity> SetupFreshData(IContainer container)
        {
            if (clearTestData)
            {
                ShipWorksDatabaseUtility.AddInitialDataAndVersion(SqlSession.Current.OpenConnection());
                ShipWorksDatabaseUtility.AddRequiredData();
            }

            using (var lifetimeScope = container.BeginLifetimeScope())
            {
                var writer = lifetimeScope.Resolve<ICustomerLicenseWriter>();
                writer.Write(string.Empty, CustomerLicenseKeyType.WebReg);
            }

            using (SqlAdapter sqlAdapter = new SqlAdapter(SqlSession.Current.OpenConnection()))
            {
                UserEntity user;
                ComputerEntity computer;

                if (clearTestData)
                {
                    user = UserUtility.CreateUser("shipworks", "shipworks@shipworks.com", string.Empty, true);
                    computer = Create.Entity<ComputerEntity>().Save(sqlAdapter);
                }
                else
                {
                    user = UserUtility.GetShipWorksUser("shipworks", string.Empty);

                    ComputerManager.InitializeForCurrentSession();
                    computer = ComputerManager.Computers.First();
                }

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
            FilterContentManager.WaitForFiltersToComplete(TimeSpan.FromSeconds(5));
            sqlSessionScope.Dispose();
            executionModeScope.Dispose();
            var db = new IntegrationDB.IntegrationDB(databaseName, databaseSource);
            if (db.CheckExists())
            {
                db.DeleteDatabase();
            }
        }
    }
}
