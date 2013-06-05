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

            OptionSet optionSet = new OptionSet()
                {
                    { "instanceID=", v =>  instanceID = v  }
                };
            optionSet.Parse(commandLine.ProgramOptions);

            if (instanceID == null)
            {
                Initialize(LoadInstanceID());
            }
            else
            {
                log.InfoFormat("Overriding InstanceID from command line: " + instanceID);
                Initialize(Guid.Parse(instanceID));
            }
        }

        /// <summary>
        /// Initialize loaded settings and identifiers, using the specified InstanceID isntead of looking it up from the registry
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
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Interapptive\ShipWorks"))
            {
                if (key != null)
                {
                    string value = key.GetValue("ComputerID") as string;

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

            throw new InstallationException(
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
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Interapptive\ShipWorks\Instances"))
            {
                if (key != null)
                {
                    string value = key.GetValue(Program.AppLocation) as string;

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

            throw new InstallationException(
                "ShipWorks could not load the InstanceID.\n\n" +
                "To fix this problem:\n" +
                    "   (1)  Reinstall the application.\n" +
                    "   (2)  For further support, contact Interapptive.");
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
