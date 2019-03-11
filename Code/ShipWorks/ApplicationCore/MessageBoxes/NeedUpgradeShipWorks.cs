﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.AutoUpdate;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Administration;

namespace ShipWorks.ApplicationCore.MessageBoxes
{
    /// <summary>
    /// Window shown to a user when the ShipWorks database is newer than what this version supports
    /// </summary>
    public partial class NeedUpgradeShipWorks : Form
    {
        private int TimeLimit = 30;
        private System.Timers.Timer timer;
        private IShipWorksCommunicationBridge communicationBridge;
        private IUpdateService updateService;

        /// <summary>
        /// Constructor
        /// </summary>
        public NeedUpgradeShipWorks()
        {
            InitializeComponent();

            communicationBridge = IoC.UnsafeGlobalLifetimeScope.Resolve<IShipWorksCommunicationBridge>(new TypedParameter(typeof(string), ShipWorksSession.InstanceID.ToString()));
            updateService = IoC.UnsafeGlobalLifetimeScope.Resolve<IUpdateService>();

            if (communicationBridge.IsAvailable())
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

            Result result = updateService.Update(SqlSchemaUpdater.GetInstalledSchemaVersion());

            if (result.Failure)
            {
                MessageHelper.ShowError(this, $"An error occurred while attempting to update.{Environment.NewLine}{result.Message}");
                update.Visible = false;
                close.Text = "Close";
            }
            else
            {
                Application.Exit();
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
            communicationBridge?.Dispose();
            timer?.Dispose();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
