using Interapptive.Shared.UI;
using log4net;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Facilitates opening the Odbc control panel(odbcad32)
    /// </summary>
    public class OdbcControlPanel : IOdbcControlPanel
    {
        private readonly ILog log;
        private readonly IMessageHelper messageHelper;
        private Action action;

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
        /// <param name="callbackAction">the action to invoice when the panel exits</param>
        /// <remarks>
        /// Invokes the callbackAction when the control panel exits
        /// </remarks>
        public void Launch(Action callbackAction)
        {
            action = callbackAction;

            try
            {
                log.Info("Starting ODBC control panel.");
                Process odbcCtrl = Process.Start("odbcad32");

                // wire up the exited events
                odbcCtrl.EnableRaisingEvents = true;
                odbcCtrl.Exited += OnExited;
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
            log.Info("ODBC control panel exited.");
            action?.Invoke();
        }
    }
}
