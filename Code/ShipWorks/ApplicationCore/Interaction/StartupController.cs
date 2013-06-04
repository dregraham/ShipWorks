using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32;
using System.Runtime.InteropServices;

namespace ShipWorks.ApplicationCore.Interaction
{
    /// <summary>
    /// Helps to deal with situations where ShipWorks requires a reboot, and what to do when ShipWorks starts after the reboot.
    /// </summary>
    public static class StartupController
    {
        static StartupAction? startupAction = null;
        static XElement startupArgument = null;

        /// <summary>
        /// Set the action ShipWorks should perform when starting up for the next time.  The value of "argument" will be available
        /// as the StartupArgument property after the action is performed.  This allows the target of the action to retrieve the data.
        /// </summary>
        public static void SetStartupAction(StartupAction action, XElement argument)
        {
            XElement xElement = new XElement("Startup", new XAttribute("action", (int) action), argument);
            xElement.Save(Path.Combine(DataPath.InstanceSettings, "startup.xml"));
        }

        /// <summary>
        /// Load the startup action and clear it, so that it's not loaded again.
        /// </summary>
        private static void LoadStartupAction()
        {
            string file = Path.Combine(DataPath.InstanceSettings, "startup.xml");

            if (!File.Exists(file))
            {
                startupAction = StartupAction.Default;
                return;
            }

            XElement xElement = XElement.Load(file);

            startupAction = (StartupAction) (int) xElement.Attribute("action");
            startupArgument = xElement.Elements().SingleOrDefault();
        }

        /// <summary>
        /// Clears the startup action so it is not processed the next time ShipWorks starts
        /// </summary>
        public static void ClearStartupAction()
        {
            if (File.Exists(Path.Combine(DataPath.InstanceSettings, "startup.xml")))
            {
                File.Delete(Path.Combine(DataPath.InstanceSettings, "startup.xml"));
            }

            startupAction = StartupAction.Default;
            startupArgument = null;
        }

        /// <summary>
        /// Gets the StartupAction that applies to the current run of ShipWorks.
        /// </summary>
        public static StartupAction StartupAction
        {
            get
            {
                if (startupAction == null)
                {
                    LoadStartupAction();
                }

                return startupAction.Value;
            }
        }

        /// <summary>
        /// If StatupAction is not Default, this is the argument that was passed when setting the StartupAction.
        /// </summary>
        public static XElement StartupArgument
        {
            get { return startupArgument; }
        }

        /// <summary>
        /// Indicates that the system must be rebooted before ShipWorks can be run again.
        /// </summary>
        public static void RequireReboot()
        {
            XDocument xDocument = new XDocument(
                new XElement("RequireReboot",
                    new XElement("SystemBootTime", WindowsUtility.LastBootTime)));

            xDocument.Save(RebootIndicatorFile);
        }

        /// <summary>
        /// Indiciates if a reboot is required before ShipWorks can be run again.
        /// </summary>
        public static bool CheckRebootRequired()
        {
            if (!File.Exists(RebootIndicatorFile))
            {
                return false;
            }

            XDocument xDocument = XDocument.Load(RebootIndicatorFile);

            DateTime lastBoot = (DateTime)xDocument.Descendants("SystemBootTime").Single();

            bool rebootRequired = false;
            try
            {
                rebootRequired = lastBoot >= WindowsUtility.LastBootTime;
            }
            catch (COMException)
            {
                // WMI was unresponsive, this only seems to happen when ShipWorks is started before the
                // system is fully running after boot.  So we take this as a sign that the machine was 
                // rebooted
            }

            if (!rebootRequired)
            {
                try
                {
                    File.Delete(RebootIndicatorFile);
                }
                catch (UnauthorizedAccessException)
                {
                    // for reasons unknown, immediately after a boot the user is unable to 
                    // delete the file that was written, so we ignore it to allow
                    // ShipWorks to continue
                }
            }

            return rebootRequired;
        }

        /// <summary>
        /// The path to the file that tracks if we need a reboot
        /// </summary>
        private static string RebootIndicatorFile
        {
            get { return Path.Combine(DataPath.SharedSettings, "reboot.xml"); }
        }
    }
}
