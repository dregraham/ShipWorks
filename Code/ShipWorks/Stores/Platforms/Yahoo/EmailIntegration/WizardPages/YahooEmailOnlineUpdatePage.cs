using System;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Quartz.Util;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.Platforms.Yahoo.EmailIntegration.WizardPages
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
                if (trackingPassword.Text.IsNullOrWhiteSpace())
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
