using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Autofac;
using Autofac.Core.Lifetime;
using Interapptive.Shared;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Data;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32;
using log4net;
using Microsoft.Win32;
using NDesk.Options;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Administration.SqlServerSetup.SqlInstallationFiles;
using ShipWorks.Data.Connection;

namespace ShipWorks.Data.Administration.SqlServerSetup
{
    /// <summary>
    /// Use to control the installation process of SQL Server
    /// </summary>
    [SuppressMessage("CSharp.Analyzers",
        "CA5350: Do not use insecure cryptographic algorithm SHA1",
        Justification = "This is what ShipWorks currently uses")]
    public class SqlServerInstaller
    {
        // Logger
        private static readonly ILog log = LogManager.GetLogger(typeof(SqlServerInstaller));
        private readonly IClrHelper clrHelper;
        private readonly IEnumerable<ISqlInstallerInfo> sqlInstallerInfos;

        // Custom install error codes
        const int msdeUpgrade08Failed = -200;
        const int unknownException = -201;

        // A value of exactly OxBC2 means installation was successful, but a reboot is now required.
        // Anything else (such as 0x0XXX0BC2) means a reboot is required before install.
        const int successRebootRequired = 0xBC2;

        /// <summary>
        /// For SQL 2012 this sometimes seems to indicate a failure to install b\c of the presence of MSDE
        /// </summary>
        const int failedDueToMsdeExists = -2068774911;

        // The exit code from the last attempt at installation
        int lastExitCode = 0;

        /// <summary>
        /// Raised when the installation executable has exited
        /// </summary>
        public event EventHandler Exited;
        
        /// <summary>
        /// Static constructor
        /// </summary>
        public SqlServerInstaller(ISqlInstallerRepository sqlInstallerRepository, IClrHelper clrHelper)
        {
            this.clrHelper = clrHelper;
            sqlInstallerInfos = sqlInstallerRepository.SqlInstallersForThisMachine();
        }

        #region Initialization and Detection

        /// <summary>
        /// The exit code that indicates that installation was successful, but a reboot is now required.
        /// </summary>
        public static int ExitCodeSuccessRebootRequired
        {
            get { return successRebootRequired; }
        }

        /// <summary>
        /// Indicates if SQL Server 2016 is supported on the current computer
        /// </summary>
        public bool IsSqlServer2016Supported
        {
            get
            {
                return sqlInstallerInfos.Any(si => si.Edition == SqlServerEditionType.Express2016 || si.Edition == SqlServerEditionType.LocalDb2016);
            }
        }

        /// <summary>
        /// Indicates if SQL Server 2014 is supported on the current computer
        /// </summary>
        public bool IsSqlServer2014Supported
        {
            get
            {
                return sqlInstallerInfos.Any(si => si.Edition == SqlServerEditionType.Express2014 || si.Edition == SqlServerEditionType.LocalDb2014);
            }
        }

        /// <summary>
        /// SQL Server requires Windows Installer 4.5
        /// </summary>
        public static bool IsWindowsInstallerRequired
        {
            get
            {
                Version version = MyComputer.WindowsInstallerVersion;

                if (version.Major > 4)
                {
                    return false;
                }

                if (version.Major == 4 && version.Minor >= 5)
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// SQL Server requires .NET 3.5
        /// </summary>
        public bool IsDotNet35Sp1Installed
        {
            get
            {
                // If we will be running SQL Server 2014, we don't need .Net 3.5 SP1 to be installed.
                if (IsSqlServer2014Supported)
                {
                    return true;
                }

                clrHelper.Reload();

                Version net4Version = new Version(4, 0, 0, 0);
                Version net35Sp1Version = new Version(3, 5, 30729, 0);

                return clrHelper.ClrVersions.Any(installedClrVersion => installedClrVersion >= net35Sp1Version && installedClrVersion < net4Version);
            }
        }

        /// <summary>
        /// Validate that the installer has been initialized for the given purpose, or throw
        /// </summary>
        private void ValidateInitialized(ISqlInstallerInfo sqlInstallerInfo)
        {
            if (sqlInstallerInfo == null)
            {
                throw new InvalidOperationException("The SqlServerInstaller has not been initialized.");
            }

            if (sqlInstallerInfo.IsLocalDB && !(IsSqlServer2016Supported || IsSqlServer2014Supported))
            {
                throw new InvalidOperationException("Cannot install LocalDB when SQL 2016 is not supported.");
            }
        }

        #endregion

        #region Local and Remote Paths

        /// <summary>
        /// The path to the location of the SQL installer for new installations on the local disk
        /// </summary>
        public string GetInstallerLocalFilePath(ISqlInstallerInfo sqlInstallerInfo)
        {
            ValidateInitialized(sqlInstallerInfo);
            
            return sqlInstallerInfo.LocalFilePath;
        }

        /// <summary>
        /// The full Uri for downloading the SQL Express installer used for new installations
        /// </summary>
        public ISqlInstallerInfo GetSqlInstaller(SqlServerInstallerPurpose purpose)
        {
            ISqlInstallerInfo sqlInstallerInfo;

            if (purpose == SqlServerInstallerPurpose.LocalDb)
            {
                sqlInstallerInfo = sqlInstallerInfos.First(si => si.Edition == SqlServerEditionType.LocalDb2016 ||
                                                             si.Edition == SqlServerEditionType.LocalDb2014);
            }
            else
            {
                sqlInstallerInfo = sqlInstallerInfos.First(si => si.Edition == SqlServerEditionType.Express2016 ||
                                                             si.Edition == SqlServerEditionType.Express2014 ||
                                                             si.Edition == SqlServerEditionType.Express2008R2Sp2);
            }

            ValidateInitialized(sqlInstallerInfo);

            return sqlInstallerInfo;
        }

        /// <summary>
        /// Determines if the installer for SQL Express installer is valid, as in not partially downloaded or corrupted.
        /// </summary>
        public bool IsInstallerLocalFileValid(SqlServerInstallerPurpose purpose)
        {
            ISqlInstallerInfo sqlInstallerInfo = GetSqlInstaller(purpose);

            string localExe = GetInstallerLocalFilePath(sqlInstallerInfo);

            if (!File.Exists(localExe))
            {
                return false;
            }

            Cursor.Current = Cursors.WaitCursor;

            byte[] computedChecksum;

            using (SHA1 sha = SHA1.Create())
            {
                using (FileStream stream = File.OpenRead(localExe))
                {
                    stream.Position = 0;
                    computedChecksum = sha.ComputeHash(stream);
                }
            }

            Cursor.Current = Cursors.Default;

            return Convert.ToBase64String(computedChecksum) == sqlInstallerInfo.Checksum;
        }

        /// <summary>
        /// Download the SQL Server installer to be used for the given purpose
        /// </summary>
        private string DownloadSqlServer(SqlServerInstallerPurpose purpose)
        {
            ISqlInstallerInfo sqlInstallerInfo = GetSqlInstaller(purpose);
            string localFile = GetInstallerLocalFilePath(sqlInstallerInfo);

            if (!IsInstallerLocalFileValid(purpose))
            {
                Uri remoteUri = sqlInstallerInfo.DownloadUri;

                log.InfoFormat("Using WebClient to download {0}", remoteUri);

                // We can do this synchronously because we are already running as a secondary background elevated process, and won't be blocking any UI
                WebClient webClient = new WebClient();
                webClient.DownloadFile(remoteUri, localFile);
            }

            return localFile;
        }

        #endregion

        #region Installation

        /// <summary>
        /// Begin the installation of the SQL Server.  The method returns before the installation is complete.
        /// </summary>
        public void InstallSqlServer(string instanceName, string sapassword)
        {
            log.Info("Launching external process to elevate permissions to install SQL server.");
            lastExitCode = 0;

            // We need to launch the process to elevate ourselves.  We'll just do it this way for XP too for code path
            // simplification.
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(Application.ExecutablePath, string.Format("/command:sqlserver -action=install -instance=\"{0}\" -password=\"{1}\"", instanceName, sapassword));
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.EnableRaisingEvents = true;
            process.Exited += new EventHandler(OnInstallerExited);

            // Elevate for vista
            if (MyComputer.IsWindowsVistaOrHigher)
            {
                process.StartInfo.Verb = "runas";
            }

            process.Start();
        }

        /// <summary>
        /// The actual installation that occurs in a seperate elevated process
        /// </summary>
        private void InstallSqlServerInternal(string instanceName, string sapassword)
        {
            // First we have to make sure it's downloaded
            string installerPath = DownloadSqlServer(SqlServerInstallerPurpose.Install);

            string args = GetInstallArgs(instanceName, sapassword, true);

            log.InfoFormat("Executing: {0}", args);

            int exitCode;
            using (Process process = new Process())
            {
                try
                {
                    process.StartInfo = new ProcessStartInfo(installerPath, args);
                    process.Start();
                    process.WaitForExit();

                    exitCode = process.ExitCode;

                    // If the install was a succses, open the firewall as long as we're already elevated
                    if (exitCode == 0 || exitCode == ExitCodeSuccessRebootRequired)
                    {
                        try
                        {
                            SqlWindowsFirewallUtility.OpenWindowsFirewallElevated(instanceName);

                            log.Info("Windows firewall opened.");
                        }
                        catch (Win32Exception ex)
                        {
                            log.Error("Failed to open windows firewall", ex);
                        }
                        catch (WindowsFirewallException ex)
                        {
                            log.Error("Failed to open windows firewall", ex);
                        }
                    }
                }
                catch (Win32Exception ex)
                {
                    log.Error("Error installing sql server", ex);
                    exitCode = ex.ErrorCode;
                }
                catch (Exception ex)
                {
                    log.Error("Error installing sql server", ex);
                    exitCode = unknownException;
                }
            }

            log.InfoFormat("Exiting with code: {0}", exitCode);
            Environment.ExitCode = exitCode;
        }

        /// <summary>
        /// Get the command line arguments for a fresh installation of SQL Server
        /// </summary>
        private static string GetInstallArgs(string instanceName, string sapassword, bool fullQuiet)
        {
            StringBuilder args = new StringBuilder();
            args.AppendFormat(fullQuiet ? "/Q " : "/QUIETSIMPLE ");
            args.AppendFormat("/HIDECONSOLE ");
            args.AppendFormat("/ACTION=Install ");
            args.AppendFormat("/IAcceptSQLServerLicenseTerms ");
            args.AppendFormat("/FEATURES=SQLENGINE ");
            args.AppendFormat("/INSTANCENAME=\"{0}\" ", instanceName);
            args.AppendFormat("/SQLSVCACCOUNT=\"NT AUTHORITY\\SYSTEM\" ");
            args.AppendFormat("/SQLSYSADMINACCOUNTS=\"BUILTIN\\Administrators\" ");
            args.AppendFormat("/ADDCURRENTUSERASSQLADMIN ");
            args.AppendFormat("/TCPENABLED=1 ");
            args.AppendFormat("/NPENABLED=1 ");
            args.AppendFormat("/SQLSVCSTARTUPTYPE=Automatic ");
            args.AppendFormat("/BROWSERSVCSTARTUPTYPE=Automatic ");
            args.AppendFormat("/SECURITYMODE=SQL ");
            args.AppendFormat("/SAPWD=\"{0}\" ", sapassword);

            return args.ToString();
        }

        #endregion

        #region LocalDB

        /// <summary>
        /// Kick off the installation of SQL Server Local DB,  The method returns before the installation is complete.
        /// </summary>
        public void InstallLocalDb()
        {
            log.Info("Launching external process to elevate permissions to install SQL server Local DB.");
            lastExitCode = 0;

            // We need to launch the process to elevate ourselves.
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(Application.ExecutablePath, "/command:sqlserver -action=localdb");
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.EnableRaisingEvents = true;
            process.Exited += new EventHandler(OnInstallerExited);

            // Elevate for vista
            if (MyComputer.IsWindowsVistaOrHigher)
            {
                process.StartInfo.Verb = "runas";
            }

            process.Start();
        }

        /// <summary>
        /// The actual installation that occurs in a seperate elevated process
        /// </summary>
        private void InstallLocalDbInternal()
        {
            // First we have to make sure it's downloaded
            string installerPath = DownloadSqlServer(SqlServerInstallerPurpose.LocalDb);

            string args = "/qn IACCEPTSQLLOCALDBLICENSETERMS=YES";

            log.InfoFormat("Executing: {0}", args);

            Process process = new Process();
            int exitCode;

            try
            {
                process.StartInfo = new ProcessStartInfo(installerPath, args);
                process.Start();
                process.WaitForExit();

                exitCode = process.ExitCode;
            }
            catch (Win32Exception ex)
            {
                log.Error("Error installing sql server", ex);
                exitCode = ex.ErrorCode;
            }
            catch (Exception ex)
            {
                log.Error("Error installing sql server", ex);
                exitCode = unknownException;
            }

            log.InfoFormat("Exiting with code: {0}", exitCode);
            Environment.ExitCode = exitCode;
        }

        /// <summary>
        /// "Upgrade" a LocalDB instance to a full instance of SQL Server.  This is done by installing a full instance, and migrating the data files to it.
        /// </summary>
        public string UpgradeLocalDb()
        {
            log.Info("Launching external process to elevate permissions to upgrade SQL server Local DB.");
            lastExitCode = 0;

            string instanceName = DetermineLocalDbUpgradedInstanceName();

            // We need to launch the process to elevate ourselves.
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(Application.ExecutablePath, string.Format("/command:sqlserver -action=upgradelocaldb -instance=\"{0}\"", instanceName));
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.EnableRaisingEvents = true;
            process.Exited += new EventHandler(OnInstallerExited);

            // Elevate for vista
            if (MyComputer.IsWindowsVistaOrHigher)
            {
                process.StartInfo.Verb = "runas";
            }

            process.Start();

            return instanceName;
        }

        /// <summary>
        /// The actual installation that occurs in a seperate elevated process
        /// </summary>
        [NDependIgnoreLongMethod]
        private void UpgradeLocalDbInternal(string instanceName)
        {
            string serverInstance = Environment.MachineName + "\\" + instanceName;

            int exitCode = 0;

            try
            {
                // If this instance is already installed, then we are reusing an existing instance.  If it's not, we need to install it now
                if (!SqlInstanceUtility.IsSqlInstanceInstalled(instanceName))
                {
                    // Go ahead and do the full install
                    InstallSqlServerInternal(instanceName, SqlInstanceUtility.ShipWorksSaPassword);

                    // Record this as our upgraded SQL Server.  It may not have even worked - that's ok. B\c when we come back through again, we won't be able to connect,
                    // and we'll use another name.
                    using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"Software\Interapptive\ShipWorks\Database"))
                    {
                        key.SetValue("Automatic", instanceName);
                    }

                    // If that wasn't succesfull, go ahead and punt now
                    if (Environment.ExitCode != 0)
                    {
                        return;
                    }
                }

                //
                // At this point, SQL Server should be succesfully installed and running.  Now we need to move the mdf\ldf
                //

                DatabaseFileInfo fileInfo;

                // Old LocalDb session
                SqlSession localDbSession = SqlSession.Current;
                if (localDbSession.Configuration.ServerInstance != SqlInstanceUtility.LocalDbServerInstance)
                {
                    throw new InvalidOperationException("Cannot upgrade a non-LocalDb server instance.");
                }

                // New upgraded session
                SqlSession newSession = new SqlSession(SqlInstanceUtility.DetermineCredentials(serverInstance));

                // Detatch the database from LocalDb
                log.InfoFormat("Detaching mdf-ldf from LocalDB");
                using (DbConnection con = localDbSession.OpenConnection())
                {
                    fileInfo = ShipWorksDatabaseUtility.DetachDatabase(localDbSession.Configuration.DatabaseName, con);
                }

                // Path where the db files are moving
                string newFilePath;

                // Figure out the path where they are going to go
                using (DbConnection con = newSession.OpenConnection())
                {
                    newFilePath = SqlUtility.GetMasterDataFilePath(con);
                }

                // Get the next available database name to use
                string databaseName = AssignAutomaticDatabaseNameInternal();

                // If it returned nul, there was an error and Environment.ExitCode is already set.
                if (databaseName == null)
                {
                    return;
                }

                // Determine what filename is available in the new folder.  Most likely there won't be a conflict, but this ensures it
                string dataFileBase = ShipWorksDatabaseUtility.DetermineAvailableFileName(newFilePath, databaseName);

                string targetMdf = Path.Combine(newFilePath, dataFileBase + ".mdf");
                string targetLdf = Path.Combine(newFilePath, dataFileBase + "_log.ldf");

                // Physically move the files
                File.Move(fileInfo.DataFile, targetMdf);
                File.Move(fileInfo.LogFile, targetLdf);

                log.InfoFormat("Attaching database {0} into newly installed SQL instance.", fileInfo.Database);

                // Now we attach the db files into the full instance
                using (DbConnection con = newSession.OpenConnection())
                {
                    DbCommandProvider.ExecuteNonQuery(con, string.Format(
                                    @"CREATE DATABASE {0}
                                    ON (FILENAME = '{1}'),
                                       (FILENAME = '{2}')
                                    FOR ATTACH", databaseName, targetMdf, targetLdf));
                }
            }
            catch (Win32Exception ex)
            {
                log.Error("Error upgrading local DB", ex);
                exitCode = ex.ErrorCode;
            }
            catch (Exception ex)
            {
                log.Error("Error installing sql server", ex);
                exitCode = unknownException;
            }

            Environment.ExitCode = exitCode;
        }

        /// <summary>
        /// Get the instance name that will be used (or has already been used) for the LocalDB upgraded SQL Server instance
        /// </summary>
        private string DetermineLocalDbUpgradedInstanceName()
        {
            // See what the automatic instance is considered to be now
            string automaticInstance = SqlInstanceUtility.AutomaticServerInstance;

            // If its LocalDB, then we need to generate a new instance name to be used for the full version to upgrade local db to
            if (automaticInstance == SqlInstanceUtility.LocalDbServerInstance)
            {
                // Now, figure out what the instance name will be
                string baseName = "ShipWorks";
                string instanceName = baseName;

                int index = 1;

                while (SqlInstanceUtility.IsSqlInstanceInstalled(instanceName))
                {
                    instanceName = string.Format("{0}{1}", baseName, index++);
                }

                return instanceName;
            }
            else
            {
                // we must have already upgraded it to full sql server in the past - just use it
                return SqlInstanceUtility.ExtractInstanceName(automaticInstance);
            }
        }

        /// <summary>
        /// Assign the automatic database name to use for the automatic full server instance that LocalDb was upgraded to
        /// </summary>
        public void AssignAutomaticDatabaseName()
        {
            log.Info("Launching external process to elevate permissions to assign the automatic database name.");
            lastExitCode = 0;

            // We need to launch the process to elevate ourselves.
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(Application.ExecutablePath, "/command:sqlserver -action=assignautomaticdbname");
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.EnableRaisingEvents = true;
            process.Exited += new EventHandler(OnInstallerExited);

            // Elevate for vista
            if (MyComputer.IsWindowsVistaOrHigher)
            {
                process.StartInfo.Verb = "runas";
            }

            process.Start();
        }

        /// <summary>
        /// Excecutes in an elevated process to assign the name to use for the database in the full version of sql server that LocalDb was upgraded to
        /// </summary>
        private string AssignAutomaticDatabaseNameInternal()
        {
            int exitCode = 0;

            try
            {
                // At this point, the AutomaticServerInstance has to be created
                if (SqlInstanceUtility.AutomaticServerInstance == SqlInstanceUtility.LocalDbServerInstance)
                {
                    throw new InvalidOperationException("Automatic instance should be created already.");
                }

                SqlSession session = new SqlSession(SqlInstanceUtility.DetermineCredentials(SqlInstanceUtility.AutomaticServerInstance));

                using (DbConnection con = session.OpenConnection())
                {
                    string databaseName = ShipWorksDatabaseUtility.GetFirstAvailableDatabaseName(con);

                    // Now record that this is the database that is to be used for full instances for this path by default
                    using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"Software\Interapptive\ShipWorks\Database"))
                    {
                        key.SetValue(ShipWorksSession.InstanceID.ToString("B"), databaseName);
                    }

                    return databaseName;
                }
            }
            catch (Win32Exception ex)
            {
                log.Error("Error upgrading local DB", ex);
                exitCode = ex.ErrorCode;
            }
            catch (Exception ex)
            {
                log.Error("Error installing sql server", ex);
                exitCode = unknownException;
            }

            Environment.ExitCode = exitCode;
            return null;
        }

        #endregion

        #region Upgrading

        /// <summary>
        /// Begin the upgrade to SQL Server on the instance that is referred to by the current SQL Session.  The method returns before the upgrade is complete.
        /// </summary>
        public void UpgradeSqlServer()
        {
            log.Info("Launching external process to elevate permissions to upgrade SQL server.");
            lastExitCode = 0;

            // We need to launch the process to elevate ourselves.  We'll just do it this way for XP too for code path
            // simplification.
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(Application.ExecutablePath, "/command:sqlserver -action=upgrade");
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.EnableRaisingEvents = true;
            process.Exited += new EventHandler(OnInstallerExited);

            // Elevate for vista
            if (MyComputer.IsWindowsVistaOrHigher)
            {
                process.StartInfo.Verb = "runas";
            }

            process.Start();
        }

        /// <summary>
        /// The actual upgrade logic that runs in a seperate elevated process
        /// </summary>
        private static void UpgradeSqlServerInternal(string installerPath, SqlSession sqlSession)
        {
            try
            {
                UpgradeSqlServerNormally(installerPath, sqlSession);
            }
            catch (Win32Exception ex)
            {
                log.Error("Error upgrading sql server", ex);
                Environment.ExitCode = ex.ErrorCode;
            }
            catch (Exception ex)
            {
                log.Error("Error upgrading sql server", ex);
                Environment.ExitCode = unknownException;
            }
        }

        /// <summary>
        /// Upgrade SQL Server using the documented method of upgrading
        /// </summary>
        private static void UpgradeSqlServerNormally(string installerPath, SqlSession sqlSession)
        {
            string instance = SqlInstanceUtility.ExtractInstanceName(sqlSession.Configuration.ServerInstance);

            string args = GetUpgradeArgs(instance, Path.GetFileName(installerPath).Contains("x86", StringComparison.InvariantCultureIgnoreCase));
            log.InfoFormat("Executing: {0}", args);

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(installerPath, args);
            process.Start();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                log.ErrorFormat("Upgrade SQL Server failed: {0}", process.ExitCode);
                Environment.ExitCode = process.ExitCode;
                return;
            }

            log.InfoFormat("SQL Server upgrade complete");

            // Make sure its running and accepting connections before moving on
            ConnectUntilSuccess(sqlSession, TimeSpan.FromSeconds(15));

            // If we are not using windows auth we have to be sure builtin\admins has access, so that we can connect using windows auth
            try
            {
                using (DbConnection con = sqlSession.OpenConnection())
                {
                    try
                    {
                        log.InfoFormat("Ensuring [{0}] has login.", WindowsIdentity.GetCurrent().Name);
                        DbCommandProvider.ExecuteNonQuery(con, string.Format("CREATE LOGIN [{0}] FROM WINDOWS", WindowsIdentity.GetCurrent().Name));
                    }
                    catch (SqlException ex)
                    {
                        log.Warn("Could not add login (may already exist): " + ex.Message);
                    }

                    log.InfoFormat("Ensuring is sysadmin");
                    DbCommandProvider.ExecuteNonQuery(con, string.Format(@"EXEC sp_addsrvrolemember @loginame = N'{0}', @rolename = N'sysadmin'", WindowsIdentity.GetCurrent().Name));
                }
            }
            catch (SqlException ex)
            {
                log.Warn("Creating windows user as sysadmin failed", ex);
            }

            EnableRemoteConnections(sqlSession);
        }

        /// <summary>
        /// Enable remote connectinos for the given sql instance
        /// </summary>
        private static void EnableRemoteConnections(SqlSession sqlSession)
        {
            string instance = SqlInstanceUtility.ExtractInstanceName(sqlSession.Configuration.ServerInstance);

            ManagementScope scope = new ManagementScope(@"\\.\root\Microsoft\SqlServer\ComputerManagement10");

            ManagementClass serverProtocols = new ManagementClass(scope, new ManagementPath("ServerNetworkProtocol"), null);
            serverProtocols.Get();

            foreach (ManagementObject protocolObject in serverProtocols.GetInstances())
            {
                protocolObject.Get();
                string protocol = ((string) protocolObject.GetPropertyValue("ProtocolName")).ToLowerInvariant();

                if (protocol == "tcp" || protocol == "np")
                {
                    log.InfoFormat("Enabling ServerNetworkProtocol: {0}", protocol);
                    protocolObject.InvokeMethod("SetEnable", null);
                }
            }

            string serviceName = string.IsNullOrEmpty(instance) ? SqlInstanceUtility.DefaultInstanceName : string.Format("MSSQL${0}", instance);
            log.InfoFormat("Stopping SQL Service: {0}", serviceName);

            ServiceController sqlService = new ServiceController(serviceName);
            sqlService.Stop();
            sqlService.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
            log.InfoFormat("Service status: {0}", sqlService.Status);

            log.InfoFormat("Starting SQL Service: {0}", serviceName);
            sqlService.Start();
            sqlService.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
            log.InfoFormat("Service status: {0}", sqlService.Status);

            // It claims it's running... but from what i've seen we still need to wait for a bit until it's ready to accept connections
            ConnectUntilSuccess(sqlSession, TimeSpan.FromSeconds(15));
        }

        /// <summary>
        /// Keep trying to connect (up to the max timespan) until its a success.  This is used after upgrading, or after the service restarts, to ensure
        /// its running before we move on.
        /// </summary>
        private static void ConnectUntilSuccess(SqlSession sqlSession, TimeSpan timeout)
        {
            Stopwatch waitTimer = Stopwatch.StartNew();

            while (waitTimer.Elapsed < timeout)
            {
                try
                {
                    using (var con = sqlSession.OpenConnection())
                    {
                        log.InfoFormat("Successful reconnect to running service.");
                    }

                    return;
                }
                catch (SqlException ex)
                {
                    log.WarnFormat("Still cant reconnect to service: {0}", ex.Message);

                    Thread.Sleep(TimeSpan.FromSeconds(2));
                }
            }
        }

        /// <summary>
        /// Get the command line arguments to send to SQL Server for upgrading the given instance
        /// </summary>
        private static string GetUpgradeArgs(string instanceName, bool installerIs32bit)
        {
            // If its the default instance, we have to specify the default instance name
            if (string.IsNullOrEmpty(instanceName))
            {
                instanceName = SqlInstanceUtility.DefaultInstanceName;
            }

            StringBuilder args = new StringBuilder();
            args.AppendFormat("/Q ");
            args.AppendFormat("/HIDECONSOLE ");
            args.AppendFormat("/ACTION=Upgrade ");
            args.AppendFormat("/IAcceptSQLServerLicenseTerms ");
            args.AppendFormat("/BROWSERSVCSTARTUPTYPE=Automatic ");
            args.AppendFormat("/INSTANCENAME=\"{0}\" ", instanceName);

            // If we are on 64 bit windows - but using the X86 installer, we have to specify the X86 flag to let it know
            // we want to use WOW
            if (MyComputer.Is64BitWindows && installerIs32bit)
            {
                args.AppendFormat("/X86 ");
            }

            return args.ToString();
        }

        #endregion

        #region Installation \ Upgrade Exiting

        /// <summary>
        /// The installation process has exited
        /// </summary>
        private void OnInstallerExited(object sender, EventArgs e)
        {
            Process process = (Process) sender;
            lastExitCode = process.ExitCode;

            Exited?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// The exit code from the last attempt at installation.
        /// </summary>
        public int LastExitCode
        {
            get { return lastExitCode; }
        }

        /// <summary>
        /// Formats the return code to be user readable.
        /// </summary>
        public static string FormatReturnCode(int code)
        {
            if (code == 1706)
            {
                return
                    "Setup failed because SQL Native Client is already installed.  To work around\n" +
                    "this problem, uninstall SQL Native Client by using Add or Remove Programs\n\n" +
                    "Then try running setup again.  SQL Native Client will be reinstalled by setup.";
            }

            if (code == 1602 || code == 1223 || code == -2147467259)
            {
                return "Setup was canceled by the user.";
            }

            if (code == unknownException)
            {
                return
                    "An error occurred that ShipWorks could not handle.  The error has been logged.\n\n" +
                    "Please contact Interapptive for further assistance with this issue.";
            }

            if (code == msdeUpgrade08Failed)
            {
                return
                    "SQL Server failed to install, but the previous version of SQL Server\n" +
                    "was already removed.\n\n" +
                    "Please contact Interapptive for assistance with completing the upgrade.";
            }

            if (IsErrorCodeRebootRequiredBeforeInstall(code))
            {
                return "Your computer must be restarted before installing SQL Server.";
            }

            if (code == failedDueToMsdeExists)
            {
                return "SQL Server failed to install.  This may be because a previous version of SQL Server (MSDE) is installed on your computer.  Uninstalling MSDE may resolve the issue.";
            }

            return string.Format("Reason code: {0:X}.", code);
        }

        /// <summary>
        /// Indicates if the specified error code means that the computer needs rebooted before installing SQL Server
        /// </summary>
        public static bool IsErrorCodeRebootRequiredBeforeInstall(int code)
        {
            // This particular code would mean we already installed SQL Server, and now need to reboot
            if (code == ExitCodeSuccessRebootRequired)
            {
                return false;
            }

            // Check the HEX values
            string hex = string.Format("{0:X}", code);

            // Per http://technet.microsoft.com/en-us/library/dd981032.aspx, anything with BC2 means reboot
            return hex.EndsWith("BC2", StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region Common and Command Line

        /// <summary>
        /// Handle commands comming in from the command line for sql server
        /// </summary>
        /// <remarks>
        /// This is marked as unused but is used by reflection to install/upgrade SQL Server
        /// </remarks>
        private class CommandLineHandler : ICommandLineCommandHandler
        {
            /// <summary>
            /// Name of the command to process
            /// </summary>
            public string CommandName
            {
                get { return "sqlserver"; }
            }

            /// <summary>
            /// Execute the command
            /// </summary>
            [NDependIgnoreLongMethod]
            public void Execute(List<string> args)
            {
                string action = null;
                string instance = null;
                string sapassword = null;

                // Need to extract the type
                OptionSet optionSet = new OptionSet()
                    {
                        { "a|action=", v => action = v  },
                        { "i|instance=", v => instance = v },
                        { "p|password=", v => sapassword = v },
                        { "<>", v => { throw new CommandLineCommandArgumentException(CommandName, v, "Invalid arguments passed to command."); } }
                    };

                optionSet.Parse(args);

                if (string.IsNullOrEmpty(action))
                {
                    throw new CommandLineCommandArgumentException(CommandName, "action", "The required 'action' parameter was not specified.");
                }

                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    ExecuteInternal(action, instance, sapassword, lifetimeScope);
                }
            }

            /// <summary>
            /// Execute the actual work for the command.
            /// </summary>
            void ExecuteInternal(string action, string instance, string sapassword, ILifetimeScope lifetimeScope)
            {
                ValidateArguments(action, instance, sapassword);

                // Normally this happens as a part of app startup, but not when running command line.
                SqlSession.Initialize();

                SqlServerInstaller installer = lifetimeScope.Resolve<SqlServerInstaller>();

                try
                {
                    switch (action)
                    {
                        case "upgrade":
                        {
                            log.InfoFormat("Processing request to upgrade SQL Sever");

                            // We need to initialize an installer to get the correct installer package exe's
                            ISqlInstallerInfo sqlInstallerInfo = installer.GetSqlInstaller(SqlServerInstallerPurpose.Upgrade);

                            UpgradeSqlServerInternal(
                                installer.GetInstallerLocalFilePath(sqlInstallerInfo),
                                SqlSession.Current);

                            break;
                        }

                        case "install":
                        {
                            log.InfoFormat("Processing request to install sql server. {0}", instance);

                            // We need to initialize an installer to get the correct installer package exe's
                            installer.InstallSqlServerInternal(instance, sapassword);

                            break;
                        }

                        case "localdb":
                        {
                            log.InfoFormat("Processing request to install LocalDB.");

                            // We need to initialize an installer to get the correct installer package exe's
                            installer.InstallLocalDbInternal();

                            break;
                        }

                        case "upgradelocaldb":
                        {
                            log.InfoFormat("Processing request to upgrade local db. {0}", instance);

                            // We need to initialize an installer to get the correct installer package exe's
                            installer.UpgradeLocalDbInternal(instance);

                            break;
                        }

                        case "assignautomaticdbname":
                        {
                            log.InfoFormat("Processing request to assign automatic database name.");

                            // We need to initialize an installer to get the correct installer package exe's
                            installer.AssignAutomaticDatabaseNameInternal();

                            break;
                        }

                        default:
                        {
                            throw new CommandLineCommandArgumentException(CommandName, "action", string.Format("Invalid value passed to 'action' parameter: {0}", action));
                        }
                    }
                }
                catch (Win32Exception ex)
                {
                    log.Error("Failed to process firewall request.", ex);
                    Environment.ExitCode = ex.NativeErrorCode;
                }
            }

            /// <summary>
            /// Validate the command arguments
            /// </summary>
            private void ValidateArguments(string action, string instance, string sapassword)
            {
                switch (action)
                {
                    case "upgrade":
                        {
                            // Can't specific instance or password for upgrade - we use SqlSession
                            if (instance != null || sapassword != null)
                            {
                                throw new CommandLineCommandArgumentException(CommandName, "instance\\password", "Invalid arguments passed to command.");
                            }

                            break;
                        }

                    case "install":
                        {
                            if (instance == null)
                            {
                                throw new CommandLineCommandArgumentException(CommandName, "instance", "The required 'instance' parameter was not specified.");
                            }

                            if (sapassword == null)
                            {
                                throw new CommandLineCommandArgumentException(CommandName, "password", "The required 'password' parameter was not specified.");
                            }
                                
                            break;
                        }

                    case "upgradelocaldb":
                        {
                            if (instance == null)
                            {
                                throw new CommandLineCommandArgumentException(CommandName, "instance", "The required 'instance' parameter was not specified.");
                            }

                            break;
                        }

                    default:
                        {
                            throw new CommandLineCommandArgumentException(CommandName, "action", string.Format("Invalid value passed to 'action' parameter: {0}", action));
                        }
                }
            }
        }

        #endregion
    }
}
