using System;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// Download settings control for a store
    /// </summary>
    public partial class DownloadSettingsControl : UserControl, IDownloadSettingsControl
    {
        private StoreEntity store;
        private ComputerDownloadPolicy downloadPolicy;

        /// <summary>
        /// constructor
        /// </summary>
        public DownloadSettingsControl(StoreEntity store)
        {
            InitializeComponent();
            this.store = store;

            // Download policy
            downloadPolicy = ComputerDownloadPolicy.Load(store);

            // Load the download policy choices and start listening for changes
            comboAllowDownload.LoadChoices(downloadPolicy.DefaultToYes);
            comboAllowDownload.SelectedIndex = -1;
            comboAllowDownload.SelectedIndexChanged += OnChangeAllowDownloading;

            // Auto download
            automaticDownloadControl.LoadStore(store);

            // Download on\off
            comboAllowDownload.SelectedValue = downloadPolicy.GetComputerAllowed(UserSession.Computer.ComputerID);
        }

        /// <summary>
        /// Change whether downloading is allowed from this computer
        /// </summary>
        private void OnChangeAllowDownloading(object sender, EventArgs e)
        {
            downloadPolicy.SetComputerAllowed(UserSession.Computer.ComputerID, (ComputerDownloadAllowed)comboAllowDownload.SelectedValue);
            automaticDownloadControl.Enabled = downloadPolicy.IsThisComputerAllowed;
        }

        /// <summary>
        /// Save the settings to the store
        /// </summary>
        public void Save()
        {
            store.ComputerDownloadPolicy = downloadPolicy.SerializeToXml();
            automaticDownloadControl.SaveToStoreEntity(store);
        }

        /// <summary>
        /// Open the window to change the download policy for all computers
        /// </summary>
        private void OnConfigureDownloadPolicy(object sender, EventArgs e)
        {
            using (ComputerDownloadPolicyDlg dlg = new ComputerDownloadPolicyDlg(downloadPolicy, store.StoreName))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    comboAllowDownload.LoadChoices(downloadPolicy.DefaultToYes);
                    comboAllowDownload.SelectedValue = downloadPolicy.GetComputerAllowed(UserSession.Computer.ComputerID);

                    // If the default changed, and the default is selected, the UI needs updated
                    OnChangeAllowDownloading(null, EventArgs.Empty);
                }
            }
        }
    }
}
