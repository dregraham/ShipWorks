using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using log4net;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Users;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Paths to directories ShipWorks uses.
    /// </summary>
    public static class DataPath
    {
        // Logger
        private static readonly ILog log = LogManager.GetLogger(typeof(DataPath));

        // A file we keep locked to ensure the temp folder isn't wiped out by another running shipworks
        private static FileStream tempFolderLockFile = null;
        private static readonly string tempFolderLockName = "running.lock";

        private static Func<string> getInstancePath;
        private static Func<string> getCommonSettingsPath;
        private static Func<string> getTempPath;

        /// <summary>
        /// Ensures that all the data paths have been created.
        /// </summary>
        public static void Initialize(string instancePath = null, string commonSettingsPath = null, string tempPath = null)
        {
            if (ShipWorksSession.SessionID == Guid.Empty)
            {
                throw new InvalidOperationException("SessionID has not been initialized.");
            }

            Func<string> BuildGetter(string path, Func<string> defaultImplementation) =>
                string.IsNullOrEmpty(path) ? defaultImplementation : () => path;

            getInstancePath = BuildGetter(instancePath, GetInstancePathDefault);
            getCommonSettingsPath = BuildGetter(commonSettingsPath, GetCommonSettingsPathDefault);
            getTempPath = BuildGetter(tempPath, GetTempPathDefault);

            // Paths we create up front
            string[] paths = new string[]
                {
                    TempRoot,
                    ShipWorksTemp,
                    WindowsUserSettings,
                    SharedSettings,
                    InstanceSettings,
                    LogRoot,
                    Components
                };

            // Create all the paths we need up front
            foreach (string path in paths)
            {
                log.DebugFormat("Ensuring data path '{0}'", path);

                Directory.CreateDirectory(path);
            }

            // Create the file that ensures the temp folder doesn't get deleted by another ShipWorks while we are running.
            tempFolderLockFile = File.Create(Path.Combine(ShipWorksTemp, tempFolderLockName), 1024, FileOptions.DeleteOnClose);
        }

        /// <summary>
        /// Provide method for background process to register thread to clean up temp folder
        /// </summary>
        public static void RegisterTempFolderCleanup()
        {
            // Listen for idle to know when to cleanup the temp folder.
            IdleWatcher.RegisterDatabaseIndependentWork("TempFolderCleanup", CleanupThread, TimeSpan.FromHours(2));
        }

        /// <summary>
        /// Some ShipWorks user settings 
        /// </summary>
        public static string WindowsUserSettings
        {
            get
            {
                if (ShipWorksSession.InstanceID == Guid.Empty)
                {
                    throw new InvalidOperationException("Attempt to access DataPath before InstanceID.");
                }

                string path = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    @"Interapptive\ShipWorks");

                return Path.Combine(path, ShipWorksSession.InstanceID.ToString("B"));
            }
        }

        /// <summary>
        /// Most ShipWorks settings go in the AllUsers area, since we pay attention to ShipWorks users, not Windows users.
        /// </summary>
        private static string CommonSettingsRoot => getCommonSettingsPath();

        /// <summary>
        /// Root path to all settings that are shared across all ShipWorks users and instances
        /// </summary>
        public static string SharedSettings
        {
            get
            {
                return Path.Combine(CommonSettingsRoot, "Shared");
            }
        }

        /// <summary>
        /// Path to redistributable installation files.
        /// </summary>
        public static string Components
        {
            get
            {
                return Path.Combine(CommonSettingsRoot, "Components");
            }
        }

        /// <summary>
        /// The root of all database specific settings.
        /// </summary>
        private static string DatabasesRoot
        {
            get { return Path.Combine(CommonSettingsRoot, "Databases"); }
        }

        /// <summary>
        /// Root of all ShipWorks settings that are specific to the current install path of shipworks.
        /// </summary>
        public static string InstanceRoot => getInstancePath();

        /// <summary>
        /// Get the folder used to store settings for the given installed instance of ShipWorks.
        /// </summary>
        public static string InstanceSettings => Path.Combine(InstanceRoot, "Settings");

        /// <summary>
        /// Get the folder used to store log files
        /// </summary>
        public static string LogRoot
        {
            get
            {
                // Otherwise log to the normal folder.
                return Path.Combine(InstanceRoot, "Log");
            }
        }

        /// <summary>
        /// Root of all Database specific storage
        /// </summary>
        private static string CurrentDatabaseRoot
        {
            get
            {
                if (UserSession.DatabaseID == null)
                {
                    throw new InvalidOperationException("Attempt to access DataPath.DatabaseRoot with no UserSession.");
                }

                return Path.Combine(DatabasesRoot, UserSession.DatabaseID);
            }
        }

        /// <summary>
        /// The folder where resources are cached.
        /// </summary>
        public static string CurrentResources
        {
            get
            {
                string path = Path.Combine(CurrentDatabaseRoot, "Resources");

                Directory.CreateDirectory(path);

                return path;
            }
        }

        /// <summary>
        /// Get every resource folder that exists on the drive, for any database.
        /// </summary>
        public static IEnumerable<string> AllResources
        {
            get
            {
                foreach (string databaseRoot in Directory.GetDirectories(DatabasesRoot))
                {
                    yield return Path.Combine(databaseRoot, "Resources");
                }
            }
        }

        /// <summary>
        /// Get the root of the temp file storage.
        /// </summary>
        private static string TempRoot => getTempPath();

        /// <summary>
        /// The ShipWorks specific temporary folder
        /// </summary>
        public static string ShipWorksTemp
        {
            get
            {
                string path = Path.Combine(TempRoot, ShipWorksSession.SessionID.ToString("N"));

                return path;
            }
        }

        /// <summary>
        /// Create a unique temporary directory under the ShipWorksTemp folder.
        /// </summary>
        public static string CreateUniqueTempPath()
        {
            string path = Path.Combine(ShipWorksTemp, Guid.NewGuid().ToString("N"));

            // Since this is a unique folder, its not created during initialization
            Directory.CreateDirectory(path);

            return path;
        }

        /// <summary>
        /// Full path to the ShipWorks.Native.dll.
        /// </summary>
        public static string NativeDll
        {
            get
            {
                return Path.Combine(Program.AppLocation, "ShipWorks.Native.dll");
            }
        }

        /// <summary>
        /// Runs on a schedule to see if any log files are in need of deleting.
        /// </summary>
        private static void CleanupThread()
        {
            log.InfoFormat("Running temp folder cleanup...");

            try
            {
                DirectoryInfo tempRoot = new DirectoryInfo(DataPath.TempRoot);

                // Look at each entry in the temp directory
                foreach (FileSystemInfo fsi in tempRoot.GetFileSystemInfos())
                {
                    // We only look at folders.  There should not be any files directly in temp, because we create everything in a folder
                    // named SessionID.
                    DirectoryInfo di = fsi as DirectoryInfo;
                    if (di != null)
                    {
                        // A little anal, but ensures that if we are running right while another ShipWorks is starting up, which is 
                        // right in between the creation of the folder - but has not yet created the lock file - that we wont delete it.
                        if (di.CreationTimeUtc + TimeSpan.FromMinutes(1) > DateTime.UtcNow)
                        {
                            log.InfoFormat("Skipping '{0}' since it was recently created.", fsi.Name);
                            continue;
                        }

                        if (File.Exists(Path.Combine(fsi.FullName, tempFolderLockName)))
                        {
                            log.InfoFormat("Skipping '{0}' since it is in use.", fsi.Name);
                            continue;
                        }

                        log.InfoFormat("Deleting temp '{0}'", fsi.Name);
                        Directory.Delete(fsi.FullName, true);

                        // If there's a bunch of stuff to delete, we don't want to peg the cpu
                        Thread.Sleep(2);
                    }

                    // Quit if we leave the idle state
                    if (!IdleWatcher.IsIdle)
                    {
                        return;
                    }
                }
            }
            catch (Exception ex) when (ex is ArgumentOutOfRangeException || ex is IOException || ex is UnauthorizedAccessException)
            {
                log.Error("Failed during temp cleanup.", ex);
            }
        }

        /// <summary>
        /// Default implementation of the getCommonSettingsPath func
        /// </summary>
        private static string GetCommonSettingsPathDefault() =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                @"Interapptive\ShipWorks");

        /// <summary>
        /// Default implementation of the getInstancePath func
        /// </summary>
        private static string GetInstancePathDefault()
        {
            if (ShipWorksSession.InstanceID == Guid.Empty)
            {
                throw new InvalidOperationException("Attempt to access DataPath before InstanceID.");
            }

            return Path.Combine(CommonSettingsRoot, "Instances\\" + ShipWorksSession.InstanceID.ToString("B"));
        }

        /// <summary>
        /// Default implementation of the getTempPath func
        /// </summary>
        private static string GetTempPathDefault()
        {
            return Path.Combine(Path.GetTempPath(), "ShipWorks");
        }
    }
}
