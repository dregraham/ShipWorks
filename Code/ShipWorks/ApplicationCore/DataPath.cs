using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ShipWorks.ApplicationCore.ExecutionMode;
using log4net;
using ShipWorks.Users;
using System.Threading;
using Interapptive.Shared;
using System.Collections;
using System.Security.AccessControl;
using ShipWorks.ApplicationCore.Interaction;
using System.Windows.Forms;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Paths to directories ShipWorks uses.
    /// </summary>
    public static class DataPath
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(DataPath));

        // A file we keep locked to ensure the temp folder isnt wiped out by another running shipworks
        static FileStream tempFolderLockFile = null;
        static string tempFolderLockName = "running.lock";

        /// <summary>
        /// Ensures that all the data paths have been created.
        /// </summary>
        public static void Initialize()
        {
            if (ShipWorksSession.SessionID == Guid.Empty)
            {
                throw new InvalidOperationException("SessionID has not been initialized.");
            }

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
        /// Provide methhod for background process to register thread to clean up temp folder
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
        private static string CommonSettingsRoot
        {
            get
            {
                string path = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    @"Interapptive\ShipWorks");

                return path;
            }
        }

        /// <summary>
        /// Root path to all settings that are shared accross all ShipWorks users and instances
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
        /// Root of all ShipWorks settings that are specific to the current intall path of shipworks.
        /// </summary>
        public static string InstanceRoot
        {
            get
            {
                if (ShipWorksSession.InstanceID == Guid.Empty)
                {
                    throw new InvalidOperationException("Attempt to access DataPath before InstanceID.");
                }

                return Path.Combine(CommonSettingsRoot, "Instances\\" + ShipWorksSession.InstanceID.ToString("B"));
            }
        }

        /// <summary>
        /// Get the folder used to store settings for the given installed instance of ShipWorks.
        /// </summary>
        public static string InstanceSettings
        {
            get
            {
                return Path.Combine(InstanceRoot, "Settings");
            }
        }

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
        private static string TempRoot
        {
            get
            {
                string path = Path.Combine(Path.GetTempPath(), "ShipWorks");

                return path;
            }
        }

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

            // Since this is a unique folder, its not created during init
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
            catch(Exception ex) when (ex is ArgumentOutOfRangeException || ex is IOException || ex is UnauthorizedAccessException)
            {
                log.Error("Failed during temp cleanup.", ex);
            }
        }
    }
}
