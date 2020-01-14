using System;
using System.Timers;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.AutoUpdate;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Settings;
using ShipWorks.Data.Administration;

namespace ShipWorks.ApplicationCore.MessageBoxes
{
    /// <summary>
    /// Window shown to a user when the ShipWorks database is newer than what this version supports
    /// </summary>
    public partial class NeedUpgradeShipWorks : Form
    {
        private int TimeLimit = 30;
        private readonly System.Timers.Timer timer;
        private readonly IUpdateService updateService;

        /// <summary>
        /// Constructor
        /// </summary>
        public NeedUpgradeShipWorks()
        {
            InitializeComponent();

            IShipWorksCommunicationBridge communicationBridge = IoC.UnsafeGlobalLifetimeScope.
                Resolve<IShipWorksCommunicationBridge>(new TypedParameter(typeof(string), ShipWorksSession.InstanceID.ToString()));
            updateService = IoC.UnsafeGlobalLifetimeScope.Resolve<IUpdateService>();

            if (!AutoUpdateSettings.LastAutoUpdateSucceeded || AutoUpdateSettings.IsAutoUpdateDisabled)
            {
                close.Text = "Close";
                update.Visible = false;
            }
            else if (communicationBridge.IsAvailable())
            {
                close.Text = "Cancel";
                update.Visible = true;
                timer = new System.Timers.Timer(1000)
                {
                    Enabled = true,
                    AutoReset = true
                };
                timer.Elapsed += OnCountdownTick;
            }
        }

        /// <summary>
        /// Update the button text, once time runs out kickoff the update
        /// </summary>
        private void OnCountdownTick(object sender, ElapsedEventArgs e)
        {
            TimeLimit -= 1;

            UpdateButtonText();

            if (TimeLimit == 0)
            {
                UpdateShipWorks();
            }
        }

        /// <summary>
        /// Try to kick off the update
        /// </summary>
        private void UpdateShipWorks()
        {
            timer.Stop();

            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(UpdateShipWorks));
            }

            Result result = updateService.Update(SqlSchemaUpdater.GetBuildVersion());

            if (result.Failure)
            {
                MessageHelper.ShowError(this, $"An error occurred while attempting to update.{Environment.NewLine}{result.Message}");
                update.Visible = false;
                close.Text = "Close";
            }
            else
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        /// <summary>
        /// Update the update button text to show the countdown
        /// </summary>
        private void UpdateButtonText()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(UpdateButtonText));
            }

            update.Text = $"Update ({TimeLimit})";
        }

        /// <summary>
        /// Click the link to download the latest version of ShipWorks
        /// </summary>
        private void OnClickDownloadLink(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WebHelper.OpenUrl("http://www.shipworks.com/shipworks/downloadcustomer.php", this);
        }

        /// <summary>
        /// Dispose on close
        /// </summary>
        private void OnClose(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            timer?.Dispose();
        }

        /// <summary>
        /// Updates Shipworks when user clicks update
        /// </summary>
        private void OnClickUpdate(object sender, EventArgs e)
        {
            UpdateShipWorks();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            timer?.Dispose();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
