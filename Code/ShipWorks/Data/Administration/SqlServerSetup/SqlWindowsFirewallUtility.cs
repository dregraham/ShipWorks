using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32;
using log4net;
using Microsoft.Win32;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Connection;

namespace ShipWorks.Data.Administration.SqlServerSetup
{
    /// <summary>
    /// Utility class for opening the windows firewall
    /// </summary>
    public static class SqlWindowsFirewallUtility
    {
        static readonly ILog log = LogManager.GetLogger(typeof(SqlWindowsFirewallUtility));

        public const int ErrorSqlInstanceKeyMissing = 654321;
        public const int ErrorSqlBrowserKeyMissing = 765432;
        public const int ErrorWrongComputer = 324987;

        /// <summary>
        /// Open windows firewall for current SQL Server session
        /// </summary>
        public static void OpenWindowsFirewall()
        {
            if (!SqlSession.IsConfigured)
            {
                throw new InvalidOperationException("Trying to OpenWindowsFirewall with no SqlSession.");
            }

            // Has to be on this computer
            if (!SqlSession.Current.IsLocalServer())
            {
                throw new WindowsFirewallException(ErrorWrongComputer, LookupMessage(ErrorWrongComputer));
            }

            // Extract the instance name from the server\instance 
            string instance = SqlInstanceUtility.ExtractInstanceName(SqlSession.Current.Configuration.ServerInstance);

            // For vista we need elevation
            if (MyComputer.IsWindowsVistaOrHigher || InterapptiveOnly.MagicKeysDown)
            {
                log.Info("Launching external process to elevate permissions.");

                try
                {
                    // We need to launch the process to elevate ourselves
                    Process process = new Process();
                    process.StartInfo = new ProcessStartInfo(Application.ExecutablePath, string.Format("/cmd:{0} {1}",
                        new CommandLineHandler().CommandName,
                        string.IsNullOrEmpty(instance) ? SqlInstanceUtility.DefaultInstanceName : instance));
                    process.StartInfo.Verb = "runas";
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    process.Start();
                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        throw new WindowsFirewallException(process.ExitCode, LookupMessage(process.ExitCode));
                    }
                }
                catch (Win32Exception ex)
                {
                    throw new WindowsFirewallException(ex);
                }
            }
            else
            {
                OpenWindowsFirewallElevated(instance);
            }
        }

        /// <summary>
        /// Opens the windows firewall for the given SQL Instance.  The process must already be elevated
        /// </summary>
        public static void OpenWindowsFirewallElevated(string instance)
        {
            if (!MyComputer.HasWindowsFirewall)
            {
                return;
            }

            WindowsFirewallUtility.ExecuteSetServiceEnabled("FILEANDPRINT");

            string sqlBrowserKeyPath = @"SYSTEM\CurrentControlSet\Services\SQLBrowser";

            // First we have to find the install path of the sql binn
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(sqlBrowserKeyPath))
            {
                if (key == null)
                {
                    log.ErrorFormat("Could not find firewall key '{0}'", sqlBrowserKeyPath);
                    throw new WindowsFirewallException(ErrorSqlBrowserKeyMissing, LookupMessage(ErrorSqlBrowserKeyMissing));
                }

                string sqlbrowser = (string) key.GetValue("ImagePath");

                WindowsFirewallUtility.ExecuteAddAllowedProgram(sqlbrowser, "SQL Browser");
            }

            string instanceID;

            try
            {
                instanceID = SqlInstanceUtility.GetInstanceID(instance);
            }
            catch (NotFoundException ex)
            {
                log.ErrorFormat("Could not find instanceID for '{0}': {1}", instance, ex.Message);
                throw new WindowsFirewallException(ErrorSqlInstanceKeyMissing, LookupMessage(ErrorSqlInstanceKeyMissing));
            }

            string sqlInstanceExeKeyPath = string.Format(@"SOFTWARE\Microsoft\Microsoft SQL Server\{0}\Setup", instanceID);

            RegistryKey instanceKey = null;

            try
            {
                instanceKey = Registry.LocalMachine.OpenSubKey(sqlInstanceExeKeyPath);

                if (instanceKey == null && MyComputer.Is64BitProcess)
                {
                    instanceKey = Registry.LocalMachine.OpenSubKey(sqlInstanceExeKeyPath.Replace("SOFTWARE", @"SOFTWARE\Wow6432Node"));
                }

                if (instanceKey == null)
                {
                    log.ErrorFormat("Could not find firewall key '{0}'", sqlInstanceExeKeyPath);
                    throw new WindowsFirewallException(ErrorSqlInstanceKeyMissing, LookupMessage(ErrorSqlInstanceKeyMissing));
                }

                string path = (string) instanceKey.GetValue("SQLPath");
                string sqlserver = Path.Combine(path, @"Binn\sqlservr.exe");

                WindowsFirewallUtility.ExecuteAddAllowedProgram(sqlserver, string.Format("SQL Server ({0})", string.IsNullOrEmpty(instance) ? SqlInstanceUtility.DefaultInstanceName : instance));
            }
            finally
            {
                if (instanceKey != null)
                {
                    instanceKey.Close();
                }
            }
        }

        /// <summary>
        /// Lookup the message to use for the given code
        /// </summary>
        private static string LookupMessage(int code)
        {
            if (code == ErrorSqlBrowserKeyMissing)
            {
                return "The SQL browser registry key was not found. The Windows Firewall can only be opened from the computer that is running SQL Server.";
            }

            if (code == ErrorSqlInstanceKeyMissing)
            {
                return "The SQL instance registry key was not found. The Windows Firewall can only be opened from the computer that is running SQL Server.";
            }

            if (code == ErrorWrongComputer)
            {
                return "The Windows Firewall can only be opened from the computer that is running SQL Server.";
            }

            return string.Format("Error code: {0}", code);
        }


        /// <summary>
        /// Process incoming commands from the command line
        /// </summary>
        private class CommandLineHandler : ICommandLineCommandHandler
        {
            /// <summary>
            /// Name of the command
            /// </summary>
            public string CommandName
            {
                get { return "opensqlfirewall"; }
            }

            /// <summary>
            /// Execute the command with the given options
            /// </summary>
            public Task Execute(List<string> args)
            {
                if (args.Count != 1)
                {
                    throw new CommandLineCommandArgumentException(CommandName, "(default)", "The SQL Server instance name was not supplied");
                }

                try
                {
                    string sqlinstance = args[0];
                    log.InfoFormat("Processing request to open firewall: {0}", sqlinstance);

                    OpenWindowsFirewallElevated(sqlinstance);
                }
                catch (Win32Exception ex)
                {
                    log.Error("Failed to process firewall request.", ex);
                    Environment.ExitCode = ex.NativeErrorCode;
                }
                catch (WindowsFirewallException ex)
                {
                    log.Error("Failed to process firewall request.", ex);
                    Environment.ExitCode = ex.ErrorCode;
                }

                return Task.CompletedTask;
            }
        }
    }
}
