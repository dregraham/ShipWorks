﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.ApplicationCore.Interaction;
using log4net;
using Interapptive.Shared.Win32;
using System.Windows.Forms;
using System.ComponentModel;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Responsible for opening the Windows Firewall for all traffic from ShipWorsk to flow through.  Initially added for buy.com since it requires active\passive FTP, but probably handy in general.
    /// 
    /// This only works if called while permissions are elevated.
    /// 
    /// This is only called by InnoSetup after the shipworks.exe has been installed.
    /// 
    /// </summary>
    public class ShipWorksWindowsFirewallOpener : ICommandLineCommandHandler
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShipWorksWindowsFirewallOpener));

        /// <summary>
        /// The CommandName that can be sent to the shipworks.exe
        /// </summary>
        public string CommandName
        {
            get { return "openshipworksfirewall"; }
        }

        /// <summary>
        /// Excecute the command
        /// </summary>
        public void Execute(List<string> args)
        {
            try
            {
                log.InfoFormat("Processing request to open firewall");

                WindowsFirewallUtility.ExecuteAddAllowedProgram(Application.ExecutablePath, "Interapptive® ShipWorks®");
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
        }
    }
}
