﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Yahoo.WizardPages
{
    /// <summary>
    /// Page for configuring Yahoo! online status updates
    /// </summary>
    public partial class YahooOnlineUpdatePage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public YahooOnlineUpdatePage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The check on the enable tracking password page changed
        /// </summary>
        private void OnEnableTrackingPasswordChanged(object sender, EventArgs e)
        {
            panelOnlineUpdate.Enabled = enableTrackingPassword.Checked;
        }

        /// <summary>
        /// Stepping next from the page
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            YahooStoreEntity store = GetStore<YahooStoreEntity>();

            if (enableTrackingPassword.Checked)
            {
                if (trackingPassword.Text.Trim().Length == 0)
                {
                    MessageHelper.ShowInformation(this, "Enter your Yahoo! email tracking password.");
                    e.NextPage = this;
                    return;
                }

                store.TrackingUpdatePassword = SecureText.Encrypt(trackingPassword.Text, "yahoo");
            }
            else
            {
                store.TrackingUpdatePassword = "";
            }
        }
    }
}
