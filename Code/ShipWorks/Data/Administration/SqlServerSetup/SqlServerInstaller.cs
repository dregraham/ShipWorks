using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.IO;
using Interapptive.Shared;
using System.Diagnostics;
using System.ComponentModel;
using log4net;
using System.Data.SqlClient;
using System.Windows.Forms;
using ShipWorks.Filters;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using System.Security.Cryptography;
using System.Threading;
using System.Data;
using System.Linq;
using Interapptive.Shared.Data;
using System.Security.Principal;
using System.Management;
using System.ServiceProcess;
using ShipWorks.ApplicationCore.Interaction;
using NDesk.Options;
using System.Xml.Linq;
using System.Xml;
using Interapptive.Shared.Win32;
using System.Net;

namespace ShipWorks.Data.Administration.SqlServerSetup
{
    /// <summary>
    /// Use to control the installation process of SQL Server
    /// </summary>
    public class SqlServerInstaller
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(SqlServerInstaller));

        const string sqlx64FileName = "SQLEXPR_x64_ENU.exe";
        const string sqlx86FileName = "SQLEXPR_x86_ENU.exe";

        // Custom isntall error codes
        const int msdeUpgrade08Failed = -200; 
        const int unknownException = -201;

        // A value of exactly OxBC2 means installation was successful, but a reboot is now required.
        // Anything else (such as 0x0XXX0BC2) means a reboot is required before install.
        const int successRebootRequired = 0xBC2;
        
        /// <summary>
        /// For SQL 2012 this sometimes seems to indicate a failure to install b\c of the presense of MSDE
        /// </summary>
        const int failedDueToMsdeExists = -2068774911;

        // The exit code from the last attempt at installation
        int lastExitCode = 0;

        // The names of the files for the install and upgrade packages.  
        string installPackageExe = null;
        string upgradePackageExe = null;


        class DatabaseLoginInfo
        {
            public string Name { get; set; }
            public byte[] Password { get; set; }
            public bool SysAdmin { get; set; }
        }

        /// <summary>
        /// Raised when the installation executable has exited
        /// </summary>
        public event EventHandler Exited;

        /// <summary>
        /// Constructor
        /// </summary>
        public SqlServerInstaller()
        {

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
        /// The full path and name of the file that contains the data on how to restore data that we had in MSDE into SQL 08 after an MSDE uinstall
        /// </summary>
        private static string MsdeMigrationInfoFile
        {
            get { return Path.Combine(DataPath.InstanceSettings, "msde.xml"); }
        }

        /// <summary>
        /// Indicates if an upgrade from MSDE to SQL 08 is in progress - MSDE has been uinstalled, but 08 is not yet installed.
        /// </summary>
        public static bool IsMsdeMigrationInProgress
        {
            get { return File.Exists(MsdeMigrationInfoFile); }
        }

        /// <summary>
        /// Indicates if SQL Server 2012 is supported on the current computer
        /// </summary>
        public static bool IsSqlServer2012Supported
        {
            get
            {
                // This is how to detect for .NET 4.5 - if this key exists, and if "Release" exists within it.  SQL 2012 doesn't necessarily need .NET 4.5, 
                // but it has the same OS requirements.  Our installer installs .NET 4.5 if the OS supports it - so the presense of .NET 4.5 in the 
                // case of ShipWorks is identicial to checking for 2012 compatibility
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full"))
                {
                    return (key != null && key.GetValue("Release") != null);
                }
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
        public static bool IsDotNet35Required
        {
            get
            {
                // Although documented as needing it, in practice from what I've seen SQL 2012 runs just fine without it
                if (IsSqlServer2012Supported)
                {
                    return false;
                }

                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5"))
                {
                    if (key != null)
                    {
                        return Convert.ToInt32(key.GetValue("SP", 0)) == 0;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Initialize the install to be used against the current SqlSession.  If there is none, then upgrading is not possible.  If there is one,
        /// then it doesnt affect a new Install, but it Upgrading can only apply to the current SqlSession.
        /// </summary>
        public void InitializeForCurrentSqlSession()
        {
            // For fresh installs we just go by the bitness of the OS
            installPackageExe = MyComputer.Is64BitWindows ? sqlx64FileName : sqlx86FileName;
            log.InfoFormat("SQL Server intallation package: {0}", installPackageExe);

            // Can only upgrade the current SqlSession - if there is one
            if (IsMsdeMigrationInProgress || (SqlSession.IsConfigured && SqlSession.Current.CanConnect()))
            {
                // For upgrades, if we are on a 32 bit OS, then we know we need the 32bit
                if (!MyComputer.Is64BitWindows)
                {
                    upgradePackageExe = sqlx86FileName;
                }
                else
                {
                    // For MSDE we do an uninstall followed by a clean install - we can use the 64bit for sure
                    if (IsMsdeMigrationInProgress || SqlSession.Current.GetServerVersion().Major <= 8)
                    {
                        upgradePackageExe = sqlx64FileName;
                    }
                    else
                    {
                        // We have to determine the bitness of the currently installed SQL server, and use the same one.
                        upgradePackageExe = SqlSession.Current.Is64Bit() ? sqlx64FileName : sqlx86FileName;
                    }
                }
            }

            log.InfoFormat("SQL Server upgrade package: {0}", upgradePackageExe);
        }

        /// <summary>
        /// Validate that the installer has been initialized for the given purpose, or throw
        /// </summary>
        private void ValidateInitialized(SqlServerInstallerPurpose purpose)
        {
            if (purpose == SqlServerInstallerPurpose.LocalDb && !IsSqlServer2012Supported)
            {
                throw new InvalidOperationException("Cannot install LocalDB when SQL 2012 is not supported.");
            }

            if (installPackageExe == null)
            {
                throw new InvalidOperationException("The SqlServerInstaller has not been initialized.");
            }

            if (purpose == SqlServerInstallerPurpose.Upgrade && upgradePackageExe == null)
            {
                throw new InvalidOperationException("The SqlServerInstaller has been initialized, but upgrading is invalid due to no SqlSession.");
            }
        }

        #endregion

        #region Local and Remote Paths

        /// <summary>
        /// The path to the location of the SQL installer for new installations on the local disk
        /// </summary>
        public string GetInstallerLocalFilePath(SqlServerInstallerPurpose purpose)
        {
            ValidateInitialized(purpose);

            string fileName;

            if (purpose == SqlServerInstallerPurpose.LocalDb)
            {
                fileName = "SqlLocalDB.MSI";
            }
            else
            {
                fileName = (purpose == SqlServerInstallerPurpose.Install) ? installPackageExe : upgradePackageExe;
            }

            return Path.Combine(DataPath.Components, fileName);
        }

        /// <summary>
        /// The full Uri for downloading the SQL Express installer used for new installations
        /// </summary>
        public Uri GetInstallerDownloadUri(SqlServerInstallerPurpose purpose)
        {
            ValidateInitialized(purpose);

            if (purpose == SqlServerInstallerPurpose.LocalDb)
            {
                return new Uri(string.Format("http://www.interapptive.com/download/components/sqlserver2012/localdb/{0}/SqlLocalDB.MSI", MyComputer.Is64BitWindows ? "x64" : "x86"));
            }
            else
            {
                string url = IsSqlServer2012Supported ? "http://www.interapptive.com/download/components/sqlserver2012/express/" : "http://www.interapptive.com/download/components/sqlexpress08r2sp2/";

                return new Uri(url + ((purpose == SqlServerInstallerPurpose.Install) ? installPackageExe : upgradePackageExe));
            }
        }

        /// <summary>
        /// Get the total file length on disk of the given installer
        /// </summary>
        public long GetInstallerFileLength(SqlServerInstallerPurpose purpose)
        {
            switch (GetInstallerDownloadUri(purpose).ToString())
            {
                case @"http://www.interapptive.com/download/components/sqlexpress08r2sp2/SQLEXPR_x64_ENU.exe": return 128331696;
                case @"http://www.interapptive.com/download/components/sqlexpress08r2sp2/SQLEXPR_x86_ENU.exe": return 115763632;
                case @"http://www.interapptive.com/download/components/sqlserver2012/express/SQLEXPR_x64_ENU.exe": return 138757208;
                case @"http://www.interapptive.com/download/components/sqlserver2012/express/SQLEXPR_x86_ENU.exe": return 122317400;
                case @"http://www.interapptive.com/download/components/sqlserver2012/localdb/x64/SqlLocalDB.MSI": return 34635776;
                case @"http://www.interapptive.com/download/components/sqlserver2012/localdb/x86/SqlLocalDB.MSI": return 29097984;
                default: throw new InvalidOperationException("Unknown SQL Server installer for checksum: " + purpose);
            }
        }

        /// <summary>
        /// Determines if the installer for SQL Express installer is valid, as in not partially downloaded or corrupted.
        /// </summary>
        public bool IsInstallerLocalFileValid(SqlServerInstallerPurpose purpose)
        {
            string localExe = GetInstallerLocalFilePath(purpose);

            if (!File.Exists(localExe))
            {
                return false;
            }

            Cursor.Current = Cursors.WaitCursor;

            SHA1 sha = SHA1.Create();
            byte[] computedChecksum = sha.ComputeHash(File.ReadAllBytes(localExe));

            Cursor.Current = Cursors.Default;

            string knownChecksum;

            switch (GetInstallerDownloadUri(purpose).ToString())
            {
                case @"http://www.interapptive.com/download/components/sqlexpress08r2sp2/SQLEXPR_x64_ENU.exe": knownChecksum = "52ijtw4/O1lu/6n1fYEvlcCgUGs="; break;
                case @"http://www.interapptive.com/download/components/sqlexpress08r2sp2/SQLEXPR_x86_ENU.exe": knownChecksum = "bONPV6E+De2VyvRAX60TPoWa3jA="; break;
                case @"http://www.interapptive.com/download/components/sqlserver2012/express/SQLEXPR_x64_ENU.exe": knownChecksum = "5FYdXKp2Gl0dqg0wX0/s7cag05w="; break;
                case @"http://www.interapptive.com/download/components/sqlserver2012/express/SQLEXPR_x86_ENU.exe": knownChecksum = "KHXaFzH7vmGDycwPKMpZBdxnKIo="; break;
                case @"http://www.interapptive.com/download/components/sqlserver2012/localdb/x64/SqlLocalDB.MSI": knownChecksum = "JCf2I6gCsTUta7bJR4KYqCroJTk="; break;
                case @"http://www.interapptive.com/download/components/sqlserver2012/localdb/x86/SqlLocalDB.MSI": knownChecksum = "Fjii+pBm3xmiR6t8vgYXJ5laqT0="; break;
                default: throw new InvalidOperationException("Unknown SQL Server installer for checksum: " + localExe);
            }

            return Convert.ToBase64String(computedChecksum) == knownChecksum;
        }

        /// <summary>
        /// Download the SQL Server installer to be used for the given purpose
        /// </summary>
        private string DownloadSqlServer(SqlServerInstallerPurpose purpose)
        {
            string localFile = GetInstallerLocalFilePath(purpose);

            if (!IsInstallerLocalFileValid(purpose))
            {
                Uri remoteUri = GetInstallerDownloadUri(purpose);

                log.InfoFormat("Using WebClient to download {0}", remoteUri);

                // We can do this synchronously becauase we are already running as a secondary background elevated process, and won't be blocking any UI
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
                using (SqlConnection con = localDbSession.OpenConnection())
                {
                    fileInfo = ShipWorksDatabaseUtility.DetachDatabase(localDbSession.Configuration.DatabaseName, con);
                }

                // Path where the db files are moving
                string newFilePath;

                // Figure out the path where they are going to go
                using (SqlConnection con = newSession.OpenConnection())
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
                using (SqlConnection con = newSession.OpenConnection())
                {
                    SqlCommandProvider.ExecuteNonQuery(con, string.Format(
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

                using (SqlConnection con = session.OpenConnection())
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
                // If they are running MSDE or less then instead of an upgrade we first uninstall MSDE and then install SQL Server.
                // We do this for two main reasons:
                //  1) I don't trust MSDE at all, especially to be upgraded
                //  2) To upgrade MSDE SP4 is required.  SW2 comes with SP3.  SP4 is another 70mb download and another point that
                //     things could go wrong.
                if (SqlServerInstaller.IsMsdeMigrationInProgress || sqlSession.GetServerVersion().Major <= 8)
                {
                    // Since uinistall MSDE is a multi-step process, we have to move the whole thing to another thread.
                    UpgradeSqlServerFromMSDE(installerPath, sqlSession);
                }
                else
                {
                    UpgradeSqlServerNormally(installerPath, sqlSession);
                }

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

            string args = GetUpgradeArgs(instance, Path.GetFileName(installerPath) == sqlx86FileName);
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
                using (SqlConnection con = sqlSession.OpenConnection())
                {
                    try
                    {
                        log.InfoFormat("Ensuring [{0}] has login.", WindowsIdentity.GetCurrent().Name);
                        SqlCommandProvider.ExecuteNonQuery(con, string.Format("CREATE LOGIN [{0}] FROM WINDOWS", WindowsIdentity.GetCurrent().Name));
                    }
                    catch (SqlException ex)
                    {
                        log.Warn("Could not add login (may already exist): " + ex.Message);
                    }

                    log.InfoFormat("Ensuring is sysadmin");
                    SqlCommandProvider.ExecuteNonQuery(con, string.Format(@"EXEC sp_addsrvrolemember @loginame = N'{0}', @rolename = N'sysadmin'", WindowsIdentity.GetCurrent().Name));
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

        /// <summary>
        /// Upgrade from an installation of MSDE.  We were installing MSDE SP3... to upgrade directly you need MSDE SP4...whose 
        /// installer kind of sucked on Vista, so we are doing some trickery to make this work.
        /// </summary>
        [NDependIgnoreLongMethod]
        private static void UpgradeSqlServerFromMSDE(string installerPath, SqlSession sqlSession)
        {
            log.InfoFormat("MSDE detected, upgrade has to jump through hoops.");

            string instance = SqlInstanceUtility.ExtractInstanceName(sqlSession.Configuration.ServerInstance);
            if (string.IsNullOrEmpty(instance))
            {
                throw new InvalidOperationException("Upgrading from default instance of MSDE not supported.");
            }

            bool needOverrideSa;
            byte[] saPasswordHash;

            List<DatabaseLoginInfo> databaseLogins;
            List<DatabaseFileInfo> databaseFiles;

            // If its not already in progress, this is our first time through.
            bool firstTimeThrough = !IsMsdeMigrationInProgress;

            if (firstTimeThrough)
            {
                // If we don't know their sa password what we will do is one to install 08, 
                // and then override it with the HASH from the current install.
                needOverrideSa = sqlSession.Configuration.WindowsAuth || sqlSession.Configuration.Username != "sa";
                saPasswordHash = null;

                if (needOverrideSa)
                {
                    log.Info("Grabbing password hash for later override.");

                    using (SqlConnection con = sqlSession.OpenConnection())
                    {
                        saPasswordHash = (byte[]) SqlCommandProvider.ExecuteScalar(con, "select [password] from master.dbo.sysxlogins where name = 'sa'");
                    }
                }

                databaseLogins = new List<DatabaseLoginInfo>();

                // We will want to move over all non-windows logins from the old database.  Likely scenario is that this is a ShipWorks created
                // instance that the user didn't mess with, and their won't be any.  Except for maybe one that SW created if pre 2.4.
                DataTable logins = new DataTable();
                using (SqlConnection con = sqlSession.OpenConnection())
                {
                    con.ChangeDatabase("master");

                    SqlDataAdapter loginsAdapter = new SqlDataAdapter(@"
                        SELECT l.name, x.password, l.sysadmin 
                        FROM syslogins l INNER JOIN sysxlogins x ON l.name = x.name 
                        WHERE l.isntname = 0 AND l.name != 'sa' ", con);
                    loginsAdapter.Fill(logins);

                    foreach (DataRow row in logins.Rows)
                    {
                        log.InfoFormat("Found login {0}: sysadmin:{1}", row["name"], row["sysadmin"]);

                        databaseLogins.Add(new DatabaseLoginInfo
                            {
                                Name = (string) row["name"],
                                Password = (byte[]) row["password"],
                                SysAdmin = (int) row["sysadmin"] != 0
                            });
                    }
                }

                // Detach all the databases in the MSDE instance
                databaseFiles = DetachMsdeDatabases(sqlSession);

                // Now we need to uninstall MSDE
                UninstallMsde(instance);

                // Save the MSDE info so if we need to reboot we can continue on
                var msdeInfo = new XElement("MSDE",
                    new XElement("NeedOverrideSa", needOverrideSa),
                    new XElement("SaPasswordHash", saPasswordHash != null ? Convert.ToBase64String(saPasswordHash) : (string) null),
                    new XElement("Logins",
                        databaseLogins.Select(l =>
                            new XElement("Login",
                                new XElement("Name", l.Name),
                                new XElement("Password", Convert.ToBase64String(l.Password)),
                                new XElement("SysAdmin", l.SysAdmin)))),
                    new XElement("Files",
                        databaseFiles.Select(f =>
                            new XElement("File",
                                new XElement("Database", f.Database),
                                new XElement("DataFile", f.DataFile),
                                new XElement("LogFile", f.LogFile)))));
                msdeInfo.Save(MsdeMigrationInfoFile);
            }
            else
            {
                var msdeInfo = XElement.Load(MsdeMigrationInfoFile);
                needOverrideSa = (bool) msdeInfo.Element("NeedOverrideSa");
                saPasswordHash = msdeInfo.Element("SaPasswordHash") != null ? Convert.FromBase64String((string) msdeInfo.Element("SaPasswordHash")) : null;

                databaseLogins = msdeInfo.Descendants("Login").Select(x => new DatabaseLoginInfo
                    {
                        Name = (string) x.Element("Name"),
                        Password = Convert.FromBase64String((string) x.Element("Password")),
                        SysAdmin = (bool) x.Element("SysAdmin")
                    }).ToList();

                databaseFiles = msdeInfo.Descendants("File").Select(x => new DatabaseFileInfo
                    {
                        Database = (string) x.Element("Database"),
                        DataFile = (string) x.Element("DataFile"),
                        LogFile = (string) x.Element("LogFile")
                    }).ToList();
            }

            // Install SQL 08 now that MSDE has been uninstalled
            if (InstallSqlServerAfterMsdeUninstall(installerPath, sqlSession, instance, needOverrideSa, saPasswordHash, databaseLogins, databaseFiles))
            {
                log.InfoFormat("Cleaning MSDE migration info file.");
                File.Delete(MsdeMigrationInfoFile);

                // If we didn't do it successfully in one go, then make them reboot to continue.  This is so that we can start sw up in the "correct" way, forcing them to login
                // to the db, and login to sw, etc.  When we are in "MSDE in progress" mode, all that is skipped since we don't have a connection to the database.
                if (!firstTimeThrough)
                {
                    Environment.ExitCode = ExitCodeSuccessRebootRequired;
                }
            }
        }

        /// <summary>
        /// Install SQL Server 08 and restore the data from the previously uinstalled MSDE 
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreTooManyParams]
        private static bool InstallSqlServerAfterMsdeUninstall(string installerPath, SqlSession sqlSession, 
            string instance, 
            bool needOverrideSa, 
            byte[] saPasswordHash,
            List<DatabaseLoginInfo> databaseLogins, 
            List<DatabaseFileInfo> databaseFiles)
        {
            // We need a new SQL Session that uses windows auth - which we know we can login as.  If the session
            // was configured to use a non SA SQL Admin it would failed, b\c it doesn't exist on this new instance (yet)
            SqlSession copySession = new SqlSession();
            copySession.Configuration.ServerInstance = sqlSession.Configuration.ServerInstance;
            copySession.Configuration.DatabaseName = "master";
            copySession.Configuration.WindowsAuth = true;

            // It's possible we are here after a successful SQL 08 install - but that needed to reboot before we moved on.  If that's the case we skip over the install (its already installed)
            // and move on to fixing up the db stuff.
            bool alreadyInstalled = copySession.CanConnect() && copySession.IsSqlServer2008OrLater();

            if (!alreadyInstalled)
            {
                string saPassword = needOverrideSa ? "temporary" : sqlSession.Configuration.Password;
                string args = GetInstallArgs(instance, saPassword, true);

                log.Info("Installing SQL server with args: " + args);

                // Now we need to install SQL Server
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo(installerPath, args);
                process.Start();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    log.InfoFormat("SQL Server failed to install for reason: '{0}'", process.ExitCode);

                    // If it's just saying we need to reboot before continuing
                    if (IsErrorCodeRebootRequiredBeforeInstall(process.ExitCode) || process.ExitCode == ExitCodeSuccessRebootRequired)
                    {
                        Environment.ExitCode = process.ExitCode;
                    }
                    else
                    {
                        Environment.ExitCode = msdeUpgrade08Failed;
                    }

                    return false;
                }
            }
            else
            {
                log.InfoFormat("Looks like SQL 08 was installed and we just need to put it back together.");
            }

            log.InfoFormat("SQL 08 upgrade completed.");

            // Make sure its running and accepting connections before moving on
            ConnectUntilSuccess(copySession, TimeSpan.FromSeconds(15));

            try
            {
                // This section moves the mdf\ldf files to the location of the new 08 install
                using (SqlConnection con = new SqlConnection(copySession.Configuration.GetConnectionString()))
                {
                    con.Open();

                    // First see if we have to restore the sa password
                    if (needOverrideSa)
                    {
                        log.InfoFormat("Setting sa password hash.");
                        SqlCommandProvider.ExecuteNonQuery(con, string.Format("ALTER LOGIN sa WITH CHECK_POLICY = OFF, PASSWORD = 0x{0} HASHED", BitConverter.ToString(saPasswordHash).Replace("-", "")));

                        // Turn check policy back on
                        SqlCommandProvider.ExecuteNonQuery(con, "ALTER LOGIN sa WITH CHECK_POLICY = ON");
                    }

                    // Get the filepath to master
                    string masterPath = Path.GetDirectoryName((string) SqlCommandProvider.ExecuteScalar(con, "SELECT physical_name FROM sys.database_files WHERE type = 0"));

                    // Move the db files to the new locationsp
                    log.InfoFormat("Moving database files to '{0}'", masterPath);

                    // Move all the database files to the new SQL install data directory
                    foreach (DatabaseFileInfo databaseInfo in databaseFiles)
                    {
                        string targetMdf = Path.Combine(masterPath, Path.GetFileName(databaseInfo.DataFile));
                        string targetLdf = Path.Combine(masterPath, Path.GetFileName(databaseInfo.LogFile));

                        File.Move(databaseInfo.DataFile, targetMdf);
                        File.Move(databaseInfo.LogFile, targetLdf);

                        log.InfoFormat("Attaching database {0} into newly installed SQL instance.", databaseInfo.Database);

                        // Now we attach the db files into the new 08 instance
                        SqlCommandProvider.ExecuteNonQuery(con, string.Format(
                            @"CREATE DATABASE {0} 
	                            ON (FILENAME = '{1}'),
	                               (FILENAME = '{2}')
	                            FOR ATTACH", databaseInfo.Database, targetMdf, targetLdf));
                    }

                    // Restore all the previous logins.  If we can tell they were created by ShipWorks (pre 2.4), then we drop the associated
                    // db user.  Otherwise, we try to rehook the db user and the shipworks user.
                    // The most likely scenario for this is a DB that was installed pre ShipWorks 2.4, where we wiped the Builtin\Admin,
                    // and created our own user
                    foreach (var login in databaseLogins)
                    {
                        log.InfoFormat("Recreating login for '{0}' on new server", login.Name);

                        SqlCommand createLoginCmd = SqlCommandProvider.Create(con);
                        createLoginCmd.CommandText = string.Format("CREATE LOGIN {0} WITH PASSWORD = 0x{1} HASHED, CHECK_POLICY = OFF",
                            login.Name,
                            BitConverter.ToString(login.Password).Replace("-", ""));
                        SqlCommandProvider.ExecuteNonQuery(createLoginCmd);

                        log.InfoFormat("Giving sysadmin to login: {0}", login.SysAdmin);

                        if (login.SysAdmin)
                        {
                            // Make that login we just created a sysadmin (this is
                            SqlCommandProvider.ExecuteNonQuery(con, string.Format("EXEC sp_addsrvrolemember @loginame = N'{0}', @rolename = N'sysadmin'", login.Name));
                        }

                        // Now we need to resolve how the login maps into each database
                        foreach (string database in databaseFiles.Select(d => d.Database))
                        {
                            log.InfoFormat("Resolving login on database '{0}'", database);

                            try
                            {
                                con.ChangeDatabase(database);

                                bool isShipWorksDb = (int) SqlCommandProvider.ExecuteScalar(con, "SELECT COALESCE(OBJECT_ID('GetSchemaVersion'), 0)") > 0;
                                log.InfoFormat("Is sw database: {0}", isShipWorksDb);

                                // Since this is a sysadmin, it's not really required in each individual database - so if it's a shipworks database, drop the user.
                                if (login.SysAdmin && isShipWorksDb)
                                {
                                    log.InfoFormat("Dropping user schema from database");
                                    SqlCommandProvider.ExecuteNonQuery(con, string.Format("DROP SCHEMA {0}", login.Name));

                                    log.InfoFormat("Dropping user from database");
                                    SqlCommandProvider.ExecuteNonQuery(con, string.Format("DROP USER {0}", login.Name));
                                }
                                else
                                {
                                    log.InfoFormat("Setting matching db user for login '{0}", login.Name);
                                    SqlCommandProvider.ExecuteNonQuery(con, string.Format("ALTER USER {0} WITH LOGIN = {0}", login.Name));
                                }
                            }
                            catch (SqlException ex)
                            {
                                log.WarnFormat("Failed to resolve login for database: {0}", ex.Message);
                            }
                        }
                    }
                }

                log.InfoFormat("Upgarde complete");

                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error migrating state to new instance.", ex);
                Environment.ExitCode = msdeUpgrade08Failed;
                return false;
            }
        }

        /// <summary>
        /// Detatch all the databases in the MSDE instance and return their file information
        /// </summary>
        private static List<DatabaseFileInfo> DetachMsdeDatabases(SqlSession sqlSession)
        {
            List<DatabaseFileInfo> databaseFiles = new List<DatabaseFileInfo>();

            // First we have to find where the old database files are
            using (SqlConnection con = sqlSession.OpenConnection())
            {                
                DataTable databaseNames = new DataTable();
                SqlDataAdapter sqlAdapter = new SqlDataAdapter("select name from master..sysdatabases where name not in ('master', 'model', 'msdb', 'tempdb')", con);
                sqlAdapter.Fill(databaseNames);

                // Go through each user database...
                foreach (string database in databaseNames.Rows.Cast<DataRow>().Select(r => (string) r[0]))
                {
                    databaseFiles.Add(ShipWorksDatabaseUtility.DetachDatabase(database, con));
                }
            }

            return databaseFiles;
        }

        /// <summary>
        /// Uinstall MSDE
        /// </summary>
        private static void UninstallMsde(string instance)
        {
            string productCode;

            string msdeKey = string.Format(@"Software\{0}Microsoft\Microsoft SQL Server\{1}\Setup", MyComputer.Is64BitProcess ? @"Wow6432Node\" : "", instance);

            // We need to find the ProductCode for MSDE
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(msdeKey))
            {
                productCode = (string) key.GetValue("ProductCode");
                log.InfoFormat("MSDE product code found to be '{0}'", productCode);
            }

            string args = string.Format("/q /X{0}", productCode);

            log.Info("Uinstall MSDE with args: " + args);

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo("MsiExec.exe", args);
            process.Start();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Win32Exception(process.ExitCode);
            }
        }

        /// <summary>
        /// If there is an MSDE -> 08 upgrade in progress, but has not yet been completed, this cancels it.
        /// </summary>
        public static void CancelMsdeMigrationInProgress()
        {
            if (File.Exists(MsdeMigrationInfoFile))
            {
                log.InfoFormat("Cancel MSDE Miration in Progress");

                File.Move(MsdeMigrationInfoFile,
                    Path.Combine(
                        Path.GetDirectoryName(MsdeMigrationInfoFile),
                        Path.GetFileNameWithoutExtension(MsdeMigrationInfoFile) +
                            "_canceled_" +
                                DateTime.UtcNow.ToString("yyyy-MM-dd HH.mm.ss") + ".xml"));
            }
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

            if (Exited != null)
            {
                Exited(this, EventArgs.Empty);
            }
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

                // Normally this happens as a part of app startup, but not when running command line.
                SqlSession.Initialize();

                try
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

                                log.InfoFormat("Processing request to upgrade SQL Sever");


                                // We need to initialize an installer to get the correct installer package exe's
                                SqlServerInstaller installer = new SqlServerInstaller();
                                installer.InitializeForCurrentSqlSession();

                                UpgradeSqlServerInternal(
                                    installer.GetInstallerLocalFilePath(SqlServerInstallerPurpose.Upgrade), 
                                    SqlSession.Current);

                                break;
                            }

                        case "install":
                            {
                                log.InfoFormat("Processing request to install sql server. {0}", instance);

                                if (instance == null)
                                {
                                    throw new CommandLineCommandArgumentException(CommandName, "instance", "The required 'instance' parameter was not specified.");
                                }

                                if (sapassword == null)
                                {
                                    throw new CommandLineCommandArgumentException(CommandName, "password", "The required 'password' parameter was not specified.");
                                }

                                // We need to initialize an installer to get the correct installer package exe's
                                SqlServerInstaller installer = new SqlServerInstaller();
                                installer.InitializeForCurrentSqlSession();
                                installer.InstallSqlServerInternal(instance, sapassword);

                                break;
                            }

                        case "localdb":
                            {
                                log.InfoFormat("Processing request to install LocalDB.");

                                // We need to initialize an installer to get the correct installer package exe's
                                SqlServerInstaller installer = new SqlServerInstaller();
                                installer.InitializeForCurrentSqlSession();
                                installer.InstallLocalDbInternal();

                                break;
                            }

                        case "upgradelocaldb":
                            {
                                log.InfoFormat("Processing request to upgrade local db. {0}", instance);

                                if (instance == null)
                                {
                                    throw new CommandLineCommandArgumentException(CommandName, "instance", "The required 'instance' parameter was not specified.");
                                }

                                // We need to initialize an installer to get the correct installer package exe's
                                SqlServerInstaller installer = new SqlServerInstaller();
                                installer.InitializeForCurrentSqlSession();
                                installer.UpgradeLocalDbInternal(instance);

                                break;
                            }

                        case "assignautomaticdbname":
                            {
                                log.InfoFormat("Processing request to assign automatic database name.");

                                // We need to initialize an installer to get the correct installer package exe's
                                SqlServerInstaller installer = new SqlServerInstaller();
                                installer.InitializeForCurrentSqlSession();
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
        }

        #endregion
    }
}
