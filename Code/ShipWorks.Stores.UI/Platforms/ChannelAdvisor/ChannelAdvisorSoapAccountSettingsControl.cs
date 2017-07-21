using System;
using System.ComponentModel;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Properties;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.ChannelAdvisor;

namespace ShipWorks.Stores.UI.Platforms.ChannelAdvisor
{
    /// <summary>
    /// ChannelAdvisor account configuration control
    /// </summary>
    [ToolboxItem(true)]
    public partial class ChannelAdvisorSoapAccountSettingsControl : AccountSettingsControlBase
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ChannelAdvisorSoapAccountSettingsControl));

        const string notAuthorizedText = "Not Authorized";

        // ChannelAdvisor store
        int storeProfileId = 0;
        string loadedAccountKey = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorSoapAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the store settings into the UI
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            ChannelAdvisorStoreEntity caStore = store as ChannelAdvisorStoreEntity;
            if (caStore == null)
            {
                throw new ArgumentException("A non ChannelAdvisor store was passed to ChannelAdvisor account settings.");
            }

            loadedAccountKey = caStore.AccountKey;
            bool keyExists = loadedAccountKey.Length > 0;

            if (caStore.ProfileID > 0)
            {
                profileId.Text = caStore.ProfileID.ToString();
            }

            statusText.Text = "Checking...";
            statusPicture.Image = Resources.indiciator_green;
            statusLabel.Visible = true;
            statusText.Visible = true;
            statusPicture.Visible = true;

            // LoadStore is only called through Manage Stores so we most likely won't need the Grant Access immediately
            accessPanel.Visible = false;

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(TestAuthorization);

            if (keyExists)
            {
                // test authorization with the key
                worker.RunWorkerAsync(caStore.AccountKey);
            }
            else
            {
                // otherwise, lookup via the profile Id
                if (caStore.ProfileID > 0)
                {
                    worker.RunWorkerAsync(caStore.ProfileID);
                }
                else
                {
                    // first time, no profile, no key so
                    statusText.Text = notAuthorizedText;
                    statusPicture.Visible = false;
                    accessPanel.Visible = true;
                }
            }
        }

        /// <summary>
        /// Access Key was tested
        /// </summary>
        private void OnTestAuthorizationComplete(bool success)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<bool>(OnTestAuthorizationComplete), success);
                return;
            }
            else
            {
                if (success)
                {
                    statusPicture.Image = Resources.check16;
                    statusPicture.Visible = true;

                    statusText.Text = "Access Granted";
                }
                else
                {
                    statusPicture.Visible = false;
                    statusText.Text = notAuthorizedText;

                    accessPanel.Visible = true;
                }
            }
        }

        /// <summary>
        /// Tests the access key in the background
        /// </summary>
        private void TestAuthorization(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (e.Argument is string)
                {
                    string accountKey = (string)e.Argument;

                    OnTestAuthorizationComplete(ChannelAdvisorSoapClient.TestConnection(accountKey));
                }
                else
                {
                    int configuredProfileId = (int)e.Argument;

                    loadedAccountKey = ChannelAdvisorSoapClient.GetAccountKey(configuredProfileId);
                    if (loadedAccountKey == null)
                    {
                        OnTestAuthorizationComplete(false);
                    }
                    else
                    {
                        OnTestAuthorizationComplete(ChannelAdvisorSoapClient.TestConnection(loadedAccountKey));
                    }
                }
            }
            catch (ChannelAdvisorException)
            {
                // fail
                OnTestAuthorizationComplete(false);
            }
        }

        /// <summary>
        /// Save UI values to the provided store entity
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            ChannelAdvisorStoreEntity caStore = store as ChannelAdvisorStoreEntity;
            if (caStore == null)
            {
                throw new ArgumentException("A non ChannelAdvisor store was passed to ChannelAdvisor account settings.");
            }

            if (profileId.Text.Trim().Length == 0)
            {
                MessageHelper.ShowError(this, "Please provide your Profile ID.");
                return false;
            }

            int profileIdNumber = 0;
            if (!int.TryParse(profileId.Text, out profileIdNumber))
            {
                MessageHelper.ShowError(this, String.Format("'{0}' is not a valid ChannelAdvisor Profile ID.", profileId.Text));
                return false;
            }

            if (loadedAccountKey == null)
            {
                // must authorize first
                MessageHelper.ShowError(this, "You must first Request Access for ShipWorks to access your ChannelAdvisor account.");
                return false;
            }

            // test the account key
            if (!ChannelAdvisorSoapClient.TestConnection(loadedAccountKey))
            {
                // show a message
                MessageHelper.ShowError(this, "ShipWorks was unable to communicate with ChannelAdvisor. Ensure you have granted ShipWorks access to your account.");
                return false;
            }

            // save the values
            caStore.AccountKey = loadedAccountKey;
            caStore.ProfileID = profileIdNumber;

            return true;
        }

        /// <summary>
        /// CA link clicked
        /// </summary>
        private void OnCALinkClick(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://www.channeladvisor.com/", this);
        }

        /// <summary>
        /// Start the Access Request process
        /// </summary>
        private void OnRequestAccess(object sender, EventArgs e)
        {
            // make sure they entered a profile id
            if (!int.TryParse(profileId.Text, out storeProfileId))
            {
                MessageHelper.ShowError(this, String.Format("'{0}' is not a valid ChannelAdvisor Profile ID.", profileId.Text));
                return;
            }

            // initiate access request
            try
            {
                ChannelAdvisorSoapClient.RequestShipWorksAccess(storeProfileId);
            }
            catch (ChannelAdvisorException ex)
            {
                MessageHelper.ShowError(this, String.Format("ShipWorks was unable to request access: {0}", ex.Message));
                return;
            }

            requestAccessButton.Enabled = false;
            statusPicture.Image = Resources.indiciator_green;
            statusText.Text = "Waiting for you to finish authorizing ShipWorks...";

            statusLabel.Visible = true;
            statusPicture.Visible = true;
            statusText.Visible = true;

            // start the browser
            OnCALinkClick(this, EventArgs.Empty);

            // start polling for Access
            timer.Start();
        }

        /// <summary>
        /// If the control is currently waiting on a token import, stop.  If not currently waiting, this does nothing.
        /// </summary>
        public void CancelWaiting()
        {
            timer.Stop();

            statusPicture.Visible = false;
            statusText.Visible = false;
        }

        /// <summary>
        /// Poll for the access key
        /// </summary>
        private void OnTimerTick(object sender, EventArgs e)
        {
            // If the user is on a different wizard page or store settings page forget it...
            if (!Visible || TopLevelControl == null)
            {
                return;
            }

            // Vista bug - this stops running on vista after a window obscures.
            statusPicture.Invalidate();

            try
            {
                loadedAccountKey = ChannelAdvisorSoapClient.GetAccountKey(storeProfileId);
                if (loadedAccountKey != null)
                {
                    // stop polling
                    timer.Stop();

                    // show success
                    statusPicture.Image = Resources.check16;
                    statusText.Text = "Access Granted";
                }
                else
                {
                    log.InfoFormat("Poll for access key failure.");
                }
            }
            catch (ChannelAdvisorException ex)
            {
                log.InfoFormat("Poll for access key failure: {0}", ex.Message);
            }
        }
    }
}
