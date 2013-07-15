using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared;
using Interapptive.Shared.Data;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Editions;
using ShipWorks.UI;
using ShipWorks.UI.Controls;

namespace ShipWorks.ApplicationCore.ExecutionMode
{
    public class ExecutionModeFactory
    {
        private readonly ShipWorksCommandLine commandLine;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionModeFactory"/> class.
        /// </summary>
        public ExecutionModeFactory(ShipWorksCommandLine commandLine)
        {
            if (commandLine == null)
            {
                throw new ArgumentNullException("commandLine");
            }

            this.commandLine = commandLine;
        }

        /// <summary>
        /// Creates an IExecutionMode instance based on the command line provided in the constructor.
        /// </summary>
        /// <returns>An IExecutionMode object for running ShipWorks as a windows service, from the command line,
        /// as a background process, or through the normal UI.</returns>
        public IExecutionMode Create()
        {
            IExecutionMode executionMode = null;

            if (!Environment.UserInteractive)
            {
                // TODO: Fill in the service execution mode when it is available
                // executionMode = new ServiceExecutionMode(...);

                throw new NotImplementedException();
            }
            else if (commandLine.IsCommandSpecified)
            {
                // TODO: Fill in the command line execution mode when it is available
                // executionMode = new CommandLineExecutionMode(...);

                throw new NotImplementedException();
            }
            else
            {
                executionMode = CreateUserInterfaceExecutionMode();
            }

            return executionMode;
        }

        /// <summary>
        /// A helper method that performs setup/initialization required for running the ShipWorks in the UI mode.
        /// </summary>
        /// <returns>A UserInterfaceExecutionMode object.</returns>
        /// <exception cref="ShipWorks.ApplicationCore.ExecutionMode.MultipleExecutionModeInstancesException">An instance of ShipWorks is already running.</exception>
        private UserInterfaceExecutionMode CreateUserInterfaceExecutionMode()
        {
            // Order is important here due to license dependencies of third party components 
            // and other ShipWorks initialization processes.
            SingleInstance.Register(ShipWorksSession.InstanceID);

            if (!InterapptiveOnly.MagicKeysDown && !InterapptiveOnly.AllowMultipleInstances)
            {
                // If the application is already running, open it now and exit.
                if (SingleInstance.ActivateRunningInstance())
                {
                    // User does not have the permissions to run multiple instances of ShipWorks
                    throw new MultipleExecutionModeInstancesException("An instance of ShipWorks is already running.");
                }
            }

            // Check for illegal cross thread calls in any mode - not just when the debugger is attached, which is the default
            Control.CheckForIllegalCrossThreadCalls = true;

            // For Divilements licensing
            Divelements.SandGrid.SandGrid.ActivateProduct("120|iTixOUJcBvFZeCMW0Zqf8dEUqM0=");
            Divelements.SandRibbon.Ribbon.ActivateProduct("120|wmbyvY12rhj+YHC5nTIyBO33bjE=");
            TD.SandDock.SandDockManager.ActivateProduct("120|cez0Ci0UI1owSCvXUNrMCcZQWik=");

            // Common initialization
            CommonInitialization();

            // Initialize window state
            WindowStateSaver.Initialize(Path.Combine(DataPath.WindowsUserSettings, "windows.xml"));
            CollapsibleGroupControl.Initialize(Path.Combine(DataPath.WindowsUserSettings, "collapsiblegroups.xml"));

            // Not ideal, but use the Program.MainForm directly due to complications with the product activiations
            // and the static nature of classes (definitely a candidate for refactoring at some point)
            return new UserInterfaceExecutionMode(Program.MainForm);
        }

        /// <summary>
        /// Do initialization common to command line or UI.  It's here instead of upfront so that if its UI the splash can already be shown.
        /// </summary>
        private static void CommonInitialization()
        {
            MyComputer.LogEnvironmentProperties();

            // Looking for all types in this assembly that have the LLBLGen DependcyInjection attribute
            DependencyInjectionDiscoveryInformation.ConfigInformation = new DependencyInjectionConfigInformation();
            DependencyInjectionDiscoveryInformation.ConfigInformation.AddAssembly(Assembly.GetExecutingAssembly());

            // SSL certificate policy
            ServicePointManager.ServerCertificateValidationCallback = WebHelper.TrustAllCertificatePolicy;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

            // Override the default of 30 seconds.  We are seeing a lot of timeout crashes in the alpha that I think are due
            // to people's machines just not being able to handle the load, and 30 seconds just wasn't enough.
            SqlCommandProvider.DefaultTimeout = TimeSpan.FromSeconds(Debugger.IsAttached ? 300 : 120);

            // Do initial edition initialization
            EditionManager.Initialize();
        }
    }
}
