﻿using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Warehouse.Configuration.Stores
{
    /// <summary>
    /// Class to import stores from the Hub
    /// </summary>
    [Component(RegistrationType.Self)]
    public class HubStoreImporter
    {
        private readonly IHubStoreConfigurator storeConfigurator;
        private readonly IHubConfigurationWebClient webClient;
        private readonly IConfigurationData configurationData;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubStoreImporter(IHubStoreConfigurator storeConfigurator,
            IHubConfigurationWebClient webClient,
            IConfigurationData configurationData)
        {
            this.storeConfigurator = storeConfigurator;
            this.webClient = webClient;
            this.configurationData = configurationData;
        }

        /// <summary>
        /// Import stores from the Hub
        /// </summary>
        public void ImportStores(IWin32Window owner)
        {
            ProgressProvider progressProvider = new ProgressProvider();
            IProgressReporter storeProgress = progressProvider.AddItem("Importing stores");
            storeProgress.CanCancel = false;
            using (ProgressDlg progressDialog = new ProgressDlg(progressProvider))
            {
                progressDialog.Title = "Importing Stores from ShipWorks Hub";
                progressDialog.AllowCloseWhenRunning = false;
                progressDialog.AutoCloseWhenComplete = true;

                ImportAndConfigureStores(storeProgress);

                progressDialog.ShowDialog(owner);
            }
        }

        /// <summary>
        /// Get the list of stores from the Hub and configure them
        /// </summary>
        private void ImportAndConfigureStores(IProgressReporter storeProgress)
        {
            IConfigurationEntity configuration = configurationData.FetchReadOnly();

            if (!string.IsNullOrEmpty(configuration.WarehouseID))
            {
                Task.Run(async () =>
                {
                    storeProgress.Detail = "Getting list of stores to import";
                    storeProgress.Starting();
                    storeProgress.PercentComplete = 0;

                    var hubConfig = await webClient.GetConfig(configuration.WarehouseID).ConfigureAwait(false);
                    storeConfigurator.Configure(hubConfig.StoreConfigurations, storeProgress);
                });
            }
        }
    }
}
