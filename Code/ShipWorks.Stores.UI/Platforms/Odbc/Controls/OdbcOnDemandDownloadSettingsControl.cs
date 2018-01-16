using System;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Users;

namespace ShipWorks.Stores.UI.Platforms.Odbc.Controls
{
    /// <summary>
    /// Download settings for Odbc On Demand downloads
    /// </summary>
    public partial class OdbcOnDemandDownloadSettingsControl : UserControl, IDownloadSettingsControl
    {
        private StoreEntity store;
        private ComputerDownloadPolicy downloadPolicy;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcOnDemandDownloadSettingsControl(StoreEntity store)
        {
            InitializeComponent();
            this.store = store;

            // Download policy
            downloadPolicy = ComputerDownloadPolicy.Load(store);

            // Load the download policy choices and start listening for changes
            comboAllowDownload.LoadChoices(downloadPolicy.DefaultToYes);
            comboAllowDownload.SelectedIndex = -1;
            comboAllowDownload.SelectedIndexChanged += OnChangeAllowDownloading;
            
            // Download on\off
            comboAllowDownload.SelectedValue = downloadPolicy.GetComputerAllowed(UserSession.Computer.ComputerID);
            
        }

        /// <summary>
        /// Change whether downloading is allowed from this computer
        /// </summary>
        private void OnChangeAllowDownloading(object sender, EventArgs e)
        {
            downloadPolicy.SetComputerAllowed(UserSession.Computer.ComputerID, (ComputerDownloadAllowed)comboAllowDownload.SelectedValue);
        }

        /// <summary>
        /// Save the download policy
        /// </summary>
        public void Save()
        {
            store.ComputerDownloadPolicy = downloadPolicy.SerializeToXml();
        }
    }
}
