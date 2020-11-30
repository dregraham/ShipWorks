using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Data;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using log4net;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.CommandLineOptions;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.AutoInstall.DTO;
using ShipWorks.Data;
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
        private static readonly ILog log = LogManager.GetLogger(typeof(AutoInstaller));
        private const string AutoInstallShipWorksFileName = "AutoInstallShipWorks.config";
        private string configFilePath = string.Empty;
        private bool createdDatabase = false;
        private bool databasePreSelected = false;
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

                configFilePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), AutoInstallShipWorksFileName);
                if (!File.Exists(configFilePath))
                {
                    SetExitInfo(AutoInstallerExitCodes.InstallFailed, $"{configFilePath} was not found.");
                    return;
                }

                GetAutoInstallConfig();

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
                    SetExitInfo(AutoInstallerExitCodes.InstallFailed, "sqlSession is null");
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
        /// Load the config
        /// </summary>
        private void GetAutoInstallConfig()
        {
            try
            {
                string autoInstallShipWorksJson = File.ReadAllText(configFilePath);
                autoInstallShipWorksConfig = JsonConvert.DeserializeObject<AutoInstallShipWorksDto>(autoInstallShipWorksJson);
            }
            catch (Exception ex)
            {
                log.Error($"Exception in GetAutoInstallConfig.  ", ex);
                throw;
            }

            if (autoInstallShipWorksConfig == null)
            {
                string msg = $"{configFilePath} failed to parse.";
                log.Error($"Exception in GetAutoInstallConfig.  {msg}");
                SetExitInfo(AutoInstallerExitCodes.InstallFailed, msg);
                throw new ArgumentException(msg);
            }
        }

        /// <summary>
        /// Perform the work of creating the db, etc...
        /// </summary>
        private async Task PerformCreation(SqlSession sqlSession, string email, string password)
        {
            log.Info("PerformCreation starting");

            if (databasePreSelected)
            {
                log.Info($"PerformCreation exiting because a database was provided, {sqlSession.Configuration.DatabaseName}");
                return;
            }

            using (var sqlSessionScope = new SqlSessionScope(sqlSession))
            {
                using (ILifetimeScope scope = IoC.BeginLifetimeScope())
                {
                    // Setup for activating and creating a user.
                    var uspsAccountManager = scope.Resolve<IUspsAccountManager>();
                    var shippingSettings = scope.Resolve<IShippingSettings>();
                    var activationService = scope.Resolve<ICustomerLicenseActivationService>();
                    var userManager = scope.Resolve<IUserService>();
                    var configurationData = scope.Resolve<IConfigurationData>();

                    uspsAccountManager.InitializeForCurrentSession();
                    shippingSettings.InitializeForCurrentDatabase();

                    log.Info("PerformCreation attempting Activate");
                    activationService.Activate(email, password);

                    log.Info("PerformCreation attempting CreateUser");
                    userManager.CreateUser(email, password, true);

                    log.Info("PerformCreation attempting SaveLastUser");
                    // Save last user so that auto login works.
                    UserSession.SaveLastUser(email, password, true);

                    // If a warehouse id is provided AND the warehouse does not have a ShipWorks db already associated,
                    // and this database doesn't have a warehouse linked,
                    // link the warehouse
                    if (!string.IsNullOrWhiteSpace(autoInstallShipWorksConfig.Warehouse.ID) &&
                        string.IsNullOrWhiteSpace(autoInstallShipWorksConfig.Warehouse.Details.ShipWorksDatabaseId) &&
                        string.IsNullOrWhiteSpace(configurationData.FetchReadOnly().WarehouseID))
                    {
                        log.Info("PerformCreation attempting Warehouse Link");
                        var warehouseLinker = scope.Resolve<IWarehouseLink>();
                        var linkResult = await warehouseLinker.Link(autoInstallShipWorksConfig.Warehouse.ID).ConfigureAwait(false);

                        if (linkResult.Failure)
                        {
                            SetExitInfo(AutoInstallerExitCodes.InstallFailed, $"Linking warehouse failed. {linkResult.Message}");
                            return;
                        }

                        configurationData.UpdateConfiguration(x =>
                        {
                            x.WarehouseID = autoInstallShipWorksConfig.Warehouse.ID;
                            x.WarehouseName = autoInstallShipWorksConfig.Warehouse.Details?.Name;
                        });
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
            string sqlServerName = string.Empty;
            string databaseName = string.Empty;
            bool windowsAuth;

            log.Info($"ConfigureSqlSession starting for connectionString {autoInstallShipWorksConfig.ConnectionString}.");

            if (string.IsNullOrWhiteSpace(autoInstallShipWorksConfig.ConnectionString))
            {
                sqlServerName = "(LocalDB)\\MSSQLLocalDB";
                windowsAuth = true;
                databasePreSelected = false;
            }
            else
            {
                var connectionStringBuilder = new SqlConnectionStringBuilder(autoInstallShipWorksConfig.ConnectionString);
                sqlServerName = connectionStringBuilder.DataSource;
                databaseName = connectionStringBuilder.InitialCatalog;
                sqlUsername = connectionStringBuilder.UserID;
                sqlPassword = GetSqlPassword(connectionStringBuilder);
                windowsAuth = connectionStringBuilder.Authentication == SqlAuthenticationMethod.ActiveDirectoryIntegrated ||
                              string.IsNullOrWhiteSpace(sqlUsername) || string.IsNullOrWhiteSpace(sqlPassword);

                databasePreSelected = !string.IsNullOrWhiteSpace(databaseName);
            }

            // before doing anything make sure we can not connect to the database 
            SqlSession.Initialize();

            SqlSessionConfiguration newConfig;
            if (SqlSession.IsConfigured)
            {
                log.Info($"ConfigureSqlSession SqlSession.IsConfigured is true.");
                newConfig = new SqlSessionConfiguration(SqlSession.Current.Configuration);
            }
            else
            {
                log.Info($"ConfigureSqlSession SqlSession.IsConfigured is FALSE.");
                newConfig = new SqlSessionConfiguration();
            }

            newConfig.ServerInstance = sqlServerName;
            newConfig.Username = sqlUsername;
            newConfig.Password = sqlPassword;
            newConfig.WindowsAuth = windowsAuth;
            newConfig.DatabaseName = string.IsNullOrWhiteSpace(databaseName) ? "master" : databaseName;

            // Create a new SqlSession with updated config
            SqlSession newSqlSession = new SqlSession(newConfig);

            if (!newSqlSession.TestConnection())
            {
                SetExitInfo(AutoInstallerExitCodes.InstallFailed, $"Unable to connect to {sqlServerName}.{databaseName}.");
                return null;
            }

            return newSqlSession;
        }

        /// <summary>
        /// Get the sql password from the connection string
        /// </summary>
        private string GetSqlPassword(SqlConnectionStringBuilder connectionStringBuilder)
        {
            string sqlPassword = SecureText.Decrypt(connectionStringBuilder.Password, connectionStringBuilder.UserID);

            // If Decrypt isn't able to decrypt, it will return blank.  This could happen if the password is given
            // to us unencrypted.
            if (string.IsNullOrWhiteSpace(sqlPassword))
            {
                sqlPassword = connectionStringBuilder.Password;
            }

            return sqlPassword;
        }

        /// <summary>
        /// Save autoInstallShipWorksConfig to disk
        /// </summary>
        private void SaveAutoConfigToDisk()
        {
            try
            {
                if (autoInstallShipWorksConfig != null)
                {
                    log.Info($"SaveAutoConfigToDisk with errorMessage: {errorMessage}.");

                    autoInstallShipWorksConfig.AutoInstallErrorMessage = errorMessage;
                    string autoInstallShipWorksConfigJson = JsonConvert.SerializeObject(autoInstallShipWorksConfig);
                    File.WriteAllText(configFilePath, autoInstallShipWorksConfigJson);
                }
            }
            catch (Exception ex)
            {
                SetExitInfo(AutoInstallerExitCodes.Unknown, $"Error saving config to disk: ${ex.Message}");
                log.Error(ex);
            }
        }

        /// <summary>
        /// Install LocalDB
        /// </summary>
        private static AutoInstallerExitCodes InstallLocalDb()
        {
            log.Info("InstallLocalDb starting.");
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                ISqlInstallerRepository sqlInstallerRepository = scope.Resolve<ISqlInstallerRepository>();
                IClrHelper clrHelper = scope.Resolve<IClrHelper>();

                SqlServerInstaller installer = new SqlServerInstaller(sqlInstallerRepository, clrHelper);
                installer.InstallLocalDb(true);


                log.Info($"InstallLocalDb installer.LastExitCode: {installer.LastExitCode}.");
                return installer.LastExitCode == 0 ? AutoInstallerExitCodes.Success : AutoInstallerExitCodes.LocalDbInstallFailed;
            }
        }

        /// <summary>
        /// Create the ShipWorks database within automatic server instance
        /// </summary>
        private void CreateDatabase(SqlSession sqlSession)
        {
            log.Info("CreateDatabase starting.");

            if (databasePreSelected)
            {
                log.Info($"CreateDatabase NOT creating database as one was provided, {sqlSession.Configuration.DatabaseName}");
                return;
            }

            sqlSession.Configuration.DatabaseName = string.Empty;

            try
            {
                // Since we installed it, we can do this without asking
                using (DbConnection con = sqlSession.OpenConnection())
                {
                    log.Info("CreateDatabase attempting EnableClr.");
                    SqlUtility.EnableClr(con);
                }

                using (DbConnection con = sqlSession.OpenConnection())
                {
                    log.Info("CreateDatabase attempting CreateDatabase.");
                    ShipWorksDatabaseUtility.CreateDatabase(ShipWorksDatabaseUtility.AutomaticDatabaseName, con);

                    sqlSession.Configuration.DatabaseName = ShipWorksDatabaseUtility.AutomaticDatabaseName;

                    createdDatabase = true;
                }

                log.Info("CreateDatabase attempting ClearAllPools.");
                // Without this the next connection didn't always work...
                SqlConnection.ClearAllPools();

                using (SqlSessionScope scope = new SqlSessionScope(sqlSession))
                {
                    log.Info("CreateDatabase attempting CreateSchemaAndData.");
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
            log.Info("DropPendingDatabase starting.");
            if (!createdDatabase)
            {
                log.Info("DropPendingDatabase createdDatabase is false, skipping.");
                return;
            }

            try
            {
                log.Info("DropPendingDatabase attempting DropDatabase.");
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
            log.Info($"SetExitInfo with exitCode: {exitCode}, errorMsg: {errorMsg}");
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
