﻿using Interapptive.Shared.UI;
using log4net;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Interapptive.Shared.Threading;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Facilitates opening the Odbc control panel(odbcad32)
    /// </summary>
    public class OdbcControlPanel : IExternalProcess
    {
        private readonly ILog log;
        private readonly IMessageHelper messageHelper;
        private Action action;
        private Process process;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcControlPanel(Func<Type, ILog> logFactory, IMessageHelper messageHelper)
        {
            log = logFactory(typeof(OdbcControlPanel));
            this.messageHelper = messageHelper;
        }

        /// <summary>
        /// Launch the Odbc control panel
        /// </summary>
        /// <param name="callbackAction">the action to invoke when the panel exits</param>
        /// <remarks>
        /// Invokes the callbackAction when the control panel exits
        /// </remarks>
        public void Launch(Action callbackAction)
        {
            action = callbackAction;

            try
            {
                log.Info("Starting ODBC control panel.");
                process = Process.Start("odbcad32");

                // wire up the exited events
                if (process != null)
                {
                    process.EnableRaisingEvents = true;
                    process.Exited += OnExited;
                }
            }
            catch (Exception ex) when (ex.GetType() == typeof(FileNotFoundException) || ex.GetType() == typeof(Win32Exception))
            {
                log.Error(ex);
                // something went wrong when opening the control panel
                messageHelper.ShowError("Unable to open ODBC control panel.");
            }
        }

        /// <summary>
        /// Event handler for when the process closes
        /// </summary>
        private void OnExited(object sender, EventArgs e)
        {
            // invoke the call back action
            log.Info("ODBC control panel exited.");
            action?.Invoke();

            // dispose the process
            process?.Dispose();
        }
    }
}
