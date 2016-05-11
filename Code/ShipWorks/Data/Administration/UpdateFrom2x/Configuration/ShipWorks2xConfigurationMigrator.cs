using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using log4net;
using ShipWorks.Data.Connection;
using System.Xml;
using System.Xml.XPath;
using Interapptive.Shared.Utility;
using ShipWorks.Users;
using System.Xml.Linq;
using Microsoft.Win32;
using System.Diagnostics;
using System.ComponentModel;
using ShipWorks.ApplicationCore.Interaction;
using NDesk.Options;
using Interapptive.Shared;
using Interapptive.Shared.Security;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Configuration
{
    /// <summary>
    /// Used to migrate sql session information from old ShipWorks2x installations
    /// </summary>
    public static class ShipWorks2xConfigurationMigrator
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShipWorks2xConfigurationMigrator));

        static string windowsUninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
        static string shipworks2xUninstallID = "ShipWorks_is1";

        /// <summary>
        /// Migrate 2x configuration to the new proper location and format if present.  If the application needs to terminate b\c
        /// migration could not complete, false is returned
        /// </summary>
        public static bool MigrateIfRequired(IWin32Window owner)
        {
            ShipWorks2xDataPaths dataPaths = new ShipWorks2xDataPaths(Application.StartupPath);

            log.InfoFormat("StartupPath: '{0}'", Application.StartupPath);
            log.InfoFormat("LastInstallPath: '{0}'", GetLastShipWorks2xInstallPath());

            bool startupPathIsLastInstall = PathUtility.IsSamePath(Application.StartupPath, GetLastShipWorks2xInstallPath());

            // If either of the sql sessions in known locations exist, we consider that an in-place migration.  Anytime the Post24 config exists we HAVE to do a local migration
            // b\c we know we were installed on top.  If the pre24 one exists we only do it if we havnt done it already, since it's in a common location.  So if you had another 2.3 installed
            // and ran it, that sqlsession file would come right back.
            if (
                   File.Exists(dataPaths.Post24SqlSessionFile)

                   ||

                   (
                        File.Exists(dataPaths.Pre24SqlSessionFile) &&
                        startupPathIsLastInstall &&
                        ConfigurationMigrationState.Action == ConfigurationMigrationAction.None
                    )
                )
            {
                log.InfoFormat("Opening configuration migration for inplace migration.");
                using (ConfigurationMigrationWizard dlg = new ConfigurationMigrationWizard(ConfigurationMigrationSource.InPlace))
                {
                    dlg.ShowDialog(owner);
                    ConfigurationMigrationState.Action = dlg.MigrationAction;
                }
            }
            // See if we need to allow selecting another folder to migrate from
            else
            {
                if (ConfigurationMigrationState.Action == ConfigurationMigrationAction.None)
                {
                    string installPath = GetLastShipWorks2xInstallPath();

                    if (string.IsNullOrEmpty(installPath))
                    {
                        log.InfoFormat("ShipWorks2 was not found to be installed");
                        ConfigurationMigrationState.Action = ConfigurationMigrationAction.NotNeeded;
                    }
                    else if (startupPathIsLastInstall)
                    {
                        // If we get here, it means we couldn't find a sql session file at all
                        log.InfoFormat("Found ShipWorks installed at '{0}', but that's where we are running from.", installPath);
                        ConfigurationMigrationState.Action = ConfigurationMigrationAction.NotNeeded;
                    }
                    else
                    {
                        log.InfoFormat("Found ShipWorks2 installed at: {0}", installPath);
                        using (ConfigurationMigrationWizard dlg = new ConfigurationMigrationWizard(ConfigurationMigrationSource.SelectFolder))
                        {
                            dlg.ShowDialog(owner);
                            ConfigurationMigrationState.Action = dlg.MigrationAction;
                        }
                    }
                }
            }

            // If we still haven't taken an action, return false so the app knows to terminate
            return ConfigurationMigrationState.Action != ConfigurationMigrationAction.None;
        }

        /// <summary>
        /// Get the last path that ShipWorks2x was installed to, if any
        /// </summary>
        public static string GetLastShipWorks2xInstallPath()
        {
            using (RegistryKey uninstallKey = Registry.LocalMachine.OpenSubKey(windowsUninstallKey + @"\" + shipworks2xUninstallID))
            {
                if (uninstallKey != null)
                {
                    return Path.GetFullPath((string) uninstallKey.GetValue("InstallLocation"));
                }
            }

            if (MyComputer.Is64BitWindows)
            {
                using (RegistryKey uninstallKey = Registry.LocalMachine.OpenSubKey(windowsUninstallKey.Replace("SOFTWARE", @"SOFTWARE\Wow6432Node") + @"\" + shipworks2xUninstallID))
                {
                    if (uninstallKey != null)
                    {
                        return Path.GetFullPath((string) uninstallKey.GetValue("InstallLocation"));
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Launch the migration in an elevated process.  Doesn't return until complete.
        /// </summary>
        public static void LaunchMigration(string installPath, bool cleanupInstance, bool cleanupCommon)
        {
            try
            {
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo(Application.ExecutablePath, string.Format("/command:migrateconfig2x -path:\"{0}\" {1} {2}",
                    installPath.TrimEnd('\\'),
                    cleanupInstance ? "-cleaninstance" : "",
                    cleanupCommon ? "-cleancommon" : ""));

                log.InfoFormat("Luanching migration using '{0}'", process.StartInfo.Arguments);

                // Elevate for vista
                if (MyComputer.IsWindowsVistaOrHigher)
                {
                    process.StartInfo.Verb = "runas";
                }

                process.Start();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new ConfigurationMigrationException(process.ExitCode);
                }
            }
            catch (Win32Exception ex)
            {
                throw new ConfigurationMigrationException(ex);
            }
        }

        /// <summary>
        /// Handle incoming command line requests
        /// </summary>
        private class CommandLineHandler : ICommandLineCommandHandler
        {
            /// <summary>
            /// The command name we listen for
            /// </summary>
            public string CommandName
            {
                get { return "migrateconfig2x"; }
            }

            /// <summary>
            /// Execute the command with the given arguments
            /// </summary>
            public void Execute(List<string> args)
            {
                string installPath = null;
                bool cleanupInstance = false;
                bool cleanupCommon = false;

                OptionSet optionSet = new OptionSet()
                    {
                        { "p|path=", v => installPath = v },
                        { "cleaninstance", v => cleanupInstance = (v != null) },
                        { "cleancommon", v => cleanupCommon = (v != null) },
                        { "<>", v => { throw new CommandLineCommandArgumentException(CommandName, v, "Invalid arguments passed to command."); } }
                    };

                optionSet.Parse(args);

                if (string.IsNullOrEmpty(installPath))
                {
                    throw new CommandLineCommandArgumentException(CommandName, "path", "The required parameter 'path' was not specified.");
                }

                log.InfoFormat("Beginning migration of path '{0}', cleanupIntance:{1}, cleanupCommon:{2}", installPath, cleanupInstance, cleanupCommon);

                try
                {
                    ElevatedMigration(installPath, cleanupInstance, cleanupCommon);
                }
                catch (Exception ex)
                {
                    log.Error("Failed migrating configuration from 2x", ex);
                    Environment.ExitCode = -11;
                }
            }
        }

        /// <summary>
        /// Ran in a seperate elevated ShipWorks process
        /// </summary>
        private static void ElevatedMigration(string installPath, bool cleanupInstance, bool cleanupCommon)
        {
            ShipWorks2xDataPaths dataPaths = new ShipWorks2xDataPaths(installPath);

            bool migratingFromPre24 = MigrateFromPre24(dataPaths);
            bool migratingFromPre30 = !migratingFromPre24 && MigrateFromPre30(dataPaths);

            if (migratingFromPre24 || migratingFromPre30)
            {
                // This is for the upgrader, so it knows where to default getting app data from
                ConfigurationMigrationState.ApplicationDataSource = new ShipWorks2xApplicationDataSource
                    {
                        SourceType = ShipWorks2xApplicationDataSourceType.AppDataFolder,
                        Path = dataPaths.ApplicationData
                    };

                // If its from pre24...the common data basically is instance data
                if (migratingFromPre24 && cleanupInstance)
                {
                    cleanupCommon = true;
                }

                // If we are cleaning up the old instance of 2x...
                if (cleanupInstance)
                {
                    // We can always delete the local configuration, that was specific to the instance we are upgrading
                    DeleteOldDirectory(dataPaths.Configuration);

                    // We can also always delete the old MSDE installer and the API logs
                    DeleteOldDirectory(Path.Combine(dataPaths.InstallPath, "MSDE"));
                    DeleteOldDirectory(Path.Combine(dataPaths.InstallPath, "Log"));

                    // If the install path we are migrating from is the one in the uinstall key, remove the key
                    if (PathUtility.IsSamePath(GetLastShipWorks2xInstallPath(), dataPaths.InstallPath))
                    {
                        DeleteInnoSetupUninstallKey();
                    }

                    // If the path we are migrating from is not the one we are installed in, get rid of the old ShipWorks
                    if (!PathUtility.IsSamePath(dataPaths.InstallPath, Application.StartupPath))
                    {
                        log.InfoFormat("Deleting old ShipWorks installation path");

                        DeleteOldInstallPath(dataPaths.InstallPath, dataPaths.ApplicationData);
                    }
                }

                if (cleanupCommon)
                {
                    // For 2.3 -
                    //   sqlsession.xml
                    //   preferences.xml
                    // For 2.4
                    //   user.xml
                    //   preferences.xml
                    DeleteOldDirectory(dataPaths.WindowsUserData);

                    // For 2.3 -
                    //   configuration.xml
                    //   printer calibrations
                    //   worldship import\export
                    // For 2.4 -
                    //   printer calibrations
                    //   worldship import\export
                    //   amazon pending feed submissions
                    DeleteOldDirectory(dataPaths.CommonData);

                    log.InfoFormat("Deleting old registry data...");

                    // Attempt to cleanup the old registry keys.
                    foreach (RegistryKey userKey in Registry.Users.GetSubKeyNames().Select(k => Registry.Users.OpenSubKey(k, true)))
                    {
                        using (RegistryKey oldKey = userKey.OpenSubKey(@"Software\Interapptive", true))
                        {
                            if (oldKey != null)
                            {
                                log.InfoFormat("Deleting for user '{0}'", userKey.Name);
                                oldKey.DeleteSubKeyTree("ShipWorks");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Migrate SqlSession settings from pre 3.0 format
        /// </summary>
        private static bool MigrateFromPre30(ShipWorks2xDataPaths dataPaths)
        {
            // Its the same format, if it exists we can just move it
            if (File.Exists(dataPaths.Post24SqlSessionFile))
            {
                log.InfoFormat("Migrating 2.4+");

                // If the destination settings file already exists, they must have installed 3x, installed 2x, and now reinstalled 3x.  We have to get rid
                // of it so the move can work.
                if (File.Exists(SqlSessionConfiguration.SettingsFile))
                {
                    log.InfoFormat("Deleting existing SQL Session before migrating 2x");
                    File.Delete(SqlSessionConfiguration.SettingsFile);
                }

                log.Info("Copying pre 3.0 sql session to new location.");
                File.Copy(dataPaths.Post24SqlSessionFile, SqlSessionConfiguration.SettingsFile);

                // See if the config file is there to pull app-data out of
                string oldConfiguration = Path.Combine(dataPaths.Configuration, "configuration.xml");
                if (File.Exists(oldConfiguration))
                {
                    XmlDocument xmlConfiguration = new XmlDocument();
                    xmlConfiguration.Load(oldConfiguration);

                    XPathNavigator xpathConfig = xmlConfiguration.CreateNavigator();
                    dataPaths.ApplicationData = XPathUtility.Evaluate(xpathConfig, "//ApplicationData", dataPaths.ApplicationData);

                    log.InfoFormat("Pulling appdata from old config: '{0}'", dataPaths.ApplicationData);
                }

                // Move the user.xml file over to where sw3 will find it
                string oldUserFile = Path.Combine(dataPaths.WindowsUserData, "user.xml");
                if (File.Exists(oldUserFile) && !File.Exists(UserSession.SettingsFilename))
                {
                    log.Info("Copying pre 3.0 user.xml to updated location.");
                    File.Copy(oldUserFile, UserSession.SettingsFilename);

                    // For 3x there is a "Remember" node that we need to add that 2x didn't have
                    XDocument xDocument = XDocument.Parse(File.ReadAllText(UserSession.SettingsFilename));
                    var xCredentials = xDocument.Root.Element("Credentials");

                    if (xCredentials != null && !string.IsNullOrEmpty((string) xCredentials.Element("Username")))
                    {
                        xCredentials.Add(new XElement("Remember", true));
                        xDocument.Save(UserSession.SettingsFilename);
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// See if we need to migrate sqlsession settings from the pre 2.4 format.
        /// </summary>
        private static bool MigrateFromPre24(ShipWorks2xDataPaths dataPaths)
        {
            // See if we need to update Sql sesion
            if (File.Exists(dataPaths.Pre24SqlSessionFile))
            {
                log.InfoFormat("Migrating 2.3-");

                // Load the data from the old file, in the old format
                XmlDocument xmlSession = new XmlDocument();
                xmlSession.Load(dataPaths.Pre24SqlSessionFile);

                // Create the XPath to read the settings
                XPathNavigator xPathSession = xmlSession.CreateNavigator();

                // Load the settings
                string username = (string) xPathSession.Evaluate("string(//Username)");
                string password = (string) xPathSession.Evaluate("string(//Password)");

                // This is how we used to decrypt the password
                password = SecureText.Decrypt(password, username + Environment.UserName);

                string sqlServerInstance = "";

                // If the config file already exists, copy the sql server instance out of it
                string pre24Configuration = Path.Combine(dataPaths.CommonData, "configuration.xml");
                if (File.Exists(pre24Configuration))
                {
                    // The sql server instance used to be in the config file.  We already moved
                    // it to its new location above, so use its current filename
                    XmlDocument xmlConfig = new XmlDocument();
                    xmlConfig.Load(pre24Configuration);

                    // Create the XPath to read the settings
                    XPathNavigator xpathConfig = xmlConfig.CreateNavigator();

                    // Load the serverinstance
                    sqlServerInstance = XPathUtility.Evaluate(xpathConfig, "//SqlServerInstance", "");

                    // Load the old app data location.
                    dataPaths.ApplicationData = XPathUtility.Evaluate(xpathConfig, "//ApplicationData", dataPaths.ApplicationData);
                }

                SqlSession sqlSession = new SqlSession();

                // Set the settings into the new format
                sqlSession.Configuration.ServerInstance = sqlServerInstance;
                sqlSession.Configuration.DatabaseName = "ShipWorks";
                sqlSession.Configuration.Username = username;
                sqlSession.Configuration.Password = password;
                sqlSession.Configuration.WindowsAuth = false;

                log.Info("Migrating SqlSession from pre 2.4 configuration.");
                sqlSession.SaveAsCurrent();

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Delete the 2x InnoSetup uninstall key out of the registry, so 2x drops out of Add\Remove programs
        /// </summary>
        private static void DeleteInnoSetupUninstallKey()
        {
            log.InfoFormat("Deleting ShipWorks uinstall registry key");

            using (RegistryKey uninstallKey = Registry.LocalMachine.OpenSubKey(windowsUninstallKey, true))
            {
                if (uninstallKey != null && uninstallKey.GetSubKeyNames().Contains(shipworks2xUninstallID))
                {
                    uninstallKey.DeleteSubKeyTree(shipworks2xUninstallID);
                }
                else if (MyComputer.Is64BitWindows)
                {
                    using (RegistryKey wowUinstallKey = Registry.LocalMachine.OpenSubKey(windowsUninstallKey.Replace("SOFTWARE", @"SOFTWARE\Wow6432Node"), true))
                    {
                        if (wowUinstallKey != null && wowUinstallKey.GetSubKeyNames().Contains(shipworks2xUninstallID))
                        {
                            log.InfoFormat("   Deleting from W0w6432");
                            wowUinstallKey.DeleteSubKeyTree(shipworks2xUninstallID);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Delete the old ShipWorks installation path, but being sure to preserve the Application Data folder
        /// </summary>
        private static void DeleteOldInstallPath(string installPath, string applicationData)
        {
            foreach (string directory in Directory.GetDirectories(installPath))
            {
                if (!PathUtility.IsSamePath(directory, applicationData))
                {
                    DeleteOldDirectory(directory);
                }
            }

            foreach (string file in Directory.GetFiles(installPath))
            {
                try
                {
                    DeleteOldFile(file);
                }
                catch (IOException ex)
                {
                    log.Warn("Failed deleting file " + file, ex);
                }
            }

            if (Directory.GetDirectories(installPath).Length == 0 && Directory.GetFiles(installPath).Length == 0)
            {
                DeleteOldDirectory(installPath);
            }
        }

        /// <summary>
        /// Attemps to delete an old file.  A failure is silently swallowed, since its not necessary for a successful migration
        /// </summary>
        private static void DeleteOldFile(string file)
        {
            try
            {
                log.InfoFormat("        Deleting old file: '{0}'", file);

                if (File.Exists(file))
                {
                    // To be successful it can't be readonly or hidden
                    File.SetAttributes(file, (File.GetAttributes(file) & ~FileAttributes.ReadOnly) & ~FileAttributes.Hidden);
                    File.Delete(file);
                }
            }
            catch (IOException ex)
            {
                log.Warn("Failed", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                log.Fatal("Failed", ex);
            }
        }

        /// <summary>
        /// Attempts to delete the old (now unneeded) directory referred to by the given path.  A failure is silently swallowed, since
        /// it does not affect operation.
        /// </summary>
        private static void DeleteOldDirectory(string path)
        {
            try
            {
                log.InfoFormat("Deleting old directory: '{0}'", path);

                if (Directory.Exists(path))
                {
                    foreach (string file in Directory.GetFiles(path))
                    {
                        DeleteOldFile(file);
                    }

                    foreach (string directory in Directory.GetDirectories(path))
                    {
                        DeleteOldDirectory(directory);
                    }

                    Directory.Delete(path);
                }
            }
            catch (IOException ex)
            {
                log.Warn("Failed", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                log.Fatal("Failed", ex);
            }
        }
    }
}
