using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Microsoft.Win32;
using Interapptive.Shared.Utility;
using System.Windows.Forms;
using System.Diagnostics;
using Interapptive.Shared;
using System.Threading;
using ThreadTimer = System.Threading.Timer;
using Interapptive.Shared.Win32;
using NDesk.Options;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.ApplicationCore.Logging;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Provides information about the current shipworks application session
    /// </summary>
    public static class ShipWorksSession
    {        
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(ShipWorksSession));

        // Unique identifiers for the computer and the installation path of the running ShipWorks
        static Guid computerID;
        static Guid instanceID;

        // Uniquely identifies this running session of ShipWorks
        static Guid sessionID;

        // Timer for logging enviroment data
        static ThreadTimer logTimer;

        /// <summary>
        /// Initialize loaded settings and identifiers
        /// </summary>
        public static void Initialize(ShipWorksCommandLine commandLine)
        {
            string instanceID = null;

            if (commandLine != null)
            {
                OptionSet optionSet = new OptionSet()
                {
                    { "instanceID=", v =>  instanceID = v  }
                };
                optionSet.Parse(commandLine.ProgramOptions);
            }

            if (instanceID == null)
            {
                Initialize(LoadInstanceID());
            }
            else
            {
                Initialize(Guid.Parse(instanceID));
            }

            // Data and logging can be initialized now that we have an instance ID
            DataPath.Initialize();
            LogSession.Initialize();

            // And now we can log...
            if (instanceID != null)
            {
                log.Info("Overriding InstanceID from command line: " + instanceID);
            }
        }

        /// <summary>
        /// Initialize loaded settings and identifiers, using the specified InstanceID instead of looking it up from the registry
        /// </summary>
        public static void Initialize(Guid instanceID)
        {
            ShipWorksSession.computerID = LoadComputerID();
            ShipWorksSession.instanceID = instanceID;

            sessionID = Guid.NewGuid();

            logTimer = new ThreadTimer(new TimerCallback(OnLogTimer), null, TimeSpan.Zero, TimeSpan.FromMinutes(30));
        }

        /// <summary>
        /// The ShipWorks ID that is unique to the current computer
        /// </summary>
        public static Guid ComputerID
        {
            get { return computerID; }
        }

        /// <summary>
        /// The ShipWorks ID that is unique to the current installation path
        /// </summary>
        public static Guid InstanceID
        {
            get { return instanceID; }
        }

        /// <summary>
        /// The ID that is unique to this run of ShipWorks
        /// </summary>
        public static Guid SessionID
        {
            get { return sessionID; }
        }

        /// <summary>
        /// Verifies that the machine-level ComputerID exists and can be read
        /// </summary>
        private static Guid LoadComputerID()
        {
            return GetRegistryLocalMachineValue(@"Software\Interapptive\ShipWorks",
                        "ComputerID",
                        "ShipWorks could not load the ComputerID.\n\n" +
                        "To fix this problem:\n" +
                            "   (1)  Reinstall the application.\n" +
                            "   (2)  For further support, contact Interapptive.");
        }

        /// <summary>
        /// Load the instance ID of this installation of ShipWorks
        /// </summary>
        private static Guid LoadInstanceID()
        {
            return GetRegistryLocalMachineValue(@"Software\Interapptive\ShipWorks\Instances", 
                        Program.AppLocation, 
                        "ShipWorks could not load the InstanceID.\n\n" +
                        "To fix this problem:\n" +
                            "   (1)  Reinstall the application.\n" +
                            "   (2)  For further support, contact Interapptive.");
        }

        /// <summary>
        /// Queries Registry.LocalMachine for a Guid to return.  
        /// </summary>
        /// <param name="subKeyPath">The path to the key starting from registryBaseKey.</param>
        /// <param name="keyName">The name of the key to parse into a Guid.</param>
        /// <param name="errorMessage">If the guild cannot be found, this is the error message to be returned.</param>
        /// <returns>If the key is found and it's value is a valid guid, the guid is returned.  Otherwise an InstallationException is thrown with errorMessage.</returns>
        private static Guid GetRegistryLocalMachineValue(string subKeyPath, string keyName, string errorMessage)
        {
            Guid guid = GetRegistryValue(Registry.LocalMachine, subKeyPath, keyName);

            if (guid == Guid.Empty)
            {
                if (MyComputer.Is64BitWindows && !MyComputer.Is64BitProcess)
                {
                    // This is for integration tests on 64 bit machines.  Try to open the Registry64 view.  If it throws,
                    // Just let the code flow to the InstallationException below.
                    using (RegistryKey registryBaseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                    {
                        guid = GetRegistryValue(registryBaseKey, subKeyPath, keyName);
                    }
                }

                if (guid == Guid.Empty)
                {
                    throw new InstallationException(errorMessage);
                }
            }

            return guid;
        }

        /// <summary>
        /// Queries the registry for a Guid to return.  
        /// </summary>
        /// <param name="registryBaseKey">The base key from which to start.</param>
        /// <param name="subKeyPath">The path to the key starting from registryBaseKey.</param>
        /// <param name="keyName">The name of the key to parse into a Guid.</param>
        /// <returns>If the key is found and it's value is a valid guid, the guid is returned.  Otherwise an empty guid is returned.</returns>
        private static Guid GetRegistryValue(RegistryKey registryBaseKey, string subKeyPath, string keyName)
        {
            using (RegistryKey key = registryBaseKey.OpenSubKey(subKeyPath))
            {
                if (key != null)
                {
                    string value = key.GetValue(keyName) as string;

                    if (value != null)
                    {
                        Guid guid;
                        if (GuidHelper.TryParse(value, out guid))
                        {
                            return guid;
                        }
                    }
                }
            }

            return Guid.Empty;
        }

        /// <summary>
        /// Timer event raised to log environment properties
        /// </summary>
        private static void OnLogTimer(object state)
        {
            Process process = Process.GetCurrentProcess();

            log.InfoFormat("-------- Process Info --------------");
            log.InfoFormat("Handles: {0}", process.HandleCount);
            log.InfoFormat("Threads: {0}", process.Threads.Count);
            log.InfoFormat("User Processor Time: {0}", process.UserProcessorTime);
            log.InfoFormat("Total Processor Time: {0}", process.TotalProcessorTime);
            log.InfoFormat("Physical Memory: {0}", StringUtility.FormatByteCount(process.WorkingSet64));
            log.InfoFormat("Virtual Memory: {0}", StringUtility.FormatByteCount(process.VirtualMemorySize64));
            log.InfoFormat("Peak Physical Memory: {0}", StringUtility.FormatByteCount(process.PeakWorkingSet64));
            log.InfoFormat("Peak Virtual Memory: {0}", StringUtility.FormatByteCount(process.PeakVirtualMemorySize64));
            log.InfoFormat("User Objects: {0}", NativeMethods.GetGuiResources(process.Handle, NativeMethods.GR_USEROBJECTS));
            log.InfoFormat("GDI Objects: {0}", NativeMethods.GetGuiResources(process.Handle, NativeMethods.GR_GDIOBJECTS));
            log.InfoFormat("------------------------------------");
        }
    }
}
