using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Data;
using Interapptive.Shared.Utility;
using log4net;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.CommandLineOptions;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.AutoInstall.DTO;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Administration.SqlServerSetup;
using ShipWorks.Data.Administration.SqlServerSetup.SqlInstallationFiles;
using ShipWorks.Data.Connection;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Settings;
using ShipWorks.Users;

namespace ShipWorks.AutoInstall
{
    /// <summary>
    /// Class for auto installing LocalDB SQL Server, the db, user, etc...
    /// </summary>
    public class AutoInstaller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UpgradeDatabaseSchemaCommandLineOption));
        private const string AutoInstallShipWorksFileName = "AutoInstallShipWorks.config";
        private bool createdDatabase = false;
        private string errorMessage = string.Empty;
        private AutoInstallShipWorksDto autoInstallShipWorksConfig;

        /// <summary>
        /// Execute the command
        /// </summary>
        public async Task Execute(List<string> args)
        {
            log.Info("Execute starting.");

            // Assume success
            SetExitInfo(AutoInstallerExitCodes.Success);

            try
            {
                string password;
                if (args.Count >= 1)
                {
                    password = args[0];
                }
                else
                {
                    SetExitInfo(AutoInstallerExitCodes.InstallFailed, "Password was not provided.");
                    return;
                }

                if (!File.Exists(@"AutoInstallShipWorks.config"))
                {
                    SetExitInfo(AutoInstallerExitCodes.InstallFailed, "AutoInstallShipWorks.config was not found.");
                    return;
                }

                string autoInstallShipWorksJson = File.ReadAllText(AutoInstallShipWorksFileName);
                autoInstallShipWorksConfig = JsonConvert.DeserializeObject<AutoInstallShipWorksDto>(autoInstallShipWorksJson);

                if (autoInstallShipWorksConfig == null)
                {
                    SetExitInfo(AutoInstallerExitCodes.InstallFailed, $"{AutoInstallShipWorksFileName} failed to parse.");
                    return;
                }

                var email = autoInstallShipWorksConfig.TangoEmail;

                if (!ShipWorksDatabaseUtility.IsLocalDBInstalled() &&
                    string.IsNullOrWhiteSpace(autoInstallShipWorksConfig.ConnectionString))
                {
                    AutoInstallerExitCodes exitCode = InstallLocalDb();

                    if (exitCode != AutoInstallerExitCodes.Success)
                    {
                        SetExitInfo(exitCode, "LocalDB install failed.");
                        return;
                    }
                }

                SqlSession sqlSession = ConfigureSqlSession();

                if (sqlSession == null)
                {
                    return;
                }

                CreateDatabase(sqlSession);

                sqlSession.SaveAsCurrent();

                await PerformCreation(sqlSession, email, password).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                SaveAutoConfigToDisk();
            }
        }

        /// <summary>
        /// Perform the work of creating the db, etc...
        /// </summary>
        private async Task PerformCreation(SqlSession sqlSession, string email, string password)
        {
            using (var sqlSessionScope = new SqlSessionScope(sqlSession))
            {
                using (ILifetimeScope scope = IoC.BeginLifetimeScope())
                {
                    // Setup for activating and creating a user.
                    var uspsAccountManager = scope.Resolve<IUspsAccountManager>();
                    var shippingSettings = scope.Resolve<IShippingSettings>();
                    var activationService = scope.Resolve<ICustomerLicenseActivationService>();
                    var userManager = scope.Resolve<IUserService>();

                    uspsAccountManager.InitializeForCurrentSession();
                    shippingSettings.InitializeForCurrentDatabase();

                    activationService.Activate(email, password);

                    userManager.CreateUser(email, password, true);

                    // Save last user so that auto login works.
                    UserSession.SaveLastUser(email, password, true);

                    // If a warehouse id is provided AND the warehouse does not have a ShipWorks db already associated,
                    // link the warehouse
                    if (!string.IsNullOrWhiteSpace(autoInstallShipWorksConfig.Warehouse.ID) &&
                        string.IsNullOrWhiteSpace(autoInstallShipWorksConfig.Warehouse.Details.ShipWorksDatabaseId))
                    {
                        var warehouseLinker = scope.Resolve<IWarehouseLink>();
                        var linkResult = await warehouseLinker.Link(autoInstallShipWorksConfig.Warehouse.ID).ConfigureAwait(false);

                        if (linkResult.Failure)
                        {
                            SetExitInfo(AutoInstallerExitCodes.InstallFailed, $"Linking warehouse failed. {linkResult.Message}");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Configure the sql session to use
        /// </summary>
        /// <returns></returns>
        private SqlSession ConfigureSqlSession()
        {
            string sqlUsername = string.Empty;
            string sqlPassword = string.Empty;
            string sqlServerName;
            bool windowsAuth;
            string email;
            if (string.IsNullOrWhiteSpace(autoInstallShipWorksConfig.ConnectionString))
            {
                sqlServerName = "(LocalDB)\\MSSQLLocalDB";
                windowsAuth = true;
            }
            else
            {
                var connectionStringBuilder = new SqlConnectionStringBuilder(autoInstallShipWorksConfig.ConnectionString);
                sqlServerName = connectionStringBuilder.DataSource;
                sqlUsername = connectionStringBuilder.UserID;
                sqlPassword = connectionStringBuilder.Password;
                windowsAuth = connectionStringBuilder.Authentication == SqlAuthenticationMethod.ActiveDirectoryIntegrated ||
                              string.IsNullOrWhiteSpace(sqlUsername) || string.IsNullOrWhiteSpace(sqlPassword);
            }

            // before doing anything make sure we can not connect to the database 
            SqlSession.Initialize();

            var newConfig = new SqlSessionConfiguration(SqlSession.Current.Configuration)
            {
                ServerInstance = sqlServerName,
                Username = sqlUsername,
                Password = sqlPassword,
                WindowsAuth = windowsAuth,
                DatabaseName = "master"
            };


            // Create a new SqlSession with updated config
            SqlSession newSqlSession = new SqlSession(newConfig);

            if (!newSqlSession.TestConnection())
            {
                SetExitInfo(AutoInstallerExitCodes.InstallFailed, "Unable to connect to LocalDB.");
                return null;
            }

            return newSqlSession;
        }

        /// <summary>
        /// Save autoInstallShipWorksConfig to disk
        /// </summary>
        private void SaveAutoConfigToDisk()
        {
            try
            {
                autoInstallShipWorksConfig.AutoInstallErrorMessage = errorMessage;
                string autoInstallShipWorksConfigJson = JsonConvert.SerializeObject(autoInstallShipWorksConfig);
                File.WriteAllText(AutoInstallShipWorksFileName, autoInstallShipWorksConfigJson);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        /// <summary>
        /// Install LocalDB
        /// </summary>
        private static AutoInstallerExitCodes InstallLocalDb()
        {
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                ISqlInstallerRepository sqlInstallerRepository = scope.Resolve<ISqlInstallerRepository>();
                IClrHelper clrHelper = scope.Resolve<IClrHelper>();

                SqlServerInstaller installer = new SqlServerInstaller(sqlInstallerRepository, clrHelper);
                installer.InstallLocalDb(true);
                return installer.LastExitCode == 0 ? AutoInstallerExitCodes.Success : AutoInstallerExitCodes.LocalDbInstallFailed;
            }
        }

        /// <summary>
        /// Create the ShipWorks database within automatic server instance
        /// </summary>
        private void CreateDatabase(SqlSession sqlSession)
        {
            sqlSession.Configuration.DatabaseName = string.Empty;

            try
            {
                // Since we installed it, we can do this without asking
                using (DbConnection con = sqlSession.OpenConnection())
                {
                    SqlUtility.EnableClr(con);
                }

                using (DbConnection con = sqlSession.OpenConnection())
                {
                    ShipWorksDatabaseUtility.CreateDatabase(ShipWorksDatabaseUtility.AutomaticDatabaseName, con);

                    sqlSession.Configuration.DatabaseName = ShipWorksDatabaseUtility.AutomaticDatabaseName;

                    createdDatabase = true;
                }

                // Without this the next connection didn't always work...
                SqlConnection.ClearAllPools();

                using (SqlSessionScope scope = new SqlSessionScope(sqlSession))
                {
                    ShipWorksDatabaseUtility.CreateSchemaAndData();
                }
            }
            // If something goes wrong, drop the db we just created
            catch (Exception ex)
            {
                HandleException(ex);
                DropPendingDatabase(sqlSession);

                throw;
            }
        }

        /// <summary>
        /// Drop the database that we created due to the use going back or canceling the wizard.
        /// </summary>
        private void DropPendingDatabase(SqlSession sqlSession)
        {
            if (!createdDatabase)
            {
                return;
            }

            try
            {
                ShipWorksDatabaseUtility.DropDatabase(sqlSession, ShipWorksDatabaseUtility.AutomaticDatabaseName);
            }
            catch (SqlException ex)
            {
                SetExitInfo(AutoInstallerExitCodes.InstallFailed, $"Exception occurred while attempting to drop database.  Exception: {ex.Message}");
                HandleException(ex);
                
                throw;
            }

            sqlSession.Configuration.DatabaseName = "";
            createdDatabase = false;
        }

        /// <summary>
        /// Set exit code
        /// </summary>
        private void SetExitInfo(AutoInstallerExitCodes exitCode, string errorMsg = "")
        {
            Environment.ExitCode = (int) exitCode;
            errorMessage = errorMsg;
        }

        /// <summary>
        /// Handle an exception
        /// </summary>
        private void HandleException(Exception ex)
        {
            log.Error(ex);

            if (Environment.ExitCode == (int) AutoInstallerExitCodes.Success)
            {
                SetExitInfo(AutoInstallerExitCodes.Unknown, ex.Message);
            }
        }
    }
}
